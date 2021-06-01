using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public Tile [,] grid = new Tile [30, 16];

    public List<Tile> tilesToCheck = new List<Tile>();

    void Start()
    {
        for (int i = 0; i < 69; i++)
        {
            PlaceMines();
        }

        PlaceClues();

        PlaceBlanks();
    }

    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            int x = Mathf.RoundToInt(mousePosition.x);
            int y = Mathf.RoundToInt(mousePosition.y);

            Tile tile = grid[x,y];

            if (tile.tileState == Tile.TileState.Normal)
            {
                if (tile.isCovered)
                {
                    if (tile.tileKind == Tile.TileKind.Mine)
                    {
                        GameOver(tile);
                    }
                    else
                    {
                      tile.SetIsCovered(false);  
                    }
            
                    if (tile.tileKind == Tile.TileKind.Blank)
                    {
                    RevealAdjacentTile (x, y);
                    }
                }      
            }
        }
    }

    private void GameOver (Tile tile)
    {
        tile.SetClickedMine();

        GameObject[] GameObjects = GameObject.FindGameObjectsWithTag("Mine");

        foreach(GameObject go in GameObjects)
        {
            Tile t = go.GetComponent<Tile>();

            if (t != tile)
            {
                t.SetIsCovered(false);
            }

        }

    }

    void PlaceMines()
    {

        int x = UnityEngine.Random.Range (0, 30);
        int y = UnityEngine.Random.Range (0, 16);

        if (grid[x, y] == null)
        {
            
            Tile mineTile = Instantiate (Resources.Load("prefab/mine", typeof(Tile)), new Vector3 (x, y, 0), Quaternion.identity) as Tile;

            grid[x, y] = mineTile;

            Debug.Log ("(" + x + ", " + y + ")");

        }else
        {
            PlaceMines();
        }
    }

    void PlaceClues()
    {

        for(int y = 0; y < 16; y++ )
        {
            for (int x = 0; x < 30; x++ )
            {

                if (grid[x,y] == null)
                {

                    int numMines = 0;

                    //north 
                    if ( y+1 < 16) 
                    {
                        if (grid[x, y+1] !=null && grid[x,y+1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                      // east  
                    if ( x+1 < 30)
                    {
                        if (grid[x+1, y] !=null && grid[x+1,y].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++; 
                        }
                    }

                    // South
                    if (y-1 >= 0) 
                    {
                        if (grid [x, y-1] !=null && grid[x,y-1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }
                    //west
                    if ( x-1 >= 0)
                    {
                        if (grid [x-1, y] !=null && grid[x-1,y].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    //northeast
                    if ( x+1 < 30 && y+1 < 16)
                    {
                        if (grid [x+1, y+1] !=null && grid[x+1,y+1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // northwest
                      if ( x-1 >= 0 && y+1 < 16)
                    {
                        if (grid [x-1, y+1] !=null && grid[x-1,y+1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    // southeast
                      if ( x+1 < 30 && y-1 >= 0) 
                    {
                        if (grid [x+1, y-1] !=null && grid[x+1,y-1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                      //southwest  
                      if ( x-1 >= 0 && y-1 >= 0)
                    {
                        if (grid [x-1, y-1] !=null && grid[x-1,y-1].tileKind == Tile.TileKind.Mine)
                        {
                            numMines++;
                        }
                    }

                    if (numMines > 0)
                    {
                        Tile clueTile = Instantiate(Resources.Load("prefab/" + numMines, typeof(Tile)), new Vector3 (x, y, 0), Quaternion.identity) as Tile;

                         grid[x,y] = clueTile;
                    }
                }
            }
        }
    }
    void PlaceBlanks ()
    {
        for ( int y = 0; y < 16; y++)
        {

            for (int x = 0; x < 30; x++ )
            {

                if (grid[x,y] == null)
                {
                    Tile blankTile = Instantiate(Resources.Load("prefab/blank", typeof(Tile)), new Vector3 (x, y, 0), Quaternion.identity) as Tile; 

                    grid[x,y] = blankTile;
                }
            }
        }
    }

    void RevealAdjacentTile (int x, int y)
    {

        if((y+1) < 16)
        {
            CheckTileAt (x, y+1);
        }

        if((x+1) < 30)
        {
            CheckTileAt(x+1, y);
        }

          if((y-1) >= 0)
        {
            CheckTileAt(x, y-1);
        }

          if((x-1) >= 0)
        {
            CheckTileAt(x-1, y);
        }

          if((y+1) < 16 && (x+1) < 30)
        {
            CheckTileAt(x+1, y+1);
        }

          if((y+1) < 16 && (x-1) >= 0)
        {
            CheckTileAt(x-1, y+1);
        }

        if((y-1) >= 0 && (x+1) < 30)
        {
            CheckTileAt(x+1, y-1);
        }

          if((y-1) >= 0 && (x-1) >= 0)
        {
            CheckTileAt(x-1, y-1);
        }

        for (int i = tilesToCheck.Count - 1; i >= 0; i--)
        {
            if(tilesToCheck[i].didCheck)
            {
                tilesToCheck.RemoveAt(i);
            }
        }

        if (tilesToCheck.Count > 0)
        {
            RevealAdjacentTilesforTiles();
        }        

    }

   private void RevealAdjacentTilesforTiles ()
    {
        for (int i = 0; i < tilesToCheck.Count; i++)
        {
            Tile tile = tilesToCheck[i];

            int x = (int)tile.gameObject.transform.localPosition.x;
            int y = (int)tile.gameObject.transform.localPosition.y;

            tile.didCheck = true;

            if (tile.tileState != Tile.TileState.Flagged)
            {
                tile.SetIsCovered(false);
            }            
            RevealAdjacentTile(x, y);

        }
    }

    private void CheckTileAt(int x, int y)
    {
        Tile tile = grid[x,y];

        if (tile.tileKind == Tile.TileKind.Blank)
        {
            tilesToCheck.Add(tile);
            Debug.Log ("Tile @ (" + x + ", " + y+ ") is a Blank tile");

        }else if (tile.tileKind == Tile.TileKind.Clue)
        {
            tile.SetIsCovered(false);
            Debug.Log ("Tile @ (" + x + ", " + y+ ") is a Clue tile");

        }else if (tile.tileKind == Tile.TileKind.Mine)
        {

            Debug.Log ("Tile @ (" + x + ", " + y+ ") is a Mine tile");

        }
        
    }
}