using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


namespace Cazaraton
{
    public class GameManager : MonoBehaviour
    {

        public static GameManager Instance;

        [SerializeField]
        float tiempoEntreSpawneosInicial = 2f;
        [SerializeField]
        GameObject[] prefabs;
        [SerializeField, Range(0, 100)]
        int[] probabilidades;
        [SerializeField]
        AmountRange rangoDeSpawneosSimultaneos;
        float _minVertical;
        float _maxVertical;
        float _minHorizontal;
        float _maxHorizontal;

        List<int> _indexesArray = new List<int>();

        float spawnTimer = 0;

        public float tiempo;
        public int puntos;
        public int ratones;
        public int cascabeles;
        public int dorados;

        [SerializeField] TextMeshProUGUI tiempoText;
        [SerializeField] TextMeshProUGUI puntosText;
        [SerializeField] TextMeshProUGUI ratonesText;
        [SerializeField] TextMeshProUGUI cascabelesText;
        [SerializeField] TextMeshProUGUI doradosText;



        private void Awake()
        {
            Instance = this;
        }

        // Start is called before the first frame update
        void Start()
        {
            _minVertical = -Camera.main.orthographicSize;
            _maxVertical = Camera.main.orthographicSize;
            _minHorizontal = (-Camera.main.orthographicSize * Camera.main.aspect);
            _maxHorizontal = (Camera.main.orthographicSize * Camera.main.aspect);
            Debug.Log("Esquina superior izquierda en X= " + _minHorizontal + "Y= " + _maxVertical);
            Debug.Log("Esquina superior derecha en X= " + _maxHorizontal + "Y= " + _maxVertical);
            Debug.Log("Esquina inferior izquierda en X= " + _minHorizontal + "Y= " + _minVertical);
            Debug.Log("Esquina inferior derecha en X= " + _maxHorizontal + "Y= " + _minVertical);

            for (int i = 0; i < probabilidades.Length; i++)
            {
                for (int j = 0; j < probabilidades[i]; j++)
                {
                    _indexesArray.Add(i);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (spawnTimer <= 0)
            {

                for (int i = 0; i < Random.Range(rangoDeSpawneosSimultaneos.min, rangoDeSpawneosSimultaneos.max); i++)
                {

                    int index = _indexesArray[Random.Range(0, _indexesArray.Count)];
                    Vector2 instancePos = Vector2.zero;
                    if (Random.value <= 0.5f)
                    {
                        if (Random.value <= 0.5f)
                        {
                            instancePos = new Vector2(Random.Range(_minHorizontal, _maxHorizontal), _minVertical - 1);
                        }
                        else
                        {
                            instancePos = new Vector2(Random.Range(_minHorizontal, _maxHorizontal), _maxVertical + 1);
                        }
                    }
                    else
                    {
                        if (Random.value <= 0.5f)
                        {
                            instancePos = new Vector2(_minHorizontal - 1, Random.Range(_minVertical, _maxVertical));
                        }
                        else
                        {
                            instancePos = new Vector2(_maxHorizontal + 1, Random.Range(_minVertical, _maxVertical));
                        }
                    }


                    Instantiate(prefabs[index], instancePos, Quaternion.identity).GetComponent<ObjectsMovement>().finalPoint = -instancePos;
                }
                spawnTimer = tiempoEntreSpawneosInicial;
            }
            else
            {
                spawnTimer -= Time.deltaTime;
            }

            tiempo -= Time.deltaTime;

            if (tiempo > 0)
            {
                if (tiempo < 10)
                {
                    tiempoText.text = "00" + tiempo.ToString("F0");
                }
                else if (puntos < 100)
                {
                    tiempoText.text = "0" + tiempo.ToString("F0");
                }
                else
                {
                    tiempoText.text = tiempo.ToString("F0");
                }
            }
            else
            {
                PlayerPrefs.SetInt("puntuaje", PlayerPrefs.GetInt("puntuaje") + puntos);
                QuitGame();
            }
        }

        public void UpdateUI()
        {
            if (tiempo < 10)
            {
                tiempoText.text = "00" + tiempo.ToString("F0");
            }
            else if (puntos < 100)
            {
                tiempoText.text = "0" + tiempo.ToString("F0");
            }
            else
            {
                tiempoText.text = tiempo.ToString("F0");
            }

            if (puntos < 10)
            {
                puntosText.text = "00" + puntos;
            }
            else if (puntos < 100)
            {
                puntosText.text = "0" + puntos;
            }
            else
            {
                puntosText.text = puntos.ToString();
            }

            if (puntos < 10)
            {
                puntosText.text = "00" + puntos;
            }
            else if (puntos < 100)
            {
                puntosText.text = "0" + puntos;
            }
            else
            {
                puntosText.text = puntos.ToString();
            }

            if (ratones < 10)
            {
                ratonesText.text = "00" + ratones;
            }
            else if (ratones < 100)
            {
                ratonesText.text = "0" + ratones;
            }
            else
            {
                ratonesText.text = ratones.ToString();
            }

            if (cascabeles < 10)
            {
                cascabelesText.text = "00" + cascabeles;
            }
            else if (puntos < 100)
            {
                cascabelesText.text = "0" + cascabeles;
            }
            else
            {
                cascabelesText.text = cascabeles.ToString();
            }

            if (dorados < 10)
            {
                doradosText.text = "00" + dorados;
            }
            else if (puntos < 100)
            {
                doradosText.text = "0" + dorados;
            }
            else
            {
                doradosText.text = dorados.ToString();
            }
        }

        public void Pause()
        {
            Time.timeScale = 0;
        }

        public void UnPause()
        {
            Time.timeScale = 1;
        }

        public void QuitGame()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(1);
        }

    }
    [System.Serializable]
    public class AmountRange
    {
        public int min = 0;
        public int max = 0;
    }
}
