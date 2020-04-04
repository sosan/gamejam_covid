using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreatureManager : MonoBehaviour
{

    [SerializeField] private Image indicadorComida = null;
    [SerializeField] private Image indicadorFelicidad = null;
    [SerializeField] private Image indicadorAburrimiento = null;
	[SerializeField] private TextMeshProUGUI textoNivel = null;
	[SerializeField] private TextMeshProUGUI textoCantidadMonedas = null;
	[SerializeField] private TextMeshProUGUI mensajeBocadillo = null;
	[SerializeField] private GameObject bocadillo = null;

	[SerializeField] private ParticleSystem[] particulasTomogochi = null;

	[SerializeField] public int monedas = 10000;

    private float felicidad = 100f;
	private int nivel = 1;
	private int progresoNivel = 0;
	private int cacaContador = 0;
	private float muerteContador = 100f;
	private float ambriento = 0f;
	private float cacaPresion = 0f;
	private float aburrimiento = 0f;
	

	public bool isSick = false;
	public bool isDead = false;
	public bool isHungry = false;
	public bool isBored = false;
	public bool isAsleep = false;
	public bool isMinigameRunning = false;


    //System.Diagnostics.Stopwatch timer;


    void Start()
    {

		mensajeBocadillo.text = "";
		bocadillo.SetActive(false);
		textoNivel.text = nivel.ToString("N0");
		ActualizarMonedas();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMinigameRunning == true) return;

        if (isDead)
        {
			return;
		}
        

        ambriento += 0.05f; //0.003f;
		cacaPresion += 0.02f;


		if (isBored) 
		{
			felicidad -= 0.010f; //0.005f;
		} 
		else
		{ 
			aburrimiento += 0.010f;  //0.002f;
		
		}
		

		if (isSick) 
		{
			felicidad -= 0.010f;// 0.005f;
			muerteContador -= 0.010f; //0.05f;
		}

		if (isHungry) 
		{
			felicidad -= 0.10f; //0.05f;
		}

		if (ambriento > 70f && isHungry == false) 
		{
			
			isHungry = true;
			//falta animacion del tamagochi
			mensajeBocadillo.text = "Tengo<br>Hambre!";
			bocadillo.SetActive(true);

			
		}

		if (aburrimiento > 80f && isBored == false) 
		{
			SetAburrido ();
		}

		if (cacaPresion >= 100f) 
		{
			SetSucioCaca();
			cacaPresion = Random.Range (0, 20f);
		}


		if (!isAsleep && (System.DateTime.Now.Hour > 23 || System.DateTime.Now.Hour < 7)) 
		{
			SetDormido ();
		} 
		else if (isAsleep && (System.DateTime.Now.Hour > 7 && System.DateTime.Now.Hour < 23))
		{
			SetDespierto ();
		}
			
		if (felicidad <= 0 || muerteContador <= 0 || ambriento >= 100) 
		{
			SetMuerto();
		}

		indicadorComida.fillAmount = ambriento/100f;
		indicadorAburrimiento.fillAmount = aburrimiento/100f;
		indicadorFelicidad.fillAmount = felicidad/100f;






    }


	private void SetMuerto()
	{ 
		if (nivel > 0)
		{
			//falta animacion de tamagochi
			//falta sonido muerto
			
			// falta imagen muerto

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


 //   public void startTimer ()
	//{
	//	timer.Start ();
	//}

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

    public void Click_BotonComida()
    { 
		if (monedas <= 0) return;
		if (ambriento <= 0) return;

		particulasTomogochi[0].Play();
		ambriento--;
		progresoNivel++;
		if (progresoNivel == 20)
		{ 
			nivel++;
		
		}
        ActualizarMonedas();
		ActualizarNivel();


    }

    public void Click_BotonFelicidad()
    { 

		if (monedas <= 0) return;
		if (felicidad >= 100) return;

		particulasTomogochi[1].Play();
		felicidad++;
		ActualizarMonedas();
    
    }

    public void Click_BotonArena()
    { 
		if (monedas <= 0) return;
		if (cacaPresion <= 0) return;

		particulasTomogochi[2].Play();
		cacaPresion--;

		ActualizarMonedas();
    
    }

    public void Click_BotonJuego()
    { 
		if (monedas <= 0) return;
		if (aburrimiento <= 0) return;
		

		particulasTomogochi[3].Play();
		aburrimiento--;
		ActualizarMonedas();
    }


}
