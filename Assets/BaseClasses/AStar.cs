using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public class AStar
    {
        public static List<Tile> GetPath(Tile target, Tile current)
        {
            List<Tile> list = new List<Tile>();
            Queue<Tile> q = new Queue<Tile>();
            
            q.Enqueue(current);

            Tile targetFlag = null;
          
            while (targetFlag == null)
            {
                var c = q.Dequeue();
        
                try
                {
                    q.Enqueue(TileBase.GetLeft(c.current));
                    q.Enqueue(TileBase.GetRight(c.current));
                    q.Enqueue(TileBase.GetTop(c.current));
                    q.Enqueue(TileBase.GetBottom(c.current));
                }
                catch (Exception e) { }
                Tile tileWithMin = null;

                double min = double.MaxValue;

                while(q.Count != 0)
                {
                    //Fitness function
                    double fitness = getH(target, q.Peek()) + getG(current, q.Peek());

                    if (min > fitness)
                    {
                        min = fitness;
                        tileWithMin = q.Peek();
                    }
                    Debug.Log("Dequeuing:");
                    Debug.Log(q.Peek());
                    q.Dequeue();
                }
                Debug.Log("MIN:");
                Debug.Log(tileWithMin.current);
                if (tileWithMin.current.Contains(new Point() { x = (int)target.current.x, y = (int)target.current.y }))
                {
                    targetFlag = tileWithMin;
                }
                else
                {
                    list.Add(tileWithMin);
                    q.Enqueue(tileWithMin);
                }

                break;
            }

            
            return list;
        }


        private static float getH(Tile next, Tile current)
        {
            if (Physics.Raycast(new Vector3(next.current.position.x, next.current.position.y, 0), new Vector3(current.current.position.x, current.current.position.y, 0)))
            {
                return float.MaxValue;
            }
            return Vector2.Distance(next.current.position.ToVector(), current.current.position.ToVector());
        }

        private static float getG(Tile origin, Tile current)
        {
            return Vector2.Distance(origin.current.position.ToVector(), current.current.position.ToVector());
        }
    }

