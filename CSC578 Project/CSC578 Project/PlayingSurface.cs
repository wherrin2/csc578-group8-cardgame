using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Navigation;

namespace CSC578_Project
{
    public partial class PlayingSurface : Form
    {
        //todo implement player turn logic with appropriate IDs
        //todo implement surface manager that orders layers

        public event EventHandler<GameObjectEventArgs> GameObjectHasMoved;
        public event EventHandler GameHasStarted;
        public event EventHandler GameHasBeenClosed;

        private bool leftClicked;
        private bool isMoving;
        private Point mouseClickPosition;
        private List<Control> formControls = new List<Control>();
        private List<Control> formBoundaries = new List<Control>(); 
        private List<GameObject> players = new List<GameObject>();
        private int currentPlayerIndex = 0; //stored as index to list
        private bool gameStarted = true;
        
         
        private string path = AppDomain.CurrentDomain.BaseDirectory + @"\";

        public PlayingSurface()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Adds Game Object to Playing Surface. Creates a control that represents it.
        /// </summary>
        /// <param name="gameObject">Game Object to add</param>
        public void AddGameObject(GameObject gameObject)
        {
            var type = gameObject.GetType();
            if (typeof(MovableObject) == type)
            {
                CreateMovableObject((MovableObject)gameObject);
            }
            else if (typeof(BoundaryObject) == type)
            {
                CreateBoundaryObject((BoundaryObject)gameObject);
            }
            else if (typeof(DrawableObject) == type)
            {
                CreateDrawableObject((DrawableObject)gameObject);
            }
            else if (gameObject.Name.ToLower().StartsWith("player"))
            {
                players.Add(gameObject);
            }

        }

        /// <summary>
        /// Removes Game Object's control from Playing Surface
        /// </summary>
        /// <param name="gameObject">Game object to remove</param>
        public void RemoveGameObject(GameObject gameObject)
        {
            List<Control> removeControls = FindControlsByGameObject(gameObject);

            foreach(var control in removeControls)
            {
                formControls.Remove(control);
                control.Dispose();
            }
        }


        /// <summary>
        /// Moves Game Object to specified point
        /// </summary>
        /// <param name="gameObject">Game Object to move</param>
        /// <param name="position">Position to move</param>
        /// <param name="animate">Animate transition to new location</param>
        public void MoveGameObject(GameObject gameObject, Position position, bool animate)
        {
            //move controls and animate with a timer if necessary
            List<Control> controls = FindControlsByGameObject(gameObject);
            foreach (var control in controls)
            {
                if (animate && !isMoving)
                {
                    moveTimer.Tag = control;
                    isMoving = true;
                    moveTimer.Start();
                }
                else
                    control.Location = new Point(position.X, position.Y);

                var boundary = GetBoundaryGameObjectIntersectsWith((PictureBox) control);
                HandleBoundaryLogicAction(boundary, gameObject, "delete()");
            }
        }

        public void RedrawObject(GameObject gameObject)
        {
            List<Control> controls = FindControlsByGameObject(gameObject);
            foreach (var control in controls)
            {
                if (gameObject.GetType() == typeof (MovableObject))
                {
                    var currentControl = (PictureBox) control;
                    var drawable = (MovableObject) gameObject;
                    if (drawable.IsFrontImage != drawable.IsFrontImagePrevious)
                    {
                        FlipImage(currentControl, drawable.IsFrontImagePrevious);
                        drawable.IsFrontImagePrevious = drawable.IsFrontImage;
                    }
                }
            }
        }

        public void ShowAllowSwapsButton()
        {
            btnSwaps.Location = new Point(Width/2, Height/2);
            btnSwaps.Visible = true;
            gameStarted = false;
        }

        public void AddLogicActionToBoundaries(LogicAction logicAction)
        {
            foreach (var control in formBoundaries)
            {
                var boundary = (BoundaryObject) ((PictureBox) control).Tag;
                foreach (var boundaryRule in logicAction.BoundaryRules)
                {
                    if (boundary.Name.Equals(boundaryRule))
                        boundary.AddLogicAction(logicAction);
                }
            }   
        }

        private BoundaryObject GetBoundaryGameObjectIntersectsWith(PictureBox pictureBox)
        {
            BoundaryObject boundary = null;
            foreach (var control in formBoundaries)
            {
                if (control.Bounds.IntersectsWith(pictureBox.Bounds))
                   return (BoundaryObject) ((PictureBox) control).Tag;
            }
            return boundary;
        }

