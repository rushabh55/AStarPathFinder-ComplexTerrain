
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vector3Col
{
	public Vector3 start;
	public Vector3 end;

	public Vector3Col(Vector3 a, Vector3 b)
	{
		start = a;
		end = b;
	}
};

public class PathFinder : MonoBehaviour {
    const float maxWidth = 40;
    const float maxHeight = 40;
    protected Tile currentTile;
    public Terrain terrain;
    public GameObject target;
    int noofhoriDivisions = 0;
    int noofVerDivisions = 0;
    public static Tile[,] matrix;

    Tile targetTile;
    Tile thisTile;

    bool done = false;

	Stack<Tile> PathFound = null;

    bool finding = false;

	public static List<Vector3Col> debugLineColl = new List<Vector3Col>();

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

        float speed = 20;
		    targetTile = TileBase.GetTileFromPos(target.transform.position);
          
            if (!targetTile.current.Contains(new Point((int)this.transform.position.x, (int)this.transform.position.y)))
                if (PathFound == null && !done)
                {
                    AStarWrapper();
                    done = false;
                }

           if (PathFound != null)
		    if(PathFound.Count > 1)
		    {
			    Vector3 towards = PathFound.Peek().current.PositionVec;
				towards.y = this.transform.position.y;

				Debug.DrawLine(this.transform.position, towards, Color.red);

                this.gameObject.GetComponent<SmoothLookAt>().target = towards;
#if _DEBUG
                speed = 30;
#endif
                this.transform.position = Vector3.MoveTowards(this.transform.position, towards, Time.deltaTime * speed);
                
				var temp = Vector3.Distance(this.transform.position, towards) ;
	            if (temp < 1)
	            {
	                PathFound.Pop();
				    Debug.Log(PathFound.Count);
	            }
		    }
		    else
		    {
                this.transform.position = Vector3.MoveTowards(this.transform.position, target.transform.position, Time.deltaTime * speed);
		    }

        if(!done && PathFound != null)
            foreach (var t in PathFound)
            {
                done = true;
                var w = GameObject.CreatePrimitive(PrimitiveType.Cube);
                w.transform.position = t.current.PositionVec;
            }

		foreach(var t in debugLineColl)
		{
			Debug.DrawLine(t.start, t.end);
		}

	}

    void AStarWrapper()
    { 
        if(thisTile == null)
            thisTile = TileBase.GetTileFromPos(this.transform.position);
		var t = BFS.GetPath(targetTile, thisTile);
        PathFound = new Stack<Tile>(t);        
    }



    void OnCollisionEnter(Collision obj)
    {

    }
}
