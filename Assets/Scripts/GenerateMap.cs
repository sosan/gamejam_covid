using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateMap : MonoBehaviour
{

    [SerializeField] private Texture2D mapa = null;
    [SerializeField] private GameObject prefabCuadrado = null;
    [SerializeField] private GameObject parentMapa = null;

    float posicionX = 0;
    float posicionY = 0;

    // Start is called before the first frame update
    void Start()
    {
        GenerarMapa();
    }


    private void GenerarMapa()
    { 

        for (int x = 0; x < mapa.width; x += 48)
        { 
            
            for (int y = 0; y <  mapa.height; y += 48)
            { 
                
                GenerarCuadro(x,y);
            
            }
        }
    
    }

    private void GenerarCuadro(int x, int y)
    { 
        var colorPixel = mapa.GetPixel(x, y);
        if (colorPixel.a == 0) return;
       
        var obj = GameObject.Instantiate(prefabCuadrado, new Vector3(x / 48, y / 48, 0), Quaternion.identity, parentMapa.transform);
       
        obj.name = "cuadrado" + x + "_" + y;
    
    }


    
}
