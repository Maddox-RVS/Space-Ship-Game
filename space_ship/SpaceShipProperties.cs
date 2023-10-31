using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShip_Game.space_ship
{
    public class SpaceShipProperties
    {
        public float x, y;
        public float rotation;
        public float translationalAcceleration;
        public float maxTranslationalVelocity;
        public float angularVelocity;
        public float angularAcceleration;
        public float maxAngularVelocity;
        public Vector2 translationalVelocity;
        public Vector2 translationalDirection;
        public Rectangle bounds;
        public SpaceShip.COLLISION_SIDE collisionSide;
        public bool isColliding;

        public SpaceShipProperties(
            float x, float y, 
            float rotation, 
            float translationalAcceleration, 
            float maxTranslationalVelocity, 
            float angularVelocity,
            float angularAcceleration,
            float maxAngularVelocity,
            Vector2 translationalVelocity, 
            Vector2 translationalDirection, 
            Rectangle bounds,
            SpaceShip.COLLISION_SIDE collisionSide,
            bool isColliding)
        {
            this.x = x;
            this.y = y;
            this.rotation = rotation;
            this.translationalAcceleration = translationalAcceleration;
            this.maxTranslationalVelocity = maxTranslationalVelocity;
            this.angularVelocity = angularVelocity;
            this.angularAcceleration = angularAcceleration;
            this.maxAngularVelocity = maxAngularVelocity;
            this.translationalVelocity = translationalVelocity;
            this.translationalDirection = translationalDirection;
            this.bounds = bounds;
            this.collisionSide = collisionSide;
            this.isColliding = isColliding;
        }
    }
}
