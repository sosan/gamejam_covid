using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLaberintoMovement : MonoBehaviour
{

    [SerializeField] private LabertintoMenuManager menuManager = null;

    
    [SerializeField] private Rigidbody2D rigid = null;
    [SerializeField] private Transform thisTFR = null;
    [SerializeField] private Animator playerAnimator = null;

    [SerializeField] private float runSpeed = 40f;
    [SerializeField] private float verticalSpeed = 10f;
    [SerializeField] private ParticleSystem particula = null;
    

    [SerializeField] private float m_JumpForce = 400f;
	[Range(0, 0.3f)] [SerializeField] private float m_MovementSmoothing = 0.05f;	
	[SerializeField] private bool m_AirControl = false;							
	[SerializeField] private LayerMask m_WhatIsGround;
    [SerializeField] private LayerMask m_WhatIsEscalera;
	[SerializeField] private Transform m_GroundCheck;							
	[SerializeField] private TrailRenderer trail = null;
    [SerializeField] private float gravityScale = 4;


    private float horizontalMove = 0f;
    private float verticalMove = 0f;

    private bool Jumping = false;
    private bool IsGround = false;

    public bool isGrounded = false;            
	private bool m_FacingRight = true;
	public bool isLanding = false; 
	public bool isTouchingWall = false;

	private Vector3 m_Velocity = Vector3.zero;


    //public float speed = 0.4f;
    Vector2 dest = Vector2.zero;


    private void Awake()
	{
		rigid = this.GetComponent<Rigidbody2D>();
        thisTFR = this.transform;
        rigid.gravityScale = gravityScale;
        dest = this.transform.position;


	}

    void Start()
    {
        _dest = transform.position;
        
    }

    public float speed = 0.4f;
    Vector2 _dest = Vector2.zero;
    Vector2 _dir = Vector2.zero;
    Vector2 _nextDir = Vector2.zero;

    [Serializable]
    public class PointSprites
    {
        public GameObject[] pointSprites;
    }

    public PointSprites points;

    public static int killstreak = 0;

    // script handles

    private bool _deadPlaying = false;

    // Use this for initialization
   
    // Update is called once per frame
    void FixedUpdate()
    {

        ReadInputAndMove();
                Animate();
        

    }

    

    void Animate()
    {
        Vector2 dir = _dest - (Vector2)transform.position;
        //GetComponent<Animator>().SetFloat("DirX", dir.x);
        //GetComponent<Animator>().SetFloat("DirY", dir.y);
    }

    bool Valid(Vector2 direction)
    {
        // cast line from 'next to pacman' to pacman
        // not from directly the center of next tile but just a little further from center of next tile
        Vector2 pos = transform.position;
        direction += new Vector2(direction.x * 0.45f, direction.y * 0.45f);
        RaycastHit2D hit = Physics2D.Linecast(pos + direction, pos);
        return hit.collider.name == "pacdot" || (hit.collider == GetComponent<Collider2D>());
    }

    public void ResetDestination()
    {
        _dest = new Vector2(15f, 11f);
        //GetComponent<Animator>().SetFloat("DirX", 1);
        //GetComponent<Animator>().SetFloat("DirY", 0);
    }

    void ReadInputAndMove()
    {
        // move closer to destination
        Vector2 p = Vector2.MoveTowards(transform.position, _dest, speed);
        GetComponent<Rigidbody2D>().MovePosition(p);

        // get the next direction from keyboard
        if (Input.GetAxis("Horizontal") > 0) _nextDir = Vector2.right;
        if (Input.GetAxis("Horizontal") < 0) _nextDir = -Vector2.right;
        if (Input.GetAxis("Vertical") > 0) _nextDir = Vector2.up;
        if (Input.GetAxis("Vertical") < 0) _nextDir = -Vector2.up;

        // if pacman is in the center of a tile
        if (Vector2.Distance(_dest, transform.position) < 0.00001f)
        {
            if (Valid(_nextDir))
            {
                _dest = (Vector2)transform.position + _nextDir;
                _dir = _nextDir;
            }
            else   // if next direction is not valid
            {
                if (Valid(_dir))  // and the prev. direction is valid
                    _dest = (Vector2)transform.position + _dir;   // continue on that direction

                // otherwise, do nothing
            }
        }
    }

    public Vector2 getDir()
    {
        return _dir;
    }

    //public void UpdateScore()
    //{
    //    killstreak++;

    //    // limit killstreak at 4
    //    if (killstreak > 4) killstreak = 4;

    //    Instantiate(points.pointSprites[killstreak - 1], transform.position, Quaternion.identity);
       

    //}

    //// Update is called once per frame
    //void Update()
    //{ 

    //    //horizontalMove = Input.GetAxisRaw("Horizontal") * runSpeed;
    //    //verticalMove = Input.GetAxisRaw("Vertical") * runSpeed;  

      

    //    if (horizontalMove != 0)
    //    { 

    //        //if (particula.isPlaying == false)
    //        //{ 

    //        //    if (horizontalMove > 0)
    //        //    { 
    //        //        var tempRot = particula.transform.rotation;
    //        //        tempRot.eulerAngles = new Vector3(0, 0, 135f);
    //        //        particula.transform.rotation = tempRot;

                    
    //        //    }
    //        //    else if (horizontalMove < 0)
    //        //    { 
    //        //        var tempRot = particula.transform.rotation;
    //        //        tempRot.eulerAngles = new Vector3(0, 0, 0);
    //        //        particula.transform.rotation = tempRot;
                    
                    
    //        //    }
                    
    //        //    particula.Play();
    //        //}
    //        if(IsGround)
    //        {
                
    //            playerAnimator.SetBool("move", true);
    //        }
    //    }
    //    else
    //    {
    //        if (IsGround)
    //        {
    //            playerAnimator.SetBool("move", false);
    //        }
    //    }


    //    if(Input.GetButtonDown("Jump"))
    //    {
    //        if (!Jumping)
    //        {
    //        }

    //    }



    //}

    //private void FixedUpdate()
    //{

    //    //Move(horizontalMove * Time.fixedDeltaTime, verticalMove * Time.fixedDeltaTime, verticalSpeed);
    //    Vector2 p = Vector2.MoveTowards(transform.position, dest, speed);
    //    rigid.MovePosition(p);

    //    // Check for Input if not moving
    //    if ((Vector2)transform.position == dest) 
    //    {
    //        if (Input.GetKey(KeyCode.UpArrow) && valid(Vector2.up))
    //            dest = (Vector2)transform.position + Vector2.up;
    //        if (Input.GetKey(KeyCode.RightArrow) && valid(Vector2.right))
    //            dest = (Vector2)transform.position + Vector2.right;
    //        if (Input.GetKey(KeyCode.DownArrow) && valid(-Vector2.up))
    //            dest = (Vector2)transform.position - Vector2.up;
    //        if (Input.GetKey(KeyCode.LeftArrow) && valid(-Vector2.right))
    //            dest = (Vector2)transform.position - Vector2.right;
    //    }

    //    // Animation Parameters
    //    Vector2 dir = dest - (Vector2)transform.position;
       
        

    //}

    //private bool valid(Vector2 dir) 
    //{
    //    Vector2 pos = transform.position;
    //    RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
    //    return (hit.collider == GetComponent<Collider2D>());
    //}

    private void Move(float moveX, float moveY, float verticalSpeed)
    {
        //rigid.drag = 0f;
        Vector3 targetVelocity = new Vector2(moveX * 10f, moveY * 10f);
        rigid.velocity = Vector3.SmoothDamp(rigid.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (moveX > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (moveX < 0 && m_FacingRight)
        {
            Flip();
        }


    }


    private void Flip()
	{
		m_FacingRight = !m_FacingRight;

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
		
		

	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (collision.CompareTag("Fin"))
        { 
           
        
        }

    }

}
