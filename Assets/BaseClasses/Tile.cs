using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

public class Point
{
    public int x;
    public int y;
    private int p1;
    private int p2;

    public Point(int p1, int p2)
    {
        this.p1 = p1;
        this.p2 = p2;
    }

    public Point()
    {

    }

    public UnityEngine.Vector2 ToVector()
    {
        return new UnityEngine.Vector2(x, y);
    }

    public static Point ToPoint(UnityEngine.Vector2 vector)
    {
        return new Point((int)vector.x, (int)vector.y);
    }
}
    public class TileBase 
    {
        public float center;
        public float x;
        public float y;
        public float width;
        public float height;

        public bool searched;

        public int i
        {
            get
            {
                return I;
            }
            set
            {
                if (value > max_i)
                    max_i = value;
                I = value;
            }
        }
        public int j
        {
            get
            {
                return J;
            }
            set
            {
                if (value > max_j)
                    max_j = value;
                J = value;
            }
        }

        private int I;
        private int J;

        public static int max_i;
        public static int max_j;

        public Point position
        {
            get
            {
                return new Point((int)x, (int)y);
            }
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Center : " + center);
            sb.AppendLine("x : " + x);
            sb.AppendLine("y : " + y);
            sb.AppendLine("Width : " + width);
            sb.AppendLine("Height : " + height);
            return sb.ToString();
        }

        public bool Contains(Point p)
        {
            if (p.x >= x && p.x <= x + width && p.y >= y && p.y <= y + height)
                return true;
            return false;
        }

        public static Tile GetTop(TileBase t)
        {
            if (t.j >= max_j)
                t.j = max_j - 1;

            return PathFinder.matrix[t.i, t.j + 1];
        }

        public static Tile GetRight(TileBase t)
        {
            if (t.i >= max_i)
                t.i = max_i - 1;

            return PathFinder.matrix[t.i + 1, t.j];
        }

        public static Tile GetLeft(TileBase t)
        {
            if(t.i <= 0)
                t.i = 1;

            return PathFinder.matrix[t.i -1, t.j];
        }

        public static Tile GetBottom(TileBase t)
        {
            if (t.j <= 0)
                t.j = 1;

            return PathFinder.matrix[t.i, t.j - 1];
        }

        public static Tile GetTileFromPos(UnityEngine.Vector3 position)
        {
            foreach (var t in PathFinder.matrix)
            {
                if (t.current.Contains(new Point() { x = (int)position.x, y = (int)position.z }))
                    return t;
            }
            throw new Exception("Element not inside");
        }

    }

    public class Tile
    {
        public TileBase current = new TileBase();
        public TileBase left = new TileBase();
        public TileBase right = new TileBase();
        public TileBase top = new TileBase();
        public TileBase bottom = new TileBase();

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Current : " + current.ToString());
            return sb.ToString();
        }
    }
