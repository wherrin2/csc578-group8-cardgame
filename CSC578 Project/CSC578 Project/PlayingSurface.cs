using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CSC578_Project
{
    public partial class PlayingSurface : Form
    {
        //rough playing surface class
        //todo implement asset manager
        //todo check boundaries
        //todo implement player turn logic with appropriate IDs
        //todo implement surface manager that orders layers

        private bool leftClicked;
        private Point mouseClickPosition;

        public PlayingSurface()
        {
            InitializeComponent();
        }
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
            } else if (typeof(DrawableObject) == type)
            {
                CreateDrawableObject((DrawableObject)gameObject);
            }

        }

        private void CreateBoundaryObject(BoundaryObject boundary)
        {
           if (boundary.ShowBoundaryOutline)
            {
                DrawBoundary(boundary);
            }
        }

        private void CreateMovableObject(MovableObject movable)
        {
            var pictureBox = CreatePictureBox(movable);
            pictureBox.MouseDown += Movable_MouseDown;
            pictureBox.MouseUp += Movable_MouseUp;
            pictureBox.Tag = movable;
            
            this.Controls.Add(pictureBox);
        }
        private void CreateDrawableObject(DrawableObject drawable)
        {
            var pictureBox = CreatePictureBox(drawable);

            if (drawable.IsBackgroundImage)
            {
                this.BackgroundImage = drawable.IsFrontImage ? drawable.FrontImage : drawable.BackImage;
            }
            else
            {
                this.Controls.Add(pictureBox);
            }
           
        }
  
        private void DrawBoundary(BoundaryObject boundary)
        {
            Graphics graphics = this.CreateGraphics();
            var rectangle = new Rectangle
            {
                Height = boundary.Height,
                Width = boundary.Width,
                Location = boundary.Position

            };

            graphics.DrawRectangle(Pens.Black, rectangle);
            graphics.Dispose();
        }

        private PictureBox CreatePictureBox(DrawableObject drawable)
        {
            var pictureBox = new PictureBox
            {
                Height = drawable.Height,
                Width = drawable.Width,
                Location = drawable.Position,
                BackColor = Color.Transparent,
                SizeMode =  PictureBoxSizeMode.StretchImage,
                
                Image = drawable.IsFrontImage ? drawable.FrontImage : drawable.BackImage
            };
            return pictureBox;
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
                pictureBox.MouseMove += Movable_MouseMove;
                mouseClickPosition = e.Location;
            }
            
        }
        private void Movable_MouseUp(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var movable = (MovableObject)pictureBox.Tag;
            movable.IsSelected = false;
            leftClicked = false;
            pictureBox.MouseMove -= Movable_MouseMove;

            //should try to check if movement is allowed in boundaries
            movable.Position = pictureBox.Location;

        }
        private void Movable_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftClicked)
            {
                var pictureBox = (PictureBox)sender;
                pictureBox.Top += e.Y - mouseClickPosition.Y;
                pictureBox.Left += e.X - mouseClickPosition.Y;
               ;
            }
        }
    }
}
