using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;
using UnityEditor;

public class unit_behaviour : NetworkBehaviour
{
    enum Status
    {
        Available,
        Moved,
        Shot
    }
    private Status status;
    public int lengthOfMovement;
    public Tile glitterTile;
    public List<int> directionsOfMovement; //ints 0 -> 7 indicating all cardinal directions
    public bool diagonal;
    GameObject movementGrid;
    Vector3Int offset = new Vector3Int(1, 3, 0);


    // Start is called before the first frame update
    void Awake()
    {
        movementGrid = transform.Find("Grid").transform.Find("MovementGrid").gameObject;
        Tile tile = glitterTile;
        if (diagonal)
        {
            for(int i = -lengthOfMovement+1; i < lengthOfMovement; i++)
            {
                for (int mul = -1; mul < 2; mul += 2)
                {
                    Vector3Int coord = new Vector3Int(i, i*mul, 0);
                    movementGrid.GetComponent<Tilemap>().SetTile(coord + offset, tile);
                }

            }
        }
        else
        {
            for(int i = -lengthOfMovement+1; i < lengthOfMovement; i++)
            {
                Vector3Int coord = new Vector3Int(i, 0, 0);               
                movementGrid.GetComponent<Tilemap>().SetTile(coord + offset, tile);
                coord = new Vector3Int(0, i, 0);
                movementGrid.GetComponent<Tilemap>().SetTile(coord + offset, tile);
            }
        }
    }

    void Update()
    {
        if (movementGrid.activeSelf && Input.GetMouseButtonDown(0))
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // (int)pos.x, (int)pos.y, 0)
            Vector3Int coordinate = transform.Find("Grid").GetComponent<Grid>().WorldToCell(pos) - offset;
            if (Mathf.Abs(coordinate.x) < lengthOfMovement && Mathf.Abs(coordinate.y) < lengthOfMovement)
            {
                Tile tile = glitterTile;
                movementGrid.GetComponent<Tilemap>().SetTile(coordinate + offset, tile);                 
                Debug.Log(coordinate);
            }
            else
                movementGrid.SetActive(false);
            
            //TileData tile = movementGrid.GetComponent<Grid>().SetColor(new Vector3Int(), Color.black);
        }
    }

    // Update is called once per frame
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            movementGrid = transform.Find("Grid").transform.Find("MovementGrid").gameObject;
            if (!movementGrid.activeSelf)
            {
                movementGrid.SetActive(true);
                //show clickable grid UI of diameter lengthOfMovement which expands in directionsOfMovement
                //get next click on grid and register movement command, change status to Moved

                Debug.Log("Pressed primary button.");
            }
            else {
                Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                // (int)pos.x, (int)pos.y, 0)
                Vector3Int coordinate = transform.Find("Grid").GetComponent<Grid>().WorldToCell(pos);
                Debug.Log(coordinate);
                //TileData tile = movementGrid.GetComponent<Grid>().SetColor(new Vector3Int(), Color.black);
            }
        }
    }

    public void ToggleGrid(bool isActive){
        Debug.Log("cooooll");
        movementGrid.SetActive(isActive);
    }
}
