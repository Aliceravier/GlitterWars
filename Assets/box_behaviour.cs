using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box_behaviour : MonoBehaviour
{
    public Sprite wholeBox;
    public Sprite destroyedBox;
    public GameObject cat;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void destroyBox()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = destroyedBox;
        //instantiate cat
        Instantiate(cat);
        Destroy(gameObject);
    }
}
