using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Worms
{
    public class Map
    {
        public int Size { get; private set; }
        public int PitCount { get; private set; }
        public int RockCount { get; private set; }
        public int AppleCount { get; private set; }

        public List<Pit> pits { get; private set; }
        public List<Rock> rocks { get; private set; }
        public List<Apple> apples { get; private set; }

        public Map(int size, int pitCount, int rockCount, int appleCount)
        {
            Size = size;
            pits = new List<Pit>();
            rocks = new List<Rock>();
            apples = new List<Apple>();
            PitCount = pitCount;
            RockCount = rockCount;
            AppleCount = appleCount;
        }

        // generation
        public void GenerationMap()
        {
            for (int i = 0; i < PitCount; i++)
            {
                int rx = new Random().Next(1, Size - 1);
                int ry = new Random().Next(1, Size - 1);

                Pit newPit = new Pit(rx, ry);
                pits.Add(newPit);
            }

            for (int i = 0; i < RockCount; i++)
            {
                int rx = new Random().Next(1, Size - 1);
                int ry = new Random().Next(1, Size - 1);

                Rock newRock = new Rock(rx, ry);
                rocks.Add(newRock);
            }

            // wall generation (map border)
            for (int i = 0; i < Size; i++)
            {
                Rock newRock = new Rock(i, 0);
                rocks.Add(newRock);
            }
            for (int i = 0; i < Size + 1; i++)
            {
                Rock newRock = new Rock(i, Size);
                rocks.Add(newRock);
            }
            for (int i = 0; i < Size; i++)
            {
                Rock newRock = new Rock(0, i);
                rocks.Add(newRock);
            }
            for (int i = 0; i < Size + 1; i++)
            {
                Rock newRock = new Rock(Size, i);
                rocks.Add(newRock);
            }

            for (int i = 0; i < AppleCount; i++)
            {
                int rx = new Random().Next(1, Size - 1);
                int ry = new Random().Next(1, Size - 1);

                apples.Add(new Apple(rx, ry));
            }
        }
    }
}
