using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

    public struct TileBase
    {
        float center;
        float x;
        float y;
        float width;
        float height;
    }

    public struct Tile{
        TileBase left;
        TileBase right;
        TileBase top;
        TileBase bottom;
    }
