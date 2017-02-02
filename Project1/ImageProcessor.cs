﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

namespace Project1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class ImageProcessor : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D image;
        Texture2D replacement = null;

		private string fileName;
		public string FileName
		{
			set
			{
				fileName = value;
			}
		}

        public ImageProcessor()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            Stream fileStream = new FileStream(fileName, FileMode.Open);
            image = Texture2D.FromStream(GraphicsDevice, fileStream);
            fileStream.Dispose();

            graphics.PreferredBackBufferWidth = image.Width;
            graphics.PreferredBackBufferHeight = image.Height;
            graphics.ApplyChanges();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            image.Dispose();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();

            spriteBatch.Draw(image, new Vector2(0, 0), Color.White);

            spriteBatch.End();

            base.Draw(gameTime);
        }



		//Image Transformation Methods
		void Resizer(int width, int height)
		{
			Texture2D alterImg = new Texture2D(GraphicsDevice, width, height);
			int[] oldImgColor = new int[image.Width * image.Height * 3];
			image.GetData<Color>()

		}

		void Contraster()
		{

		}

		void Brightener()
		{

		}

		void Saturator()
		{

		}

		void Blurrer()
		{

		}

		void Sharpener()
		{

		}

		void EdgeDetector()
		{

		}

		void Saver()
		{

		}
	}
}
