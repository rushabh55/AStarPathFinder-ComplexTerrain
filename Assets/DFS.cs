using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class DFS
{
    public static List<Tile> GetPath(Tile origin, Tile Target)
    {
        List<Tile> path = new List<Tile>();
        Queue<Tile> internalData = new Queue<Tile>();
        internalData.Enqueue(origin);
        Point p = new Point((int)Target.current.x, (int)Target.current.y);
        while (internalData.Count > 0)
        {
            path.Add(internalData.Peek());
            foreach (var u in internalData)
            {
                if (u.current.Contains(p))
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        PathFinder.debugLineColl.Add(new Vector3Col(path.ElementAt(i).current.PositionVec, path.ElementAt(i + 1).current.PositionVec));
                    }
                    return path;
                }
            }

            var t = TileBase.GetLeft(internalData.Peek().current);
            if (t != null)
                internalData.Enqueue(t);

            t = TileBase.GetRight(internalData.Peek().current);
            if (t != null)
                internalData.Enqueue(t);

            t = TileBase.GetTop(internalData.Peek().current);
            if (t != null)
                internalData.Enqueue(t);

            t = TileBase.GetBottom(internalData.Peek().current);
            if (t != null)
                internalData.Enqueue(t);

            internalData.Dequeue();
        }

        return path;
    }
}
