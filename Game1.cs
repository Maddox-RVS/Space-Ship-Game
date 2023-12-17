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

namespace SpaceShip_Game
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch spriteBatch;
        public static Rectangle screenBounds;
        public static Dictionary<string, Texture2D> textures;
        public static Dictionary<string, SpriteFont> fonts;
        public static List<GameObject> gameObjects;
        public bool debugMode;
        private SpaceShip spaceShip;
        public static List<Astroid> astroids;
        public Random rnd = new Random();

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
            astroids = new List<Astroid>();
            debugMode = true;

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

            for (int i = 0; i < 5; i++)
            {
                float size = rnd.Next(40, 121);
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

            gameObjects = concatenateGameObjects(
                new List<GameObject> { (GameObject)spaceShip },
                castListToGameObjects(astroids)
            );

            foreach (Astroid astroid in astroids)
            {
                astroid.Update(gameObjects);
            }
            spaceShip.Update(spriteBatch, gameObjects);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            spaceShip.Draw(spriteBatch);
            foreach (Astroid astroid in astroids)
            {
                astroid.Draw(spriteBatch);
            }

            if (debugMode) DebugDraw(spriteBatch);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void DebugDraw(SpriteBatch spriteBatch)
        {
            Texture2D texture = new Texture2D(GraphicsDevice, 1, 1);
            texture.SetData(new[] { Color.White });

            foreach (GameObject gameObject in gameObjects)
            {
                spriteBatch.Draw(texture, gameObject.getBounds(), new Color(0, 0, 255, 20));
            }

            spriteBatch.DrawString(fonts["Debug"], "Rotation: " + spaceShip.getProperties().rotation.ToString(), new Vector2(15, 10), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "Velocity: " + spaceShip.getProperties().translationalVelocity.X.ToString() + ", " + spaceShip.getProperties().translationalVelocity.Y.ToString(), new Vector2(15, 80), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "Direction: " + spaceShip.getProperties().translationalDirection.X.ToString() + ", " + spaceShip.getProperties().translationalDirection.Y.ToString(), new Vector2(15, 150), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "IsColliding: " + spaceShip.getProperties().isColliding.ToString(), new Vector2(15, 220), Color.Blue);
            spriteBatch.DrawString(fonts["Debug"], "CollisionSide: " + spaceShip.getProperties().collisionSide.ToString(), new Vector2(15, 290), Color.Blue);
        }
    }
}