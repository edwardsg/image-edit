using Microsoft.Xna.Framework;
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

			if (replacement != null)
			{
				image.Dispose();
				image = replacement;
				replacement = null;
			}

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

		//Utility Methods
		Color Interpolate(Color[] oldImgColor, double col, double row, int width, double colRatio, double rowRatio)
		{
			//These 4 variables are the 4 pixels around the column/row specified. Floor is used to round down (get the left/upper pixels) and Ceiling is used to round up (get the right/lower pixels)
			//In the case of receiving 5.2,8.4 , c1r1 will be pixel 5,8 , c1r2 will be 5,9 , c2r1 will be 6,8 , c2r2 will be 6,9
			int c1r1 = (int)((Math.Floor(col)) + (Math.Floor(row) * width));
			int c1r2 = (int)((Math.Floor(col)) + (Math.Ceiling(row) * width));
			int c2r1 = (int)((Math.Ceiling(col)) + (Math.Floor(row) * width));
			int c2r2 = (int)((Math.Ceiling(col)) + (Math.Ceiling(row) * width));

			//The addition of these shortcuts to each of the 4 pixels surrounding the desired location in the oldImgColor array also allows checking
			//to ensure that none of them reference a value outside of bounds of the oldImgColor array (in accordance with the dimensions of the texture image).
			Color c1r1Color, c1r2Color, c2r1Color, c2r2Color;

			c1r1Color = oldImgColor[c1r1];
			c1r2Color = oldImgColor[c1r2];
			c2r1Color = oldImgColor[c2r1];
			c2r2Color = oldImgColor[c2r2];

			if (col > image.Width)
			{
				c2r1Color = oldImgColor[c1r1];
				c2r2Color = oldImgColor[c1r2];
			}
			if(row > image.Height)
			{
				c1r2Color = oldImgColor[c1r1];
				c2r2Color = oldImgColor[c2r1];
			}

			//For color values A,R,G,B, takes avg of left side pixels weighted by rowRatio, then does the same for the right side, then averages the left and right weighted by colRatio
			double leftAvg = ((c1r1Color.B*(1-(rowRatio%1))) + (c1r2Color.B * (rowRatio % 1))) / 2;
			double rightAvg = (c2r1Color.B * (1 - (rowRatio % 1)) + c2r2Color.B * (rowRatio % 1)) / 2;
			int totalAvgB = (int) Math.Round((leftAvg * (1 - (colRatio % 1)) + rightAvg * (colRatio % 1)) / 2);

			leftAvg = ((c1r1Color.G * (1 - (rowRatio % 1))) + (c1r2Color.G * (rowRatio % 1))) / 2;
			rightAvg = (c2r1Color.G * (1 - (rowRatio % 1)) + c2r2Color.G * (rowRatio % 1)) / 2;
			int totalAvgG = (int) Math.Round((leftAvg * (1 - (colRatio % 1)) + rightAvg * (colRatio % 1)) / 2);

			leftAvg = ((c1r1Color.R * (1 - (rowRatio % 1))) + (c1r2Color.R * (rowRatio % 1))) / 2;
			rightAvg = (c2r1Color.R * (1 - (rowRatio % 1)) + c2r2Color.R * (rowRatio % 1)) / 2;
			int totalAvgR = (int) Math.Round((leftAvg * (1 - (colRatio % 1)) + rightAvg * (colRatio % 1)) / 2);

			return new Color(totalAvgR, totalAvgG, totalAvgB);
		}

		//Image Transformation Methods
		public void Resize(int width, int height)
		{
			Color[] oldImgColor = new Color[image.Width * image.Height];	//So we have 2D image, but GetData and SetDate are 1D arrays. Will have to account for that in the loops somehow
			image.GetData<Color>(oldImgColor);									//A 200x400 image will be 600x1200 in data, and thus moving to a "new row" would be adding 100 and reverting to "zero"

			Texture2D newImage = new Texture2D(GraphicsDevice, width, height);
			Color[] newImgColor = new Color[width * height];

			double colRatio = image.Width / width;
			double rowRatio = image.Height / height;
			for(int row=0; row<height; row++)
			{
				for(int col=0; col<width; col++)
				{
					newImgColor[col + (row * width)] = Interpolate(oldImgColor, col * colRatio, row * rowRatio, width, colRatio, rowRatio);
				}
			}

			newImage.SetData<Color>(newImgColor);
			replacement = newImage;

			graphics.PreferredBackBufferWidth = width;
			graphics.PreferredBackBufferHeight = height;
			graphics.ApplyChanges();
		}

		int clamp(int value)
		{
			if (value < 0) return 0;
			else if (value > 255) return 255;
			else return value;
		}

		public void Contrast(double contrast)
		{
			Color[] oldImgColor = new Color[image.Width * image.Height];    //So we have 2D image, but GetData and SetDate are 1D arrays. Will have to account for that in the loops somehow
			image.GetData<Color>(oldImgColor);                                  //A 200x400 image will be 600x1200 in data, and thus moving to a "new row" would be adding 100 and reverting to "zero"

			Texture2D newImage = new Texture2D(GraphicsDevice, image.Width, image.Height);
			Color[] newImgColor = new Color[image.Width * image.Height];

			for (int i = 0; i < image.Width * image.Height; i++)
			{
				int r = clamp((int) (oldImgColor[i].R * contrast + contrast * -128 + 128));
				int g = clamp((int) (oldImgColor[i].G * contrast + contrast * -128 + 128));
				int b = clamp((int) (oldImgColor[i].B * contrast + contrast * -128 + 128));
				newImgColor[i] = new Color(r, g, b);
			}

			newImage.SetData<Color>(newImgColor);
			replacement = newImage;
		}

		public void Brighten(double brightness)
		{
			Color[] oldImgColor = new Color[image.Width * image.Height];    //So we have 2D image, but GetData and SetDate are 1D arrays. Will have to account for that in the loops somehow
			image.GetData<Color>(oldImgColor);                                  //A 200x400 image will be 600x1200 in data, and thus moving to a "new row" would be adding 100 and reverting to "zero"

			Texture2D newImage = new Texture2D(GraphicsDevice, image.Width, image.Height);
			Color[] newImgColor = new Color[image.Width * image.Height];

			for (int i = 0; i < image.Width * image.Height; i++)
			{
				int r = clamp((int) (oldImgColor[i].R * brightness));
				int g = clamp((int) (oldImgColor[i].G * brightness));
				int b = clamp((int) (oldImgColor[i].B * brightness));
				newImgColor[i] = new Color(r, g, b);
			}

			newImage.SetData<Color>(newImgColor);
			replacement = newImage;

		}

		public void Saturate()
		{

		}

		public void Blur()
		{

		}

		public void Sharpen()
		{

		}

		public void DetectEdge()
		{

		}

		public void Save()
		{

		}
	}
}
