using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UniRx;
using UniRx.Async;
using System;

public class LabertintoMenuManager : MonoBehaviour
{

    [SerializeField] private GameObject menuOpciones = null;
    [SerializeField] public GameObject explicacion = null;
    [SerializeField] private GameObject menuElegirFase = null;
    
    [SerializeField] private GameObject[] objetosFase = null;


    [SerializeField] private GameObject[] niveles = null;

    [SerializeField] private GameObject playerPrefab = null;
    [SerializeField] private Transform[] posicionesPlayer = null;

    [SerializeField] private TextMeshProUGUI textoCrono = null;
    [SerializeField] private Animation animaciones = null;
    //[SerializeField] private TextMeshProUGUI textoGameover = null;
    [SerializeField] private PlayerLabMovement player = null;
    
    [SerializeField] private GameObject panelGameOver = null;
    [SerializeField] private GameObject panelPausa = null;
    [SerializeField] public GameObject panelGanador = null;

    [Header("Textos contadores")]
    [SerializeField] private TextMeshProUGUI textoContadorLatas = null;
    [SerializeField] private TextMeshProUGUI textoContadorBaterias = null;
    [SerializeField] private TextMeshProUGUI textoContadorBarril = null;

    [SerializeField] private TextMeshProUGUI textoContadorLatasGanador = null;
    [SerializeField] private TextMeshProUGUI textoContadorBateriasGanador = null;
    [SerializeField] private TextMeshProUGUI textoContadorBarrilGanador = null;



    [SerializeField] private TextMeshProUGUI textoPuntuaje = null;
    [SerializeField] private GameObject[] objetosFase1 = null;
    [SerializeField] private GameObject[] objetosFase2 = null;

    private IDisposable crono = null;
    public bool isGameOver = false;
    public bool terminadoNivel = false;
    private int currentNivel = 0;

    private void Awake()
    {
        panelGameOver.SetActive(false);
        menuOpciones.SetActive(false);
        panelPausa.SetActive(false);
        panelGanador.SetActive(false);
        terminadoNivel = false;
        
        isGameOver = false;
        
        for (ushort i = 0; i < niveles.Length; i++)
        { 
            niveles[i].SetActive(false);
            objetosFase[i].SetActive(false);
        
        }
        

    }

    // Start is called before the first frame update
    void Start()
    {

        
        Init();
        explicacion.SetActive(true);
        


    }

