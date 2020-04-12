using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{

    [SerializeField] private Texture2D mapa = null;
    [SerializeField] private GameObject prefabCuadrado = null;
    [SerializeField] private GameObject parentMapa = null;
    [SerializeField] private float prefabCellSize = 0.48f;
    [SerializeField] private int cellPixelSizeInTexture = 1;

    [SerializeField] private Color colorRejillas = Color.white;

    void Start()
    {
        GenerarMapa();
    }


    private void GenerarMapa()
    { 

        Vector2 spawnPos = Vector2.zero;

        for (int x = 0; x < mapa.width; x += cellPixelSizeInTexture)
        { 
            
            for (int y = 0; y <  mapa.height; y += cellPixelSizeInTexture)
            { 
                
                GenerarCuadro(spawnPos, x, y);
                spawnPos.y += prefabCellSize;
            
            }

            spawnPos.y = 0;
            spawnPos.x += prefabCellSize;
        }
    
    }

    private void GenerarCuadro(Vector2 pos, int keyPosx, int keyPosy)
    { 
        var colorPixel = mapa.GetPixel(keyPosx, keyPosy);
        
        // pixel con alpha no se pinta
        if (colorPixel.a == 0) return;

        //TODO: segun el pixel color, instanciar el tipo de prefabs
        //if (colorPixel == colorRejillas)
        //{ 
        
        
        //}

        if (colorPixel == Color.black)
        { 
            var obj = GameObject.Instantiate(prefabCuadrado, pos, Quaternion.identity, parentMapa.transform);
            obj.name = "celda_x=" + keyPosx + "_y=" + keyPosy;
       
        
        }

        
    
    }


    
}
