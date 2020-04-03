using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;



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
    
    [SerializeField] private GameObject[] panelesMenuPrincipal;
    [SerializeField] private Animation animaciones = null;

    
    


    [Header("Menu Opciones")]
    [SerializeField] private RectTransform punteroOpcionesTransform = null;




    // Start is called before the first frame update
    private async void Start()
    {
        Application.targetFrameRate = 120;
        Application.runInBackground = true;

        DisablePanels();
        TextToNotSelectedColor();
        
        animaciones.Play("MenuPrincipalAparecer");
        await UniTask.Delay(TimeSpan.FromSeconds(animaciones.GetClip("MenuPrincipalAparecer").length));
        
        //panelesMenuPrincipal[1].SetActive(true);


    }

    

    public void ClickJugar()
    { 
        ShowFX(positionsParticleMenuPrincipal[0].anchoredPosition);
    
    }


    public void ClickControles()
    {
        ShowFX(positionsParticleMenuPrincipal[1].anchoredPosition);
    
    }


    public void ClickConfiguracion()
    { 
        ShowFX(positionsParticleMenuPrincipal[2].anchoredPosition);
    
    }

    public void ClickCreditos()
    { 
        ShowFX(positionsParticleMenuPrincipal[3].anchoredPosition);
    
    }

    public async void ClickSalir()
    { 
    
        ShowFX(positionsParticleMenuPrincipal[4].anchoredPosition);

        await UniTask.Delay(TimeSpan.FromMilliseconds(600));


        #if (UNITY_EDITOR)
            UnityEditor.EditorApplication.isPlaying = false;
        #elif (UNITY_STANDALONE) 
            Application.Quit();
        #elif (UNITY_WEBGL)
            Application.OpenURL("about:blank");
        #endif
    
    }

#region HOVERS

    public void HoverEntrarJugar()
    { 
        TextToNotSelectedColor();
        textosMenuPrincipal[0].color = selectedColor;
    
    }


    public void HoverExitJugar()
    { 
        TextToNotSelectedColor();
    
    }


    public void HoverEntrarControles()
    { 
        TextToNotSelectedColor();
        textosMenuPrincipal[1].color = selectedColor;
    
    }

    public void HoverExitControles()
    { 
        TextToNotSelectedColor();
    
    }

    public void HoverEnterConfiguracion()
    {
        TextToNotSelectedColor();
        textosMenuPrincipal[2].color = selectedColor;
    }
    
    
    public void HoverExitConfiguracion()
    { 
        TextToNotSelectedColor();
    
    }

    public void HoverEnterCreditos()
    {
        TextToNotSelectedColor();
        textosMenuPrincipal[3].color = selectedColor;
    }
    
    
    public void HoverExitCreditos()
    { 
        TextToNotSelectedColor();
    
    }

    public void HoverEnterSalir()
    {
        TextToNotSelectedColor();
        textosMenuPrincipal[4].color = selectedColor;
    }   
    
    
    public void HoverExitSalir()
    { 
        TextToNotSelectedColor();
    
    }

#endregion

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

    private void DisablePanels()
    { 
    
        
        for (ushort i = 0; i < panelesMenuPrincipal.Length; i++)
        { 
        
            panelesMenuPrincipal[i].SetActive(false);
        
        }
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

}
