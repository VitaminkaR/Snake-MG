using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Worms
{
    public interface ViewObject
    {
        Vector2 Pos { get; set; }
        Vector2 ViewPos { get; set; }

        void Drawn(SpriteBatch spriteBatch);
    }
}
