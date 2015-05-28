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
        Brick b1;

        public override void Setup()
        {
            b1 = new Brick(0, 180);
        }
        

        public override void Draw()
        {
            b1.MakeSrf();
            b1.MoveSrf();
            b1.Display(doc);
        }
    }
}