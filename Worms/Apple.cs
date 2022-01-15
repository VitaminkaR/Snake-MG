using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Worms
{
    public class Apple : ViewObject
    {
        static public Texture2D texture;
        static public SpriteBatch _spriteBatch;

        public Vector2 Pos { get; set; }
        public Vector2 ViewPos { get; set; }

        public Apple(int x, int y)
        {
            Pos = new Vector2(x, y);
            Game1.viewObjects.Add(this);
        }

        public Rectangle GetCollider()
        {
            return new Rectangle((int)Pos.X, (int)Pos.Y, texture.Width, texture.Height);
        }

        public void Drawn(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ViewPos, Color.White);
        }
    }
}
