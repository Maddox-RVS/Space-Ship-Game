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

        private bool isColliding;
        private COLLISION_SIDE collisionSide;

        public enum COLLISION_SIDE
        {
            NONE,
            TOP,
            BOTTOM,
            LEFT,
            RIGHT
        }

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
            origin = new Vector2(width / 2, height / 2);

            translationalVelocity = new Vector2(0, 0);
            maxTranslationalVelocity = Constants.AstroidConsts.MAX_TRANSLATIONAL_VELOCITY;
            translationalAcceleration = Constants.AstroidConsts.TRANSLATIONAL_ACCELERATION;

            angularVelocity = 0.0f;
            maxAngularVelocity = Constants.AstroidConsts.MAX_ANGULAR_VELOCITY;
            angularAcceleration = Constants.AstroidConsts.ANGULAR_ACCELERATION;
            angularDecceleration = Constants.AstroidConsts.ANGULAR_DECCELERATION;
        }

        public void Update(List<GameObject> gameObjects)
        {
            updateMovement();
            checkCollisions(gameObjects);
        }

        private void updateMovement()
        {
            //temporary
            //------------------------------------------------------------
            if (x < -width) x = Game1.screenBounds.Width + width;
            else if (x > Game1.screenBounds.Width + width) x = -width;
            if (y < -height) y = Game1.screenBounds.Height + height;
            else if (y > Game1.screenBounds.Height + height) y = -height;
            //------------------------------------------------------------

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

            float topDistance = Math.Abs(getBounds().Top - gameObject.getBounds().Bottom);
            float bottomDistance = Math.Abs(getBounds().Bottom - gameObject.getBounds().Top);
            float leftDistance = Math.Abs(getBounds().Left - gameObject.getBounds().Right);
            float rightDistance = Math.Abs(getBounds().Right - gameObject.getBounds().Left);

            if (topDistance < bottomDistance && topDistance < leftDistance && topDistance < rightDistance)
            {
                collisionSide = COLLISION_SIDE.TOP;
                this.y += topDistance;
            }
            else if (bottomDistance < topDistance && bottomDistance < leftDistance && bottomDistance < rightDistance)
            {
                collisionSide = COLLISION_SIDE.BOTTOM;
                this.y -= bottomDistance;
            }
            else if (leftDistance < topDistance && leftDistance < bottomDistance && topDistance < rightDistance)
            {
                collisionSide = COLLISION_SIDE.LEFT;
                this.x += leftDistance;
            }
            else if (rightDistance < topDistance && rightDistance < bottomDistance && rightDistance < leftDistance)
            {
                collisionSide = COLLISION_SIDE.RIGHT;
                this.x -= rightDistance;
            }
        }

        public void checkCollisions(List<GameObject> gameObjects)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                if (getBounds().Intersects(gameObject.getBounds()) && gameObject != this)
                {
                    isColliding = true;
                    gameObject.hasCollidedWithGameObject(this);
                }
                else
                {
                    isColliding = false;
                    collisionSide = COLLISION_SIDE.NONE;
                }
            }
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)x - (int)(width/2), (int)y - (int)(height/2), (int)width, (int)height);
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
                isColliding,
                collisionSide,
                texture
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                texture,
                new Vector2(x, y),
                null,
                Color.Gold,
                Helpers.degreesToRadians(rotation),
                origin,
                Helpers.dimensionsToScale(texture, width, height),
                SpriteEffects.None, 0);
        }
    }
}
