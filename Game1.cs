using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShip_Game.space_ship;
using System.Collections.Generic;
using SpaceShip_Game.util;
using SpaceShip_Game.astroid;
using System.Diagnostics;
using System.Linq;
using System.Collections;
using System;
using System.Net;

namespace SpaceShip_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;
        public static Rectangle screenBounds;
        public static Dictionary<string, Texture2D> textures;
        public static Dictionary<string, SpriteFont> fonts;
        public static List<GameObject> gameObjects;
        public bool debugMode;
        private SpaceShip spaceShip;
        public static List<Astroid> astroids;
        public Random rnd = new Random();
        private RenderTarget2D viewPortDisplay;
        public float screenZoom;
        public static ViewPort viewPort;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreparingDeviceSettings += (object s, PreparingDeviceSettingsEventArgs args) =>
            {
                args.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
            };
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1200;
            graphics.PreferredBackBufferHeight = 800;
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            screenZoom = 10f;
            screenBounds = new Rectangle(0, 0, (int)(graphics.PreferredBackBufferWidth * screenZoom), (int)(graphics.PreferredBackBufferHeight * screenZoom));
            viewPort = new ViewPort();
            textures = new Dictionary<string, Texture2D>();
            fonts = new Dictionary<string, SpriteFont>();
            astroids = new List<Astroid>();
            debugMode = false;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            viewPortDisplay = new RenderTarget2D(
                spriteBatch.GraphicsDevice,
                (int)screenBounds.Width,
                (int)screenBounds.Height,
                false,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                spriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat
            );

            textures.Add("SpaceShip", Content.Load<Texture2D>("SpaceShip\\SpaceShip1"));
            textures.Add("Astroid", Content.Load<Texture2D>("Astroids\\AstroidNormal1"));

            fonts.Add("Debug", Content.Load<SpriteFont>("Fonts\\Default"));

            spaceShip = new SpaceShip(textures["SpaceShip"], screenBounds.Width/2, screenBounds.Height/2);
            spaceShip.setPositionDefault();

            for (int i = 0; i < 400; i++)
            {
                float size = rnd.Next(40, 521);
                astroids.Add(new Astroid(
                    textures["Astroid"], 
                    rnd.Next(0, screenBounds.Width+1), 
                    rnd.Next(0, screenBounds.Height+1), 
                    size, 
                    size, 
                    false, 
                    0.50f, 
                    false));
            }
        }

        public List<GameObject> castListToGameObjects<T>(List<T> list)
        {
            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < list.Count; i++)
            {
                gameObjects.Add((GameObject)list[i]);
            }
            return gameObjects;
        }

        public List<GameObject> concatenateGameObjects(params List<GameObject>[] uncastedGameObjectLists)
        {
            List<GameObject> combinedGameObjectList = new List<GameObject>();
            foreach (List<GameObject> gameObjectList in uncastedGameObjectLists)
            {
                combinedGameObjectList.AddRange(gameObjectList);
            }
            return combinedGameObjectList;
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            viewPort.setTranslation(new Vector2(viewPortDisplay.Width/2, viewPortDisplay.Height/2) - spaceShip.getPosition());

            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                screenZoom += 0.1f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                screenZoom -= 0.1f;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.R))
            {
                screenZoom = 1f;
            }

            screenZoom = Math.Clamp(screenZoom, 1f, 10f);

            gameObjects = concatenateGameObjects(
                new List<GameObject> { (GameObject)spaceShip },
                castListToGameObjects(astroids)
            );

            foreach (Astroid astroid in astroids)
            {
                astroid.Update(gameObjects, spriteBatch);
            }
            spaceShip.Update(spriteBatch, gameObjects);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.GraphicsDevice.SetRenderTarget(viewPortDisplay);

            spriteBatch.Begin();

            spaceShip.Draw(spriteBatch);
            foreach (Astroid astroid in astroids)
            {
                astroid.Draw(spriteBatch);
            }
            if (debugMode) DebugDraw(spriteBatch);
            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);

            spriteBatch.Begin();
            spriteBatch.Draw(
                viewPortDisplay,
                new Vector2(graphics.PreferredBackBufferWidth/2, graphics.PreferredBackBufferHeight/2),
                null,
                Color.White,
                Helpers.degreesToRadians(0.0f),
                new Vector2(viewPortDisplay.Width/2, viewPortDisplay.Height/2),
                screenZoom / (screenZoom * screenZoom),
            SpriteEffects.None, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });

            foreach (GameObject gameObject in gameObjects)
            {
                spriteBatch.Draw(texture, screenBounds, new Color(0, 0, 100, 20));
                spriteBatch.Draw(texture, gameObject.getEntireBounds(), new Color(0, 0, 100, 20));
                foreach (Rectangle bound in gameObject.getSpecialBounds())
                {
                    spriteBatch.Draw(texture, bound, new Color(0, 0, 255, 20));
                }
            }

            spriteBatch.DrawString(fonts["Debug"], "Rotation: " + spaceShip.getProperties().rotation.ToString(), new Vector2(15, 10), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "Velocity: " + spaceShip.getProperties().translationalVelocity.X.ToString() + ", " + spaceShip.getProperties().translationalVelocity.Y.ToString(), new Vector2(15, 80), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "Direction: " + spaceShip.getProperties().translationalDirection.X.ToString() + ", " + spaceShip.getProperties().translationalDirection.Y.ToString(), new Vector2(15, 150), Color.Blue);
        }
    }
}