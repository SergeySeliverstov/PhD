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

        public Coord(double xMin, double xMax, double yMin, double yMax, int sizeX, int sizeY)
        {
            this.xMin = xMin;
            this.xMax = xMax;
            this.yMin = yMin;
            this.yMax = yMax;

            this.sizeX = sizeX;
            this.sizeY = sizeY;
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
