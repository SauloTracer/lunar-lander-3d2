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

namespace SkyDome
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        float aspectRatio;
        
        CameraManager cameraManager;
        MouseCamera camera1;

        Skydome skyDome;

        MouseState originalMouseState;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            aspectRatio = GraphicsDevice.Viewport.AspectRatio;
            skyDome = new Skydome(this);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            originalMouseState = Mouse.GetState();

            cameraManager = new CameraManager();
            camera1 = new MouseCamera(aspectRatio);

            Services.AddService(typeof(CameraManager), cameraManager);

            camera1.SetLookAt(new Vector3(10.0f, 10.0f, 10.0f), new Vector3(-10.0f, -10.0f, -10.0f), Vector3.Up);
            camera1.SetPerspectiveFov(MathHelper.ToRadians(45.0f), aspectRatio, 0.1f, 5000.0f);

            cameraManager.Add("Camera1", camera1);
            skyDome.Load();

            cameraManager.ActiveCamera.Position = new Vector3(cameraManager.ActiveCamera.Position.X,
                cameraManager.ActiveCamera.Position.Y - 10.0f,
               cameraManager.ActiveCamera.Position.Z
              );
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            Vector3 moveVector = new Vector3(0, 0, 0);
            float timeDifference = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0f;

            MouseState currentMouseState = Mouse.GetState();
            if (currentMouseState != originalMouseState)
            {
                float xDifference = currentMouseState.X - originalMouseState.X;
                float yDifference = currentMouseState.Y - originalMouseState.Y;
                camera1.LeftrightRot -= camera1.RotationSpeed * xDifference * timeDifference;
                camera1.UpdownRot -= camera1.RotationSpeed * yDifference * timeDifference;
                Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
            }

            KeyboardState keyState = Keyboard.GetState();
            GamePadState padState = GamePad.GetState(PlayerIndex.One);

            if (keyState.IsKeyDown(Keys.Escape))
                this.Exit();
            if (keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, -1);
            if (keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, 1);
            if (keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(1, 0, 0);
            if (keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-1, 0, 0);
            if (keyState.IsKeyDown(Keys.Q))
                moveVector += new Vector3(0, 1, 0);
            if (keyState.IsKeyDown(Keys.Z))
                moveVector += new Vector3(0, -1, 0);
            if (keyState.IsKeyDown(Keys.Enter))
                camera1.Shake(0.2f, 1.0f);

            moveVector += new Vector3(0, padState.ThumbSticks.Left.Y, padState.ThumbSticks.Left.X);

            cameraManager.ActiveCamera.addToCameraPosition(moveVector * timeDifference);
            cameraManager.ActiveCamera.Update(gameTime);

            skyDome.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Blue);
            skyDome.Draw(gameTime);
            base.Draw(gameTime);
        }
    }

}
