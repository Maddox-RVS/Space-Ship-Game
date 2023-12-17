using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShip_Game.astroid
{
    public class AstroidProperties
    {
        public bool splitOnDestruction;
        public bool isOffspring;
        public float offspringSizePercent;

        public float x, y;
        public float width, height;
        public float rotation;

        public Vector2 translationalVelocity;
        public float maxTranslationalVelocity;
        public float translationalAcceleration;

        public float angularVelocity;
        public float maxAngularVelocity;
        public float angularAcceleration;
        public float angularDecceleration;

        public bool isColliding;
        public Astroid.COLLISION_SIDE collisionSide;

        public Texture2D texture;

        public AstroidProperties(
            bool splitOnDestruction,
            bool isOffspring,
            float offspringSizePercent,
            float x, 
            float y,
            float width, 
            float height,
            float rotation,
            Vector2 translationalVelocity,
            float maxTranslationalVelocity,
            float translationalAcceleration,
            float angularVelocity,
            float maxAngularVelocity,
            float angularAcceleration,
            float angularDecceleration,
            bool isColliding,
            Astroid.COLLISION_SIDE collisionSide,
            Texture2D texture)
        {
            this.splitOnDestruction = splitOnDestruction;
            this.isOffspring = isOffspring;
            this.offspringSizePercent= offspringSizePercent;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.rotation = rotation;
            this.translationalVelocity = translationalVelocity;
            this.maxTranslationalVelocity = maxTranslationalVelocity;
            this.translationalAcceleration = translationalAcceleration;
            this.angularVelocity = angularVelocity;
            this.maxAngularVelocity = maxAngularVelocity;
            this.angularAcceleration = angularAcceleration;
            this.angularDecceleration = angularDecceleration;
            this.isColliding = isColliding;
            this.collisionSide =collisionSide;
            this.texture = texture;
        }
    }
}
