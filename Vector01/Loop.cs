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
            for (int i = 0; i < 30; i++)
            {
                brick.Add(new Brick(i , 80 , 360));
            }
        }
        

        public override void Draw()
        {
            brick[0].MakeStartSrf();

            for (int i = 1; i < brick.Count; i++)
            {
                brick[i].MakeStartSrf();
                brick[i].MakeSrf(brick);
                brick[i].MoveSrf(brick.Count);
                brick[i].Display(doc);
            }
        }
    }
}