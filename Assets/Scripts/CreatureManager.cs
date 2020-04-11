using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UniRx;
using UniRx.Async;
using System;


public class CreatureManager : MonoBehaviour
{

    [SerializeField] private Image indicadorComida = null;
    [SerializeField] private Image indicadorFelicidad = null;
    [SerializeField] private Image indicadorAburrimiento = null;
	[SerializeField] private Image indicadorSalud = null;
	[SerializeField] private TextMeshProUGUI textoNivel = null;
	[SerializeField] private TextMeshProUGUI textoPuntos = null;
	
	[SerializeField] private TextMeshProUGUI textoCantidadMonedas = null;
	[SerializeField] private TextMeshProUGUI mensajeBocadillo = null;
	[SerializeField] private GameObject bocadillo = null;

	[SerializeField] private ParticleSystem[] particulasTomogochi = null;
	[SerializeField] private Image rellenoPescado = null;
	[SerializeField] private Image rellenoAgua = null;
	[SerializeField] private Image rellenoLimpiar = null;
	[SerializeField] private Image rellenoJugar_1 = null;
	[SerializeField] private Image rellenoJugar_2 = null;


	[Header("Letras")]
	[SerializeField] private GameObject letrasPrefab = null;
	[SerializeField] private Transform posicionLetras = null;
	[SerializeField] private Animation animacionPlayer = null;


	[Header("Sin puntos")]
	[SerializeField] private GameObject sinPuntosPrefab = null;
	[SerializeField] private Transform[] posicionsinPuntos = null;
	


	[SerializeField] private TextMeshProUGUI textoMuerte = null;

	[SerializeField] private Image anillosPrefab = null;
	[SerializeField] private Image ojosPrefab = null;
	[SerializeField] private Sprite[] secuenciaAnillos = null;
	[SerializeField] private Sprite[] secuenciaOjos = null;
	[SerializeField] private GameObject[] alimentosCuadrados = null;

	[SerializeField] public int monedas = 10000;

	[Header("Colores Ojos")]
	[SerializeField] private Color colorOjosNormal = Color.white;
	[SerializeField] private Color colorOjosEnfadado = Color.white;
	[SerializeField] private Color colorOjosTristes = Color.white;


	[Header("Progresos")]
	[SerializeField] private float m_Ambriento = 0.003f; 
	[SerializeField] private float m_CacaPresion = 0.02f; 
	[SerializeField] private float m_Felicidad = 0.005f; 
	[SerializeField] private float m_FelicidadAmbriento = 0.05f; 

	
	

    private float felicidad = 70f;
	public int nivel = 1;
	public int puntos = 5000; 
	private int progresoNivel = 0;
	private int cacaContador = 0;
	private float muerteContador = 100f;
	[HideInInspector] public float comida = 89f;
	private float cacaPresion = 0f;
	private float aburrimiento = 0f;
	

	public bool isSick = false;
	public bool isDead = false;
	public bool isHungry = false;
	public bool isBored = false;
	public bool isAsleep = false;
	public bool isMinigameRunning = false;


	System.Diagnostics.Stopwatch timer;

	
	private IDisposable disposableBotonComida = null;
	private IDisposable disposableBotonAgua = null;
	private IDisposable disposableBotonLimpiar = null;
	private IDisposable disposableBotonJugar_1 = null;
	private IDisposable disposableBotonJugar_2 = null;

	//private IDisposable disposableAnimationBarraAlimento = null;


	//
	private float internalVelocidadProgresoAlimento = 0.003f;
	private float internalVelocidadProgresoFelicidad = 0.005f;
	private float internalVelocidadCooldownPescado = 3f;
	private float internalVelocidadCooldownAgua = 3f;
	private float internalVelocidadCooldownLimpiar = 3f;
	private float internalVelocidadCooldownRaton = 3f;
	private float internalVelocidadCooldownCorazon = 3f;

