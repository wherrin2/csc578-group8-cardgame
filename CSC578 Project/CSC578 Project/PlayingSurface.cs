using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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

        private bool leftClicked;
        private Point mouseClickPosition;
        private List<Control> formControls = new List<Control>();
        private List<Control> formBoundaries = new List<Control>(); 
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
                if (animate)
                {
                    moveTimer.Tag = control;
                    moveTimer.Start();
                }
                else
                    control.Location = new Point(position.X, position.Y);
            }
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

        private void Movable_MouseDown(object sender, MouseEventArgs e)
        {
            //fix ID in isSelectable - should be current player's ID
            var pictureBox = (PictureBox)sender;
            var movable = (MovableObject)pictureBox.Tag;
            if (movable.IsSelectable(-1))
            {
                pictureBox.BringToFront();
                movable.IsSelected = true;
                
                leftClicked = e.Button == MouseButtons.Left;
                if (leftClicked)
                {
                    pictureBox.MouseMove += Movable_MouseMove;
                }
                mouseClickPosition = e.Location;
            }
            
        }

        private void Movable_MouseUp(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var movable = (MovableObject)pictureBox.Tag;
            leftClicked = false;
            pictureBox.MouseMove -= Movable_MouseMove;
            movable.IsSelected = false;

            
            GameObject currentObject = (GameObject) pictureBox.Tag;
            var hasMoved = false;
            foreach (var boundary in formBoundaries)
            {
                if (boundary.Bounds.IntersectsWith(pictureBox.Bounds))
                {
                    hasMoved = true;
                    GameObjectHasMoved?.Invoke(currentObject, new GameObjectEventArgs()
                    {
                        CollidingObject = boundary.Tag,
                        Position = new Position { X = pictureBox.Location.X, Y = pictureBox.Location.Y}
                        
                    });
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
            var pictureBox = (PictureBox) sender;
            Rectangle ee = new Rectangle(0, 0, pictureBox.Width, pictureBox.Height);
            using (Pen pen = new Pen(Color.Red, 2))
            {
                e.Graphics.DrawRectangle(pen, ee);
            }
        }

        private void PlayingSurface_FormClosing(object sender, FormClosingEventArgs e)
        {
            RemoveAllFormControls();
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
            var control = (Control)((Timer) sender).Tag;
            var gameObject = (GameObject) control.Tag;

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
                    moveTimer.Stop();

        }
    }
}
