using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;
using UnityEditor;

public class unit_behaviour : MonoBehaviour
{
    enum Status
    {
        Available,
        Moved,
        Shot
    }
    private Status status = Status.Available;

    public int lengthOfMovement;
    public Tile glitterTile;
    public List<int> directionsOfMovement; //ints 0 -> 7 indicating all cardinal directions
    public bool diagonal;
    public int id;
    public Command command;
    GameObject movementGrid;
    Vector3Int offset = new Vector3Int(1, 3, 0);
    public List<Vector3Int> allowedSquares = new List<Vector3Int>();

    private bool waiting = false;

    private Vector3Int moveloc;

    private Vector3Int shootloc;

    // Start is called before the first frame update
    void Awake()
    {

        generateCoords(lengthOfMovement);
        resetShootAndMove();
    }

    public void resetShootAndMove() {
        moveloc = new Vector3Int(0, 0, 0);
        shootloc = new Vector3Int(0, 0, 0);
    }

    void Update()
    {
        if (movementGrid.activeSelf && Input.GetMouseButtonDown(0) && !waiting)
        {
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // (int)pos.x, (int)pos.y, 0)
            Vector3Int coordinate = transform.Find("Grid").GetComponent<Grid>().WorldToCell(pos) - offset;
            if ((Mathf.Abs(coordinate.x) < lengthOfMovement && Mathf.Abs(coordinate.y) < lengthOfMovement)
                && status == Status.Available)
            {
                if (allowedSquares.Contains(coordinate))
                {
                    Tile tile = glitterTile;
                    movementGrid.GetComponent<Tilemap>().SetTile(coordinate + offset, tile);
                    Debug.Log("Here we go!!!!");
                    status = Status.Moved;
                    moveloc = coordinate;
                    StartCoroutine(Wait(1));
                    generateCoords(20);
                }
            }
            else if (allowedSquares.Contains(coordinate) && status == Status.Moved) {
                Debug.Log("Time to shoo1111");
                status = Status.Shot;
                shootloc = coordinate;
                StartCoroutine(Wait(1));
                movementGrid.SetActive(false);
                Command com = getDirections();
                //Debug.Log((com.nbOfSteps, com.unitID, com.directionOfMovement, com.directionOfShot));
            }
            else
                movementGrid.SetActive(false);
            
            //TileData tile = movementGrid.GetComponent<Grid>().SetColor(new Vector3Int(), Color.black);
        }
    }

    void generateCoords(int length)
    {
        movementGrid = transform.Find("Grid").transform.Find("MovementGrid").gameObject;
        Tile tile = glitterTile;
        if (diagonal)
        {
            for (int i = -length+ 1; i < length; i++)
            {
                for (int mul = -1; mul < 2; mul += 2)
                {
                    Vector3Int coord = new Vector3Int(i, i * mul, 0);
                    allowedSquares.Add(coord);
                    movementGrid.GetComponent<Tilemap>().SetTile(coord + offset, tile);
                }

            }
        }
        else
        {
            for (int i = -length + 1; i < length; i++)
            {
                Vector3Int coord = new Vector3Int(i, 0, 0);
                movementGrid.GetComponent<Tilemap>().SetTile(coord + offset, tile);
                allowedSquares.Add(coord);
                coord = new Vector3Int(0, i, 0);
                movementGrid.GetComponent<Tilemap>().SetTile(coord + offset, tile);
                allowedSquares.Add(coord);
            }
        }
    }

    public Command getDirections() {
        int steps;
        Command.Direction shootdir;
        Command.Direction movedir;
        if (diagonal)
        {
            steps = Mathf.Abs(moveloc.x);
            movedir = getDiagonalDirection(moveloc);
            shootdir = getDiagonalDirection(shootloc);
        }
        else {
            if (moveloc.x == 0) {
                steps = moveloc.y;
            } else {
                steps = moveloc.x;
            }
            movedir = getCardinalDirection(moveloc);
            shootdir = getCardinalDirection(shootloc);
        }
        Debug.Log(moveloc);
        Debug.Log(shootloc);
        Debug.Log(movedir);
        Debug.Log(shootdir);
        return new Command(steps, id, movedir, shootdir);
    }

    Command.Direction getDiagonalDirection(Vector3Int loc) {
        if (loc.x >= 0 && loc.y >= 0)
            return Command.Direction.NorthEast;
        else if (loc.x >= 0 && loc.y < 0)
            return Command.Direction.SouthEast;
        else if (loc.x < 0 && loc.y < 0)
            return Command.Direction.SouthWest;
        else
            return Command.Direction.NorthWest;
    }

    Command.Direction getCardinalDirection(Vector3Int loc)
    {
        if (loc.x >= 0 && loc.y == 0)
            return Command.Direction.East;
        else if (loc.x < 0 && loc.y == 0)
            return Command.Direction.West;
        else if (loc.x == 0 && loc.y >= 0)
            return Command.Direction.North;
        else
            return Command.Direction.South;
    }


    // Update is called once per frame
    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0) && !waiting && status != Status.Shot)
        {
            movementGrid = transform.Find("Grid").transform.Find("MovementGrid").gameObject;
            if (!movementGrid.activeSelf)
            {
                movementGrid.SetActive(true);
                StartCoroutine(Wait(1));
            }
        }
    }

    public void ToggleGrid(bool isActive){
        Debug.Log("cooooll");
        movementGrid.SetActive(isActive);
    }

    IEnumerator Wait(float seconds)
    {
        // suspend execution for 5 seconds
        waiting = true;
        yield return new WaitForSeconds(seconds);
        waiting = false;
        Debug.Log("waiting");
    }
}
