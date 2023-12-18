using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShip_Game.util;
using SpaceShip_Game.space_ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SpaceShip_Game.space_ship.SpaceShip;
using System.Diagnostics;

namespace SpaceShip_Game.astroid
{
    public class Astroid : GameObject
    {
        private Texture2D texture;
        private bool splitOnDestruction;
        private bool isOffspring;
        private float offspringSizePercent;

        private float x, y;
        private float width, height;
        private float rotation;
        private Vector2 origin;

        private Vector2 translationalVelocity;
        private float maxTranslationalVelocity;
        private float translationalAcceleration;

        private float angularVelocity;
        private float maxAngularVelocity;
        private float angularAcceleration;
        private float angularDecceleration;

        public Astroid(Texture2D texture, float x, float y, float width, float height, bool splitOnDestruction, float offspringSizePercent, bool isOffspring)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.splitOnDestruction = splitOnDestruction;
            this.offspringSizePercent = offspringSizePercent;
            this.isOffspring = isOffspring;

            rotation = 0.0f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            translationalVelocity = Vector2.Zero;
            maxTranslationalVelocity = Constants.AstroidConsts.MAX_TRANSLATIONAL_VELOCITY;
            translationalAcceleration = Constants.AstroidConsts.TRANSLATIONAL_ACCELERATION;

            angularVelocity = 0.0f;
            maxAngularVelocity = Constants.AstroidConsts.MAX_ANGULAR_VELOCITY;
            angularAcceleration = Constants.AstroidConsts.ANGULAR_ACCELERATION;
            angularDecceleration = Constants.AstroidConsts.ANGULAR_DECCELERATION;
        }

        public void Update(List<GameObject> gameObjects, SpriteBatch spriteBatch)
        {
            updateMovement();
            checkCollisions(gameObjects, spriteBatch);
        }

        private void updateMovement()
        {
            x += translationalVelocity.X;
            y += translationalVelocity.Y;
            translationalVelocity.X = Math.Clamp(translationalVelocity.X, -maxTranslationalVelocity, maxTranslationalVelocity);
            translationalVelocity.Y = Math.Clamp(translationalVelocity.Y, -maxTranslationalVelocity, maxTranslationalVelocity);

            rotation += (angularVelocity);

            if (angularVelocity > 0) angularVelocity -= angularDecceleration;
            else if (angularVelocity < 0) angularVelocity += angularDecceleration;

            angularVelocity = Math.Clamp(angularVelocity, -maxAngularVelocity, maxAngularVelocity);
        }

        public void hasCollidedWithGameObject(GameObject gameObject)
        {
            translationalVelocity = Helpers.calculateSystemVelocity(this, gameObject);

            float topDistance = Math.Abs(getEntireBounds().Top - gameObject.getEntireBounds().Bottom);
            float bottomDistance = Math.Abs(getEntireBounds().Bottom - gameObject.getEntireBounds().Top);
            float leftDistance = Math.Abs(getEntireBounds().Left - gameObject.getEntireBounds().Right);
            float rightDistance = Math.Abs(getEntireBounds().Right - gameObject.getEntireBounds().Left);

            if (topDistance < bottomDistance && topDistance < leftDistance && topDistance < rightDistance)
            {
                this.y += topDistance;
            }
            else if (bottomDistance < topDistance && bottomDistance < leftDistance && bottomDistance < rightDistance)
            {
                this.y -= bottomDistance;
            }
            else if (leftDistance < topDistance && leftDistance < bottomDistance && topDistance < rightDistance)
            {
                this.x += leftDistance;
            }
            else if (rightDistance < topDistance && rightDistance < bottomDistance && rightDistance < leftDistance)
            {
                this.x -= rightDistance;
            }
        }

        public void checkCollisions(List<GameObject> gameObjects, SpriteBatch spriteBatch)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (getEntireBounds().Intersects(gameObject.getEntireBounds()) && gameObject != this)
                    gameObject.hasCollidedWithGameObject(this);
            }
        }

        public Rectangle getEntireBounds()
        {
            float boundsMultiplier = 1.0f;
            return new Rectangle(
                (int)x - (int)((width * boundsMultiplier)/2), 
                (int)y - (int)((height * boundsMultiplier)/2), 
                (int)(width * boundsMultiplier), 
                (int)(height * boundsMultiplier));
        }

        public List<Rectangle> getSpecialBounds()
        {
            return new List<Rectangle> { 
                new Rectangle(
                (int)x - (int)((width)/2), 
                (int)y - (int)((height)/2), 
                (int)(width), 
                (int)(height)) 
            };
        }

        public Vector2 getPosition()
        {
            return new Vector2(x, y);
        }

        public float getRotationDegrees()
        {
            return rotation;
        }

        public Vector2 getVelocity()
        {
            return translationalVelocity;
        }

        public GameObject.Object getObjectType()
        {
            return GameObject.Object.ASTROID;
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public AstroidProperties GetProperties()
        {
            return new AstroidProperties(
                splitOnDestruction,
                isOffspring,
                offspringSizePercent,
                x, y,
                width, height,
                rotation,
                translationalVelocity,
                maxTranslationalVelocity,
                translationalAcceleration,
                angularVelocity,
                maxAngularVelocity,
                angularAcceleration,
                angularDecceleration,
                texture
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                Game1.viewPort.applyTranslation(new Vector2(x, y)),
                null,
                Color.Gold,
                Helpers.degreesToRadians(rotation),
                origin,
                Helpers.dimensionsToScale(texture, width, height),
                SpriteEffects.None, 0);
        }
    }
}
