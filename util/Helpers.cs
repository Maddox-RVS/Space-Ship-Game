﻿

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace SpaceShip_Game.util
{
    internal class Helpers
    {
        public static Vector2 degreesToSlope(float degrees)
        {   
            double radians = degreesToRadians(degrees);
            double rise = Math.Sin(radians);
            double run = Math.Cos(radians);
            return new Vector2((float)rise, (float)run);
        }

        public static Vector2 calculateSystemVelocity(GameObject object1, GameObject object2)
        {
            float object1Mass = (object1.getEntireBounds().Width * object1.getEntireBounds().Height) * 0.1f;
            float object2Mass = (object2.getEntireBounds().Width * object2.getEntireBounds().Height) * 0.1f;
            Vector2 object1Velo = new Vector2(object1.getVelocity().X * Constants.UPDATE_RATE, object1.getVelocity().Y * Constants.UPDATE_RATE);
            Vector2 object2Velo = new Vector2(object2.getVelocity().X * Constants.UPDATE_RATE, object2.getVelocity().Y * Constants.UPDATE_RATE);
            float xvelo = ((object1Mass * object1Velo.X)+(object2Mass * object2Velo.X))/(object1Mass + object2Mass);
            float yvelo = ((object1Mass * object1Velo.Y)+(object2Mass * object2Velo.Y))/(object1Mass + object2Mass);
            return new Vector2(xvelo / 60f, yvelo / 60f);
        }

        public static bool intersects(List<Rectangle> bounds1, List<Rectangle> bounds2)
        {
            foreach (Rectangle bound1 in bounds1)
            {
                foreach (Rectangle bound2 in bounds2)
                {
                    if (bound1.Intersects(bound2)) return true;
                }
            }
            return false;
        }

        public static float scope0To360(float degrees)
        {
            if (degrees >= 0) return degrees%360;
            else return 360+(degrees%360);
        }

        public static float radiansToDegrees(float radians)
        {
            return (float)(radians*(180/Math.PI));
        }

        public static float degreesToRadians(float degrees)
        {
            return (float)(degrees/(180/Math.PI));
        }

        public static Vector2 dimensionsToScale(Texture2D texture, float width, float height)
        {
            return new Vector2(width/texture.Width, height/texture.Height);
        }

        public static bool calculatePixelCollision(SpriteBatch spriteBatch, Texture2D texture1, Rectangle rectangle1, float degrees1, Texture2D texture2, Rectangle rectangle2, float degrees2, bool inSpriteBatch)
        {
            if (!rectangle1.Intersects(rectangle2)) return false;

            texture1 = Helpers.scaleTexture2D(spriteBatch, texture1, rectangle1.Width, rectangle1.Height, inSpriteBatch);
            texture1 = Helpers.rotateTexture2D(spriteBatch, texture1, degrees1, inSpriteBatch);
            texture2 = Helpers.scaleTexture2D(spriteBatch, texture2, rectangle2.Width, rectangle2.Height, inSpriteBatch);
            texture2 = Helpers.rotateTexture2D(spriteBatch, texture2, degrees2, inSpriteBatch);

            Rectangle overlap = Rectangle.Intersect(rectangle1, rectangle2);
            Rectangle normalizedOverlap1 = new Rectangle(
                overlap.X - rectangle1.X, overlap.Y - rectangle1.Y, 
                overlap.Width, overlap.Height);
            Rectangle normalizedOverlap2 = new Rectangle(
                overlap.X - rectangle2.X, overlap.Y - rectangle2.Y,
                overlap.Width, overlap.Height);

            int pixelCount = overlap.Width * overlap.Height;

            Color[] colorData1 = new Color[pixelCount];
            Color[] colorData2 = new Color[pixelCount];

            texture1.GetData<Color>(0, normalizedOverlap1, colorData1, 0, colorData1.Length);
            texture2.GetData<Color>(0, normalizedOverlap2, colorData2, 0, colorData2.Length);

            for (int i = 0; i < pixelCount; i++)
            {
                if (colorData1[i].A != 0 && colorData2[i].A != 0) return true;
            }

            return false;
        }

        public static Texture2D scaleTexture2D(SpriteBatch spriteBatch, Texture2D texture, float width, float height, bool inSpriteBatch)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
                spriteBatch.GraphicsDevice,
                (int)width,
                (int)height,
                false,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                spriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat
            );

            if (inSpriteBatch) spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);

            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Draw(texture, new Rectangle(0, 0, (int)width, (int)height), Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            if (inSpriteBatch) spriteBatch.Begin();

            return (Texture2D) renderTarget;
        }

        public static Texture2D rotateTexture2D(SpriteBatch spriteBatch, Texture2D texture, float degrees, bool inSpriteBatch)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
                spriteBatch.GraphicsDevice,
                (int)texture.Width,
                (int)texture.Height,
                false,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                spriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat
            );

            if (inSpriteBatch) spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);

            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Draw(
                    texture,
                    new Vector2(texture.Width/2, texture.Height/2), 
                    null, 
                    Color.White, 
                    degreesToRadians(degrees), 
                    new Vector2(texture.Width / 2, texture.Height / 2), 
                    1.0f, 
                    SpriteEffects.None, 0);
            spriteBatch.End();

            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            if (inSpriteBatch) spriteBatch.Begin();

            return (Texture2D)renderTarget;
        }
    }
}
