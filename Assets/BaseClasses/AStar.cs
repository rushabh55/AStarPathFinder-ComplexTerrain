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
				    float h = getH(target, q.Peek());
				    float g = getG(current, q.Peek());
                    double fitness =  h + g;

                    if (min > fitness)
                    {
                        min = fitness;
                        tileWithMin = q.Peek();
                    }
                    q.Dequeue();
                }

				Point p = new Point((int)target.current.x, (int)target.current.y);
                if (tileWithMin.current.Contains(p))
                {
                    targetFlag = tileWithMin;
					list.Add(targetFlag);
					break;
                }
                else
                {
					if(list.Count > 5000)
						break;
                    
					list.Add(tileWithMin);
                    q.Enqueue(tileWithMin);
                }

             //   break;
            }            
            return list;
        }


        private static float getH(Tile next, Tile current)
        {
            if (!Physics.Raycast(new Vector3(next.current.position.x, 0, next.current.position.y), new Vector3(current.current.position.x, 0, next.current.position.y)))
            {
                Debug.Log("Max");
                return float.MaxValue;
            }
		    Vector2 ONE = next.current.position.ToVector();
		    Vector2 TWO =  current.current.position.ToVector();
		return Vector2.Distance(ONE, TWO);
        }

        private static float getG(Tile origin, Tile current)
        {
            return Vector2.Distance(origin.current.position.ToVector(), current.current.position.ToVector());
        }
    }

