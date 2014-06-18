using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class Coord
    {
        double xMin;
        double xMax;
        double yMin;
        double yMax;
        int sizeX; int sizeY;

        double scaleX
        {
            get
            {
                return (xMax - xMin) / sizeX;
            }
        }

        double scaleY
        {
            get
            {
                return (yMax - yMin) / sizeY;
            }
        }

        public int X0
        {
            get
            {
                return GetX(0);
            }
        }

        public int Y0
        {
            get
            {
                return GetY(0);
            }
        }

        public double XMin
        {
            get
            {
                return xMin;
            }
        }

        public double XMax
        {
            get
            {
                return xMax;
            }
        }

        public double YMin
        {
            get
            {
                return yMin;
            }
        }

        public double YMax
        {
            get
            {
                return yMax;
            }
        }

        public Coord(double xMin, double xMax, double yMin, double yMax, int sizeX, int sizeY)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;

            this.sizeX = sizeX;
            this.sizeY = sizeY;
        }

        public Coord(params Func[] funcs)
        {
            this.xMax = double.MinValue;
            this.yMax = double.MinValue;
            this.xMin = double.MaxValue;
            this.yMin = double.MaxValue;
            foreach (var func in funcs)
            {
                if (this.xMax < func.x.Max())
                    this.xMax = func.x.Max();
                if (this.yMax < func.y.Max())
                    this.yMax = func.y.Max();
                if (this.xMin > func.x.Min())
                    this.xMin = func.x.Min();
                if (this.yMin > func.y.Min())
                    this.yMin = func.y.Min();
            }

            this.sizeX = funcs.Max(t => t.x.Length);
            this.sizeY = funcs.Max(t => t.y.Length);
        }

        public int GetX(double x)
        {
            return (int)((x - xMin) / scaleX);
        }

        public int GetY(double y)
        {
            return (int)((yMax - y) / scaleY);
        }
    }
}
