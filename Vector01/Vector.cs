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

        private Point3d a0 , b0 , c0 , d0 , a1, b1, c1, d1;
        private NurbsSurface srf, srf1, srf2, srf3, srf4;


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
            Point3d a = new Point3d(0, 0, 0);
            Point3d b = new Point3d(100, 0, 0);
            Point3d c = new Point3d(100, 200, 0);
            Point3d d = new Point3d(0, 200, 0);

            srf = NurbsSurface.CreateFromCorners(a, b, c, d);
        }

        public void MakeSrf(List<Brick> brick)
        {
            for (int i = 0; i < id; i++)
            {
                srf.Translate(brick[i].NormalVector() * height);
            }
        }

        private double TriangleArea(Point3d p0 , Point3d p1 , Point3d p2)
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

        public void MakeBrick(List<Brick> brick)
        {
            srf1 = NurbsSurface.CreateFromCorners(brick[id - 1].a1, brick[id - 1].b1, b0, a0);
            srf2 = NurbsSurface.CreateFromCorners(brick[id - 1].b1, brick[id - 1].c1, c0, b0);
            srf3 = NurbsSurface.CreateFromCorners(brick[id - 1].c1, brick[id - 1].d1, d0, c0);
            srf4 = NurbsSurface.CreateFromCorners(brick[id - 1].d1, brick[id - 1].a1, a0, d0);
            //srf5 = NurbsSurface.CreateFromCorners(a1 , b1 , c1 , d1);
        }

        private Vector3d NormalVector()//get normal vector in surface
        {
            Vector3d normvec , ab , ad , cd , cb;
            a1 = srf.PointAt(0 , 0);
            b1 = srf.PointAt(100 , 0);
            c1 = srf.PointAt(100 , 200);
            d1 = srf.PointAt(0 , 200);

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

        private Point3d Center()
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
            _doc.Objects.AddSurface(srf1);
            _doc.Objects.AddSurface(srf2);
            _doc.Objects.AddSurface(srf3);
            _doc.Objects.AddSurface(srf4);
        }

    }
}
