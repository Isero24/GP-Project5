using System;
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
    /// <summary>
    /// This is a game component that implements IUpdateable.
    /// </summary>
    public class ModelManager : Microsoft.Xna.Framework.DrawableGameComponent
    {
        public Vector2 targetPos;
        public Vector3 center;
        public Matrix world;

        List<BasicModel> models = new List<BasicModel>();
        Texture2D targetTexture;
        SpriteBatch spriteBatch;
        KeyboardState oldKeyState;
        float maxdistance;
        bool toggle;
        
        public ModelManager(Game game)
            : base(game)
        {
            // TODO: Construct any child components here
        }

        /// <summary>
        /// Allows the game component to perform any initialization it needs to before starting
        /// to run.  This is where it can query for any required services and load content.
        /// </summary>
        public override void Initialize()
        {
            // TODO: Add your initialization code here

            oldKeyState = Keyboard.GetState();
            toggle = false;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // Load the target sprite texture
            targetTexture = Game.Content.Load<Texture2D>("Crosshairs");

            models.Add(new BasicModel(
                Game.Content.Load<Model>("Models/spaceship")));

            base.LoadContent();
        }

        /// <summary>
        /// Allows the game component to update itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            // TODO: Add your update code here            

            // Loop through all models and call Update
            for (int i = 0; i < models.Count; ++i)
            {
                models[i].Update();

                BoundingSphere totalbounds = models[i].modelBoundSphere();
                center = modelCenterPos(totalbounds);
                targetPos.X = center.X;
                targetPos.Y = center.Y;
                ((Game1)Game).camera.world = models[i].GetWorld();


                BoundingBox extents = BoundingBox.CreateFromSphere(totalbounds);
                maxdistance = 0;
                float distance;
                Vector3 screencorner;
                foreach (Vector3 corner in extents.GetCorners())
                {
                    screencorner = GraphicsDevice.Viewport.Project(corner,
                        ((Game1)Game).camera.projection, 
                        ((Game1)Game).camera.view, Matrix.Identity);
                        distance = Vector3.Distance(screencorner, center);

                    if (distance > maxdistance)
                        maxdistance = distance;
                }

                
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            // TODO: Add your drawing code here

            // Loop through and draw each model
            foreach (BasicModel bm in models)
            {
                bm.Draw(((Game1)Game).camera);
            }

            if (Keyboard.GetState().IsKeyDown(Keys.T) && oldKeyState.IsKeyUp(Keys.T))
            {
                toggle = !toggle;
            }

            if (toggle == true)
            {
                spriteBatch.Begin();
                targetPos.X -= maxdistance/8;
                targetPos.Y -= maxdistance/5;
                spriteBatch.Draw(
                    targetTexture,
                    targetPos,
                    new Rectangle(0, 0, 256, 256),
                    Color.White, 
                    0, 
                    Vector2.Zero,
                    .5f, 
                    SpriteEffects.None, 
                    0);
                spriteBatch.End();
            }
            oldKeyState = Keyboard.GetState();

            base.Draw(gameTime);
        }

        public Vector3 modelCenterPos(BoundingSphere totalbounds)
        {

            return GraphicsDevice.Viewport.Project(totalbounds.Center,
                ((Game1)Game).camera.projection,
                ((Game1)Game).camera.view, Matrix.Identity);
        }
    }
}
