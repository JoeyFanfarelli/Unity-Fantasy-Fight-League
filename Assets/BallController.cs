using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{

    public AudioSource Whoosh;
    public AudioSource Huzzah;
    public AudioSource Cheer;

    private bool pauseEnabled;
    public GameObject quitButton;
    public GameObject Player1;
    public GameObject Player2;

    //Ball, P1, P2
    private Vector2 startPosition;  
    private Vector2 P1StartPosition;
    private Vector2 P2StartPosition;

    private int ballStartDirection;
    public float speed;
    private float xSpeed;
    private float ySpeed;

    private float relativeDirectionX;
    private float relativeDirectionY;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    //Score vars
    public int redXP;
    public int blueXP;

    public TextMeshProUGUI blueXPText;
    public TextMeshProUGUI redXPText;
    public TextMeshProUGUI redWinsText;
    public TextMeshProUGUI blueWinsText;


    // Start is called before the first frame update
    void Start()
    {

        //Zero out score, identify start positions for players and ball, and randomize ball start direction.
        redXP = 0;
        blueXP = 0;

        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        P1StartPosition = Player1.GetComponent<Rigidbody2D>().position;
        P2StartPosition = Player2.GetComponent<Rigidbody2D>().position;
        ballStartDirection = Random.Range(0, 4);

        if (ballStartDirection == 0)
        {
            xSpeed = speed;
            ySpeed = speed;
        }
        else if (ballStartDirection == 1)
        {
            xSpeed = -speed;
            ySpeed = speed;
        }
        else if (ballStartDirection == 2)
        {
            xSpeed = -speed;
            ySpeed = -speed;
        }
        else if (ballStartDirection == 3)
        {
            xSpeed = speed;
            ySpeed = -speed;
        }
        else 
        {
            xSpeed = speed;
            ySpeed = -speed;
        }

        moveVelocity = new Vector2(xSpeed, ySpeed);
    }

    void Update()
    {
        //Pausing
        if (Input.GetKeyDown("escape"))
        {    
            if (pauseEnabled == true)
            {
                //unpause the game
                Time.timeScale = 1;
                quitButton.SetActive(false);
                pauseEnabled = false;
            }
            else if (pauseEnabled == false)
            {
                Time.timeScale = 0;
                quitButton.SetActive(true);
                pauseEnabled = true;
            }
        }


    }

    private void FixedUpdate()
    {
        rb.velocity = moveVelocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Top")
        {

            ySpeed = -ySpeed;

        }
        else if (collision.gameObject.tag == "Side")
        {
       
            xSpeed = -xSpeed;
           
        }
        else if (collision.gameObject.tag == "Paddle")
        {
            Whoosh.Play();

            relativeDirectionX = collision.gameObject.transform.position.x - transform.position.x;
            relativeDirectionY = collision.gameObject.transform.position.y - transform.position.y;   //Used to figure out which part of the paddle the ball hit.

            if (relativeDirectionX > .65 || relativeDirectionX < -.65)
            {
                xSpeed = -xSpeed;
            }

            if (relativeDirectionY > .79 || relativeDirectionY < -.79)
            {
                ySpeed = -ySpeed;
            }


            if (relativeDirectionX > .2)   
            {
                xSpeed = -speed * Mathf.Abs(relativeDirectionY * 2);
            }
            else if (relativeDirectionX < -.2)
            {
                xSpeed = speed * Mathf.Abs(relativeDirectionY) * 2;
            }
            else
            {

            }


            if (relativeDirectionY > .2)   
            {
                ySpeed = -speed * Mathf.Abs(relativeDirectionY * 2);
            }
            else if (relativeDirectionY < -.2)
            {
                ySpeed = speed * Mathf.Abs(relativeDirectionY) * 2;
            }
            else
            {

            }

        }
        else if (collision.gameObject.tag == "Obstacle")
        {
            relativeDirectionX = collision.gameObject.transform.position.x - transform.position.x;  //used to figure out which part of the obstacle was hit by the ball, to determine proper bounce trajectory
            relativeDirectionY = collision.gameObject.transform.position.y - transform.position.y;

            if (relativeDirectionX > 0.5 || relativeDirectionX < -0.5)
            {
                xSpeed = -xSpeed;
            }
            
            if (relativeDirectionY > 0.5 || relativeDirectionY < -0.5)
            {
                ySpeed = -ySpeed;
            }
        }

        if (xSpeed > 20)        //Speed cap to make sure the ball doesn't move too fast.
        {
            xSpeed = 20;
        }

        moveVelocity = new Vector2(xSpeed, ySpeed);
        rb.velocity = moveVelocity;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When player scores a goal...
        if (collision.gameObject.tag == "Goal")
        {
            Cheer.Play();
            Huzzah.Play();

            //Identify which goal.
            if (collision.gameObject.name == "BlueGoal")
            {
                redXP += 100;
                redXPText.text = redXP.ToString();
                Debug.Log("Red Scored!");
                Debug.Log("Red XP: " + redXP);
            }

            if (collision.gameObject.name == "RedGoal")
            {
                blueXP += 100;
                blueXPText.text = blueXP.ToString();
                Debug.Log("Blue Scored!");
                Debug.Log("Blue XP: " + blueXP);
            }
            
            //Identify if win condition has been met.
            if (blueXP >= 500 || redXP >= 500)
            {
                StartCoroutine(ResetGame());
            }
            else
            {
                ResetBall();

            }

        }
    }

    //Place ball back and players back at start position.
    private void ResetBall()
    {
        Player1.GetComponent<Rigidbody2D>().transform.position = P1StartPosition;
        Player2.GetComponent<Rigidbody2D>().transform.position = P2StartPosition;
        rb.MovePosition(startPosition);

        ballStartDirection = Random.Range(0, 4);

        if (ballStartDirection == 0)
        {
            xSpeed = speed;
            ySpeed = speed;
        }
        else if (ballStartDirection == 1)
        {
            xSpeed = -speed;
            ySpeed = speed;
        }
        else if (ballStartDirection == 2)
        {
            xSpeed = -speed;
            ySpeed = -speed;
        }
        else if (ballStartDirection == 3)
        {
            xSpeed = speed;
            ySpeed = -speed;
        }
        else
        {
            xSpeed = speed;
            ySpeed = -speed;
        }
        moveVelocity = new Vector2(xSpeed, ySpeed);
    }

    //Runs if win condition has been met. Decide who wins, notify via the log and reset game state.
    IEnumerator ResetGame()
    {
        if (redXP > blueXP)
        {
            Debug.Log("Red Wins");
            redWinsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            redWinsText.gameObject.SetActive(false);
        }
        else if (blueXP > redXP)
        {
            Debug.Log("Blue Wins");
            blueWinsText.gameObject.SetActive(true);
            yield return new WaitForSeconds(4);
            blueWinsText.gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("else");
            yield return new WaitForSeconds(0);
        }

        ResetBall();
        redXP = 0;
        blueXP = 0;
        blueXPText.text = blueXP.ToString();
        redXPText.text = redXP.ToString();
        Player1.GetComponent<Rigidbody2D>().transform.position = P1StartPosition;
        Player2.GetComponent<Rigidbody2D>().transform.position = P2StartPosition;
        
    }

}
