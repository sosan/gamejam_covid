using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameScene : MonoBehaviour
{
    public void QuitGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
