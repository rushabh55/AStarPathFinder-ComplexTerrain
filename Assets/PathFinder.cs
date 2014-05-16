
using UnityEngine;
using System.Collections;

public class PathFinder : MonoBehaviour {
    const int maxWidth = 20;
    const int maxHeight = 20;
    protected Tile currentTile;
    public Terrain terrain;
    public GameObject target;
    int noofhoriDivisions = 0;
    int noofVerDivisions = 0;
    public static Tile[,] matrix;
    private bool obstacleFound = false;

    Tile targetTile;
    Tile thisTile;


	// Use this for initialization
	void Start () {
        currentTile = new Tile();
        Init();
	}

    public void Init()
    {
        noofhoriDivisions = (int)terrain.terrainData.size.x / maxWidth;
        noofVerDivisions = (int)terrain.terrainData.size.z / maxHeight;
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
                Debug.Log(t);
                t.current.i = i;
                t.current.j = j;
                matrix[i, j] = t;
            }
        }

    }
	
	// Update is called once per frame
	void Update () {

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


        if (!obstacleFound)
        {
            //skip
        }
        else
        {
            obstacleFound = false ;
            AStarWrapper();
        }

      //  if(!obstacleFound)
        {
            this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, Time.deltaTime * 10);
        }

	}

    void AStarWrapper()
    {        
        targetTile = TileBase.GetTileFromPos(this.transform.position);

        if(thisTile == null)
            thisTile = TileBase.GetTileFromPos(target.transform.position);

        var t = AStar.GetPath(targetTile, thisTile);
        Debug.Log("path");
        foreach(var t1 in t)
        {
            Debug.Log(t1.current);
        }
    }



    void OnCollisionEnter(Collision obj)
    {
        if (obstacleFound)
        {
            if (obj.gameObject.name == "Terrain")
            {
                  this.transform.Rotate(0, 90, 0);
            }
        }
    }
}
