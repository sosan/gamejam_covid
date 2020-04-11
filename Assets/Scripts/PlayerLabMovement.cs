using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UniRx.Async;
using System;

public class PlayerLabMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody2D rigid = null;
    [SerializeField] private TextMeshProUGUI textoObjetosConseguidos = null;
    [SerializeField] private TextMeshProUGUI puntuaje = null;
    [SerializeField] private LabertintoMenuManager laberintoManager = null;
    [SerializeField] private Animator animacionLab = null;

    [SerializeField] public bool isDead = false;
    
    
    public Vector2Int gridMoveDirection;

    private void Start()
    {
        isDead = false;
    }

    private void Update()
    {

        if (isDead == true) return;
        if (laberintoManager.terminadoNivel == true) return;
        
        HandleInput();
        HandleGridMovement();
    
    }

    private void HandleInput()
    {
        if (Input.GetButtonDown("Up"))
        {
            gridMoveDirection.x = 0;
            gridMoveDirection.y = +1;
        }

        if (Input.GetButtonDown("Down"))
        {
            gridMoveDirection.x = 0;
            gridMoveDirection.y = -1;
        }

        if (Input.GetButtonDown("Right"))
        {
            gridMoveDirection.x = +1;
            gridMoveDirection.y = 0;
        }

        if (Input.GetButtonDown("Left"))
        {
            gridMoveDirection.x = -1;
            gridMoveDirection.y = 0;
        }
    }
    private Vector3 m_Velocity = Vector3.zero;
    [SerializeField] private float speedRun = 5f;

    private void HandleGridMovement()
    {
        Vector3 targetVelocity = new Vector2(gridMoveDirection.x * speedRun, gridMoveDirection.y * speedRun);
        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref m_Velocity, 0.05f);
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection)-90);
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }


    private async void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("lata"))
        { 

            laberintoManager.SetLatas();
            collision.gameObject.SetActive(false);
            //Destroy(collision.gameObject);

            animacionLab.SetBool("cogerobjeto", true);
            await UniTask.Delay(TimeSpan.FromMilliseconds(200));
            animacionLab.SetBool("cogerobjeto", false);
            return;
        
        }

        

        if (collision.CompareTag("bateria"))
        { 

            laberintoManager.SetBaterias();
            
            collision.gameObject.SetActive(false);
            animacionLab.SetBool("cogerobjeto", true);
            await UniTask.Delay(TimeSpan.FromMilliseconds(200));
            animacionLab.SetBool("cogerobjeto", false);
            return;
        }

        if (collision.CompareTag("barril"))
        { 

            laberintoManager.SetBarril();
            collision.gameObject.SetActive(false);
            
            animacionLab.SetBool("cogerobjeto", true);
            await UniTask.Delay(TimeSpan.FromMilliseconds(200));
            animacionLab.SetBool("cogerobjeto", false);
            return;
        }

        if (collision.CompareTag("Fin"))
        { 
            
            laberintoManager.SetGanador();
        
        
        }


    }

   

}
