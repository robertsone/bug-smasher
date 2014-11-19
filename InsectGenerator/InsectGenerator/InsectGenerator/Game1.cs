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
        SpriteFont Font1;
        Vector2 FontPos;
        Texture2D background, spritesheet, pause, button,moneybag,shopImg,button2;
        Random rand = new Random(System.Environment.TickCount);
        float timeRemaining = 0.0f;
        float TimePerUpdate = 2.00f;
        List<Bug> bugs = new List<Bug>();
        int bugNum = 100;
        bool Canclick = true;
        bool leftMouseClicked=false;
        int powerups =0;
        List<Sprite> bars = new List<Sprite>();
        bool paused = false;
        Color color = Color.Transparent;
        bool inshop = false;
        Sprite end;
        int poweruptype = 1;
        Color textcolor = Color.Pink;
        int colortimer = 0;
        int money = 0;
        int stardistance = 500;
        Song song;
        int tospawn = 0;
        SoundEffect splat, explode, levelup, scream,cash;
        bool powerupin;
        int timer = 0;
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
            Font1 = Content.Load<SpriteFont>("SpriteFont1");
            FontPos = new Vector2(600, 210);
            song = Content.Load<Song>("Elevator_Music");
            splat = Content.Load<SoundEffect>("Splat_sound_effect");
            explode = Content.Load<SoundEffect>("Grenade_sound_effect_2_mp3cut");
            levelup = Content.Load<SoundEffect>("Party_Horn_Sound_Effect_mp3cut");
            scream = Content.Load<SoundEffect>("Screams_Sound_Effect_mp3cut");
            cash = Content.Load<SoundEffect>("Cash_register_sound_effect_mp3cut");
            button2 = Content.Load<Texture2D>("buttons");
            background = Content.Load<Texture2D>("background");
            spritesheet = Content.Load<Texture2D>("spritesheet");
            pause = Content.Load<Texture2D>("Save-Toshi-Pause-menu");
            button = Content.Load<Texture2D>("pause");
            moneybag = Content.Load<Texture2D>("shop");
            shopImg = Content.Load<Texture2D>("shopImg");
            spriteBatch = new SpriteBatch(GraphicsDevice);
            powerupin = false;
            

            EffectManager.Initialize(graphics, Content);
            EffectManager.LoadContent();

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(song);

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
            }

            

            if (paused == false && inshop==false)
            {
                if (ms.LeftButton != ButtonState.Pressed)
                {
                    Canclick = true;


                }

                if (leftMouseClicked)
                {
                    if (powerups >= 1 && poweruptype==1 && ms.Y>=100 && !powerupin)
                    {
                        explode.Play();
                        powerups--;
                        EffectManager.Effect("BasicExplosionWithHalo").Trigger(new Vector2(ms.X + 16, ms.Y + 16)); EffectManager.Effect("BasicExplosionWithHalo").Trigger(new Vector2(ms.X + 16, ms.Y + 16));
                        EffectManager.Effect("BasicExplosionWithHalo").Trigger(new Vector2(ms.X + 16, ms.Y + 16)); EffectManager.Effect("BasicExplosionWithHalo").Trigger(new Vector2(ms.X + 16, ms.Y + 16));
                        EffectManager.Effect("BasicExplosionWithHalo").Trigger(new Vector2(ms.X + 16, ms.Y + 16)); EffectManager.Effect("BasicExplosionWithHalo").Trigger(new Vector2(ms.X + 16, ms.Y + 16));

                    }
                    if (powerups >= 1 && poweruptype == 2 && ms.Y >= 100 && !powerupin)
                    {
                        stardistance = 1000;
                        powerups--;
                        powerupin = true;
                        tospawn = 0;

                    }
                    

                }
                if (powerupin)
                {
                    stardistance -= 15;
                    for (int i = 0; i < bugs.Count; i++)
                    {
                        if (stardistance<=0)
                        {
                            splat.Play();
                            bugs[i].Change();
                            tospawn += 1;
                            
                        }

                        EffectManager.Effect("StarTrail").Trigger(new Vector2(bugs[i].Center.X, bugs[i].Center.Y - stardistance));
                        EffectManager.Effect("MagicTrail").Trigger(new Vector2(bugs[i].Center.X, bugs[i].Center.Y - stardistance));
                        
                    }
                    if (stardistance <= 0)
                    {
                        powerupin = false;
                    }
                    if (tospawn >= 0)
                    {
                        for (int i = 0; i < tospawn; i++)
                        {
                            SpawnBug(new Vector2(rand.Next(-960, -64), rand.Next(20, 400)));
                        }
                    }
                }
                for (int i = 0; i < bugs.Count; i++)
                {

                    bugs[i].Update(gameTime);
                    if (bugs[i].mood != BugMoods.Lady)
                    {
                        bugs[i].mood = BugMoods.Normal;
                    }
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
                    if (powerups >= 1 && poweruptype==1 &&!powerupin)
                    {
                        mouserectangle = new Rectangle(ms.X - 64, ms.Y - 64, 128, 128);
                    }
                    if (bugs[i].IsBoxColliding(mouserectangle) && leftMouseClicked == true && !bugs[i].Dead)
                    {
                        if (bugs[i].mood == BugMoods.Lady)
                        {
                            scream.Play();
                            List<Sprite> it = new List<Sprite>();
                            bars = it;
                            EffectManager.Effect("PulseTracker").Trigger(new Vector2(bugs[i].Center.X, bugs[i].Center.Y));

                        }
                        else
                            EffectManager.Effect("Ship Cannon Fire").Trigger(new Vector2(bugs[i].Center.X, bugs[i].Center.Y));



                            splat.Play();
                            bugs[i].Change();
                            if (bugs[i].mood == BugMoods.Lady)
                            {
                                money=-10;
                            }
                            money += 10;
                            

                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            if (bars.Count > 0)
                            {
                                bars.Add(new Sprite(new Vector2(bars[bars.Count - 1].BoundingBoxRect.X + 7, 19), spritesheet, new Rectangle(30, 382, 10, 64), new Vector2(0, 0)));
                                if (bars[bars.Count - 1].BoundingBoxRect.X >= 577)
                                {
                                    List<Sprite> it = new List<Sprite>();
                                    bars = it;
                                    powerups += 10;
                                    levelup.Play();
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

                        if (dist < 50 && bugs[i].Center.X < bugs[j].Center.X && bugs[i].mood!=BugMoods.Lady)
                        {

                            bugs[i].wait();
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
                        if (rand.Next(1, 10) == 1)
                        {
                            bugs[bugs.Count - 1].makelady();

                            
                        }
                        if (ms.RightButton == ButtonState.Pressed)
                        {
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                            SpawnBug(new Vector2(rand.Next(-160, -64), rand.Next(20, 400)));
                        }

                    }

                    if (toremove != -1)
                        bugs.Remove(bugs[toremove]);

                }


                int num = 0;
                for (int i = 0; i < bugs.Count; i++)
                {

                    if (bugs[i].Dead == true)
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
                new Sprite(new Vector2(213, 19), spritesheet, new Rectangle(30, 382, 10, 64), new Vector2(0, 0));


                EffectManager.Update(gameTime);
                if (ms.X > 726 && ms.X < 800 && ms.Y > 10 && ms.Y < 74 && leftMouseClicked)
                    paused = true;
                if (ms.X > 656 && ms.X < 720 && ms.Y > 10 && ms.Y < 74 && leftMouseClicked)
                    inshop = true;
                if (ms.X > 10 && ms.X < 76 && ms.Y > 10 && ms.Y < 74 && leftMouseClicked)
                    poweruptype = 1;
                if (ms.X > 80 && ms.X < 146 && ms.Y > 10 && ms.Y < 74 && leftMouseClicked)
                    poweruptype = 2;
                base.Update(gameTime);
            }
            if (powerups >= 1 && inshop == false && paused == false && poweruptype==1)
            {
                EffectManager.Effect("BasicExplosion").Trigger(new Vector2(ms.X - 10, ms.Y));
                EffectManager.Effect("ShieldsUp").Trigger(new Vector2(ms.X - 10, ms.Y));

            }
            if (powerups >= 1 && inshop == false && paused == false && poweruptype == 2)
            {
                EffectManager.Effect("ShieldBounce").Trigger(new Vector2(ms.X - 10, ms.Y));
                EffectManager.Effect("StarTrail").Trigger(new Vector2(ms.X - 10, ms.Y));

            }
            if (inshop==false && paused==true)
            {
                if (leftMouseClicked)
                {

                    if (ms.X > 249 && ms.X < 549 && ms.Y > 134 && ms.Y < 194)
                        paused = false;
                }
                EffectManager.Effect("MagicTrail").Trigger(new Vector2(20 + rand.Next(0, 170), 300 + rand.Next(0, 150)));
                EffectManager.Update(gameTime);
                
            }
            if (inshop == true && paused == false)
            {
                if (leftMouseClicked)
                {

                    if (ms.X > 311 && ms.X < 420 && ms.Y > 123 && ms.Y < 180)
                        inshop = false;
                    if (ms.X > 301 && ms.X < 728 && ms.Y > 196 && ms.Y < 247)
                    {
                        if (money >= 100)
                        {
                            money -= 100;
                            powerups += 2;
                            cash.Play();
                        }
                        else
                        {
                            textcolor = Color.Red;
                            colortimer = 100;
                        }

                    }
                }
            }
            
        }
        
        public Color randomcolor()
        {
            int num = rand.Next(1, 11);
            if (num==1) return Color.Red;
            if (num==2) return Color.Black;
            if (num==3) return Color.Black;
            if (num==4) return Color.DarkSeaGreen;
            if (num==5) return Color.Wheat;
            if (num==6) return Color.Beige;
            if (num==7) return Color.Maroon;
            if (num==8) return Color.Lime;
            if (num==9) return Color.Salmon;
            if (num == 10) return Color.SaddleBrown;
            return Color.Tomato;
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
            if (paused==false && inshop==false)
                spriteBatch.Draw(background, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White); 
            if (paused==true)
                spriteBatch.Draw(pause, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
            if (inshop == true)
            {
                spriteBatch.Draw(shopImg, new Rectangle(0, 0, this.Window.ClientBounds.Width, this.Window.ClientBounds.Height), Color.White);
                Vector2 FontOrigin = Font1.MeasureString(Convert.ToString(money)) / 2;
                spriteBatch.DrawString(Font1, Convert.ToString(money), FontPos, textcolor, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                
            }
            if (!paused && !inshop)
            {
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
            }
            MouseState ms = Mouse.GetState();
            Rectangle mouserectangle = new Rectangle(ms.X, ms.Y, 1, 1);

            colortimer -= 1;
            if (colortimer <= 0)
            {
                textcolor = Color.Pink;
            }

            
            

            if (paused == false && inshop==false)
            {
                (new Sprite(new Vector2(163, 00), spritesheet, new Rectangle(0, 301, 512, 80), new Vector2(0, 0))).Draw(spriteBatch);
                (new Sprite(new Vector2(720, 10), button, new Rectangle(0, 0, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
                (new Sprite(new Vector2(650, 10), moneybag, new Rectangle(0, 0, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
                (new Sprite(new Vector2(10, 10), button2, new Rectangle(0, 0, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
                (new Sprite(new Vector2(80, 10), button2, new Rectangle(64, 0, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
                Sprite start = (new Sprite(new Vector2(193, 19), spritesheet, new Rectangle(0, 382, 30, 50), new Vector2(0, 0)));
                start.Draw(spriteBatch);
                for (int i = 0; i < bars.Count; i++)
                {
              
                    bars[i].Draw(spriteBatch);
                }

                if (bars.Count == 0)
                    end =new Sprite(new Vector2(start.BoundingBoxRect.X + start.BoundingBoxRect.Width - 10, start.BoundingBoxRect.Y), spritesheet, new Rectangle(60, 382, 64, 64), new Vector2(0, 0));
                else
                    end = new Sprite(new Vector2(bars[bars.Count - 1].BoundingBoxRect.X + 6, start.BoundingBoxRect.Y), spritesheet, new Rectangle(60, 382, 64, 64), new Vector2(0, 0));
                end.Draw(spriteBatch);
                (new Sprite(new Vector2(500, 20), spritesheet, new Rectangle(356, 64, 70, 70), new Vector2(0, 0))).Draw(spriteBatch);
                string it = "dont kill the lady bugs!";
                timer--;
                if (timer >= 0)
                {
                    Vector2 FontOrigin = Font1.MeasureString(Convert.ToString(it)) / 2;
                    spriteBatch.DrawString(Font1, Convert.ToString(it), new Vector2(400, 450), Color.White, 0, FontOrigin, 1.0f, SpriteEffects.None, 0.5f);
                    spriteBatch.Draw(button, new Rectangle(726, 10, 64, 64), Color.White);
                }
            }
            if (powerups>=1 && inshop==false && paused==false && poweruptype==1)
            {
                
            }
            else
                (new Sprite(new Vector2(mouserectangle.X - 32, mouserectangle.Y - 32), spritesheet, new Rectangle(143, 64 * 3, 64, 64), new Vector2(0, 0))).Draw(spriteBatch);
            spriteBatch.End();
            EffectManager.Draw();

            base.Draw(gameTime);
        }
    }
}
