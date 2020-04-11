using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MenuManager : MonoBehaviour
{

    [Header("Menu Principal")]
    [SerializeField] private TextMeshProUGUI[] textosMenuPrincipal = null;
    [SerializeField] private Color notSelectedColor = Color.white;
    [SerializeField] private Color selectedColor = Color.white;

    [SerializeField] private RectTransform punteroMenuTransform = null;
    [SerializeField] private RectTransform[] positionsPunteroMenuPrincipal = null;

    [SerializeField] private RectTransform[] positionsParticleMenuPrincipal = null; 
    [SerializeField] private RectTransform positionParticleMenuPrincipal = null; 
    [SerializeField] private ParticleSystem particleMenuClicked = null;
    
    
    [SerializeField] private Animation animaciones = null;

    [SerializeField] private GameObject huella = null;
    [SerializeField] private Transform[] posicionHuellas = null;


    [SerializeField] private RectTransform posicionPanelMover = null;
    [SerializeField] private RectTransform panelMover = null;


    [SerializeField] private TextMeshProUGUI textosOpciones = null;
    
    [Header("Paneles")]
    [SerializeField] private GameObject panelMenuPrincipal;
    [SerializeField] private GameObject panelOpciones = null;
    [SerializeField] private GameObject panelCreditos = null;
    [SerializeField] private GameObject panelAmpliado = null;
    [SerializeField] private GameObject panelNormal = null;
    [SerializeField] private GameObject panelInformacion = null;

    
    [Header("Menu Opciones")]
    [SerializeField] private RectTransform punteroOpcionesTransform = null;

    [Header("Menu Progreso")]
    [SerializeField] private GameObject panelProgresos = null;

    [Header("Menu Informacion")]
    [SerializeField] private GameObject[] fondosJugar = null;

    [Header("AudioSource")]
    [SerializeField] private AudioSource audio = null;


    // Start is called before the first frame update
    private async void Start()
    {
        Application.targetFrameRate = 120;
        Application.runInBackground = true;

        huella.SetActive(false);
        panelProgresos.SetActive(true);
        DisablePanels();
        TextToNotSelectedColor();

        panelNormal.SetActive(true);
        



        animaciones.Play("MenuPrincipalAparecer_Menu");
        await UniTask.Delay(TimeSpan.FromSeconds(animaciones.GetClip("MenuPrincipalAparecer_Menu").length));





    }


    public async void ClickJugar()
    { 

        print("jugar");

        ShowFX(positionsParticleMenuPrincipal[0].anchoredPosition);
        await UniTask.Delay(TimeSpan.FromMilliseconds(particleMenuClicked.main.duration + 100 ));
        ShowFX_Huella(posicionHuellas[0].position);
        
        panelMover.anchoredPosition = posicionPanelMover.anchoredPosition;

        fondosJugar[0].SetActive(false);
        fondosJugar[1].SetActive(true);


    
    }


    public async void ClickControles()
    {
        ShowFX(positionsParticleMenuPrincipal[1].anchoredPosition);
        await UniTask.Delay(TimeSpan.FromMilliseconds(particleMenuClicked.main.duration + 100));
        ShowFX_Huella(posicionHuellas[1].position);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        animaciones.Play("Menu_ControlesAparecer");
        await UniTask.Delay(TimeSpan.FromSeconds(animaciones.GetClip("Menu_ControlesAparecer").length));
    
    }


    public async void ClickConfiguracion()
    { 
        ShowFX(positionsParticleMenuPrincipal[2].anchoredPosition);
        await UniTask.Delay(TimeSpan.FromMilliseconds(particleMenuClicked.main.duration + 100));
        ShowFX_Huella(posicionHuellas[2].position);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        animaciones.Play("Menu_ConfiguracionAparacer");
        await UniTask.Delay(TimeSpan.FromSeconds(animaciones.GetClip("Menu_ConfiguracionAparacer").length));
        
    
    }

    public async void ClickCreditos()
    { 
        ShowFX(positionsParticleMenuPrincipal[3].anchoredPosition);
        await UniTask.Delay(TimeSpan.FromMilliseconds(particleMenuClicked.main.duration + 100));
        ShowFX_Huella(posicionHuellas[3].position);
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        animaciones.Play("Menu_CreditosAparecer");
        await UniTask.Delay(TimeSpan.FromSeconds(animaciones.GetClip("Menu_CreditosAparecer").length));
        

    }

    public async void ClickSalir()
    { 
    
        ShowFX(positionsParticleMenuPrincipal[4].anchoredPosition);
        await UniTask.Delay(TimeSpan.FromMilliseconds(particleMenuClicked.main.duration + 100 ));
        ShowFX_Huella(posicionHuellas[4].position);
        
        await UniTask.Delay(TimeSpan.FromSeconds(1));

        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
        #endif
    
    }


    public void Informacion()
    { 
    
        panelInformacion.SetActive(true);
        
    
    }

#region HOVERS

    public void HoverJuego_1(TextMeshProUGUI texto)
    { 
        HoverEnterTextMeshGUIPro(texto);
        punteroMenuTransform.gameObject.SetActive(true);
        punteroMenuTransform.anchoredPosition = positionsPunteroMenuPrincipal[0].anchoredPosition;

    
    
    }

    public void HoverJuego_2(TextMeshProUGUI texto)
    { 
        HoverEnterTextMeshGUIPro(texto);
        punteroMenuTransform.gameObject.SetActive(true);
        punteroMenuTransform.anchoredPosition = positionsPunteroMenuPrincipal[1].anchoredPosition;
    
    
    }

    public void HoverJuego_3(TextMeshProUGUI texto)
    { 
        HoverEnterTextMeshGUIPro(texto);
        punteroMenuTransform.gameObject.SetActive(true);
        punteroMenuTransform.anchoredPosition = positionsPunteroMenuPrincipal[2].anchoredPosition;
    
    
    }




    public void HoverEnterTextMeshGUIPro(TextMeshProUGUI texto)
    { 
        TextToNotSelectedColor();
        texto.color = selectedColor;

    
    }

    public void HoverExit()
    { 
    
        TextToNotSelectedColor();
        punteroMenuTransform.gameObject.SetActive(false);
    
    }

    
    private void TextToNotSelectedColor()
    { 
    
        for (ushort i = 0; i < textosMenuPrincipal.Length; i++)
        {

            textosMenuPrincipal[i].color = notSelectedColor;
            

        }
    
    }

    private void TextToSelectedColor()
    { 
    
        for (ushort i = 0; i < textosMenuPrincipal.Length; i++)
        {

            textosMenuPrincipal[i].color = selectedColor;
            

        }
    
    }





#endregion


    private void DisablePanels()
    { 
    
        panelInformacion.SetActive(false);
        panelAmpliado.SetActive(false);    
        panelCreditos.SetActive(false);
        panelOpciones.SetActive(false);
        panelMenuPrincipal.SetActive(false);
        
        
    }




    private async void PlayFx(TextMeshProUGUI texto)
    { 
    
        texto.color = selectedColor;
        await UniTask.Delay(150);
        texto.color = notSelectedColor;

        await UniTask.Delay(200);
    
    }


    private void ShowFX(Vector2 position)
    {
        positionParticleMenuPrincipal.anchoredPosition = position;
        particleMenuClicked.Play();

    }

    private async void ShowFX_Huella(Vector3 position)
    { 
        huella.transform.position = position;
        huella.SetActive(true);
        await UniTask.Delay(TimeSpan.FromMilliseconds(1300));
        huella.SetActive(false);
    
    }

    [SerializeField] private CreatureManager creatureManager = null;


    public async void Juegos(int numeroJuego)
    { 
        PlayerPrefs.SetInt("puntuaje", creatureManager.puntos );
        PlayerPrefs.SetInt("guardadoComida", 1);
        PlayerPrefs.SetFloat("comida", creatureManager.comida);

        await UniTask.Delay(TimeSpan.FromSeconds(1));

        SceneManager.LoadScene(numeroJuego);
    
    }


   #region

        

    public void ShowCreditos()
    { 

        panelProgresos.SetActive(false);
        panelCreditos.SetActive(true);
    
        
        
    
    }

    public void CloseCreditos()
    { 
        panelProgresos.SetActive(true);
        panelCreditos.SetActive(false);

    
    }


    public void ShowOpciones()
    { 
        panelOpciones.SetActive(true);
        panelProgresos.SetActive(false);
    
    }

    public void CloseOpciones()
    { 
        panelOpciones.SetActive(false);
        panelProgresos.SetActive(true);
    
    }




    public void SubirVolumen()
    { 
        audio.volume += 0.1f;
        var vol = Math.Round(audio.volume * 100);
        
        textosOpciones.text = vol.ToString();
    
    }

    public void BajarVolumen()
    { 
        
        audio.volume -= 0.1f;
        
        var vol = Math.Round(audio.volume * 100);
        if (vol <= 0)
        { 
            vol = 0;
        
        }

        textosOpciones.text = vol.ToString();
    
    }

    public void ClickJugarAmpliar()
    { 
    
        panelAmpliado.SetActive(true);
        panelNormal.SetActive(false);
        panelProgresos.SetActive(true);
    
    }

    public void ClickJugarReducir()
    { 
    
        panelAmpliado.SetActive(false);
        panelNormal.SetActive(true);
        panelProgresos.SetActive(true);
    
    }


    public void CloseOptionsPanel()
    { 
        
        panelOpciones.SetActive(false);
        panelProgresos.SetActive(true);
    
    }


    public void CloseInformacion()
    { 
        
        panelInformacion.SetActive(false);
        panelProgresos.SetActive(true);
    }

    
    
    
   


   


    


#endregion

    [SerializeField] private ScrollRect scroll = null;
    [SerializeField] private Image[] botonesImagen = null;


    public void MovimientoTipo1(Image botonColorOn)
    { 
    
        scroll.movementType = ScrollRect.MovementType.Elastic;
        botonColorOn.color = Color.red;
        botonesImagen[1].color = Color.white;
    }

    public void MovimientoTipo2(Image botonColorOn)
    { 
        scroll.movementType = ScrollRect.MovementType.Clamped;
        botonColorOn.color = Color.red;
        botonesImagen[0].color = Color.white;
    }


    public void ClickedURL(string uri)
    { 
         Application.OpenURL(uri);
    
    }



}
