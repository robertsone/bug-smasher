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

namespace InsectGenerator
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Texture2D background, spritesheet;
        Random rand = new Random(System.Environment.TickCount);

        List<Sprite> bugs = new List<Sprite>();


        int bugNum = 100;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            

            
        }

        
        protected override void Initialize()
        {
            

            base.Initialize();
        }


        protected override void LoadContent()
        {

            background = Content.Load<Texture2D>("background");
            spritesheet = Content.Load<Texture2D>("spritesheet");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            for (int i = 0; i < bugNum; i++)
            {
                int bugX = rand.Next(0, 3);
                int bugY = rand.Next(0, 2);
                Sprite bug = new Sprite(new Vector2 (rand.Next(-400,-50),rand.Next(50,400)), spritesheet, new Rectangle(64*bugX, 64*bugY, 64, 64), new Vector2(rand.Next(40,150),rand.Next(-40,40)));
                bugs.Add(bug);
            }
        }

        public void moveBugs()
        {
            for (int i = 0; i < bugs.Count; i++)
            {



                //todo:collision
                for (int j = 0; j < bugs.Count; j++)
                {
                    if (bugs[i].IsBoxColliding(bugs[j].BoundingBoxRect))
                    {

                    }
                }
            }

        }

        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            moveBugs();

            for (int i = 0; i < bugs.Count; i++)
            {
                bugs[i].Update(gameTime);
            }

            base.Update(gameTime);
        }


        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();


            for (int i = 0; i < bugs.Count; i++)
            {
                bugs[i].Draw(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
