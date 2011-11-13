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

namespace Caixa3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        KeyboardState teclado;

        Model caixa;
        Model plano;
        Texture2D textura;// não necessario...
        BasicEffect efeitoBasico;
        DualTextureEffect efeitoDuplo;
        Video video;
        VideoPlayer player;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            efeitoDuplo = new DualTextureEffect(GraphicsDevice); //"desenhador"/"pintador" de 3D
            caixa = Content.Load<Model>("caixa");
            (caixa.Meshes[0].Effects[0] as BasicEffect).EnableDefaultLighting();
            plano = Content.Load<Model>("plano");
            textura = Content.Load<Texture2D>("chao");
            efeitoDuplo.Texture = textura;
            efeitoDuplo.Texture2 = Content.Load<Texture2D>("mapadeluzes");
            video = Content.Load<Video>("video");

            player = new VideoPlayer();

            player.IsLooped = true;//repetidamente

            player.Play(video);//toca o video

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            
            teclado = Keyboard.GetState();

            if (teclado.IsKeyDown(Keys.Escape))
            {
                this.Exit();
            }

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            BasicEffect effect =
                caixa.Meshes[0].Effects[0] as BasicEffect;

            effect.TextureEnabled = true;

            effect.Texture = player.GetTexture();

            //Matrix.CreateRotationX((float)gameTime.TotalGameTime.TotalSeconds)*
            //Matrix.CreateRotationY((float)gameTime.TotalGameTime.TotalSeconds / 2)*
            //Matrix.CreateScale(3.0f),

            Matrix.CreateLookAt(
            new Vector3(5, 0, -5),
            Vector3.Zero,
            Vector3.Up);

            Matrix.CreatePerspectiveFieldOfView(
            MathHelper.PiOver4,
            GraphicsDevice.Viewport.AspectRatio,
            1.0f,
            100.0f);

            Matrix view = Matrix.CreateLookAt(
                new Vector3(0, 42, 1),//posicao da camera
                new Vector3(0, 0, 0),//apontamento = Vector3.Zero
                Vector3.Up);//'cima' = new Vector3(0,1,0))

            Matrix proj = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,// abertura '45o.'
                GraphicsDevice.Viewport.AspectRatio,//largura/altura
                1.0f,//near
                100.0f);//far

            Vector3 pos = new Vector3(-1,-1, 0);

            caixa.Draw(
                Matrix.CreateTranslation(pos),
                view,
                proj);

            efeitoDuplo.View = view;
            efeitoDuplo.Projection = proj;

            //DrawModel(plano, Matrix.Identity, efeitoDuplo);


            spriteBatch.Begin();
            //desenja a gui / interface
            spriteBatch.End();

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
