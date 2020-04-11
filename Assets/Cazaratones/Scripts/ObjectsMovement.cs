using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cazaraton
{
    public class ObjectsMovement : MonoBehaviour
    {
        enum TipoDeMovimiento
        {
            recto,
            ondulado,
            espiral
        }

        [SerializeField]
        TipoDeMovimiento tipoDeMovimiento;

        [SerializeField]
        float speed = 1f;
        public Vector2 finalPoint;
        Vector2 _direction;
        float _onduladoTimer = 0.1f;
        float onduladoDesviacion = 1f;

        float timeAlive = 0f;

        float _minVertical;
        float _maxVertical;
        float _minHorizontal;
        float _maxHorizontal;

        // Start is called before the first frame update
        void Start()
        {
            _direction = (finalPoint - (Vector2)transform.position);

            //if (tipoDeMovimiento == TipoDeMovimiento.espiral)
            //{
            //    _minVertical = -Camera.main.orthographicSize;
            //    _maxVertical = Camera.main.orthographicSize;
            //    _minHorizontal = (-Camera.main.orthographicSize * Camera.main.aspect);
            //    _maxHorizontal = (Camera.main.orthographicSize * Camera.main.aspect);
            //    transform.position = new Vector2(Random.Range(_minHorizontal, _maxHorizontal), Random.Range(_minVertical, _maxVertical));
            //}

            transform.rotation = Quaternion.LookRotation(-Vector3.forward, _direction);
        }

        // Update is called once per frame
        void Update()
        {
            if (Time.timeScale != 0)
            {
                switch (tipoDeMovimiento)
                {
                    case TipoDeMovimiento.recto:
                        transform.rotation = Quaternion.LookRotation(-Vector3.forward, _direction);
                        transform.position = transform.position + (transform.up * speed * Time.deltaTime);
                        break;
                    case TipoDeMovimiento.ondulado:
                        if (_onduladoTimer <= 0)
                        {
                            transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z + Random.Range(-25f, 25f));
                            _onduladoTimer = 0.1f;
                            speed += 1f;
                        }
                        else
                        {
                            _onduladoTimer -= Time.deltaTime;
                        }
                        transform.position = transform.position + (transform.up * speed * Time.deltaTime);
                        break;
                    case TipoDeMovimiento.espiral:

                        transform.RotateAround(transform.up * 0.05f, -transform.forward, onduladoDesviacion * Time.deltaTime);
                        transform.position = transform.position + (transform.up * speed * Time.deltaTime);
                        break;
                    default:
                        break;
                }

                timeAlive += Time.deltaTime;

                if (Vector2.Distance(transform.position, finalPoint) <= 0.2f || timeAlive >= 2f)
                {
                    Destroy(gameObject);
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GetComponent<PointsCollector>().CollectPoints();
            collision.enabled = false;
            Destroy(gameObject);
        }
    }
}