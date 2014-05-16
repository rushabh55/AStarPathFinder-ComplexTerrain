
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathFinder : MonoBehaviour {
    const float maxWidth = 20;
    const float maxHeight = 20;
    protected Tile currentTile;
    public Terrain terrain;
    public GameObject target;
    int noofhoriDivisions = 0;
    int noofVerDivisions = 0;
    public static Tile[,] matrix;
    private bool obstacleFound = false;

    Tile targetTile;
    Tile thisTile;

    Queue<Tile> PathFound = null;

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
        for(int i = 0 ; i < noofhoriDivisions; i++)
        {
            for(int j = 0 ; j < noofVerDivisions; j++)
            {
                Tile t = new Tile();
                t.current.x = i * maxWidth;
                t.current.y = j * maxHeight;
                t.current.width = maxWidth;
                t.current.height = maxHeight;
                t.current.center = terrain.terrainData.GetHeight((int)t.current.x + (int)t.current.width / 2, (int)t.current.y + (int)t.current.height);
                t.current.i = i;
                t.current.j = j;
                matrix[i, j] = t;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {
		
		targetTile = TileBase.GetTileFromPos(this.transform.position);
        if (target != null)
        {
            Ray r = new Ray(this.transform.position, target.transform.position);
            if (Physics.Raycast(r))
            {
                obstacleFound = true;
            }
            else
                obstacleFound = false;
        }

            obstacleFound = false ;
            if (PathFound == null || targetTile.current.Contains(new Point((int)this.transform.position.x, (int)this.transform.position.y)))
                AStarWrapper();

           
		    if(PathFound.Count > 1)
		    {
			    Vector3 towards = PathFound.Peek().current.position.ToVector(this.transform.position.z);
			    this.transform.position = Vector3.MoveTowards(this.transform.position, towards, Time.deltaTime * 10);

	            if (Vector3.Distance(this.transform.position, PathFound.Peek().current.position.ToVector(this.transform.position.z)) < 10)
	            {
	                PathFound.Dequeue();
	            }
		    }
		    else
		    {
			    Debug.Log("Breaking");
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
