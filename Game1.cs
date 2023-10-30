using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShip_Game.space_ship;
using System.Collections.Generic;
using SpaceShip_Game.util;
using SpaceShip_Game.astroid;
using System.Diagnostics;

namespace SpaceShip_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        public static Rectangle screenBounds;
        public static Dictionary<string, Texture2D> textures;
        public static Dictionary<string, SpriteFont> fonts;
        public bool debugMode;
        private SpaceShip spaceShip;
        private Astroid astroid;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreparingDeviceSettings += (object s, PreparingDeviceSettingsEventArgs args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            Content.RootDirectory = "Content";
            _graphics.PreferredBackBufferWidth = 1200;
            _graphics.PreferredBackBufferHeight = 800;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenBounds = new Rectangle(0, 0, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            debugMode = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            textures.Add("SpaceShip", Content.Load<Texture2D>("SpaceShip\\SpaceShip1"));
            textures.Add("Astroid", Content.Load<Texture2D>("Astroids\\AstroidNormal1"));

            fonts.Add("Debug", Content.Load<SpriteFont>("Fonts\\Default"));

            spaceShip = new SpaceShip(textures["SpaceShip"], 0, 0);
            spaceShip.setPositionDefault();

            astroid = new Astroid(textures["Astroid"], 800, 300, 1, 1, false, 0.50f, false);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            spaceShip.Update();
            astroid.Update(spaceShip);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            astroid.Draw(spriteBatch);
            spaceShip.Draw(spriteBatch, astroid);

            if (debugMode) DebugDraw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });

            spriteBatch.Draw(texture, spaceShip.getBounds(), new Color(100, 0, 0, 20));
            spriteBatch.Draw(texture, spaceShip.getPixelBounds(), new Color(100, 0, 0, 20));

            spriteBatch.DrawString(fonts["Debug"], "Rotation: " + spaceShip.getProperties().rotation.ToString(), new Vector2(15, 10), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "Velocity: " + spaceShip.getProperties().translationalVelocity.X.ToString() + ", " + spaceShip.getProperties().translationalVelocity.Y.ToString(), new Vector2(15, 80), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "Direction: " + spaceShip.getProperties().translationalDirection.X.ToString() + ", " + spaceShip.getProperties().translationalDirection.Y.ToString(), new Vector2(15, 150), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "IsColliding: " + spaceShip.getProperties().isColliding.ToString(), new Vector2(15, 220), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "CollisionSide: " + spaceShip.getProperties().collisionSide.ToString(), new Vector2(15, 290), Color.Blue);

            if (spaceShip.getBounds().Intersects(astroid.getBounds()))
                spriteBatch.Draw(texture, Rectangle.Intersect(spaceShip.getBounds(), astroid.getBounds()), new Color(0, 0, 255, 50));
        }
    }
}