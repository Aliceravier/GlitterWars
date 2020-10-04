using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cat_behaviour : MonoBehaviour
{
    Camera cam;
    public float speed = 0.0001f;
    // Start is called before the first frame update
    void Start()
    {
        //slide left till out of camera view then destroy itself
        cam = GetComponent<Camera>();
        
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = new Vector2(transform.position.x - (speed * Time.deltaTime), transform.position.y);
        if (gameObject.transform.position.x < -40)
        {
            Destroy(gameObject);
        }
    }
}
