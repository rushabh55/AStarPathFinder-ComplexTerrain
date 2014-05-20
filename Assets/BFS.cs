using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BFS
{
    public static List<Tile> GetPath(Tile origin, Tile Target)
    {
        List<Tile> path = new List<Tile>();
        Queue<Tile> internalData = new Queue<Tile>();
        internalData.Enqueue(origin);
        Point p = new Point((int)Target.current.x, (int)Target.current.y);
        while (internalData.Count > 0)
        {
            double min = double.MaxValue;
			Tile minTile = internalData.FirstOrDefault();
            
            foreach (var u in path)
            {
                if (u.current.Contains(p))
                {
                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        Debug.Log(u);
                        Debug.Log(getH(Target, path.ElementAt(i)) + getG(origin, path.ElementAt(i)));

						if(path.ElementAt(i) != null && path.ElementAt(i + 1) != null)
                        	PathFinder.debugLineColl.Add(new Vector3Col(path.ElementAt(i).current.PositionVec, path.ElementAt(i + 1).current.PositionVec));
                    }
                    return path;
                }
            }

            bool flag = true;
            foreach(var tw in path)
                if (tw.current.x == minTile.current.x && tw.current.y == minTile.current.y)
                {
                    flag = false;
                    Debug.Log("BROKE");
                    break;
                }

            if (flag)
            {
                path.Add(minTile);
            }

			var t = TileBase.GetLeft(internalData.Peek().current);
			if(t != null && !internalData.Contains(t))
				internalData.Enqueue(t);

			t = TileBase.GetRight(internalData.Peek().current);
            if (t != null && !internalData.Contains(t))
				internalData.Enqueue(t);

			t = TileBase.GetTop (internalData.Peek().current);
            if (t != null && !internalData.Contains(t))
				internalData.Enqueue(t);

			t = TileBase.GetBottom(internalData.Peek().current);
            if (t != null && !internalData.Contains(t))
				internalData.Enqueue(t);

            internalData.Dequeue();
        }

        return path;
    }

    private static float getH(Tile next, Tile current)
    {
        //if (!Physics.Raycast(new Vector3(next.current.position.x, 0, next.current.position.y), new Vector3(current.current.position.x, 0, next.current.position.y)))
        //{
        //    return float.MaxValue;
        //}
		Vector3 ONE = next.current.PositionVec;
		Vector3 TWO = current.current.PositionVec;
		return Vector3.Distance(ONE, TWO) + (next.current.terrainHeight + current.current.terrainHeight) / 2;
    }

    private static float getG(Tile origin, Tile current)
    {
        return Vector2.Distance(origin.current.position.ToVector(), current.current.position.ToVector());
    }
}
