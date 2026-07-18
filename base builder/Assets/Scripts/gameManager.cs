using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Linq;

public class gameManager : MonoBehaviour
{
    public TileBase grassTile;
    public TileBase waterTile;
    public Tilemap tilemap;
    private Vector3Int origin;
    private List<Vector3Int> waterTileList = new List<Vector3Int>();
    Vector3Int[] GetNeighbors(Vector3Int cell)
    {
        bool evenColumn = cell.y % 2 == 0;
        if (evenColumn)
        {
            return new Vector3Int[]
            {
            new Vector3Int( 1,  0, 0), // East
            new Vector3Int( -1,  0, 0), // East
            new Vector3Int( -1,  1, 0), // North-West
            new Vector3Int( 0,  1, 0), // North-East
            new Vector3Int( -1,  -1, 0), // South-West
            new Vector3Int( 0,  -1, 0), // North-East
            };
        }

        return new Vector3Int[]
        {
        new Vector3Int( 1,  0, 0), // East
        new Vector3Int( -1,  0, 0), // West
        new Vector3Int( 1,  1, 0), // North-East
        new Vector3Int( 0,  1, 0), // North-West
        new Vector3Int( 0,  -1, 0), // North-West
        new Vector3Int( 1,  -1, 0), // North-East
        };
    }

    void Start()
    {
        origin = tilemap.WorldToCell(Camera.main.transform.position);
        generateMap();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
    private void generateMap() // currently working, next add shapes that are added randomly to add some form to the map, also might have to look into making it more efficient
    {
        createWater();
        Vector3Int curpos = new Vector3Int(0, 0, 0);
        for (int i = 0; i < 1000; i++)
        {
            tilemap.SetTile(curpos, grassTile);
            waterTileList.Remove(curpos);
            thistile newtile = new thistile { Name = "grass", cordinates = curpos, Tile = grassTile };
            grassTileList.Add(newtile);
            Vector3Int[] neighbors = GetNeighbors(curpos);
            curpos += neighbors[Random.Range(0, neighbors.Length)];
            if (Vector3.Distance(curpos, origin) > 50f)
            {
                break;
            }
        }
        curpos = new Vector3Int(0, 0, 0);
        for (int i = 0; i < 1000; i++)
        {
            tilemap.SetTile(curpos, grassTile);
            waterTileList.Remove(curpos);
            thistile newtile = new thistile { Name = "grass", cordinates = curpos, Tile = grassTile };
            grassTileList.Add(newtile);
            Vector3Int[] neighbors = GetNeighbors(curpos);
            curpos += neighbors[Random.Range(0, neighbors.Length)];
            if (Vector3.Distance(curpos, origin) > 50f)
            {
                break;
            }
        }
        fillHoles();
        fillHoles();
    }
    private void createWater()
    {
        for (int xway = -50; xway < 50; xway++)
        {
            for (int yway = -50; yway < 50; yway++)
            {
                Vector3Int pos = new Vector3Int(xway, yway, 0);
                tilemap.SetTile(pos, waterTile);
                waterTileList.Add(pos);
            }
        }
    }
    private void fillHoles()
    {
        foreach (Vector3Int hole in waterTileList)
        {
            int amountOfNeighbours = 0;
            Vector3Int[] neighbours = GetNeighbors(hole);
            foreach (Vector3Int offset in GetNeighbors(hole))
            {
                Vector3Int neighbour = hole + offset;

                if (grassTileList.Any(t => t.cordinates == neighbour))
                {
                    amountOfNeighbours++;
                }
            }
            if (amountOfNeighbours >= 4)
            {
                tilemap.SetTile(hole, grassTile);
                thistile newtile = new thistile { Name = "grass", cordinates = hole, Tile = grassTile };
                grassTileList.Add(newtile);
            }
        }
    }
    public class thistile
    {
        public string Name;
        public Vector3Int cordinates;
        public TileBase Tile;

    }
    public List<thistile> grassTileList = new List<thistile>();
}