	[Header("progresos")]
	[SerializeField] private TextMeshProUGUI textoProgresoAlimento = null;
	[SerializeField] private TextMeshProUGUI textoProgresoFelicidad = null;
	[SerializeField] private TextMeshProUGUI textoCooldownComida = null;
	[SerializeField] private TextMeshProUGUI textoCooldownAgua = null;
	[SerializeField] private TextMeshProUGUI textoCooldownLimpiar = null;
	[SerializeField] private TextMeshProUGUI textoCooldownRaton = null;
	[SerializeField] private TextMeshProUGUI textoCooldownCorazon = null;

	[Header("Alimento")]
	[SerializeField] private Animation[] animationsBarraAlimento = null;


	void Start()
    {



		//progresos
		{
			
			ActualizarProgreso();

			
		
		}

		if (PlayerPrefs.HasKey("guardadoComida") == true)
		{ 
		
			int isSaved = PlayerPrefs.GetInt("guardadoComida");
			if (isSaved == 0)
			{ 
				comida = 89f;
			}
			else
			{ 
				comida = PlayerPrefs.GetFloat("comida");

				PlayerPrefs.SetFloat("guardadoComida", 0);
			}
		
		}



		mensajeBocadillo.text = "";
		bocadillo.SetActive(false);
		//ActualizarNivel();
		PrimeraActualizacionPuntos();
		ActualizarMonedas();

		rellenoPescado.fillAmount = 0;
		rellenoAgua.fillAmount = 0;
		rellenoLimpiar.fillAmount = 0;
		rellenoJugar_1.fillAmount = 0;
		rellenoJugar_2.fillAmount = 0;
		ojosPrefab.color = colorOjosNormal;

		textoMuerte.text = "";

		InitCooldowns();

		InitAnimations();


        
    }

	private ushort contadorAnimations = 0;
	private ushort contadorOjosAnimations = 0;

	private void InitAnimations()
	{ 
		IDisposable animationAnillos = null;
		 animationAnillos = Observable
        .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(250))
        .Subscribe(time =>
        {

            contadorAnimations++;
			if (contadorAnimations == secuenciaAnillos.Length)
			{ 
			
				contadorAnimations = 0;
			
			}
			
			anillosPrefab.sprite = secuenciaAnillos[contadorAnimations];
			
               

        }
        , ex => { Debug.Log("OnError:" + ex.Message); animationAnillos.Dispose(); },
        () => //completado
        {

			
            if (animationAnillos != null) animationAnillos.Dispose();

        }).AddTo(this.gameObject);
	

