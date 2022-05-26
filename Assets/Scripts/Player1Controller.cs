using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player1Controller : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private SpriteRenderer mySpriteRenderer;
    private string playerDirection;

    private Animator anim;

    public GameObject explosion;                
    public GameObject ball;

    private Vector2 explosionDistance;          //How far away the explosion spell will be cast
    private GameObject instantiatedSpell;       //Explosion spell that is instantiated.
    private bool spellActive;                   //Determines whether or not a spell is currently being cast.

    public TextMeshProUGUI blueXPText;
    public TextMeshProUGUI redXPText;


    //Set defaults and cache references.
    void Start()
    {
        
        playerDirection = "right"; 
        spellActive = false;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();

    }

    
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveVelocity = moveInput.normalized * speed;
        
        //flip player sprite depending on move direction.
        if (moveVelocity.x < 0)
        {
            mySpriteRenderer.flipX = true;
            playerDirection = "left";
        }
        else if (moveVelocity.x > 0)
        {
            mySpriteRenderer.flipX = false;
            playerDirection = "right";
        }


        //See if a spell is already active. If not, calculate where the explosion should occur, and cast it through the "Cast" CoRoutine.
        if (Input.GetButton("ExplosionP1"))
        {
            if (spellActive == false)
            {
                
                if (playerDirection == "right")
                {
                    explosionDistance = transform.position + transform.right*8;
                }
                else if (playerDirection == "left")
                {
                    explosionDistance = transform.position + -transform.right*8;
                }
                
                StartCoroutine(Cast("Explosion"));
            }

        }

    }

    private void FixedUpdate()
    {

        if (rb.position.y + moveVelocity.y * Time.fixedDeltaTime < 6.5 && rb.position.y + moveVelocity.y * Time.fixedDeltaTime > -6 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime < 15.3 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime > -15.4)
        {
            rb.MovePosition(rb.position + moveVelocity * Time.fixedDeltaTime);
        }
        else
        {
            rb.MovePosition(rb.position);
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            StartCoroutine(AttackCoroutine());
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Increment score and trigger the coroutine which handles the pain animation.
        if (collision.gameObject.tag == "Explosion")
        {
            ball.GetComponent<BallController>().redXP += 20;
            redXPText.text = ball.GetComponent<BallController>().redXP.ToString();
            StartCoroutine(Pain());

        }
    }

    IEnumerator AttackCoroutine()
    {
        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.21f);
        anim.SetBool("isAttacking", false);
    }

    //This is set up for future spells via elseif. Right now, it only handles the Explosion. Instantiates, destroys, and handles the spellActive boolean.
    IEnumerator Cast(string spell)
    {

        if (spell == "Explosion")
        {
            instantiatedSpell = Instantiate(explosion, explosionDistance, transform.rotation);
            spellActive = true;
            yield return new WaitForSeconds(2);
            Destroy(instantiatedSpell);
            spellActive = false;
        }
        else
        {
            yield return new WaitForSeconds(1);
        }

    }

    //Triggers the pain animation.
    IEnumerator Pain()
    {
        anim.SetBool("inPain", true);
        yield return new WaitForSeconds(2);
        anim.SetBool("inPain", false);
    }

}
