using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace InsectGenerator
{
    enum BugMoods
    {
        Waiting,
        Angry,
        Normal,
        Dead
    }


    class Bug : Sprite
    {
        public BugMoods mood = BugMoods.Normal;
        public bool Dead = false;

        private Random rand = new Random((int)DateTime.UtcNow.Ticks);

        float timeRemaining = 0.0f;
        float timeTotal = 0.3f;
        float TimePerUpdate = 2.00f;

        float steerTimer = 0f;
        float steerTime = 270f;

        public Bug(
           Vector2 location,
           Texture2D texture,
           Rectangle initialFrame,
           Vector2 velocity)
                : base(location, texture, initialFrame, velocity)
        {
            System.Threading.Thread.Sleep(1);
        }
        
        public override void Update(GameTime gameTime)
        {
            if (!Dead)
            {
                if (Location.Y > 471 && velocity.Y > 0)   
                     Velocity *= new Vector2(1, -1);

                if (Location.Y < -80 && velocity.Y < 0)
                    Velocity *= new Vector2(1, -1);

                steerTimer += (float)gameTime.ElapsedGameTime.Milliseconds;

                if (steerTimer > steerTime)
                {
                    steerTimer = rand.Next(0, (int)steerTime / 2);


                    Vector2 mpos = new Vector2(Location.X + 400 * (velocity.X > 0 ? 1 : -1), Location.Y + rand.Next(-300, 300));
                    Vector2 vel = mpos - Location;
                    vel.Normalize();
                    vel *= 100;
                    Rotation = (float)Math.Atan2(vel.Y, vel.X);

                    Velocity = vel;
                }

                if (mood == BugMoods.Waiting)
                {
                    this.velocity /= 4;
                }

            }
            



            base.Update(gameTime);
        }
        public void Change()
        {
            if (Dead) return;

            //this.Rotation = (float)rand.Next(0, 360) * MathHelper.Pi / 180f;
            this.frames[0] = new Rectangle(0, 128, 128, 128);
            this.Velocity *= new Vector2(0, 0);
            this.Dead = true;
            this.Location -= new Vector2(32, 32);
            
           
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            /*if (mood == BugMoods.Angry)
            {            

                this.TintColor = Color.Red;
                this.Velocity *= new Vector2(1.1f, 1f);

                if (Velocity.Length() > 150)
                {
                    this.velocity.Normalize();
                    this.velocity *= 150;
                }
            }
            else
            {
                this.TintColor = Color.White;
            }*/

            base.Draw(spriteBatch);
        }
    }
}
