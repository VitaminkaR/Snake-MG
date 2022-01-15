using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Worms
{
    class Camera
    {
        int ViewSizeX;
        int ViewSizeY;

        public Camera()
        {
            ViewSizeX = Game1.widht / 32;
            ViewSizeY = Game1.height / 32;
        }

        public void Vision()
        {
            List<ViewObject> viewObjects = Game1.viewObjects;
            int x1 = (int)(Worm.wormParts[0].Pos.X - ViewSizeX / 2);
            int y1 = (int)(Worm.wormParts[0].Pos.Y - ViewSizeY / 2);
            int x2 = x1 + ViewSizeX;
            int y2 = y1 + ViewSizeY;

            for (int i = 0; i < viewObjects.Count; i++)
            {
                int vox = (int)viewObjects[i].Pos.X;
                int voy = (int)viewObjects[i].Pos.Y;

                if (x1 <= vox && x2 >= vox && y1 <= voy && y2 >= voy)
                {
                    viewObjects[i].ViewPos = Translate(vox, voy, x1, y1);
                }
                else
                {
                    viewObjects[i].ViewPos = new Vector2(1000, 1000);
                }
            }
        }

        Vector2 Translate(int _vox, int _voy, int _x1, int _y1)
        {
            float viewX = (_vox - _x1) * 32;
            float viewY = (_voy - _y1) * 32;

            return new Vector2(viewX, viewY);
        }
    }
}
