﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Tilemaps;

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
    public List<int> directionsOfMovement; //ints 0 -> 7 indicating all cardinal directions
    public List<int> directionsOfShot;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject movementGrid = transform.Find("Grid").transform.Find("MovementGrid").gameObject;
            movementGrid.SetActive(true);
            //show clickable grid UI of diameter lengthOfMovement which expands in directionsOfMovement
            //get next click on grid and register movement command, change status to Moved
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // (int)pos.x, (int)pos.y, 0)
            Vector3Int coordinate = transform.Find("Grid").GetComponent<Grid>().WorldToCell(pos);
            Debug.Log(coordinate);
            //TileData tile = movementGrid.GetComponent<Grid>().SetColor(new Vector3Int(), Color.black);

            Debug.Log("Pressed primary button.");
        }
    }
}
