using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cazaraton
{
    public class PawManager : MonoBehaviour
    {
        [SerializeField]
        GameObject GarraDerecha;
        Transform tfm_GarraDerecha;
        Vector2 posInicial_GarraDerecha;
        float timer_GarraDerecha = 0f;
        Collider2D trigger_GarraDerecha;

        [SerializeField]
        GameObject GarraIzquierda;
        Transform tfm_GarraIzquierda;
        Vector2 posInicial_GarraIzquierda;
        float timer_GarraIzquierda = 0f;
        Collider2D trigger_GarraIzquierda;

        // Start is called before the first frame update
        void Start()
        {
            tfm_GarraDerecha = GarraDerecha.transform;
            tfm_GarraIzquierda = GarraIzquierda.transform;
            trigger_GarraDerecha = GarraDerecha.GetComponent<Collider2D>();
            trigger_GarraIzquierda = GarraIzquierda.GetComponent<Collider2D>();
            trigger_GarraDerecha.enabled = false;
            trigger_GarraIzquierda.enabled = false;
            posInicial_GarraDerecha = tfm_GarraDerecha.position;
            posInicial_GarraIzquierda = tfm_GarraIzquierda.position;
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale != 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    var v3 = Input.mousePosition;
                    v3.z = 10.0f;
                    v3 = Camera.main.ScreenToWorldPoint(v3);

                    if (v3.x < 0)
                    {
                        tfm_GarraIzquierda.position = (Vector2)v3;
                        timer_GarraIzquierda = 0.2f;
                        trigger_GarraIzquierda.enabled = true;
                    }
                    else
                    {
                        tfm_GarraDerecha.position = (Vector2)v3;
                        timer_GarraDerecha = 0.2f;
                        trigger_GarraDerecha.enabled = true;
                    }
                }

                if (timer_GarraDerecha <= 0)
                {
                    if ((Vector2)tfm_GarraDerecha.position != posInicial_GarraDerecha)
                    {
                        tfm_GarraDerecha.position = posInicial_GarraDerecha;
                        trigger_GarraDerecha.enabled = false;
                    }
                }
                else
                {
                    timer_GarraDerecha -= Time.deltaTime;
                }

                if (timer_GarraIzquierda <= 0)
                {
                    if ((Vector2)tfm_GarraIzquierda.position != posInicial_GarraIzquierda)
                    {
                        tfm_GarraIzquierda.position = posInicial_GarraIzquierda;
                        trigger_GarraIzquierda.enabled = false;
                    }
                }
                else
                {
                    timer_GarraIzquierda -= Time.deltaTime;
                }
            }
        }
    }
}