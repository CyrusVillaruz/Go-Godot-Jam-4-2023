using Godot;
using System;
using System.Collections.Generic;

public partial class Raft : Node2D
{        
    class RaftTile 
    {
        public Node2D tile;
        public Node2D floor;

        public RaftTile() {
            tile = null;
            floor = null;
        }
    }

    enum TileType {floor, tile}

    // CONSTANT
    [Export] Node2D centerFloorTile;
    [Export] Node2D raftCore;
    PackedScene floorTile;
    const int maxRaftSize = 21;
    Vector2 raftTileSize = new Vector2(97, 97);

    // VARYING
    List<List<RaftTile>> raftTileGrid = new List<List<RaftTile>>(maxRaftSize);
    bool buildMode = false;
    Node2D newTile;
    Sprite2D newTileSprite;
    TileType tileType = TileType.floor;
    Vector2 mouseGridPosition;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
        floorTile = ResourceLoader.Load<PackedScene>("res://Scenes/raftFloorTile.tscn");

        // initialize grid to all null
        for (int x = 0; x < maxRaftSize; x++) {
            raftTileGrid.Add(new List<RaftTile>(maxRaftSize));
            for (int y = 0; y < maxRaftSize; y++) {
                raftTileGrid[x].Add(new RaftTile());
            }
        }

        int half = (int)Mathf.Floor(maxRaftSize/2);
        raftTileGrid[half][half].floor = centerFloorTile;
        raftTileGrid[half][half].tile = raftCore;
	}

    void ToggleBuild() {
        buildMode = !buildMode;
        if (buildMode) {
            InstantiateTile();
        }
        else {
            newTile.QueueFree();
            newTile = null;
        }
    }

    void InstantiateTile() {
        newTile = floorTile.Instantiate<Node2D>();
        newTileSprite = (Sprite2D)newTile.Get("sprite");
        newTileSprite.SelfModulate = new Color(newTileSprite.SelfModulate, 0.5f);
        AddChild(newTile);
    }

    public void PlaceTile() {
        if (!newTile.Visible) {return;}

        Vector2 gridIndex = PositionToIndex(mouseGridPosition);
        
        raftTileGrid[(int)gridIndex.X][(int)gridIndex.Y].floor = newTile;
        newTile.Call("StartPlaceAnimation");
        InstantiateTile();

    }

    Vector2 mouseToPosition() {
        Vector2 mousePosition = GetLocalMousePosition();
        float gridX = Mathf.Snapped(mousePosition.X, raftTileSize.X);
        float gridY = Mathf.Snapped(mousePosition.Y, raftTileSize.Y);
        Vector2 gridPosition = new Vector2(gridX, gridY);

        return gridPosition;
    }

    Vector2 PositionToIndex(Vector2 position) {
        float halfRaftSize = Mathf.Floor(maxRaftSize/2);

        float gridX = (position.X/raftTileSize.X) + halfRaftSize;
        float gridY = (position.Y/raftTileSize.Y) + halfRaftSize;
        Vector2 gridIndex = new Vector2(gridX, gridY);

        return gridIndex;
    }

    bool isIndexOutsideGrid(int x, int y) {return (x <= 0 || x >= maxRaftSize || y <= 0 || y >= maxRaftSize);}

    bool isValidPosition(Vector2 position) {
        Vector2 gridIndex = PositionToIndex(position);
        int x = (int)gridIndex.X;
        int y = (int)gridIndex.Y;
       
        if (isIndexOutsideGrid(x, y)) {return false;}

        if (tileType == TileType.floor) {
            if (raftTileGrid[x][y].floor != null) {return false;}

            // right, left, down, up
            int[] xList = {1, -1,  0,  0};
            int[] yList = {0,  0,  1, -1};
            int checks = 4;

            for (int i = 0; i < checks; i++) {
                int xIndex = x + xList[i];
                int yIndex = y + yList[i];
                if (isIndexOutsideGrid(xIndex, yIndex)) {continue;}

                if (raftTileGrid[xIndex][yIndex].floor != null) {
                    return true;
                }
            }
        }
        else if (tileType == TileType.tile) {
            return raftTileGrid[x][y].floor != null;
        }

        return false;
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
        if (!buildMode) {return;}
        
        mouseGridPosition = mouseToPosition();
        if (isValidPosition(mouseGridPosition)) {
            newTile.Position = mouseGridPosition;
            newTile.Visible = true;
        }
        else {
            newTile.Visible = false;
        }

	}
}
