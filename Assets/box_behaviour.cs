using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class box_behaviour : MonoBehaviour
{
    public Sprite wholeBox;
    public Sprite destroyedBox;
    public GameObject cat;
    public int id;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void destroyBox()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer.sprite == wholeBox)
        {
            spriteRenderer.sprite = destroyedBox;
            //instantiate cat
            Instantiate(cat, transform.position, Quaternion.identity);
        }
    }
}