		IDisposable animationOjos = null;
		 animationOjos = Observable
        .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(200))
        .Subscribe(time =>
        {

            contadorOjosAnimations++;
			if (contadorOjosAnimations == secuenciaOjos.Length)
			{ 
			
				contadorOjosAnimations = 0;
			
			}
			
			ojosPrefab.sprite = secuenciaOjos[contadorOjosAnimations];
			
               

        }
        , ex => { Debug.Log("OnError:" + ex.Message); animationAnillos.Dispose(); },
        () => //completado
        {

			
            if (animationAnillos != null) animationAnillos.Dispose();

        }).AddTo(this.gameObject);


		
		

		





	
	}



    // Update is called once per frame
    void Update()
    {
        

        if (isDead)
        {
			return;
		}
        

        comida -= internalVelocidadProgresoAlimento; //m_Ambriento; // 0.003f; 
		cacaPresion += m_CacaPresion; // 0.02f;


		if (isBored) 
		{
			felicidad -= internalVelocidadProgresoFelicidad;//m_Felicidad; // 0.005f;
		} 
		else
		{ 
			aburrimiento += 0.010f;  //0.002f;
		
		}
		

		if (isSick) 
		{
			felicidad -= 0.010f;// 0.005f;
			muerteContador -= 0.010f; //0.05f;
			ojosPrefab.color = colorOjosTristes;
		}

		if (isHungry) 
		{
			felicidad -=  m_FelicidadAmbriento; //0.05f;
		}

		if (comida < 30f && isHungry == false) 
		{
			
			isHungry = true;
			//falta animacion del tamagochi
			mensajeBocadillo.text = "Tengo<br>Hambre!";
			bocadillo.SetActive(true);
			ojosPrefab.color = colorOjosEnfadado;

			

			
		}

		if (aburrimiento > 80f && isBored == false) 
		{
			SetAburrido ();
		}

		if (cacaPresion >= 100f) 
		{
			SetSucioCaca();
			cacaPresion = UnityEngine.Random.Range (0, 20f);
		}


		if (!isAsleep && (System.DateTime.Now.Hour > 23 || System.DateTime.Now.Hour < 7)) 
		{
			SetDormido ();
		} 
		else if (isAsleep && (System.DateTime.Now.Hour > 7 && System.DateTime.Now.Hour < 23))
		{
			SetDespierto ();
		}
			
		if (felicidad <= 0 || muerteContador <= 0 || comida <= 0) 
		{
			SetMuerto();
		}


		ActualizarAlimentos();




		//for (int i = 10; i > (int)(comida / 10) ; i--)
		//{ 
		//	alimentosCuadrados[i - 1].SetActive(false);
			
		//}
		indicadorComida.fillAmount = comida / 100f;
		indicadorFelicidad.fillAmount = felicidad/100f;


    }

	private void ActualizarAlimentos()
	{ 
		
		if (comida > 90 )
		{ 

			for (ushort i = 0; i < 10; i++)
			{ 
				
				alimentosCuadrados[i].SetActive(true);

			}

			
		}
		else
		{ 
			if (comida > 80 && comida <= 90 )
			{ 
				
				animationsBarraAlimento[0].Stop();
				animationsBarraAlimento[1].Stop();
				animationsBarraAlimento[2].Stop();

							
				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(true);
				alimentosCuadrados[4].SetActive(true);
				alimentosCuadrados[5].SetActive(true);
				alimentosCuadrados[6].SetActive(true);
				alimentosCuadrados[7].SetActive(true);
				alimentosCuadrados[8].SetActive(true);
				alimentosCuadrados[9].SetActive(false);
			}
		
			else if (comida > 70 && comida <= 80 )
			{ 
				animationsBarraAlimento[0].Stop();
				animationsBarraAlimento[1].Stop();
				animationsBarraAlimento[2].Stop();

				

				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(true);
				alimentosCuadrados[4].SetActive(true);
				alimentosCuadrados[5].SetActive(true);
				alimentosCuadrados[6].SetActive(true);
				alimentosCuadrados[7].SetActive(true);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}


			else if (comida > 60 && comida <= 70 )
			{ 
				animationsBarraAlimento[0].Stop();
				animationsBarraAlimento[1].Stop();
				animationsBarraAlimento[2].Stop();

				
			
				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(true);
				alimentosCuadrados[4].SetActive(true);
				alimentosCuadrados[5].SetActive(true);
				alimentosCuadrados[6].SetActive(true);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}

			else if (comida > 50 && comida <= 60 )
			{ 
				animationsBarraAlimento[0].Stop();
				animationsBarraAlimento[1].Stop();
				animationsBarraAlimento[2].Stop();

			
				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(true);
				alimentosCuadrados[4].SetActive(true);
				alimentosCuadrados[5].SetActive(true);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}


			else if (comida > 40 && comida <= 50 )
			{ 

				animationsBarraAlimento[0].Stop();
				animationsBarraAlimento[1].Stop();
				animationsBarraAlimento[2].Stop();

				alimentosCuadrados[0].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[1].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[2].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[3].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[4].GetComponent<Image>().color = colorOjosTristes;

				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(true);
				alimentosCuadrados[4].SetActive(true);
				alimentosCuadrados[5].SetActive(false);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}


			else if (comida > 30 && comida <= 40 )
			{ 
				animationsBarraAlimento[0].Stop();
				animationsBarraAlimento[1].Stop();
				animationsBarraAlimento[2].Stop();


				alimentosCuadrados[0].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[1].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[2].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[3].GetComponent<Image>().color = colorOjosTristes;
				alimentosCuadrados[4].GetComponent<Image>().color = colorOjosTristes;
			
				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(true);
				alimentosCuadrados[4].SetActive(false);
				alimentosCuadrados[5].SetActive(false);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}
			else if (comida > 20 && comida <= 30 )
			{ 
			
				alimentosCuadrados[0].GetComponent<Image>().color = colorOjosEnfadado;
				alimentosCuadrados[1].GetComponent<Image>().color = colorOjosEnfadado;
				alimentosCuadrados[2].GetComponent<Image>().color = colorOjosEnfadado;

				animationsBarraAlimento[0].Play();
				animationsBarraAlimento[1].Play();
				animationsBarraAlimento[2].Play();


				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(true);
				alimentosCuadrados[3].SetActive(false);
				alimentosCuadrados[4].SetActive(false);
				alimentosCuadrados[5].SetActive(false);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}
			else if ( comida > 10 && comida <= 20 )
			{ 
				animationsBarraAlimento[0].Play();
				animationsBarraAlimento[1].Play();
				animationsBarraAlimento[2].Play();
				alimentosCuadrados[0].GetComponent<Image>().color = colorOjosEnfadado;
				alimentosCuadrados[1].GetComponent<Image>().color = colorOjosEnfadado;
				alimentosCuadrados[2].GetComponent<Image>().color = colorOjosEnfadado;
			
				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(true);
				alimentosCuadrados[2].SetActive(false);
				alimentosCuadrados[3].SetActive(false);
				alimentosCuadrados[4].SetActive(false);
				alimentosCuadrados[5].SetActive(false);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}

			else if (comida > 0 && comida <= 10 )
			{ 
				animationsBarraAlimento[0].Play();
				animationsBarraAlimento[1].Play();
				animationsBarraAlimento[2].Play();
				alimentosCuadrados[0].GetComponent<Image>().color = colorOjosEnfadado;
				alimentosCuadrados[1].GetComponent<Image>().color = colorOjosEnfadado;
				alimentosCuadrados[2].GetComponent<Image>().color = colorOjosEnfadado;

				alimentosCuadrados[0].SetActive(true);
				alimentosCuadrados[1].SetActive(false);
				alimentosCuadrados[2].SetActive(false);
				alimentosCuadrados[3].SetActive(false);
				alimentosCuadrados[4].SetActive(false);
				alimentosCuadrados[5].SetActive(false);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}
		
			else if (comida <= 0 )
			{ 
				print("aqui");
				alimentosCuadrados[0].SetActive(false);
				alimentosCuadrados[1].SetActive(false);
				alimentosCuadrados[2].SetActive(false);
				alimentosCuadrados[3].SetActive(false);
				alimentosCuadrados[4].SetActive(false);
				alimentosCuadrados[5].SetActive(false);
				alimentosCuadrados[6].SetActive(false);
				alimentosCuadrados[7].SetActive(false);
				alimentosCuadrados[8].SetActive(false);
				alimentosCuadrados[9].SetActive(false);
			}


		}

	
	
	
	
	}


	private void SetMuerto()
	{ 
		if (nivel > 0)
		{
			//falta animacion de tamagochi
			//falta sonido muerto
			
			// falta imagen muerto
			textoMuerte.text = "MUERTO!";
			isDead = true;
		}
	
	}


	private void SetDespierto()
	{ 
	
		//falta imagen despierto
	}

	private void SetDormido()
	{ 
	
		//falta imagen dormido
	}

	private void SetSucioCaca()
	{ 
	
	
	}


	private void SetAburrido()
	{ 
	
	
	}


	public void startTimer()
	{
		timer.Start();
	}

	private void ActualizarNivel()
	{ 
		if (nivel < 10)
		{ 
		
			textoNivel.text = "NIVEL 0" + nivel.ToString("N0");
		}
		else
		{ 
			textoNivel.text = "NIVEL " + nivel.ToString("N0");
		
		}
		
		
	
	}


	private void SetearPuntos()
	{ 
	
		PlayerPrefs.SetInt("puntuaje", puntos);
	
	}

	private void ActualizarPuntos()
	{ 



		int puntuaje = PlayerPrefs.GetInt("puntuaje");

		puntos = puntuaje;
		textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");
		PlayerPrefs.SetInt("puntuaje", puntos);
		
	
	}

	private void PrimeraActualizacionPuntos()
	{ 

		print("puntos" + puntos);
		if (PlayerPrefs.HasKey("puntuaje"))
		{ 
		
			int puntuaje = PlayerPrefs.GetInt("puntuaje");
			puntos = puntuaje;
			textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");
			
		}
		else
		{ 
		
			PlayerPrefs.SetInt("puntuaje", puntos);
			textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");
		
		}
		print("puntos" + puntos);
	
	}


	private void ActualizarMonedas()
	{ 
	
		monedas--;
		if (monedas <= 0)
		{ 
			monedas = 0;
		}


		if (monedas < 10)
		{ 
		
			textoCantidadMonedas.text = "x0" + monedas.ToString("N0");
		}
		else
		{ 
			textoCantidadMonedas.text = "x" + monedas.ToString("N0");
		
		}
	
	}

	private BoolReactiveProperty botonPescadoCooldown = new BoolReactiveProperty(false);
	private BoolReactiveProperty botonAguaCooldown = new BoolReactiveProperty(false);
	private BoolReactiveProperty botonLimpiarCooldown = new BoolReactiveProperty(false);
	private BoolReactiveProperty botonJugar_1_Cooldown = new BoolReactiveProperty(false);
	private BoolReactiveProperty botonJugar_2_Cooldown = new BoolReactiveProperty(false);
	//private BoolReactiveProperty animationBarraAlimento = new BoolReactiveProperty(false);

	private bool botonPescadoBusy = false;
	private bool botonAguaBusy = false;
	private bool botonLimpiarBusy = false;

	private bool botonjugar1Busy = false;
	private bool botonjugar2Busy = false;


    public async void ClickBotonPescado()
    { 
		
		
		if (isDead == true) return;
		if (comida >= 100) return;
		if (botonPescadoCooldown.Value == true) return;
		if (botonPescadoBusy == true) return;

		if (puntos <= 0 || puntos < 100)
		{ 
			var puntos = GameObject.Instantiate(sinPuntosPrefab, posicionsinPuntos[0]);
			puntos.GetComponent<TextMeshProUGUI>().text = "SIN PUNTOS";
			Destroy(puntos, 3);

			botonPescadoBusy = true;
			
			await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownPescado * 100));
			botonPescadoBusy = false;

			return;
		}
		


		puntos -= 100;
		if (puntos <= 0)
		{ 
			puntos = 0;
		
		}

		//print("comida=" + comida);

		particulasTomogochi[0].Play();
		comida++;

		if (comida >= 100)
		{ 
			comida = 100;
		
		}

		progresoNivel++;
		if (progresoNivel == 20)
		{ 
			nivel++;
			progresoNivel = 0;
		
		}

		rellenoPescado.enabled = true;
		rellenoPescado.fillAmount = 1;
		
		var letras = GameObject.Instantiate(letrasPrefab, posicionLetras);
        letras.GetComponent<TextMeshProUGUI>().text = "+100 alimento";
        Destroy(letras, 5);

		animacionPlayer.Play("Tomogochi_DadoComida");

        ActualizarMonedas();
		//ActualizarPuntos();
		//ActualizarNivel();
		textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");
		print("ms=" + internalVelocidadCooldownPescado * 100);
		botonPescadoCooldown.Value = true;
		await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownPescado * 100));
		botonPescadoCooldown.Value = false;
		rellenoPescado.enabled = false;
		animacionPlayer.Play("Tomogochi_Danzarin");

    }

	public async void ClickBotonAgua()
    { 
		
		
		if (isDead == true) return;
		if (comida >= 100) return;
		if (botonAguaCooldown.Value == true) return;
		if (botonAguaBusy == true) return;

		if (puntos <= 0 || puntos < 200)
		{ 
			var puntos = GameObject.Instantiate(sinPuntosPrefab, posicionsinPuntos[1]);
			puntos.GetComponent<TextMeshProUGUI>().text = "SIN PUNTOS";
			Destroy(puntos, 3);

			botonAguaBusy = true;
			await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownAgua * 100));
			botonAguaBusy = false;

			return;
		}
		

		particulasTomogochi[1].Play();
		comida += 2;
		if (comida >= 100)
		{ 
			comida = 100;
		
		}

		puntos -= 200;
		if (puntos <= 0)
		{ 
			puntos = 0;
		
		}

		progresoNivel++;
		if (progresoNivel == 20)
		{ 
			nivel++;
			progresoNivel = 0;
		
		}

		rellenoAgua.enabled = true;
		rellenoAgua.fillAmount = 1;
		
		var letras = GameObject.Instantiate(letrasPrefab, posicionLetras);
        letras.GetComponent<TextMeshProUGUI>().text = "+200 alimento";
        Destroy(letras, 5);

		animacionPlayer.Play("Tomogochi_DadoComida");

        ActualizarMonedas();
		//ActualizarPuntos();
		//ActualizarNivel();
		textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");

		botonAguaCooldown.Value = true;
		await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownAgua * 100));
		botonAguaCooldown.Value = false;
		rellenoAgua.enabled = false;
		animacionPlayer.Play("Tomogochi_Danzarin");
    }

	public async void Click_BotonLimpiar()
    { 
		if (isDead == true) return;
		if (felicidad >= 100) return;
		if (botonLimpiarCooldown.Value == true) return;
		if (botonLimpiarBusy == true) return;

		if (puntos <= 0 || puntos < 300)
		{ 
			var puntos = GameObject.Instantiate(sinPuntosPrefab, posicionsinPuntos[2]);
			puntos.GetComponent<TextMeshProUGUI>().text = "SIN PUNTOS";
			Destroy(puntos, 3);

			botonLimpiarBusy = true;
			await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownLimpiar * 100));
			botonLimpiarBusy = false;

			return;
		}
		if (cacaPresion <= 0) return;

		particulasTomogochi[2].Play();
		cacaPresion--;
		felicidad++;

		puntos -= 300;
		if (puntos <= 0)
		{ 
			puntos = 0;
		
		}

		if (felicidad >= 100)
		{ 
			felicidad = 100;
		
		}


		rellenoLimpiar.enabled = true;
		rellenoLimpiar.fillAmount = 1;
		
		var letras = GameObject.Instantiate(letrasPrefab, posicionLetras);
        letras.GetComponent<TextMeshProUGUI>().text = "+100 felicidad";
        Destroy(letras, 5);

		animacionPlayer.Play("Tomogochi_DadoComida");

        ActualizarMonedas();
		//ActualizarPuntos();
		//ActualizarNivel();
		textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");

		botonLimpiarCooldown.Value = true;
		await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownLimpiar * 100));
		botonLimpiarCooldown.Value = false;
		rellenoLimpiar.enabled = false;
		animacionPlayer.Play("Tomogochi_Danzarin");
    }

    public async void Click_BotonJugar_1()
    { 
		if (isDead == true) return;

		if (felicidad >= 100) return;
		if (comida >= 100) return;
		if (botonJugar_1_Cooldown.Value == true) return;
		if (botonjugar1Busy == true) return;

		if (puntos <= 0 || puntos < 400)
		{ 
			var puntos = GameObject.Instantiate(sinPuntosPrefab, posicionsinPuntos[3]);
			puntos.GetComponent<TextMeshProUGUI>().text = "SIN PUNTOS";
			Destroy(puntos, 3);

			botonjugar1Busy = true;
			await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownRaton * 100));
			botonjugar1Busy = false;

			return;
		}
		if (comida <= 0) return;

		puntos -= 400;
		if (puntos <= 0)
		{ 
			puntos = 0;
		
		}

		particulasTomogochi[3].Play();
		felicidad++;
		comida++;

		if (felicidad >= 100)
		{ 
			felicidad = 100;
		
		}
		if (comida >= 100)
		{ 
			comida = 100;
		
		}



		rellenoJugar_1.enabled = true;
		rellenoJugar_1.fillAmount = 1;
		
		var letras = GameObject.Instantiate(letrasPrefab, posicionLetras);
        letras.GetComponent<TextMeshProUGUI>().text = "+100 alimento<br>+100 felicidad";
        Destroy(letras, 5);

		animacionPlayer.Play("Tomogochi_DadoComida");

		ActualizarMonedas();
		//ActualizarPuntos();
		//ActualizarNivel();
		textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");


		botonJugar_1_Cooldown.Value = true;
		await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownRaton * 100));
		botonJugar_1_Cooldown.Value = false;
		rellenoJugar_1.enabled = false;
		animacionPlayer.Play("Tomogochi_Danzarin");

    
    }




	
	

    public async void Click_BotonJugar_2()
    { 
		if (isDead == true) return;
		if (felicidad >= 100) return;
		if (botonJugar_2_Cooldown.Value == true) return;
		if (botonjugar2Busy == true) return;

		if (puntos <= 0 || puntos < 500)
		{ 
			var puntos = GameObject.Instantiate(sinPuntosPrefab, posicionsinPuntos[4]);
			puntos.GetComponent<TextMeshProUGUI>().text = "SIN PUNTOS";
			Destroy(puntos, 3);

			botonjugar2Busy = true;
			await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownCorazon * 100));
			botonjugar2Busy = false;

			return;
		}

		if (aburrimiento <= 0) return;
		

		particulasTomogochi[4].Play();
		aburrimiento--;
		felicidad += 2;

		puntos -= 500;
		if (puntos <= 0)
		{ 
			puntos = 0;
		
		}
		
		if (felicidad >= 100)
		{ 
			felicidad = 100;
		
		}
		

		rellenoJugar_2.enabled = true;
		rellenoJugar_2.fillAmount = 1;
		
		var letras = GameObject.Instantiate(letrasPrefab, posicionLetras);
        letras.GetComponent<TextMeshProUGUI>().text = "+200 felicidad";
        Destroy(letras, 5);

		animacionPlayer.Play("Tomogochi_DadoComida");

        ActualizarMonedas();
		//ActualizarPuntos();
		//ActualizarNivel();
		textoPuntos.text = "PUNTOS: " + puntos.ToString("N0");

		botonJugar_2_Cooldown.Value = true;
		await UniTask.Delay(TimeSpan.FromMilliseconds(internalVelocidadCooldownCorazon * 100));
		botonJugar_2_Cooldown.Value = false;
		rellenoJugar_2.enabled = false;
    
		animacionPlayer.Play("Tomogochi_Danzarin");


    }



	private void InitCooldowns()
	{ 
	
	
		botonPescadoCooldown.Where(_ => botonPescadoCooldown.Value == true).Subscribe(
        _ =>
        {
            
            disposableBotonComida = Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(30))
            .Take(10)
            .Subscribe(time =>
            {

                rellenoPescado.fillAmount -= 0.1f;
               

            }
            , ex => { Debug.Log("OnError:" + ex.Message); disposableBotonComida.Dispose(); },
            () => //completado
            {

				rellenoPescado.fillAmount = 1;
                if (disposableBotonComida != null) disposableBotonComida.Dispose();

            }).AddTo(this.gameObject);




        }).AddTo(this.gameObject);


		botonAguaCooldown.Where(_ => botonAguaCooldown.Value == true).Subscribe(
        _ =>
        {
            
            disposableBotonAgua = Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(30))
            .Take(10)
            .Subscribe(time =>
            {

                rellenoAgua.fillAmount -= 0.1f;
               

            }
            , ex => { Debug.Log("OnError:" + ex.Message); disposableBotonAgua.Dispose(); },
            () => //completado
            {

				rellenoAgua.fillAmount = 1;
                if (disposableBotonAgua != null) disposableBotonAgua.Dispose();

            }).AddTo(this.gameObject);




        }).AddTo(this.gameObject);


		botonLimpiarCooldown.Where(_ => botonLimpiarCooldown.Value == true).Subscribe(
        _ =>
        {
            
            disposableBotonLimpiar = Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(30))
            .Take(10)
            .Subscribe(time =>
            {

                rellenoLimpiar.fillAmount -= 0.1f;
               

            }
            , ex => { Debug.Log("OnError:" + ex.Message); disposableBotonLimpiar.Dispose(); },
            () => //completado
            {

				rellenoLimpiar.fillAmount = 1;
                if (disposableBotonLimpiar != null) disposableBotonLimpiar.Dispose();

            }).AddTo(this.gameObject);




        }).AddTo(this.gameObject);

		botonJugar_1_Cooldown.Where(_ => botonJugar_1_Cooldown.Value == true).Subscribe(
        _ =>
        {
            
            disposableBotonJugar_1 = Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(30))
            .Take(10)
            .Subscribe(time =>
            {

                rellenoJugar_1.fillAmount -= 0.1f;
               

            }
            , ex => { Debug.Log("OnError:" + ex.Message); disposableBotonJugar_1.Dispose(); },
            () => //completado
            {

				rellenoJugar_1.fillAmount = 1;
                if (disposableBotonJugar_1 != null) disposableBotonJugar_1.Dispose();

            }).AddTo(this.gameObject);




        }).AddTo(this.gameObject);
	

		botonJugar_2_Cooldown.Where(_ => botonJugar_2_Cooldown.Value == true).Subscribe(
        _ =>
        {
            
            disposableBotonJugar_2 = Observable
            .Timer(TimeSpan.FromSeconds(0), TimeSpan.FromMilliseconds(30))
            .Take(10)
            .Subscribe(time =>
            {

                rellenoJugar_2.fillAmount -= 0.1f;
               

            }
            , ex => { Debug.Log("OnError:" + ex.Message); disposableBotonJugar_2.Dispose(); },
            () => //completado
            {

				rellenoJugar_2.fillAmount = 1;
                if (disposableBotonJugar_2 != null) disposableBotonJugar_2.Dispose();

            }).AddTo(this.gameObject);




        }).AddTo(this.gameObject);


	
	}


