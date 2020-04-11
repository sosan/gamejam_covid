using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cazaraton
{
    public class PointsCollector : MonoBehaviour
    {
        enum TipoObjeto
        {
            raton,
            cascabel,
            dorado
        }

        [SerializeField]
        TipoObjeto tipoObjeto;

        [SerializeField]
        float tiempoExtra;
        [SerializeField]
        int puntos;



        public void CollectPoints()
        {
            switch (tipoObjeto)
            {
                case TipoObjeto.raton:
                    GameManager.Instance.ratones++;
                    break;
                case TipoObjeto.cascabel:
                    GameManager.Instance.cascabeles++;
                    break;
                case TipoObjeto.dorado:
                    GameManager.Instance.dorados++;
                    break;
                default:
                    break;
            }
            GameManager.Instance.puntos += puntos;
            GameManager.Instance.tiempo += tiempoExtra;
            GameManager.Instance.UpdateUI();
        }
    }
}