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

namespace LunarLander3D2
{

    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        BasicEffect texture;

        Model Modelo;

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
            spriteBatch = new SpriteBatch(GraphicsDevice);
            texture = new BasicEffect(GraphicsDevice);
            Modelo = Content.Load<Model>("Modelos/box");

            texture.View = Matrix.CreateLookAt(
            new Vector3(0, 3, 1),
            new Vector3(0, 0, 0),
            Vector3.Up);

            //Matriz de projeção
            texture.Projection = Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,// campo de visao
            GraphicsDevice.Viewport.AspectRatio, // aspecto da tela 3x4 , 16/9
            0.1f, // plano mais proximo
            100.0f); // plano mais distante
            Vector3 pos = new Vector3(-1, -1, 0);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }


            base.Update(gameTime);
        }

        private void DrawModel(Model m, Matrix world, BasicEffect be)
        {
            foreach (ModelMesh mm in m.Meshes)
            {
                foreach (ModelMeshPart mmp in mm.MeshParts)
                {
                    be.World = world;
                    GraphicsDevice.SetVertexBuffer(mmp.VertexBuffer, mmp.VertexOffset);
                    GraphicsDevice.Indices = mmp.IndexBuffer;
                    be.CurrentTechnique.Passes[0].Apply();
                    GraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList, 0, 0,
                        mmp.NumVertices, mmp.StartIndex, mmp.PrimitiveCount);
                }
            }
        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Modelo.Draw(texture.World, texture.View, texture.Projection);

        }
    }
}
