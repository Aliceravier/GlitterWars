using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CursorBehaviour : NetworkBehaviour
{
    unit_behaviour selectedUnit;
    Grid grid;
    Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        position = new Vector3(position.x, position.y, 0);
        transform.position = grid.CellToWorld(grid.WorldToCell(position)) + grid.cellSize / 2;  
        if (Input.GetMouseButton(0)){
            SelectUnit();
        }
        
    }

    void SelectUnit()
    {   
        RaycastHit whatdIHit;
        Ray selectionLaser = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(selectionLaser, out whatdIHit)){
            if (whatdIHit.collider.gameObject.CompareTag("Unit"))
            {
                Debug.Log("wwwwwwww");
                if (selectedUnit){
                    selectedUnit.ToggleGrid(false);
                }

                selectedUnit = whatdIHit.collider.GetComponent<unit_behaviour>();
                selectedUnit.ToggleGrid(true);
                
            }
        }

    }



    void GiveCommand()
    {

    }

    
}
