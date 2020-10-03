using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
    GameObject movementGrid;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        movementGrid = transform.Find("Grid").transform.Find("MovementGrid").gameObject;   
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //transform.Find("Grid").transform.Find("MovementGrid").gameObject.active = true;
            //show clickable grid UI of diameter lengthOfMovement which expands in directionsOfMovement
            //get next click on grid and register movement command, change status to Moved

            Debug.Log("Pressed primary button.");
        }
    }

    public void ToggleGrid(bool isActive){
        movementGrid.SetActive(isActive);
    }
}
