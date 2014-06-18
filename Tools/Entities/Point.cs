using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tools
{
    public class Point
    {
        public int i;
        public int j;

        public Point(int x, int y)
        {
            this.i = x;
            this.j = y;
        }

        public static Point GetPosition(Point p, int c)
        {
            switch (c)
            {
                case 0: return new Point(p.i - 1, p.j - 1);
                case 1: return new Point(p.i, p.j - 1);
                case 2: return new Point(p.i + 1, p.j - 1);
                case 3: return new Point(p.i - 1, p.j);
                case 4: return new Point(p.i + 1, p.j);
                case 5: return new Point(p.i - 1, p.j + 1);
                case 6: return new Point(p.i - 1, p.j + 1);
                case 7: return new Point(p.i - 1, p.j + 1);
            }
            return p;
        }
    }
}
