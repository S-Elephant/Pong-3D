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
using XNALib;

namespace Pong3D
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            XNALib.Global.Content = Content;
            Engine.Instance = new Engine();
            Engine.Instance.Graphics = graphics;
            Engine.Instance.Game = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
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

            Resolution.Init(ref graphics);
            Resolution.SetResolution(1024, 768, false);
            //Resolution.SetResolution(1024, 768, false);
            Resolution.SetVirtualResolution(1280, 720); // Don't change

            Engine.Instance.SpriteBatch = spriteBatch;


            XNALib._3D.DebugShapeRenderer.Initialize(GraphicsDevice);

            Level.Instance = new Level();
            Engine.Instance.ActiveState = Level.Instance;
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
            Engine.Instance.GameTime = gameTime;

            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Engine.Instance.KB.KeyIsReleased(Keys.Escape))
                this.Exit();

            Engine.Instance.ActiveState.Update(gameTime);
            Engine.Instance.KB.Update();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkMagenta);

            // Draw BG
            spriteBatch.Begin();
            spriteBatch.Draw(Common.str2Tex("stars01"), Engine.Instance.ScreenArea, Color.White);
            spriteBatch.End();

            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            GraphicsDevice.SamplerStates[0] = SamplerState.LinearWrap;
            //GraphicsDevice.RasterizerState = RasterizerState.CullClockwise;

            //spriteBatch.Begin(SpriteSortMode.BackToFront,BlendState.Opaque,SamplerState.LinearClamp,DepthStencilState.None,RasterizerState.CullCounterClockwise);
            Level.Instance.Draw();

            spriteBatch.Begin();
            Level.Instance.DrawGUI();
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
