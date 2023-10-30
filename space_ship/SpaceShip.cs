using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceShip_Game.util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpaceShip_Game;
using SpaceShip_Game.astroid;
using System.Net.WebSockets;
using System.Reflection.Metadata;

namespace SpaceShip_Game.space_ship
{
    internal class SpaceShip
    {
        private Texture2D texture;
        private float x, y;
        private float width, height;
        private float boundsWidth, boundsHeight;
        private float rotation;
        private Vector2 origin;

        private float maxAngularVelocity;
        private float angularAcceleration;
        private float angularDecceleration;
        private float angularVelocity;

        private float maxTranslationalVelocity;
        private float translationalAcceleration;
        private Vector2 translationalVelocity;
        private Vector2 translationalDirection;

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

        public SpaceShip(Texture2D texture, float x, float y)
        {
            this.texture = texture;
            this.x = x;
            this.y = y;

            width = Constants.SpaceShipConsts.WIDTH;
            height = Constants.SpaceShipConsts.HEIGHT;
            boundsWidth = width + 10;
            boundsHeight = height + 10;
            rotation = 0.0f;
            origin = new Vector2(texture.Width / 2, texture.Height / 2);

            translationalAcceleration = Constants.SpaceShipConsts.TRANSLATIONAL_ACCELERATION;
            maxTranslationalVelocity = Constants.SpaceShipConsts.MAX_TRANSLATIONAL_VELOCITY;
            translationalVelocity = Vector2.Zero;

            angularAcceleration = Constants.SpaceShipConsts.ANGULAR_ACCELERATION;
            angularDecceleration = Constants.SpaceShipConsts.ANGULAR_DECCELERATION;
            maxAngularVelocity = Constants.SpaceShipConsts.MAX_ANGULAR_VELOCITY;
            angularVelocity = 0;
        }

        public void Update()
        {
            updateMovement();
            checkCollisions();
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

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                Vector2 newVelo = Helpers.degreesToSlope(rotation);
                translationalDirection = newVelo;

                translationalVelocity.X += newVelo.X * translationalAcceleration;
                translationalVelocity.Y += newVelo.Y * (-1.0f * translationalAcceleration);
            }

            rotation += (angularVelocity);

            if (angularVelocity > 0) angularVelocity -= angularDecceleration;
            else if (angularVelocity < 0) angularVelocity += angularDecceleration;

            angularVelocity = Math.Clamp(angularVelocity, -maxAngularVelocity, maxAngularVelocity);

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                angularVelocity -= angularAcceleration;
            }
            else if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                angularVelocity += angularAcceleration;
            }
        }

        public void checkCollisions()
        {
            if (isColliding)
            {
                switch (collisionSide)
                {
                    case COLLISION_SIDE.NONE: break;
                    case COLLISION_SIDE.TOP:
                        translationalVelocity.Y *= -(1-Constants.SpaceShipConsts.COLLISION_VELOCITY_LOSS_PERCENT);
                        break;
                    case COLLISION_SIDE.BOTTOM:
                        translationalVelocity.Y *= -(1-Constants.SpaceShipConsts.COLLISION_VELOCITY_LOSS_PERCENT);;
                        break;
                    case COLLISION_SIDE.LEFT:
                        translationalVelocity.X *= -(1-Constants.SpaceShipConsts.COLLISION_VELOCITY_LOSS_PERCENT);;
                        x += 5.0f;
                        break;
                    case COLLISION_SIDE.RIGHT: 
                        translationalVelocity.X *= -(1-Constants.SpaceShipConsts.COLLISION_VELOCITY_LOSS_PERCENT);;
                        break;
                }
            }
        }

        public void setPositionDefault()
        {
            x = Game1.screenBounds.Width / 2 - width / 2;
            y = Game1.screenBounds.Height / 2 - height / 2;
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)x - (int)(boundsWidth/2), (int)y - (int)(boundsHeight/2), (int)boundsWidth, (int)boundsHeight);
        }

        public Rectangle getPixelBounds()
        {
            return new Rectangle((int)x - (int)(width/2), (int)y - (int)(height/2), (int)width, (int)height);
        }

        public Texture2D getTexture()
        {
            return texture;
        }

        public float getRotationDegrees()
        {
            return rotation;
        }

        public SpaceShipProperties getProperties()
        {
            return new SpaceShipProperties(
                x, y, 
                rotation, 
                translationalAcceleration,
                maxTranslationalVelocity,
                angularVelocity,
                angularAcceleration,
                maxAngularVelocity,
                translationalVelocity,
                translationalDirection,
                getBounds(),
                collisionSide,
                isColliding
            );
        }

        public void Draw(SpriteBatch spriteBatch, Astroid astroid)
        {
            updatePixelCollision(spriteBatch, astroid);

            if (isColliding)
                spriteBatch.Draw(
                    texture,
                    new Vector2(x, y), 
                    null, 
                    Color.Red, 
                    Helpers.degreesToRadians(rotation), 
                    origin, 
                    Helpers.dimensionsToScale(texture, width, height), 
                    SpriteEffects.None, 0);
            else
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

        public void updatePixelCollision(SpriteBatch spriteBatch, Astroid astroid)
        {
            if (Helpers.calculatePixelCollision(spriteBatch, texture, getPixelBounds(), rotation, astroid.getTexture(), astroid.getBounds(), astroid.GetProperties().rotation))
            {
                isColliding = true;

                float topDistance = Math.Abs(getPixelBounds().Top - astroid.getBounds().Bottom);
                float bottomDistance = Math.Abs(getPixelBounds().Bottom - astroid.getBounds().Top);
                float leftDistance = Math.Abs(getPixelBounds().Left - astroid.getBounds().Right);
                float rightDistance = Math.Abs(getPixelBounds().Right - astroid.getBounds().Left);

                if (topDistance < bottomDistance && topDistance < leftDistance && topDistance < rightDistance) 
                    collisionSide = COLLISION_SIDE.TOP;
                else if (bottomDistance < topDistance && bottomDistance < leftDistance && bottomDistance < rightDistance)
                    collisionSide = COLLISION_SIDE.BOTTOM;
                else if (leftDistance < topDistance && leftDistance < bottomDistance && topDistance < rightDistance)
                    collisionSide = COLLISION_SIDE.LEFT;
                else if (rightDistance < topDistance && rightDistance < bottomDistance && rightDistance < leftDistance)
                    collisionSide = COLLISION_SIDE.RIGHT;
            }
            else
            {
                isColliding = false;
                collisionSide = COLLISION_SIDE.NONE;
            }
        }
    }
}
