using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("GameHandler.Start");

        GameObject carSpriteObj = new GameObject();
        SpriteRenderer carSpriteRend = carSpriteObj.gameObject.AddComponent<SpriteRenderer>();
        carSpriteRend.sprite = GameAssets.i.carSprite;    
    }


    // Update is called once per frame
    void Update()
    {

    }

}


