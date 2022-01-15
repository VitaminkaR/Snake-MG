using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Worms
{
    class WormPart : ViewObject
    {
        static public Texture2D texture;
        public Color color = Color.White;

        public Vector2 Pos { get; set; }
        public Vector2 ViewPos { get; set; }

        public bool isHead = false;

        public WormPart(Vector2 pos, bool isHead = false)
        {
            Pos = pos;
            this.isHead = isHead;
            Game1.viewObjects.Add(this);
        }

        public void Drawn(SpriteBatch spriteBatch)
        {
            if(!isHead) 
                spriteBatch.Draw(texture, ViewPos, color);
            else
                spriteBatch.Draw(texture, ViewPos, Color.DarkGreen);
        }
    }
}
