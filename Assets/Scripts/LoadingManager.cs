using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx.Async;
using System;

public class LoadingManager : MonoBehaviour
{

    [SerializeField] private Animation animaciones = null;

    // Start is called before the first frame update
    private async void Start()
    {
        await UniTask.Delay(TimeSpan.FromSeconds(animaciones.GetClip("MenuPrincipalAparecer").length));
        SceneManager.LoadScene(1);

    }

    



}
