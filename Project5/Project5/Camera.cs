﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Project5
{
    public class Camera : Microsoft.Xna.Framework.GameComponent
    {
        // Camera matrices
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }
        public Matrix world;

        // Camera vectors
        public Vector3 cameraPosition { get; protected set; }
        Vector3 cameraTarget;
        Vector3 cameraDirection;
        Vector3 cameraUp;

        float speed = 3;
        bool toggle = false;
        KeyboardState oldKeyState;
        MouseState prevMouseState;

        private float angle = 0f;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Build camera view matrix
            cameraPosition = pos;
            cameraTarget = target;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                (float)Game.Window.ClientBounds.Width /
                (float)Game.Window.ClientBounds.Height,
                1, 3000);
        }

        public override void Initialize()
        {
            // TODO: Add your initialization coe here

            // Set old keyboard position
            oldKeyState = Keyboard.GetState();

            // Set moues position and do initial get state
            Mouse.SetPosition(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height / 2);
            prevMouseState = Mouse.GetState();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here


            if (Keyboard.GetState().IsKeyDown(Keys.T) && oldKeyState.IsKeyUp(Keys.T))
            {
                toggle = !toggle;

                if (toggle == true)
                {
                    cameraTarget = ((Game1)Game).modelManager.world.Translation;

                    //cameraPosition = Vector3.Transform(cameraPosition - modelPos2, Matrix.CreateFromAxisAngle(cameraUp, 3 * angle)) + modelPos2;

                    //view = Matrix.CreateLookAt(cameraPosition, modelPos2, cameraUp);
                }
                else
                {
                    cameraDirection = cameraTarget - cameraPosition;
                }
            }

            

            // Move forward/backward
            /*if (Keyboard.GetState().IsKeyDown(Keys.W))
                cameraPosition += cameraDirection * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                cameraPosition -= cameraDirection * speed;

            // Move side to side
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                //cameraPosition += Vector3.Cross(cameraUp, cameraDirection) * speed;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                cameraPosition -= Vector3.Cross(cameraUp, cameraDirection) * speed;*/

            /*// Roll rotation
            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                cameraUp = Vector3.Transform(cameraUp,
                    Matrix.CreateFromAxisAngle(cameraDirection,
                    MathHelper.PiOver4 / 45));
            }
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                cameraUp = Vector3.Transform(cameraUp,
                    Matrix.CreateFromAxisAngle(cameraDirection,
                    -MathHelper.PiOver4 / 45));
            }*/

            //if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            //{
                // Yaw rotation
                /*cameraDirection = Vector3.Transform(cameraDirection,
                    Matrix.CreateFromAxisAngle(cameraUp, (-MathHelper.PiOver4 / 150) *
                    (Mouse.GetState().X - prevMouseState.X)));*/

                //Vector2 modelPos = ((Game1)Game).getTargetPos();

            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                if (toggle == true)
                {
                    cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(Vector3.Up, .05f)) + cameraTarget;
                }
                else
                {
                    cameraDirection = Vector3.Transform(cameraDirection,
                        Matrix.CreateFromAxisAngle(cameraUp, (-MathHelper.PiOver4 / 45)));
                }
            }

            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                if (toggle == true)
                {
                    cameraPosition = Vector3.Transform(cameraPosition - cameraTarget, Matrix.CreateFromAxisAngle(Vector3.Up, -.05f)) + cameraTarget;
                }
                else
                {
                    cameraDirection = Vector3.Transform(cameraDirection,
                        Matrix.CreateFromAxisAngle(cameraUp, (MathHelper.PiOver4 / 45)));
                }
            }
                

                /*// Pitch rotation
                cameraDirection = Vector3.Transform(cameraDirection,
                    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
                    (MathHelper.PiOver4 / 100) *
                    (Mouse.GetState().Y - prevMouseState.Y)));

                cameraUp = Vector3.Transform(cameraUp,
                    Matrix.CreateFromAxisAngle(Vector3.Cross(cameraUp, cameraDirection),
                    (MathHelper.PiOver4 / 100) *
                    (Mouse.GetState().Y - prevMouseState.Y)));*/
            //}

            // Reset previous keyboard state 
            oldKeyState = Keyboard.GetState();
            // Reset prevMouseState
            prevMouseState = Mouse.GetState();

            // Recreate the camera view matrix
            CreateLookAt();

            if (toggle == true)
            {
                view = Matrix.CreateLookAt(cameraPosition,
                    cameraTarget, cameraUp);
            }
            else
            {
                CreateLookAt();
            }

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition,
                cameraPosition + cameraDirection, cameraUp);
        }
        
    }
}
