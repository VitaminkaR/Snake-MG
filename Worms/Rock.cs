using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace Worms
{
    public class Rock : ViewObject
    {
        static public Texture2D texture;

        public Vector2 Pos { get; set; }
        public Vector2 ViewPos { get; set; }

        public Rock(int x, int y)
        {
            Pos = new Vector2(x, y);
            Game1.viewObjects.Add(this);
        }

        public void Drawn(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, ViewPos, Color.White);
        }
    }
}
