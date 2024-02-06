using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Diagnostics;
using System;

namespace LukeWaltersAssignment1
{
    public class Game1 : Game
    {
        private Texture2D _texture;
        private float _rotation;

        public Vector2 Position;
        public Vector2 Origin;


        Random rand = new Random();

        public float RotationVelocity;
        public float LinearVelocity;
        public bool rotatingLeft;
        public bool movingLeft;
        public bool movingUp;


        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }


        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            //Random rotational and directional velocity
            RotationVelocity = rand.Next(80,150);
            LinearVelocity = rand.Next(80, 150);
  


            Position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            rotatingLeft = rand.Next(2) != 0;
            movingLeft = rand.Next(2) != 0;
            movingUp = rand.Next(2) != 0;


            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
  

            // TODO: use this.Content to load your game content here
            _texture = Content.Load <Texture2D>("DVD");
            //sets the origin of the sprite to the center of the sprite
            Origin = new Vector2(_texture.Width / 2, _texture.Height / 2);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //Movement conditions for movement along the x,y axis and rotation of the sprite

            if (rotatingLeft)
            {
                _rotation -= MathHelper.ToRadians(RotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                _rotation += MathHelper.ToRadians(RotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
            }

            if (movingLeft)
            {
                Position.X -= LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Position.X += LinearVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (movingUp)
            {
                Position.Y -= (LinearVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            else
            {
                Position.Y += (LinearVelocity) * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }



            // Some older code that allows you to control spinning if that is preffered.
            /*            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                        {
                            _rotation -= MathHelper.ToRadians(RotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }
                        else if (Keyboard.GetState().IsKeyDown(Keys.Right))
                        {
                            _rotation += MathHelper.ToRadians(RotationVelocity * (float)gameTime.ElapsedGameTime.TotalSeconds);
                        }*/

            //These next four groupings of code are used for finding the four corners of the rectangle for collision detection.
            float tempLeft = - _texture.Width / 2;
            float tempRight = _texture.Width / 2;
            float tempTop = - _texture.Height / 2;
            float tempBottom = _texture.Height / 2;

  

            Vector2 topLeft = new Vector2(tempLeft , tempTop);
            Vector2 topRight = new Vector2(tempRight, tempTop );
            Vector2 bottomRight = new Vector2(tempRight , tempBottom );
            Vector2 bottomLeft = new Vector2(tempLeft, tempBottom);

            topLeft = Vector2.Transform(topLeft, Matrix.CreateRotationZ(_rotation));
            topRight = Vector2.Transform(topRight, Matrix.CreateRotationZ(_rotation));
            bottomLeft = Vector2.Transform(bottomLeft, Matrix.CreateRotationZ(_rotation));
            bottomRight = Vector2.Transform(bottomRight, Matrix.CreateRotationZ(_rotation));

            topLeft += Position;
            topRight += Position;
            bottomRight += Position;
            bottomLeft += Position;


            //Checks for collisions on the right side of the screen
            if (topLeft.X >= _graphics.PreferredBackBufferWidth || topRight.X >= _graphics.PreferredBackBufferWidth || bottomLeft.X >= _graphics.PreferredBackBufferWidth || bottomRight.X >= _graphics.PreferredBackBufferWidth)
            //if (Position.X > _graphics.PreferredBackBufferWidth)
            {
                
                rotatingLeft = !rotatingLeft;
                movingLeft = !movingLeft;
                
                
            }
            //Checks for collisions on the left side of the screen
            else if (topLeft.X <= 0 || topRight.X <= 0 || bottomRight.X <= 0 || bottomLeft.X <= 0) 
            //if(Position.X < 0)
            {
                Debug.WriteLine("left");
                rotatingLeft = !rotatingLeft;
                movingLeft = !movingLeft;
                

            }
            //Checks for collisions on the bottom side of the screen
            else if (topLeft.Y >= _graphics.PreferredBackBufferHeight || topRight.Y >= _graphics.PreferredBackBufferHeight || bottomLeft.Y >= _graphics.PreferredBackBufferHeight || bottomRight.Y >= _graphics.PreferredBackBufferHeight)
            //if (Position.Y > _graphics.PreferredBackBufferHeight)
            {
                Debug.WriteLine("bottom");
                rotatingLeft = !rotatingLeft;
                
                movingUp = !movingUp;

            }
            //Checks for collisions on the top side of the screen
            else if (topLeft.Y <= 0 || topRight.Y <= 0 || bottomLeft.Y <= 0 || bottomRight.Y <= 0)
            //if(Position.Y < 0)
            {
                Debug.WriteLine("top");
                rotatingLeft = !rotatingLeft;
                
                movingUp = !movingUp;

            }



            

            //this stuff could be used for an asteriods like game mechanics.
            /*            var direction = new Vector2((float)Math.Cos(_rotation), -(float)Math.Sin(_rotation));

                        if (Keyboard.GetState().IsKeyDown(Keys.Up)) {
                            Position += direction * LinearVelocity;
                        }*/

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            _spriteBatch.Draw(_texture, Position, null, Color.White, _rotation, Origin, Vector2.One, SpriteEffects.None, 0f);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }


}
