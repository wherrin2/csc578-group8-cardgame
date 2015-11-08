﻿using System;
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
        //todo implement player turn logic with appropriate IDs
        //todo implement surface manager that orders layers

        public event EventHandler<Point> GameObjectHasMoved;

        private bool leftClicked;
        private Point mouseClickPosition;
        private Control.ControlCollection formControls;

        public PlayingSurface()
        {
            InitializeComponent();
            formControls = this.Controls;
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
            } else if (typeof(DrawableObject) == type)
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
        /// <param name="point">Location to move</param>
        /// <param name="animate">Animate transition to new location</param>
        public void MoveGameObject(GameObject gameObject, Point point, bool animate)
        {
            //move controls and animate with a timer if necessary
            List<Control> controls = FindControlsByGameObject(gameObject);
            foreach (var control in controls)
            {
                control.Location = point;
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
            
            formControls.Add(pictureBox);
        }

        private void CreateDrawableObject(DrawableObject drawable)
        {
            if (drawable.IsBackgroundImage)
            {
                this.BackgroundImage = drawable.IsFrontImage ? drawable.FrontImage : drawable.BackImage;
            }
            else
            {
                formControls.Add(CreatePictureBox(drawable));
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

            //Fire Event with new point
            GameObjectHasMoved?.Invoke((GameObject)pictureBox.Tag, pictureBox.Location);

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
