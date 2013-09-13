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
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Factories;

namespace WindowsGame1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Body body, body2, floor;

        const float unitToPixel = 100.0f;
        const float pixelToUnit = 1 / unitToPixel;

        Fixture fixture1, fixture2;
        World world;
        Texture2D PlainTexture;

        Vector2 Size, Size2, size, size2, SizeFloor, sizeFloor;


        float height,width,density,posx,posy, floor_width, floor_height;

        float height2,width2, density2,posx2,posy2;

        float gravity;

        public Game1()
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
            height = 50  ;
            width = 50 ;
            density = 1f;
            posx = 5 ;
            posy = 5 ;

            height2 = 50 ;
            width2 = 50 ;
            density2 = 1f;
            posx2 = 360 ;
            posy2 = 65 ;

            floor_width = GraphicsDevice.Viewport.Width;
            floor_height = 100.0f;

            gravity = 9.8f;

            // TODO: Add your initialization logic here
            world = new World(new Vector2(0, gravity));

            body = BodyFactory.CreateRectangle(world, width*pixelToUnit, height*pixelToUnit, density);
            body.BodyType = BodyType.Dynamic;
            size = new Vector2(width , height );
            Size = size;
            body.Position = new Vector2((GraphicsDevice.Viewport.Width / 2.0f) * pixelToUnit, 0);

            body2 = BodyFactory.CreateRectangle(world, width2 * pixelToUnit, height2 * pixelToUnit, density2);
            body2.BodyType = BodyType.Dynamic;
            size2 = new Vector2(width2 , height2 );
            Size2 = size2;
            body2.Position = new Vector2(posx2 * pixelToUnit, posy2 * pixelToUnit);

            floor = BodyFactory.CreateRectangle(world, floor_width * pixelToUnit, floor_height * pixelToUnit, 1000f);
            floor.Position = new Vector2((GraphicsDevice.Viewport.Width / 2.0f) * pixelToUnit, (GraphicsDevice.Viewport.Height - 50) * pixelToUnit);
            floor.BodyType = BodyType.Static;
            sizeFloor = new Vector2(floor_width, floor_height);
            SizeFloor = sizeFloor;



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

            // TODO: use this.Content to load your game content here
            PlainTexture = new Texture2D(GraphicsDevice, 1, 1);
            PlainTexture.SetData(new[] { Color.White });
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

            Vector2 force1 = Vector2.Zero;
            Vector2 force2 = Vector2.Zero;
            float forcePower = 1;

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.W))
                force1 += new Vector2(0, -forcePower);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.A))
                force1 += new Vector2(-forcePower, 0);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.S))
                force1 += new Vector2(0, forcePower);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D))
                force1 += new Vector2(forcePower, 0);

            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Up))
                force2 += new Vector2(0, -forcePower);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Left))
                force2 += new Vector2(-forcePower, 0);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Down))
                force2 += new Vector2(0, forcePower);
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.Right))
                force2 += new Vector2(forcePower, 0);


            body.ApplyForce(force1);
            body2.ApplyForce(force2);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Vector2 scale = new Vector2(Size.X / (float)PlainTexture.Width, Size.Y / (float)PlainTexture.Height);
            Vector2 scale2 = new Vector2(Size2.X / (float)PlainTexture.Width, Size2.Y / (float)PlainTexture.Height);
            Vector2 scaleFloor = new Vector2(SizeFloor.X / (float)PlainTexture.Width, SizeFloor.Y / (float)PlainTexture.Height);

            spriteBatch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend);

            // TODO: Add your drawing code here
            
            spriteBatch.Draw(PlainTexture, body.Position*unitToPixel, null, Color.White, body.Rotation, new Vector2(PlainTexture.Width / 2.0f, PlainTexture.Height / 2.0f), scale, SpriteEffects.None, 0);
            spriteBatch.Draw(PlainTexture, body2.Position*unitToPixel, null, Color.Red, body2.Rotation, new Vector2(PlainTexture.Width / 2.0f, PlainTexture.Height / 2.0f), scale2, SpriteEffects.None, 0);
            spriteBatch.Draw(PlainTexture, floor.Position * unitToPixel, null, Color.White, floor.Rotation, new Vector2(PlainTexture.Width / 2.0f, PlainTexture.Height / 2.0f), scaleFloor , SpriteEffects.None, 0);

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
