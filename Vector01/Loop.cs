using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Rhino;
using Rhino.Geometry;

namespace _5.Classes
{
    class Loop : Rhino_Processing
    {
        List<Brick> brick = new List<Brick>();

        public override void Setup()
        {
            for (int i = 0; i < 10; i++)
            {
                brick.Add(new Brick(i , 50 , 180));
            }
        }
        

        public override void Draw()
        {
            foreach (Brick n in brick)
            {
                n.MakeSrf();
                n.MoveSrf();
                n.Display(doc);
            }
        }
    }
}