using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

    public class AStar
    {
        public static List<Tile> GetPath2(Tile target, Tile origin)        
        {  
            List<Tile> openset = new List<Tile>();
            List<Tile> closedSet = new List<Tile>();
            openset.Add(origin);
            Point p = new Point((int)target.current.x, (int)target.current.y);
            System.Collections.Hashtable graph = new System.Collections.Hashtable();
            System.Collections.Hashtable gscore = new System.Collections.Hashtable();
            System.Collections.Hashtable fscore = new System.Collections.Hashtable();
            gscore[origin] = 0d;
            fscore[origin] = (double)gscore[origin] + getH(target, origin);

            Tile targetFlag = null;
            double totalCost = 0;
			while(openset.Count != 0)
			{
	                Tile current = null;
	                double min = double.MinValue;
	                
	                for (int i =0 ;  i < openset.Count; i++)
	                {
	                    float h = getH(target, openset.ElementAt(i));
	                    float g = getG(origin, openset.ElementAt(i));

	                    fscore[openset.ElementAt(i)] = Convert.ToDouble(h + g);

					    if (min > (Convert.ToDouble(fscore[openset.ElementAt(i)])))
	                    {
                            min = (double)fscore[openset.ElementAt(i)];
	                        current = openset.ElementAt(i);
	                    }

	                    current = openset.ElementAt(i);

	                    if (current.current.Contains(p))
	                    {							
	                        var w =  reconstructPath(graph, current);

							for (int j = 0 ; j < w.Count - 1; j ++ )
	                        {								
								PathFinder.debugLineColl.Add(new Vector3Col(w.ElementAt(j).current.PositionVec, w.ElementAt(j + 1).current.PositionVec));
	                        }
	                        return w;
	                    }

	                    openset.Remove(current);
	                    if (!closedSet.Contains(current))
	                        closedSet.Add(current);

	                    foreach (var t in TileBase.GetNeighbours(current.current))
	                    {
	                        if(closedSet.Contains(t))
	                            continue;

	                        double tentativeScore = (double)gscore[current] + getH(origin, current);
							if(gscore[t] == null)
								gscore[t] = 0d;
	                        if (tentativeScore < (double)gscore[t] || !openset.Contains(t))
	                        {
	                            graph[t] = current;
	                            totalCost += tentativeScore;
	                            totalCost += getH(target, t) + getH(origin, t);
                                gscore[t] = tentativeScore;
                                fscore[t] = (double)gscore[t] + getH(target, t);
	                            if (!openset.Contains(t))
	                                openset.Add(t);
	                        }
	                    }
	                             
	            }
			}
			foreach (var t in closedSet)
			{
				PathFinder.debugLineColl.Add(new Vector3Col(t.current.PositionVec, new Vector3(t.current.x + t.current.width, 0 ,t.current.y + t.current.height)));
			}
            return closedSet;
        }


    //    function reconstruct_path(came_from, current_node)
    //if current_node in came_from
    //    p := reconstruct_path(came_from, came_from[current_node])
    //    return (p + current_node)
    //else
    //    return current_node


        public static List<Tile> reconstructPath(System.Collections.Hashtable hashtable, Tile current)
        {
            if (hashtable.ContainsKey(current))
            {
                var w = reconstructPath(hashtable, (Tile)hashtable[current]);
                List<Tile> res = new List<Tile>();
                res.AddRange(w);
				foreach(Tile t in hashtable.Values)
				{
					res.Add((Tile)t);
				}
                //res.AddRange((ICollection<Tile>)hashtable.Values);
                return res;
            }
            else
            {
                List<Tile> t = new List<Tile>();
                t.Add(current);
                return t;
            }
        }
            //try
            //    {
            //        var t = TileBase.GetLeft(currentT.current);
            //        if (t != null && !closedSet.Contains(t))
            //            openset.Enqueue(t);

            //        t = TileBase.GetRight(currentT.current);
            //        if (t != null && !closedSet.Contains(t))
            //            openset.Enqueue(t);

            //        t = TileBase.GetTop(currentT.current);
            //        if (t != null && !closedSet.Contains(t))
            //            openset.Enqueue(t);

            //        t = TileBase.GetBottom(currentT.current);
            //        if (t != null && !closedSet.Contains(t))
            //            openset.Enqueue(t);
                    
            //    }
            //    catch (Exception e)
            //    {

            //    }
        public static List<Tile> GetPath(Tile target, Tile current)
        {
            List<Tile> list = new List<Tile>();
            Queue<Tile> openset = new Queue<Tile>();

            openset.Enqueue(current);

            Tile targetFlag = null;
          
            while (targetFlag == null)
            {
                var c = openset.Dequeue();
        
                try
                {
					var t = TileBase.GetLeft(c.current);
					if(t != null)
                        openset.Enqueue(t);
					else
					{
						Debug.Log ("P");
					}
					t = TileBase.GetRight(c.current);
                	if(t != null)
                        openset.Enqueue(t);
					else
					{
						Debug.Log ("P");
					}
					t = TileBase.GetTop (c.current);
					if(t != null)
                        openset.Enqueue(t);
					else
					{
						Debug.Log ("P");
					}
					t = TileBase.GetBottom(c.current);
					if(t != null)
                        openset.Enqueue(t);
					else
					{
						Debug.Log ("P");
					}
				}
                catch (Exception e) 
				{ 
					
				}
                Tile tileWithMin = null;

                double min = double.MaxValue;

				List<Tile> templist = new List<Tile>();

                while (openset.Count != 0)
                {
                    //Fitness function
                    var temp = openset.Peek();
                    float h = getH(target, openset.Peek());
                    float g = getG(current, openset.Peek());

                    double fitness =  h + g;

                    if (min > fitness)
                    {
                        min = fitness;
                        tileWithMin = openset.Peek();
                    }
					//Debug.DrawLine(tileWithMin.current.PositionVec, new Vector3(tileWithMin.current.x, 100, tileWithMin.current.y));
					PathFinder.debugLineColl.Add(new Vector3Col(tileWithMin.current.PositionVec, new Vector3(tileWithMin.current.x, 0, tileWithMin.current.y)));
                    templist.Add(openset.Dequeue());
                }

                openset.Enqueue(tileWithMin);
				Point p = new Point((int)target.current.x, (int)target.current.y);

				foreach(var t in templist)
				{
	                if (tileWithMin.current.Contains(p))
	                {
	                    targetFlag = tileWithMin;
						list.Add(targetFlag);
						break;
	                }
				}
			
			if(!list.Contains(tileWithMin))
				list.Add(tileWithMin);

			if(list.Count >= 7)
				break;
            }            
			var w = list;
			for (int j = 0 ; j < w.Count - 1; j ++ )
			{								
				PathFinder.debugLineColl.Add(new Vector3Col(w.ElementAt(j).current.PositionVec, w.ElementAt(j + 1).current.PositionVec));
			}
            return list;
        }




        private static float getH(Tile next, Tile current)
        {
            //if (!Physics.Raycast(new Vector3(next.current.position.x, 0, next.current.position.y), new Vector3(current.current.position.x, 0, next.current.position.y)))
            {
             //   Debug.Log("Max");
              //  return float.MaxValue;
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

