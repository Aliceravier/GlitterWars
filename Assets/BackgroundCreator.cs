using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundCreator : MonoBehaviour
{
    public Vector2Int startPoint;
    public int numberOfTiles;
    public GameObject[] backgroundTiles;
    Grid grid; 
    Camera mainCamera;
    // Start is called before the first frame update
    void Awake()
    {
        grid = GameObject.Find("Grid").GetComponent<Grid>();
        mainCamera = Camera.main;

        for (int i = 0; i < numberOfTiles; i++){
            GameObject tile = SelectTile();
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    GameObject SelectTile(){
        int selected = Random.Range(0, backgroundTiles.Length);
        return backgroundTiles[selected];
    }
}