        private void CreateBoundaryObject(BoundaryObject boundary)
        {
            var pictureBox = new PictureBox()
            {
                Height = boundary.Height,
                Width = boundary.Width,
                BackColor = Color.Transparent,
                Location = new Point(boundary.Position.X, boundary.Position.Y)
            };
            if (boundary.ShowBoundaryOutline)
            {
                pictureBox.Paint += PictureBox_Paint;
            }
            pictureBox.Tag = boundary;
            formBoundaries.Add(pictureBox);
            Controls.Add(pictureBox); 
        }

        private void CreateMovableObject(MovableObject movable)
        {
            var pictureBox = CreatePictureBox(movable);
            if (pictureBox != null)
            {
                pictureBox.MouseDown += Movable_MouseDown;
                pictureBox.MouseUp += Movable_MouseUp;
                pictureBox.Tag = movable;

                formControls.Add(pictureBox);
                Controls.Add(pictureBox);
            }
        }

        private void CreateDrawableObject(DrawableObject drawable)
        {
            if (drawable.IsBackgroundImage)
            {
                try
                {
                    Width = drawable.Width;
                    Height = drawable.Height;
                    BackgroundImage =
                        Image.FromFile(drawable.IsFrontImage ? path + drawable.FrontImage : path + drawable.BackImage);
                }
                catch (FileNotFoundException e)
                {
                    MessageBox.Show("Background Image not found. Check path in configuration files.\n" + e.Message, "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                var pictureBox = CreatePictureBox(drawable);
                if (pictureBox != null)
                {
                    formControls.Add(pictureBox);
                    Controls.Add(pictureBox);
                }
            }
           
        }

        private PictureBox CreatePictureBox(DrawableObject drawable)
        {
            try
            {
                var pictureBox = new PictureBox
                {
                    Height = drawable.Height,
                    Width = drawable.Width,
                    Location = new Point(drawable.Position.X, drawable.Position.Y),
                    BackColor = Color.Transparent,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Image =
                        Image.FromFile(drawable.IsFrontImage ? path + drawable.FrontImage : path + drawable.BackImage)
                };
                return pictureBox;
            }
            catch (FileNotFoundException e)
            {
                MessageBox.Show(
                    "Image File not Found!\n" + drawable.FrontImage + " or\n " + drawable.BackImage +
                    "\nPlease verify path in configuration files\n" + e.Message, "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            catch (NullReferenceException e)
            {
                MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private List<Control> FindControlsByGameObject(GameObject gameObject)
        {
            List<Control> foundControls = formControls
                .Cast<Control>()
                .Where(item => (GameObject)item.Tag == gameObject)
                .ToList();

            return foundControls;
        }

        private PictureBox RotatePictureBox(PictureBox pictureBox)
        {
            //should have a drawable to be flipped
            var drawable = (DrawableObject) pictureBox.Tag;
            int temp = drawable.Width;
            drawable.Width = drawable.Height;
            pictureBox.Width = drawable.Height;
            drawable.Height = temp;
            pictureBox.Height = temp;
            var image = pictureBox.Image;
            //this only works if image is double-sided when already rotated
            image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            pictureBox.Image = image;
            drawable.IsRotated = !drawable.IsRotated;
            return pictureBox;
        } 

        private void FlipImage(PictureBox pictureBox , bool isFront)
        {
            try
            {
                var drawable = (DrawableObject) pictureBox.Tag;
                pictureBox.Image = isFront
                    ? Image.FromFile(path + drawable.BackImage)
                    : Image.FromFile(path + drawable.FrontImage);
                pictureBox.Invalidate();
                pictureBox.Refresh();
                pictureBox.BringToFront();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        private bool HandleBoundaryLogicAction(BoundaryObject boundary, GameObject gameObject, string eventString)
        {
            var logicActions = boundary.GetLogicActions();
            var handledAny = false;
            if (logicActions.Count > 0)
            {
               var actions = logicActions.FindAll(x => x.Event.ToLower().Equals(eventString.ToLower()));
                foreach (var action in actions)
                {
                    var logicObject = new LogicObjectHandler(action.Actions, gameObject);
                    if (logicObject.ExecuteLogicObject())
                        handledAny = true;
                }
            }
            return handledAny;
        }


        private void Movable_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                //fix ID in isSelectable - should be current player's ID
                if (!isMoving)
                {
                    var pictureBox = (PictureBox) sender;
                    var movable = (MovableObject) pictureBox.Tag;

                    leftClicked = e.Button == MouseButtons.Left;

                    if (movable.IsSelectable(players[currentPlayerIndex].Id))
                    {
                        pictureBox.BringToFront();
                        movable.IsSelected = true;

                       
                        if (leftClicked)
                        {
                            pictureBox.MouseMove += Movable_MouseMove;
                        }
                        mouseClickPosition = e.Location;
                    }

                    if (leftClicked && gameStarted)
                    {
                        var boundary = GetBoundaryGameObjectIntersectsWith(pictureBox);
                        HandleBoundaryLogicAction(boundary, movable, "select()");
                    }


                }
            }
            catch (Exception error)
            {
                Debug.WriteLine(error.StackTrace);
            }

        }

        private void NextPlayerTurn()
        {
            currentPlayerIndex++;
            if (players.Count <= currentPlayerIndex)
                currentPlayerIndex = 0;
            DisplayPlayerTurn();
        }

        private void DisplayPlayerTurn()
        {
            try
            {
                Text = "It is " + players[currentPlayerIndex].Name + "'s turn. Id = " + players[currentPlayerIndex].Id;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.StackTrace);
            }
        }

        private void PreviousPlayerTurn()
        {
            currentPlayerIndex--;
            if (players.Count < 0)
                currentPlayerIndex = players.Count - 1;

            DisplayPlayerTurn();
        }

        private void Movable_MouseUp(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var movable = (MovableObject)pictureBox.Tag;
            leftClicked = false;
            pictureBox.MouseMove -= Movable_MouseMove;
            movable.IsSelected = false;


            GameObject currentObject = (GameObject)pictureBox.Tag;
            var hasMoved = false;
            foreach (var boundary in formBoundaries)
            {
                if (boundary.Bounds.IntersectsWith(pictureBox.Bounds))
                {
                    hasMoved = true;
                    GameObjectHasMoved?.Invoke(currentObject, new GameObjectEventArgs()
                    {
                        CollidingObject = boundary.Tag,
                        Position = new Position { X = pictureBox.Location.X, Y = pictureBox.Location.Y }

                    });
 
                    //NextPlayerTurn();
                }
            }
            if (!hasMoved)
            {
                MoveGameObject(currentObject, currentObject.Position, true);
            }

        }

        private void Movable_MouseMove(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            pictureBox.Top += e.Y - mouseClickPosition.Y;
            pictureBox.Left += e.X - mouseClickPosition.X;
        }

        private void PlayingSurface_KeyPress(object sender, KeyPressEventArgs e)
        {
            //todo add restart and menu calls
            if (e.KeyChar == (char)Keys.Escape)
            {
                using (var menu = new Menu())
                {
                    menu.StartPosition = FormStartPosition.CenterParent;
                    menu.ShowDialog();
                    switch (menu.Selection)
                    {
                        case "restart":
                            GameEngine.Instance.ReloadGame();
                            break;
                        case "desktop":
                            Application.Exit();
                            break;
                        case "menu":
                            this.Close();
                            break;

                    }
                }
            }
        }

        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            Rectangle ee = new Rectangle(0, 0, pictureBox.Width, pictureBox.Height);
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, ee);
            }
        }

        private void PlayingSurface_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveAllFormControls();
            GameHasBeenClosed?.Invoke(sender, e);
        }

        private void RemoveAllFormControls()
        {
            foreach (var control in formControls)
            {
                Controls.Remove(control);
                control.Dispose();
            }
            formControls.Clear();
            foreach (var control in formBoundaries)
            {
                Controls.Remove(control);
                control.Dispose();
            }
            formBoundaries.Clear();

        }

        private void moveTimer_Tick(object sender, EventArgs e)
        {

            //gameObject has been set with new location already
            var control = (Control)((Timer)sender).Tag;
            var gameObject = (GameObject)control.Tag;

            int moveXInterval = Math.Abs(gameObject.Position.X - control.Location.X) < 20 ? Math.Abs(gameObject.Position.X - control.Location.X) : 20;
            int moveYInterval = Math.Abs(gameObject.Position.Y - control.Location.Y) < 20 ? Math.Abs(gameObject.Position.Y - control.Location.Y) : 20;

            if (gameObject.Position.X > control.Location.X)
                control.Left += moveXInterval;
            else if (gameObject.Position.X < control.Location.X)
                control.Left -= moveXInterval;

            if (gameObject.Position.Y > control.Location.Y)
                control.Top += moveYInterval;
            else if (gameObject.Position.Y < control.Location.Y)
                control.Top -= moveYInterval;

            if (control.Location.X == gameObject.Position.X)
                if (control.Location.Y == gameObject.Position.Y)
                {
                    moveTimer.Stop();
                    isMoving = false;
                }

        }

        //Scaled images in Picture Box control seems to cause redraw issues
        //method is a workaround to resolve redraw lag
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        private void PlayingSurface_Shown(object sender, EventArgs e)
        {
            GameHasStarted?.Invoke(sender, e);
        }

        private void btnSwaps_Click(object sender, EventArgs e)
        {
            PlayingSurfaceManager.DisableSwaps();
            btnSwaps.BringToFront();
            btnSwaps.Visible = false;
            gameStarted = true;
        }
    }
}
