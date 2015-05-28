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
        private double height;
        private double angle;
        
        private Point3d a, b, c, d;
        private NurbsSurface srf;


         public Brick(
            int _id,
             double _height,
            double _angle
        )
        {
            id = _id;
            height = _height; 
            angle = _angle;
        }

        public void MakeStartSrf()
        {
            a = new Point3d(0, 0, 0);
            b = new Point3d(100, 0, 0);
            c = new Point3d(100, 200, 0);
            d = new Point3d(0, 200, 0);
            RhinoApp.WriteLine(String.Format("{0}", a));

            srf = NurbsSurface.CreateFromCorners(a, b, c, d); 
        }

        public void MakeSrf(List<Brick> brick)
        {
            //a = Point3d.Subtract(brick[id - 1].a, NormalVector() * (id * height));
            //b = Point3d.Add(brick[id - 1].b, NormalVector() * (id * height));
            //c = Point3d.Add(brick[id - 1].c, NormalVector() * (id * height));
            //d = Point3d.Add(brick[id - 1].d, NormalVector() * (id * height));
            //RhinoApp.WriteLine(String.Format("{0}", NormalVector()));

            //srf = NurbsSurface.CreateFromCorners(a, b, c, d); 
            for (int i = 0; i < id; i++)
            {
                srf.Translate(brick[i].NormalVector() * height);
            }
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

        public Vector3d NormalVector()//get normal vector in surface
        {
            Vector3d normvec , ab , ad , cd , cb;
            Point3d a1 = srf.PointAt(0 , 0);
            Point3d b1 = srf.PointAt(100 , 0);
            Point3d c1 = srf.PointAt(100 , 200);
            Point3d d1 = srf.PointAt(0 , 200);
            double s1 = TriangleArea(a1 , b1 , d1);
            double s2 = TriangleArea(c1 , d1 , b1);

            double sum = s1 + s2;

            ab = new Vector3d(b1) - new Vector3d(a1);
            ad = new Vector3d(d1) - new Vector3d(a1);
            cd = new Vector3d(d1) - new Vector3d(c1);
            cb = new Vector3d(b1) - new Vector3d(c1);

            normvec = Vector3d.CrossProduct(ab , ad) * (s1 / sum);
            normvec += Vector3d.CrossProduct(cd, cb) * (s2 / sum);
            
            return normvec / normvec.Length;
        }

        public Point3d Center()
        {
            Point3d center = srf.PointAt(50 , 100);
            return center;
        }

        public void MoveSrf(int num)//pile of brick
        {
            double ang = RhinoMath.ToRadians((angle / (num - 1)) * id);

            bool flag2 = srf.Rotate(ang , NormalVector(), Center());

            bool flag3 = srf.Rotate(ang, new Vector3d(0, 1, 0), Center());
        }
        
        public void Display(RhinoDoc _doc)
        {
            _doc.Objects.AddSurface(srf);
        }

    }
}
