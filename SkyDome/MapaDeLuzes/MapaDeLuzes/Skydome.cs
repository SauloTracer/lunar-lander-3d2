using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SkyDome
{
    class Skydome : DrawableGameComponent
    {
        Model model;
        World world;

        Texture2D texture;
        
        CameraManager cameraManager;
        bool isInitialized = false;

        public World World
        {
            get { return world; }
            set { world = value; }
        }

        public Skydome(Game game)
            : base(game)
        {
            world = new World();
            isInitialized = false;
        }

        public override void Initialize()
        {
            cameraManager = Game.Services.GetService(typeof(CameraManager)) as CameraManager;
            isInitialized = true;
            base.Initialize();
        }

        public void Load()
        {
            if (!isInitialized)
                Initialize();
            model = Game.Content.Load<Model>("Models/SkyDome/Skydome");
            texture = Game.Content.Load<Texture2D>("Models/SkyDome/earth");
        }

        public override void Update(GameTime time)
        {
            CameraBase camera = cameraManager.ActiveCamera;
            world.Translate = new Vector3(camera.Position.X, camera.Position.Y - 10.0f, camera.Position.Z);
            world.Scale = new Vector3(100, 100, 100);
            world.Rotate = new Vector3(-90, 1, 1);  
            base.Update(time);
        }

        public override void Draw(GameTime time)
        {
            foreach (ModelMesh modelMesh in model.Meshes)
            {
                foreach (BasicEffect basicEffect in modelMesh.Effects)
                {
                    // Material da textura
                    basicEffect.Texture = texture;
                    basicEffect.TextureEnabled = true;

                    // Transformacao
                    basicEffect.World = world.Matrix;
                    basicEffect.View = cameraManager.ActiveCamera.View;
                    basicEffect.Projection = cameraManager.ActiveCamera.Projection;
                }
                modelMesh.Draw();
            }

            base.Draw(time);
        }

    }
}
