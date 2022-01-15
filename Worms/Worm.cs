using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Threading;
using System;
using System.Text.Json;

namespace Worms
{
    class Worm
    {
        const float speed = 32;

        static internal List<WormPart> wormParts;
        static internal int dir;

        internal Rectangle collider;
        internal bool isLive;

        internal Worm()
        {
            wormParts = new List<WormPart>();
            AddWormPart(1, true,false, Game1.widht / 32 / 2, Game1.height / 32 / 2);
            AddWormPart(3, false, false, Game1.widht / 32 / 2, Game1.height / 32 / 2);
            dir = 3;
            isLive = true;
        }

        internal void UpdateWorm(object target)
        {
            if (isLive)
            {




                for (int i = wormParts.Count - 1; i > -1; i--)
                {
                    // if part is main (control snake head)
                    if (wormParts[i].isHead)
                    {
                        if (dir == 0) wormParts[i].Pos = new Vector2(wormParts[i].Pos.X + 1, wormParts[i].Pos.Y);
                        if (dir == 1) wormParts[i].Pos = new Vector2(wormParts[i].Pos.X, wormParts[i].Pos.Y - 1);
                        if (dir == 2) wormParts[i].Pos = new Vector2(wormParts[i].Pos.X - 1, wormParts[i].Pos.Y);
                        if (dir == 3) wormParts[i].Pos = new Vector2(wormParts[i].Pos.X, wormParts[i].Pos.Y + 1);

                        EatApple();
                        PitUse();
                        RockCollide();
                        continue;
                    }

                    // if part not main
                    wormParts[i].Pos = wormParts[i - 1].Pos;

                    // check death
                    if (wormParts[i].Pos == wormParts[0].Pos && i > 3)
                        Death();
                }

                // синхронизация карты
                string map = JsonSerializer.Serialize(Game1.map, typeof(Map), Game1.game.options);
                Game1.client.Send("!m=" + map);
            }
            Game1.game.camera.Vision();
        }

        void RockCollide()
        {
            var _rocks = Game1.map.rocks;

            if (_rocks != null)
            {
                for (int i = 0; i < _rocks.Count; i++)
                {
                    if (wormParts[0].Pos == _rocks[i].Pos)
                    {
                        Death();
                    }
                }
            }
        }

        void PitUse()
        {
            var _pits = Game1.map.pits;

            if (_pits != null)
            {
                for (int i = 0; i < _pits.Count; i++)
                {
                    if (wormParts[0].Pos == _pits[i].Pos)
                    {
                        int otherPit = i;

                        do
                        {
                            otherPit = new Random().Next(0, _pits.Count);
                        } while (otherPit == i);

                        wormParts[0].Pos = _pits[otherPit].Pos;
                        if (dir == 0) wormParts[0].Pos = new Vector2(wormParts[0].Pos.X + 1, wormParts[0].Pos.Y);
                        if (dir == 1) wormParts[0].Pos = new Vector2(wormParts[0].Pos.X, wormParts[0].Pos.Y - 1);
                        if (dir == 2) wormParts[0].Pos = new Vector2(wormParts[0].Pos.X - 1, wormParts[0].Pos.Y);
                        if (dir == 3) wormParts[0].Pos = new Vector2(wormParts[0].Pos.X, wormParts[0].Pos.Y + 1);
                    }
                }
            }
        }

        void EatApple()
        {
            // check collision
            foreach (var a in  Game1.map.apples)
            {
                if (wormParts[0].Pos == a.Pos)
                {
                    Game1.game.score += 1;
                    Game1.map.apples.Remove(a);
                    Game1.viewObjects.Remove(a);
                    AddWormPart(1, false, true);
                    break;
                }
            }
        }

        internal void Death()
        {
            for (int i = 0; i < wormParts.Count; i++)
            {
                wormParts[i].color = Color.Black;
            }
            isLive = false;
        }

        internal void AddWormPart(int count, bool _isHead = false, bool isSpawn = false, int x = 1000, int y = 1000)
        {
            for (int j = 0; j < count; j++)
            {
                wormParts.Add(new WormPart(new Vector2(x, y), _isHead));
            }
        }
    }
}