    private void Init()
    { 
    
        puntuajeLatas = 0;
        puntuajeBaterias = 0;
        puntuajeBarril = 0;
        textoCrono.text = "TIEMPO: 00:00";
        textoPuntuaje.text = "PUNTOS: 000";
        player.isDead = false;
        

        textoContadorLatas.text = "+ " + puntuajeLatas.ToString();
        textoContadorBarril.text = "+ " + puntuajeBarril.ToString();
        textoContadorBaterias.text = "+ " + puntuajeBaterias.ToString();
    
    
    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            menuOpciones.SetActive(true);
            
        
        
        }

       


    }

    
    public async void JugarNivel(int nivel)
    { 
        
        player.gameObject.SetActive(false);
        currentNivel = nivel;
        puntuajeLatas = 0;
        puntuajeBaterias = 0;
        puntuajeBarril = 0;

        terminadoNivel = false;
        isGameOver = false;
        textoContadorLatas.text = puntuajeLatas.ToString();
        textoContadorBarril.text = puntuajeBarril.ToString();
        textoContadorBaterias.text = puntuajeBaterias.ToString();
        textoCrono.text = "TIEMPO: 00:00";
        textoPuntuaje.text = "PUNTOS: 000";
        player.isDead = false;
        
        
        
        

        if (nivel == 0)
        { 
            for(ushort i = 0; i < objetosFase1.Length; i++)
            { 
            
                objetosFase1[i].SetActive(true);
            
            }
            animaciones.Play("Boton_Nivel1_Laberinto");
        }
        else if (nivel == 1)
        { 

            for(ushort i = 0; i < objetosFase2.Length; i++)
            { 
            
                objetosFase2[i].SetActive(true);
            
            }

            animaciones.Play("Boton_Nivel2_Laberinto");
            
        }
        await UniTask.Delay(TimeSpan.FromMilliseconds( animaciones.GetClip("Boton_Nivel2_Laberinto").length + 500) );
        
        animaciones.Stop();

        explicacion.SetActive(false);
        niveles[nivel].SetActive(true);

        if (nivel == 1)
        { 
            InitCrono(180); //180
        }
        else
        { 
            InitCrono(180);
        
        }

        objetosFase[nivel].SetActive(true);
        player.gridMoveDirection = Vector2Int.zero;
        playerPrefab.transform.position = posicionesPlayer[nivel].position;
        player.gameObject.SetActive(true);
    
    }

    private float tiempoCurrentBatalla = 0;
    public BoolReactiveProperty terminadoCrono = new BoolReactiveProperty(false);
    
    private void InitCrono(ushort tiempoSegundos)
    { 
        
        DateTime date1 = new DateTime(2019, 1, 1,9, 0, 0);
        long binLocal = date1.ToBinary();

        //int minutos = 0;
        //int horas = 9;
        tiempoCurrentBatalla = tiempoSegundos;

        terminadoCrono.Value = false;

        crono = Observable.Timer(
        TimeSpan.FromSeconds(0), //esperamos 1 segundos 
        TimeSpan.FromSeconds(1), Scheduler.MainThread).Do(x => { }).
        ObserveOnMainThread().
        //Take(tiempoSegundos + 1).
        Where(termina => terminadoCrono.Value == false)
        .Subscribe
        (_ =>
        {

            textoCrono.text =  "TIEMPO: "+ DateTime.FromBinary(binLocal).AddSeconds(tiempoCurrentBatalla).ToString("mm:ss");
            tiempoCurrentBatalla--;

            if (tiempoCurrentBatalla < 0)
            { 
                terminadoCrono.Value = true;
                print("terminado");
                if (crono != null ) crono.Dispose();
            
                player.isDead = true;
                panelGameOver.SetActive(true);
                tiempoCurrentBatalla = 0;
                isGameOver = true;
            
            }

        }
        , ex => { Debug.Log(" cuentaatrasantes OnError:" + ex.Message); if (crono != null) crono.Dispose(); },
        () => //completado
        {
            print("terminado");
            if (crono != null ) crono.Dispose();
            
            player.isDead = true;
            panelGameOver.SetActive(true);
            tiempoCurrentBatalla = 0;
            isGameOver = true;
            

        }).AddTo(this.gameObject);
    
    
    }

    public void AmpliarCrono(int tiempoAnadido)
    { 
        
        tiempoCurrentBatalla += tiempoAnadido;
        
    }


    


    public void ShowMenuPause()
    { 
        
        if (isGameOver == true || terminadoNivel == true) return;
        panelPausa.SetActive(true);
        Time.timeScale = 0;

        
    
    
    }

    public void CloseMenuPause()
    { 
    
        Time.timeScale = 1;
        panelPausa.SetActive(false);
    
    }


    public async void SalirMiniJuego()
    {
        Time.timeScale = 1;
        await UniTask.Delay(TimeSpan.FromSeconds(1));
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }


    public void SetGanador()
    {
        terminadoNivel = true;
        textoContadorLatasGanador.text = "+ " + puntuajeLatas.ToString();
        textoContadorBateriasGanador.text = "+ " + puntuajeBaterias.ToString();
        textoContadorBarrilGanador.text = "+ " + puntuajeBarril.ToString();


        panelGanador.SetActive(true);
        terminadoCrono.Value = true;

    
    }
    
    private ushort puntuajeLatas = 0;
    private ushort puntuajeBaterias = 0;
    private ushort puntuajeBarril = 0;

    public void SetLatas()
    { 
        int puntuajeTotal = PlayerPrefs.GetInt("puntuaje");
        puntuajeTotal++;
        PlayerPrefs.SetInt("puntuaje", puntuajeTotal);

        puntuajeLatas++;
        textoContadorLatas.text = "+ " + puntuajeLatas.ToString();
        
        int puntuajeSuma = puntuajeLatas + puntuajeBaterias + puntuajeBarril;
        textoPuntuaje.text = "PUNTOS: " + puntuajeSuma.ToString("N0");
        
    
    }

    public void SetBaterias()
    { 
        int puntuajeTotal = PlayerPrefs.GetInt("puntuaje");
        puntuajeTotal++;
        PlayerPrefs.SetInt("puntuaje", puntuajeTotal);

        puntuajeBaterias++;
        textoContadorBaterias.text = "+ " + puntuajeBaterias.ToString();
        
        int puntuajeSuma = puntuajeLatas + puntuajeBaterias + puntuajeBarril;
        textoPuntuaje.text = "PUNTOS: " + puntuajeSuma.ToString("N0");
        AmpliarCrono(30);
    
    }


    public void SetBarril()
    { 
        int puntuajeTotal = PlayerPrefs.GetInt("puntuaje");
        puntuajeTotal++;
        PlayerPrefs.SetInt("puntuaje", puntuajeTotal);

        puntuajeBarril++;
        textoContadorBarril.text = "+ " + puntuajeBaterias.ToString();
        
        int puntuajeSuma = puntuajeLatas + puntuajeBaterias + puntuajeBarril;
        textoPuntuaje.text = "PUNTOS: " + puntuajeSuma.ToString("N0");
    
        AmpliarCrono(60);
    
    }

    public void RepetirNivel()
    { 
        player.gameObject.SetActive(false);
        Awake();
        Init();
        panelGameOver.SetActive(false);
        JugarNivel(currentNivel);
        
    
    
    }


    public void SiguienteNivel()
    { 
        player.gameObject.SetActive(false);
        Awake();
        Init();
        panelGameOver.SetActive(false);
        if (currentNivel == 0)
        { 
            JugarNivel(1);
        
        }
        else if (currentNivel == 1)
        { 
        
            JugarNivel(0);
        
        }
        player.gameObject.SetActive(true);

        
    
    
    
    
    }
    
    

    
   


}
