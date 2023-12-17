using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SpaceShip_Game.space_ship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShip_Game.util
{
    public interface GameObject
    {
        public enum Object
        {
            SPACE_SHIP,
            ASTROID
        }

        public Rectangle getBounds();
        public Vector2 getPosition();
        public float getRotationDegrees();
        public Vector2 getVelocity();
        public Texture2D getTexture();
        public Object getObjectType();

        public void hasCollidedWithGameObject(GameObject gameObject);
    }
}
