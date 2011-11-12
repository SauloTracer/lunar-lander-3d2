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

        enum Telas {Inicial, InVideo, Menu, Jogo};

        Telas telaAtual = Telas.Inicial;

        Texture2D texturaTela;
        Video videoInicial;
        VideoPlayer videoPlay;
        KeyboardState keyboardState, previousState;

        BasicEffect texture;

        Model Modelo;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = 600;
            graphics.PreferredBackBufferWidth = 800;
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
            videoPlay = new VideoPlayer();
            Modelo = Content.Load<Model>("Modelos/box");
            texturaTela = Content.Load<Texture2D>("Telas/TelaInicial");

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
            keyboardState = Keyboard.GetState();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if ((keyboardState.IsKeyDown(Keys.Enter)) && (previousState.IsKeyUp(Keys.Enter)))
            {
                #region SwitchTelas

                switch (telaAtual)
                {
                    case Telas.Inicial:
                        {
                            telaAtual = Telas.InVideo;
                            videoInicial = Content.Load<Video>("Telas/videoInicial");
                            break;
                        }
                        
                    case Telas.InVideo:
                        {
                            telaAtual = Telas.Menu;
                            texturaTela = Content.Load<Texture2D>("Telas/TelaMenu");
                            break;
                        }
                    case Telas.Menu:
                        {
                            telaAtual = Telas.Jogo;
                            break;
                        }
                }
                #endregion
            }

            previousState = keyboardState;
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
            spriteBatch.Begin();
            if (telaAtual == Telas.Inicial || telaAtual == Telas.Menu)
            {
                spriteBatch.Draw(texturaTela, Vector2.Zero, Color.White);
            }

            if (telaAtual == Telas.InVideo)
            {
                videoPlay.Play(videoInicial);
            }
            else
            {
                Modelo.Draw(texture.World, texture.View, texture.Projection);
            }
            spriteBatch.End();

        }
    }
}