#region

	//private ushort vecesClickedComida = 2;

	public void Mas_Comida()
	{ 
		

		internalVelocidadProgresoAlimento += 0.001f;

		if (internalVelocidadProgresoAlimento >= 0.050f)
		{ 
			internalVelocidadProgresoAlimento = 0.050f;
		
		}

		ActualizarProgreso();


	
	}

	public void Menos_Comida()
	{ 
	
		internalVelocidadProgresoAlimento -= 0.001f;
		if (internalVelocidadProgresoAlimento <= 0.001f)
		{ 
			internalVelocidadProgresoAlimento = 0.001f;
		
		}

		ActualizarProgreso();
	}


	public void Mas_Felicidad()
	{ 
		

		internalVelocidadProgresoFelicidad += 0.001f;

		if (internalVelocidadProgresoFelicidad >= 0.050f)
		{ 
			internalVelocidadProgresoFelicidad = 0.050f;
		
		}

		ActualizarProgreso();


	
	}

	public void Menos_Felicidad()
	{ 
	
		internalVelocidadProgresoFelicidad -= 0.001f;
		if (internalVelocidadProgresoFelicidad <= 0.001f)
		{ 
			internalVelocidadProgresoFelicidad = 0.001f;
		
		}

		ActualizarProgreso();
	}



	public void Mas_CooldownPescado()
	{ 
		
		
		internalVelocidadCooldownPescado += 1f;

		if (internalVelocidadCooldownPescado >= 100f)
		{ 
			internalVelocidadCooldownPescado = 100f;
		
		}
		ActualizarProgreso();


	
	}


	public void Menos_CooldownPescado()
	{ 
		
		print("menos cooldown pescado");
		internalVelocidadCooldownPescado -= 1f;

		if (internalVelocidadCooldownPescado <= 1f)
		{ 
			internalVelocidadCooldownPescado = 1f;
		
		}

		ActualizarProgreso();


	
	}


	private void ActualizarProgreso()
	{ 
	
	
		textoProgresoAlimento.text = Math.Round((internalVelocidadProgresoAlimento * 1000)).ToString();
		textoProgresoFelicidad.text = Math.Round((internalVelocidadProgresoFelicidad * 1000)).ToString();
		
		textoCooldownComida.text = internalVelocidadCooldownPescado.ToString();
		textoCooldownAgua.text = internalVelocidadCooldownAgua.ToString();
		textoCooldownLimpiar.text = internalVelocidadCooldownLimpiar.ToString();
		textoCooldownRaton.text = internalVelocidadCooldownRaton.ToString();
		textoCooldownCorazon.text = internalVelocidadCooldownCorazon.ToString();
		
	
	}


#endregion

}
