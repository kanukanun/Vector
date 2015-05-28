using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Rhino;
using Rhino.Geometry;

namespace _5.Classes
{
    class Brick
    {
        private int id;
        private double size;
        private double angle;
        
        private Point3d a, b, c, d;
        private NurbsSurface srf;


         public Brick(
            int _id,
            double _angle
        )
        {
            id = _id;
            angle = _angle;
        }

        public void MakeSrf()
        {
            a = new Point3d(0, 0, 0);
            b = new Point3d(100, 0, 0);
            c = new Point3d(100, 200, 0);
            d = new Point3d(0, 200, 0);

            srf = NurbsSurface.CreateFromCorners(a, b, c, d);
            
        }

        public double TriangleArea(Point3d p0 , Point3d p1 , Point3d p2)
        {
            double s;
            double half;
            Line l1 , l2 , l3;

            l1 = new Line(p0 , p1);
            l2 = new Line(p1 , p2);
            l3 = new Line(p2 , p0);
            half = (l1.Length + l2.Length + l3.Length) / 2;
            s = Math.Sqrt(half * (half - l1.Length) * (half - l2.Length) * (half - l3.Length));
            
            return s;
        }

        public Vector3d NormalVector()
        {
            Vector3d normvec , ab , ad , cd , cb;

            double s1 = TriangleArea(a , b , d);
            double s2 = TriangleArea(c , d , b);

            double sum = s1 + s2;

            ab = new Vector3d(b) - new Vector3d(a);
            ad = new Vector3d(d) - new Vector3d(a);
            cd = new Vector3d(d) - new Vector3d(c);
            cb = new Vector3d(b) - new Vector3d(c);

            normvec = Vector3d.CrossProduct(ab , ad) * (s1 / sum);
            normvec += Vector3d.CrossProduct(cd, cb) * (s2 / sum);
            
            return normvec / normvec.Length;
        }

        public void MoveSrf()
        {
            bool flag = srf.Translate(NormalVector());
        }
        
        public void Display(RhinoDoc _doc)
        {
            _doc.Objects.AddSurface(srf);
        }

    }
}
