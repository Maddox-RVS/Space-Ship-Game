using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceShip_Game.util
{
    public class ViewPort
    {
        private Vector2 translation;

        public ViewPort()
        {
            translation = Vector2.Zero;
        }

        public void translateX(float val)
        {
            translation.X += val;
        }

        public void translateY(float val)
        {
            translation.Y += val;
        }

        public void setXTranslation(float val)
        {
            translation.X = val;
        }

        public void setYTranslation(float val)
        {
            translation.Y = val;
        }

        public void translate(Vector2 vals)
        {
            translation += vals;
        }

        public void setTranslation(Vector2 vals)
        {
            translation = vals;
        }

        public void reset()
        {
            translation = Vector2.Zero;
        }

        public Vector2 getTranslation()
        {
            return translation;
        }

        public Vector2 applyTranslation(Vector2 vals)
        {
            return vals + translation;
        }

        public float applyXTranslation(float val)
        {
            return val + translation.X;
        }

        public float applyYTranslation(float val)
        {
            return val + translation.Y;
        }

        public bool isInView(Vector2 position, Texture2D objectTexture)
        {
            Rectangle objectBounds = new Rectangle((int)applyXTranslation(position.X), (int)applyYTranslation(position.Y), objectTexture.Width, objectTexture.Height);
            return Game1.screenBounds.Contains(objectBounds);
        }
    }
}
