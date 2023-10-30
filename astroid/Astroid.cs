﻿using Microsoft.Xna.Framework;
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

namespace SpaceShip_Game.astroid
{
    internal class Astroid
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
            this.width = texture.Width;
            this.height = texture.Height;
            this.splitOnDestruction = splitOnDestruction;
            this.offspringSizePercent = offspringSizePercent;
            this.isOffspring = isOffspring;

            rotation = 0.0f;
            origin = new Vector2(width / 2, height / 2);

            translationalVelocity = Vector2.Zero;
            maxTranslationalVelocity = Constants.AstroidConsts.MAX_TRANSLATIONAL_VELOCITY;
            translationalAcceleration = Constants.AstroidConsts.TRANSLATIONAL_ACCELERATION;

            angularVelocity = 0.0f;
            maxAngularVelocity = Constants.AstroidConsts.MAX_ANGULAR_VELOCITY;
            angularAcceleration = Constants.AstroidConsts.ANGULAR_ACCELERATION;
            angularDecceleration = Constants.AstroidConsts.ANGULAR_DECCELERATION;
        }

        public void Update(SpaceShip spaceShip)
        {
            updateMovement();
            checkCollision(spaceShip);
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

        public void checkCollision(SpaceShip spaceShip)
        {
            if (getBounds().Intersects(spaceShip.getPixelBounds()))
            {
                isColliding = true;

                float topDistance = Math.Abs(getBounds().Top - spaceShip.getPixelBounds().Bottom);
                float bottomDistance = Math.Abs(getBounds().Bottom - spaceShip.getPixelBounds().Top);
                float leftDistance = Math.Abs(getBounds().Left - spaceShip.getPixelBounds().Right);
                float rightDistance = Math.Abs(getBounds().Right - spaceShip.getPixelBounds().Left);

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

            if (isColliding)
            {
                translationalVelocity += spaceShip.getProperties().translationalVelocity;
            }
        }

        public Rectangle getBounds()
        {
            return new Rectangle((int)x, (int)y, (int)width, (int)height);
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
                collisionSide
            );
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, getBounds(), null, Color.Gold, Helpers.degreesToRadians(rotation), origin, SpriteEffects.None, 0);
        }
    }
}