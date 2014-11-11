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
        float timeRemaining = 0.0f;
        float timeTotal = 0.3f;
        float TimePerUpdate = 2.00f;
        List<Bug> bugs = new List<Bug>();
        int bugNum = 100;
        bool Canclick = true;
        bool leftMouseClicked=false;
        int powerups = 0;
        List<Sprite> bars = new List<Sprite>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);

            Content.RootDirectory = "Content";

            

            
        }

        
        protected override void Initialize()
        {
            

            base.Initialize();
        }
        public void SpawnBug(Vector2 location)
        {
            int bugX = rand.Next(0, 3);
            int bugY = rand.Next(0, 2);
            int X = rand.Next(40, 150);
            int Y = rand.Next(-40, 40);
            if (Y == 0)
            {
                Y = 10;
            }

            Vector2 velocity = new Vector2(X, Y);
            velocity.Normalize();
            velocity *= 200;

            Bug bug = new Bug(location, spritesheet, new Rectangle(64 * bugX, 64 * bugY, 64, 64), velocity);
            bugs.Add(bug);
        }

        protected override void LoadContent()
        {

            background = Content.Load<Texture2D>("background");
            spritesheet = Content.Load<Texture2D>("spritesheet");
            spriteBatch = new SpriteBatch(GraphicsDevice);

            EffectManager.Initialize(graphics, Content);
            EffectManager.LoadContent();

            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    SpawnBug(new Vector2(-50 - (x*100) + rand.Next(-60, 60), 100 + y * 45 + rand.Next(-10, 10)));
                }
            }


            for (int i = 0; i < bugNum; i++)
            {
                
            }
            
        }



        protected override void UnloadContent()
        {

        }


        protected override void Update(GameTime gameTime)
        {
            
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            MouseState ms = Mouse.GetState();
            IsMouseVisible = false;
            leftMouseClicked = false;
            //Canclick = true;

            if (ms.LeftButton != ButtonState.Pressed)
            {
                Canclick = true;
                

            }
            if (ms.LeftButton == ButtonState.Pressed && Canclick == true)
            {
                leftMouseClicked = true;
                Canclick = false;
                if (powerups >= 1)
                {
                    powerups--;
                    EffectManager.Effect("BasicExplosionWithTrails2").Trigger(new Vector2(ms.X + 16, ms.Y + 16));
                }
                
            }

            if (powerups >= 1)
            {
                EffectManager.Effect("StarTrail").Trigger(new Vector2(rand.Next(100,700), rand.Next(0,30)));
                
            }
           
            for (int i = 0; i < bugs.Count; i++)
            {
                bugs[i].Update(gameTime);
                bugs[i].mood = BugMoods.Normal;
                if (bugs[i].Location.X > this.Window.ClientBounds.Width && bugs[i].Velocity.X > 0) 
                {
                    bugs[i].Velocity *= new Vector2(-1, 1);
                    bugs[i].FlipHorizontal = true; 
                }

                if (bugs[i].Location.X > -70 && bugs[i].Velocity.X < 0)
                {
                    bugs[i].Velocity *= new Vector2(-1, 1);
                    //bugs[i].FlipHorizontal = true; 
                }

                Rectangle mouserectangle = new Rectangle(ms.X, ms.Y, 1, 1);
                if (powerups >= 1)
                {
                    mouserectangle = new Rectangle(ms.X - 64, ms.Y - 64, 128, 128);
                }
                if(bugs[i].IsBoxColliding(mouserectangle) && leftMouseClicked == true && !bugs[i].Dead)
                {
                    bugs[i].Change();
                    EffectManager.Effect("Ship Cannon Fire").Trigger(new Vector2(bugs[i].Center.X+32, bugs[i].Center.Y+32));

                    SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                    if (bars.Count > 0)
                    {
                        bars.Add(new Sprite(new Vector2(bars[bars.Count - 1].BoundingBoxRect.X + 7, 19), spritesheet, new Rectangle(30, 382, 10, 64), new Vector2(0, 0)));
                        if (bars[bars.Count - 1].BoundingBoxRect.X >= 577)
                        {
                            List<Sprite> it = new List<Sprite>();
                            bars = it;
                            powerups += 10;
                        }
                        
                    }
                    else
                        bars.Add(new Sprite(new Vector2(213, 19), spritesheet, new Rectangle(30, 382, 10, 64), new Vector2(0, 0)));
                        
                }

                int toremove = -1;
                for (int j = 0; j < bugs.Count; j++)
                {
                   
                    if (bugs[i].IsBoxColliding(bugs[j].BoundingBoxRect))
                    {
                        
                    }
                }

                for (int j = 0; j < bugs.Count; j++)
                {
                    if (i == j || bugs[j].Dead || bugs[i].Dead)
                        continue;

                    float dist = Vector2.Distance(bugs[i].Center, bugs[j].Center);

                    if (dist < 50 && bugs[i].Center.X < bugs[j].Center.X)
                    {
                        bugs[i].mood = BugMoods.Waiting;
                    }
                   
                }
                /*if (bugs[j].Velocity.Y > 0 && bugs[i].Velocity.Y>0)
                        {
                            bugs[i].Velocity *= new Vector2(1, -1);
                        }
                        else if (bugs[j].Velocity.Y < 0 && bugs[i].Velocity.Y < 0)
                        {
                            bugs[i].Velocity *= new Vector2(1, -1);
                        }*/
                

                if (bugs[i].Location.X > this.Window.ClientBounds.Width + 100)
                {
                    toremove = i;
                    SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                    
                }

                if (toremove != -1)
                    bugs.Remove(bugs[toremove]);
     
            }


            int num = 0;
            for (int i = 0; i < bugs.Count; i++)
            {
                
                if (bugs[i].Dead==true)
                {
                    num++;
                    if (num >= 10)
                    {
                        for (int j = 0; j < bugs.Count; j++)
                        {
                            if (bugs[j].Dead)
                            {
                                bugs[j].TintColor *= 0.999f;
                                if (bugs[j].TintColor.A < 5f)
                                    bugs.Remove(bugs[j]);

                                break;
                            }

                        }
                    }
                }
                
            }


            EffectManager.Update(gameTime);

            base.Update(gameTime);
        }

        public void Method(GameTime gameTime)
        {
        //
        
         timeRemaining = MathHelper.Max(0, timeRemaining -
        (float)gameTime.ElapsedGameTime.TotalSeconds);
        if (timeRemaining == 0.0f)
         {
            timeRemaining = TimePerUpdate;
         }
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            spriteBatch.Draw(background, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White); 


            for (int i = 0; i < bugs.Count; i++)
            {
                if (bugs[i].Dead)
                    bugs[i].Draw(spriteBatch);
            }

            for (int i = 0; i < bugs.Count; i++)
            {
                if (!bugs[i].Dead)
                    bugs[i].Draw(spriteBatch);
            }
            MouseState ms = Mouse.GetState();
            Rectangle mouserectangle = new Rectangle(ms.X, ms.Y, 1, 1);



            (new Sprite(new Vector2(163, 00), spritesheet, new Rectangle(0, 301, 512, 80), new Vector2(0, 0))).Draw(spriteBatch);

            Sprite start = (new Sprite(new Vector2(193, 19), spritesheet, new Rectangle(0, 382, 30, 50), new Vector2(0, 0)));
            start.Draw(spriteBatch);

            for (int i = 0; i < bars.Count; i++)
            {
                bars[i].Draw(spriteBatch);
            }

            if (bars.Count==0) 
                (new Sprite(new Vector2(start.BoundingBoxRect.X+start.BoundingBoxRect.Width-10, start.BoundingBoxRect.Y), spritesheet, new Rectangle(60, 382, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
            else
                (new Sprite(new Vector2(bars[bars.Count-1].BoundingBoxRect.X + 6 , start.BoundingBoxRect.Y), spritesheet, new Rectangle(60, 382, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
            
            (new Sprite(new Vector2(mouserectangle.X - 32, mouserectangle.Y - 32), spritesheet, new Rectangle(143, 64 * 3, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
            spriteBatch.End();

            EffectManager.Draw();

            base.Draw(gameTime);
        }
    }
}
