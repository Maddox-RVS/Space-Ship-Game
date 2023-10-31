using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShip_Game
{
    internal class Constants
    {
        public static class SpaceShipConsts
        {
            public const float WIDTH = 80.0f;
            public const float HEIGHT = 80.0f;

            public const float TRANSLATIONAL_ACCELERATION = 0.1f;
            public const float MAX_TRANSLATIONAL_VELOCITY = 7.5f;

            public const float ANGULAR_ACCELERATION = 0.15f;
            public const float ANGULAR_DECCELERATION = 0.05f;
            public const float MAX_ANGULAR_VELOCITY = 10.0f;
            
            public const float COLLISION_VELOCITY_LOSS_PERCENT = 0.90f;
            public const float COLLISION_ROTATIONAL_VELOCITY = 3.0f;
        }

        public static class AstroidConsts
        {
            public const float TRANSLATIONAL_ACCELERATION = 0.1f;
            public const float MAX_TRANSLATIONAL_VELOCITY = 7.5f;

            public const float ANGULAR_ACCELERATION = 0.15f;
            public const float ANGULAR_DECCELERATION = 0.05f;
            public const float MAX_ANGULAR_VELOCITY = 10.0f;

            public const float COLLISION_VELOCITY_LOSS_PERCENT = 0.25f;
        }
    }
}
