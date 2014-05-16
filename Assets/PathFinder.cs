
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {
    const float maxWidth = 25;
    const float maxHeight = 25;
    protected Tile currentTile;
    public Terrain terrain;
    public GameObject target;
    int noofhoriDivisions = 0;
    int noofVerDivisions = 0;
    public static Tile[,] matrix;

    Tile targetTile;
    Tile thisTile;

    bool done = false;

    Queue<Tile> PathFound = null;

    bool finding = false;

	// Use this for initialization
	void Start () {
        currentTile = new Tile();
        Init();
	}

    public void Init()
    {
        noofhoriDivisions = (int)(terrain.terrainData.size.x / maxWidth);
        noofVerDivisions = (int)(terrain.terrainData.size.z / maxHeight);
        matrix = new Tile[noofhoriDivisions, noofVerDivisions];
        int y = 0;
        for(int i = 0 ; i < noofhoriDivisions; i++)
        {
            for(int j = 0 ; j < noofVerDivisions; j++)
            {
                Tile t = new Tile();
                t.current.x = i * maxWidth;
                t.current.y = j * maxHeight;
                t.current.width = maxWidth;
                t.current.height = maxHeight;
                t.current.terrainHeight = terrain.terrainData.GetHeight((int)t.current.x + (int)t.current.width / 2, (int)t.current.y + (int)t.current.height);                
                t.current.i = i;
                t.current.j = j;
                matrix[i, j] = t;
                Color r = Color.red;
                r.r += y++;
             //   Debug.DrawLine(t.current.PositionVec, new Vector3(), r);
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
		    targetTile = TileBase.GetTileFromPos(this.transform.position);
            this.transform.LookAt(target.transform);
            //if (target != null)
            //{
            //    Ray r = new Ray(this.transform.position, target.transform.position);
            //    if (Physics.Raycast(r))
            //    {
            //        obstacleFound = true;
            //    }
            //    else
            //        obstacleFound = false;
            //}


            if (!targetTile.current.Contains(new Point((int)this.transform.position.x, (int)this.transform.position.y)))
             if (PathFound == null && !done)
            {
                AStarWrapper();
                done = false;
            }

           
		    if(PathFound.Count > 1)
		    {
			    Vector3 towards = PathFound.Peek().current.PositionVec;
                //towards = new Vector3(PathFound.Peek().current.x, 0, PathFound.Peek().current.y);
			    this.transform.position = Vector3.MoveTowards(this.transform.position, towards, Time.deltaTime * 10);

	            if (Vector3.Distance(this.transform.position, towards) < 10)
	            {
	                PathFound.Dequeue();
	            }
		    }
		    else
		    {
			    Debug.Log("Breaking");
		    }

        if(!done)
            foreach (var t in PathFound)
            {
                done = true;
                //var w = GameObject.CreatePrimitive(PrimitiveType.Cube);
                //w.transform.position = t.current.PositionVec;
            }
	}

    void AStarWrapper()
    { 
        if(thisTile == null)
            thisTile = TileBase.GetTileFromPos(target.transform.position);

        PathFound = new Queue<Tile>(AStar.GetPath(targetTile, thisTile));        
    }



    void OnCollisionEnter(Collision obj)
    {

    }
}
