

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

        public static bool calculatePixelCollision(SpriteBatch spriteBatch, Texture2D texture1, Rectangle rectangle1, float degrees1, Texture2D texture2, Rectangle rectangle2, float degrees2)
        {
            if (!rectangle1.Intersects(rectangle2)) return false;

            texture1 = Helpers.scaleTexture2D(spriteBatch, texture1, rectangle1.Width, rectangle1.Height);
            texture1 = Helpers.rotateTexture2D(spriteBatch, texture1, degrees1);
            texture2 = Helpers.scaleTexture2D(spriteBatch, texture2, rectangle2.Width, rectangle2.Height);
            texture2 = Helpers.rotateTexture2D(spriteBatch, texture2, degrees2);

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

        public static Texture2D scaleTexture2D(SpriteBatch spriteBatch, Texture2D texture, float width, float height)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
                spriteBatch.GraphicsDevice,
                (int)width,
                (int)height,
                false,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                spriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat
            );

            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);

            spriteBatch.Begin();
            spriteBatch.GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Draw(texture, new Rectangle(0, 0, (int)width, (int)height), Color.White);
            spriteBatch.End();
            spriteBatch.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin();

            return (Texture2D) renderTarget;
        }

        public static Texture2D rotateTexture2D(SpriteBatch spriteBatch, Texture2D texture, float degrees)
        {
            RenderTarget2D renderTarget = new RenderTarget2D(
                spriteBatch.GraphicsDevice,
                (int)texture.Width,
                (int)texture.Height,
                false,
                spriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat,
                spriteBatch.GraphicsDevice.PresentationParameters.DepthStencilFormat
            );

            spriteBatch.End();
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
            spriteBatch.Begin();

            return (Texture2D)renderTarget;
        }
    }
}
