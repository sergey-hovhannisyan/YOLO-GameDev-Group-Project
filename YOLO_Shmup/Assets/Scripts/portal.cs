using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portal : MonoBehaviour
{
    public Sprite[] spriteArray;
    float next = 0.2f;
    int currSprite = 0;
    float currTime = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(currTime >= next)
        {
            currTime = 0;
            changeSprite();
        }
    }
    void changeSprite()
    {
        if(currSprite < 3)
        {
            currSprite++;
        }
        else
        {
            currSprite = 0;
        }
        GetComponent<SpriteRenderer>().sprite = spriteArray[currSprite];
    }
    
}
