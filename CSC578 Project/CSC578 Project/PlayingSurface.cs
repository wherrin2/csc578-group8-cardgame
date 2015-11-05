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
        private bool leftClicked;
        private Point mouseClickPosition;

        public PlayingSurface()
        {
            InitializeComponent();
        }
        public void AddGameObject(GameObject gameObject)
        {
            if (typeof(MoveableObject) == gameObject.GetType())
            {
                CreateMoveableObject((MoveableObject)gameObject);
            }
            else if (typeof(BoundaryObject) == gameObject.GetType())
            {
                CreateBoundaryObject((BoundaryObject)gameObject);
            }

        }

        private void CreateBoundaryObject(BoundaryObject boundary)
        {
           if (boundary.ShowBoundaryOutline)
            {
                DrawBoundary(boundary);
            }
        }

        private void CreateMoveableObject(MoveableObject moveable)
        {
            var pictureBox = new PictureBox
            {
                Image = moveable.IsFrontImage ? moveable.FrontImage : moveable.BackImage,
                Height = moveable.Height,
                Width = moveable.Height,
                Location = moveable.Position
            };
   
            pictureBox.MouseDown += Moveable_MouseDown;
            pictureBox.MouseUp += Moveable_MouseUp;
            pictureBox.Tag = moveable;
            this.Controls.Add(pictureBox);
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

        private void Moveable_MouseDown(object sender, MouseEventArgs e)
        {
            //fix ID in isSelectable - should be current player's ID
            var pictureBox = (PictureBox)sender;
            var moveable = (MoveableObject)pictureBox.Tag;
            if (moveable.IsSelectable(-1))
            {
                pictureBox.BringToFront();
                moveable.IsSelected = true;
                
                leftClicked = e.Button == MouseButtons.Left;
                pictureBox.MouseMove += Moveable_MouseMove;
                mouseClickPosition = e.Location;
            }
            
        }
        private void Moveable_MouseUp(object sender, MouseEventArgs e)
        {
            var pictureBox = (PictureBox)sender;
            var moveable = (MoveableObject)pictureBox.Tag;
            moveable.IsSelected = false;
            leftClicked = false;
            pictureBox.MouseMove -= Moveable_MouseMove;

            //should try to check if movement is allowed in boundaries
            moveable.Position = pictureBox.Location;

        }
        private void Moveable_MouseMove(object sender, MouseEventArgs e)
        {
            if (leftClicked)
            {
                var pictureBox = (PictureBox)sender;
                pictureBox.Top += e.Y - mouseClickPosition.Y;
                pictureBox.Left += e.X - mouseClickPosition.Y;
            }
        }
    }
}
