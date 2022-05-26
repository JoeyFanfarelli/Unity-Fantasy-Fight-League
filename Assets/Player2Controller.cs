using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player2Controller : MonoBehaviour
{
    public float speed;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;
    private SpriteRenderer mySpriteRenderer;
    private string playerDirection;

    private Animator anim;

    public GameObject explosion;
    public GameObject ball;

    private Vector2 explosionDistance;
    private GameObject instantiatedSpell;
    private bool spellActive;

    public TextMeshProUGUI blueXPText;
    public TextMeshProUGUI redXPText;


    // Start is called before the first frame update
    void Start()
    {
        playerDirection = "right";
        spellActive = false;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal2"), Input.GetAxisRaw("Vertical2"));
        moveVelocity = moveInput.normalized * speed;

        if (moveVelocity.x < 0)
        {
            mySpriteRenderer.flipX = false;
            playerDirection = "left";
        }
        else if (moveVelocity.x > 0)
        {
            mySpriteRenderer.flipX = true;
            playerDirection = "right";
        }

        if (Input.GetButton("ExplosionP2"))
        {
            if (spellActive == false)
            {




                if (playerDirection == "right")
                {
                    explosionDistance = transform.position + transform.right * 8;
                }
                else if (playerDirection == "left")
                {
                    explosionDistance = transform.position + -transform.right * 8;
                }

                StartCoroutine(Cast("Explosion"));
            }

        }

    }

    private void FixedUpdate()
    {

        if (rb.position.y + moveVelocity.y * Time.fixedDeltaTime < 6.5 && rb.position.y + moveVelocity.y * Time.fixedDeltaTime > -6 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime < 15.3 && rb.position.x + moveVelocity.x * Time.fixedDeltaTime > -15.4) //Keep paddle in-bounds
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
        if (collision.gameObject.tag == "Explosion")
        {
            ball.GetComponent<BallController>().blueXP += 20;
            blueXPText.text = ball.GetComponent<BallController>().blueXP.ToString();
            StartCoroutine(Pain());

        }
    }


    IEnumerator AttackCoroutine()
    {

        anim.SetBool("isAttacking", true);
        yield return new WaitForSeconds(0.21f);
        anim.SetBool("isAttacking", false);
    }

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

    IEnumerator Pain()
    {
        anim.SetBool("inPain", true);
        yield return new WaitForSeconds(2);
        anim.SetBool("inPain", false);
    }


}
