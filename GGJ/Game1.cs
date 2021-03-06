﻿using GGJ.Managers;
using GGJ.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GGJ
{

    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }
        protected override void Initialize()
        {
            IsMouseVisible = false;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Window.Title = "Strangers";
            graphics.PreferredBackBufferWidth = GameConstants.GameWidth;
            graphics.PreferredBackBufferHeight = GameConstants.GameHeight;
            graphics.ApplyChanges();

            Fullscreen();

            ContentManager.Instance.Load(Content);
            
            // Starting menu screen.
            ScreenManager.Instance.ChangeScreen(new SplashScreen(this));
        }

        protected override void UnloadContent()
        {
        }

        public void Quit()
        {
            Exit();
        }

        public bool Fullscreen()
        {
            graphics.IsFullScreen = !graphics.IsFullScreen;
            graphics.ApplyChanges();

            return graphics.IsFullScreen;
        }

        protected override void Update(GameTime gameTime)
        {
            GameManager.Instance.Update();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            GameManager.Instance.Paint(spriteBatch);

            base.Draw(gameTime);
        }
    }
}
