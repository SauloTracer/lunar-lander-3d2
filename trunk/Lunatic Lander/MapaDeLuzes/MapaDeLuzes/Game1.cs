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

namespace MapaDeLuzes
{
    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState teclado;

        DualTextureEffect efeitoDuplo;
        Texture2D textura, quadrado, lua;
        Model plano, caixa;

        Vector3 posMundo, posCamera, posCaixa, posCamera2;
        Matrix view, projection, world;

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
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            efeitoDuplo = new DualTextureEffect(GraphicsDevice);
            
            plano = Content.Load<Model>("plano");
            caixa = Content.Load<Model>("box");
            textura = Content.Load<Texture2D>("moonGround");
            quadrado = Content.Load<Texture2D>("quadrado");
            lua = Content.Load<Texture2D>("lua");
            
            efeitoDuplo.Texture = textura;
            efeitoDuplo.Texture2 = quadrado;

            (caixa.Meshes[0].Effects[0] as BasicEffect).TextureEnabled = true;
            (caixa.Meshes[0].Effects[0] as BasicEffect).Texture = lua;

            GraphicsDevice.Clear(Color.Black);

            

            projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4 / 2,// abertura '45o.'
                GraphicsDevice.Viewport.AspectRatio,//largura/altura
                1.0f,//near
                100.0f);//far
                        //Wherever you are :P

            posMundo = new Vector3(-1, -1, 0);
            posCaixa = new Vector3(-1, -0.09f, 0);

            posCamera = new Vector3(posCaixa.X, posCaixa.Y+3, posCaixa.Z+6);
            posCamera2 = posCaixa;

            atualizaCamera();

            world = Matrix.CreateTranslation(posMundo);
        }

        public void atualizaCamera()
        {
            view = Matrix.CreateLookAt(
                posCamera,//posicao da camera
                posCamera2,//apontamento = Vector3.Zero
                Vector3.Up);//'cima' = new Vector3(0,1,0))

            posCamera = new Vector3(posCaixa.X, posCaixa.Y + 1.5f, posCaixa.Z + 10);
            posCamera2 = posCaixa;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            teclado = Keyboard.GetState();
            #region TecladoInput
            if (teclado.IsKeyDown(Keys.Escape)) this.Exit();
            if (teclado.IsKeyDown(Keys.W))
            {
                posCaixa.Y += 0.01f;

            }
            if (teclado.IsKeyDown(Keys.S))
            {
                posCaixa.Y -= 0.01f;

            }
            if (teclado.IsKeyDown(Keys.A))
            {
                posCaixa.X -= 0.01f;

            }
            if (teclado.IsKeyDown(Keys.D))
            {
                posCaixa.X += 0.01f;

            }

            if (teclado.IsKeyDown(Keys.Left))
            {
                //(caixa.Meshes[0].Effects[0] as BasicEffect).World *= Matrix.CreateRotationX(0.01f);
                world *= Matrix.CreateRotationY(0.01f);        
            }
            if (teclado.IsKeyDown(Keys.Right))
            {
                //(caixa.Meshes[0].Effects[0] as BasicEffect).World *= Matrix.CreateRotationX(0.01f);
                world *= Matrix.CreateRotationY(-0.01f);
            }
            if (teclado.IsKeyDown(Keys.Up))
            {
                //(caixa.Meshes[0].Effects[0] as BasicEffect).World *= Matrix.CreateRotationX(0.01f);
                world *= Matrix.CreateRotationX(0.01f);
            }
            if (teclado.IsKeyDown(Keys.Down))
            {
                //(caixa.Meshes[0].Effects[0] as BasicEffect).World *= Matrix.CreateRotationX(0.01f);
                world *= Matrix.CreateRotationX(-0.01f);
            }
            #endregion

            atualizaCamera();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            efeitoDuplo.View = view;
            efeitoDuplo.Projection = projection;

            caixa.Draw(Matrix.CreateTranslation(posCaixa), view, projection);
            DrawModel(plano, world, efeitoDuplo);

            base.Draw(gameTime);
        }

        private void DrawModel(Model m, Matrix world, DualTextureEffect be)
        {
            foreach (ModelMesh mm in m.Meshes)
            {
                foreach (ModelMeshPart mmp in mm.MeshParts)
                {
                    be.World = world;
                    GraphicsDevice.SetVertexBuffer(mmp.VertexBuffer, mmp.VertexOffset);
                    GraphicsDevice.Indices = mmp.IndexBuffer;
                    be.CurrentTechnique.Passes[0].Apply();
                    GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                        mmp.NumVertices, mmp.StartIndex, mmp.PrimitiveCount);
                }
            }
        }

    }

}
