using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreatureManager : MonoBehaviour
{

    [SerializeField] private Image indicadorComida = null;
    [SerializeField] private Image indicadorFelicidad = null;
    [SerializeField] private Image indicadorAburrimiento = null;

    private float felicidad = 100f;
	private int nivel = 0;
	private int cacaContador = 0;
	private float muerte = 100f;
	private float ambriento = 0f;
	private float cacaPresion = 0f;
	private float aburrimiento = 0f;

	public bool isSick = false;
	public bool isDead = false;
	public bool isHungry = false;
	public bool isBored = false;
	public bool isAsleep = false;
	public bool isMinigameRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
