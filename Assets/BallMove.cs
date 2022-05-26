using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Experimental.Rendering.Universal;

public class BallMove : MonoBehaviour
{
    private bool pauseEnabled;
    public GameObject quitButton;

    private Vector2 startPosition;
    
    public float speed;
    private float xSpeed;
    private float ySpeed;

    private int score;
    private int highScore;
    private int scoreMultiplier;

    private int goal;
    private int goalPoints;
    public int numHitsCons; //number of times the ball has hit a paddle without hitting a wall.
    public int numScoresCons; //number of scores without hitting a wall.
    public float timer = 0;
    public bool timerActive = false;

    private string lastHit;

    private float relativeDirectionX;
    private float relativeDirectionY;

    private Rigidbody2D rb;
    private Vector2 moveVelocity;

    public TextMeshProUGUI scoreText; 
    public TextMeshProUGUI highScoreText;
    public TextMeshProUGUI multiplierText;

    public TextMeshProUGUI goalText;
    public TextMeshProUGUI goalPointsText;

    //CameraShake Vars
    public GameObject cameraObject;
    private Transform objTransform;
    private float shakeDuration = 0f;
    private float shakeMagnitude = 0.1f;
    private float dampingSpeed = 1.0f;
    Vector3 camInitialposition;


    Light2D lt;
    Light2D ballLight;
    public GameObject titleHighlight;
    public GameObject goalHighlight;


    // Start is called before the first frame update
    void Start()
    {
        generateGoal();  //creates a random player goal/achievement
        score = 0;
        highScore = 0;
        scoreMultiplier = 1;
        shakeMagnitude = 0.1f;
        lastHit = "none";

        ballLight = gameObject.GetComponent<Light2D>();

        objTransform = cameraObject.gameObject.transform;    //For Camera Shake
        camInitialposition = objTransform.position;

        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
        xSpeed = 0;
        ySpeed = speed;
        moveVelocity = new Vector2(xSpeed, ySpeed);
    }

    // Update is called once per frame
    void Update()
    {
      

        if (timerActive == true)
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);
            if (timer <= 0.0f)
            {
                if (goal == 3)
                {
                     StartCoroutine(GoalFailed());
                }
                if (goal == 4)
                {

                    StartCoroutine(GoalAchieved());
                }

                timerActive = false;
            }
        }






        if (shakeDuration > 0)
        {
            objTransform.localPosition = camInitialposition + Random.insideUnitSphere * shakeMagnitude;
            shakeDuration -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            shakeDuration = 0f;
            objTransform.localPosition = camInitialposition;
        }


        //Pause
        if (Input.GetKeyDown("escape"))
        {
            //check if game is already paused       
            if (pauseEnabled == true)
            {
                //unpause the game
                Time.timeScale = 1;
                quitButton.SetActive(false);
                pauseEnabled = false;
            }
            //else if game isn't paused, then pause it
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

               

        if (rb.position.y < 5.4 && rb.position.y > -5.5 && rb.position.x < 8.4 && rb.position.x > -8.5) //Make sure ball is still in-bounds
        {
            rb.velocity = moveVelocity;
        }
        else
        {
            StartCoroutine(GoalFailed());
            rb.MovePosition(startPosition);
            ballLight.intensity = 30;
            scoreMultiplier = 1;
            dampingSpeed = 1.0f;
            multiplierText.text = scoreMultiplier.ToString();
            shakeMagnitude = 0.1f;
            xSpeed = 0;
            ySpeed = speed;
            moveVelocity = new Vector2(xSpeed, ySpeed);

            /*if (score > highScore)
            {
                highScore = score;
                highScoreText.text = highScore.ToString();
            }

            if (goal == 4)
            {
                StartCoroutine(GoalFailed());
            }*/

            score = 0;
            scoreText.text = score.ToString();
        }



    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
        


        if (collision.gameObject.tag == "Top")
        {
            numHitsCons = 0;
            numScoresCons = 0;
            
            ySpeed = -ySpeed;
            scoreMultiplier = 1;
            ballLight.intensity = 30;
            shakeMagnitude = 0.1f;
            dampingSpeed = 1.0f;
            multiplierText.text = scoreMultiplier.ToString();
            lastHit = "Wall";

        }
        else if (collision.gameObject.tag == "Side")
        {
            numHitsCons = 0;
            numScoresCons = 0;

            xSpeed = -xSpeed;
            scoreMultiplier = 1;
            ballLight.intensity = 30;
            shakeMagnitude = 0.1f;
            dampingSpeed = 1.0f;
            multiplierText.text = scoreMultiplier.ToString();
            lastHit = "Wall";
        }
        else if (collision.gameObject.tag == "Paddle")
        {
            numHitsCons++ ;
            if (goal == 0 && numHitsCons == 5)   //goal 0 is numHitsCons = 5
            {

                StartCoroutine(GoalAchieved());
                
            }
            
            if (goal == 1 && numHitsCons == 10)   //goal 1 is numHitsCons = 10
            {

                StartCoroutine(GoalAchieved());

            }

            lt = collision.gameObject.GetComponent<Light2D>();
            StartCoroutine(LightIntensity(lt));

            if (collision.gameObject.name == "Paddle2")
            {
                if (lastHit == "Paddle1" && scoreMultiplier < 3)
                {
                    scoreMultiplier++;
                    ballLight.intensity += 50;
                    shakeMagnitude += 0.01f;
                    dampingSpeed -= 0.3f;
                    multiplierText.text = scoreMultiplier.ToString();
                }
                else if (lastHit == "Paddle2")
                {
                    scoreMultiplier = 1;
                    ballLight.intensity = 30;
                    shakeMagnitude = 0.1f;
                    dampingSpeed = 1.0f;
                    multiplierText.text = scoreMultiplier.ToString();
                }
                lastHit = "Paddle2";
            }
            else if (collision.gameObject.name == "Paddle")
            {
                if (lastHit == "Paddle2" && scoreMultiplier < 3)
                {
                    scoreMultiplier++;
                    ballLight.intensity += 50;
                    shakeMagnitude += 0.01f;
                    dampingSpeed -= 0.3f;
                    multiplierText.text = scoreMultiplier.ToString();
                }
                else if (lastHit == "Paddle1")
                {
                    scoreMultiplier = 1;
                    ballLight.intensity = 30;
                    shakeMagnitude = 0.1f;
                    dampingSpeed = 1.0f;
                    multiplierText.text = scoreMultiplier.ToString();
                }
                lastHit = "Paddle1";
            }

            ySpeed = -ySpeed;

            relativeDirectionX = collision.gameObject.transform.position.x - transform.position.x;   //Used to figure out which part of the paddle the ball hit.
            
            if (relativeDirectionX > 0.1)
            {
                xSpeed = -speed * Mathf.Abs(relativeDirectionX * 2);
            }
            else if (relativeDirectionX < -0.1)
            {
                xSpeed = speed * Mathf.Abs(relativeDirectionX) * 2;
            }
            else
            {

            }

        }
        else if (collision.gameObject.tag == "Enemy")
        {
            cameraShake();
            lt = collision.gameObject.GetComponent<Light2D>();
            StartCoroutine(LightIntensity(lt));

            numScoresCons++;
            if (goal == 2 && numScoresCons == 3)   //goal 2 is 3 consecutive scores without walling
            {

                StartCoroutine(GoalAchieved());

            }

            if (goal == 3)   //goal 1 is numHitsCons = 10
            {

                StartCoroutine(GoalAchieved());

            }


            if (scoreMultiplier == 3)
            {
                StartCoroutine(Blink());
            }

            score += 1 * scoreMultiplier;
            //scoreMultiplier = 1;
            //ballLight.intensity = 30;
            //shakeMagnitude = 0.1f;
            //multiplierText.text = scoreMultiplier.ToString();
            scoreText.text = score.ToString();  //Update GUI


            //Calculate position of ball relative to enemy square
            relativeDirectionX = collision.gameObject.transform.position.x - transform.position.x;
            relativeDirectionY = collision.gameObject.transform.position.y - transform.position.y;

            
            //Bounce off the correct trajectory from enemy square
            if (relativeDirectionX > .5 || relativeDirectionX < -.5)
            {
                xSpeed = -xSpeed;
            }
            else if (relativeDirectionY > .5 || relativeDirectionY < -.5)
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

    public void cameraShake()
    {
        shakeDuration = 0.25f;
    }

    void generateGoal()
    {
        goal = Random.Range(0, 5);
        numHitsCons = 0;
        numScoresCons = 0;
        timer = 0;
        timerActive = false;

        if (goal == 0)
        {
            goalPoints = 5;
            goalPointsText.text = goalPoints.ToString() + " pts";
            goalText.text = "5 Consecutive Paddles";

        }
        else if (goal == 1)
        {
            goalPoints = 20;
            goalPointsText.text = goalPoints.ToString() + " pts";
            goalText.text = "10 Consecutive Paddles";

        }
        else if (goal == 2)
        {
            goalPoints = 30;
            goalPointsText.text = goalPoints.ToString() + " pts";
            goalText.text = "Score 3x Without Walling";
        }
        else if (goal == 3)
        {
            timer = 10.0f;
            timerActive = true;
            goalPoints = 20;
            goalPointsText.text = goalPoints.ToString() + " pts";
            goalText.text = "Score within 10sec";
        }
        else if (goal == 4)
        {
            timer = 20.0f;
            timerActive = true;
            goalPoints = 30;
            goalPointsText.text = goalPoints.ToString() + " pts";
            goalText.text = "Keep Ball On Field for 20s";
        }
    }

    IEnumerator LightIntensity(Light2D lightToFade)
    {


        lightToFade.intensity = Mathf.Lerp(40, 100, 2000);

        yield return new WaitForSeconds(1.5f);

        lightToFade.intensity = Mathf.Lerp(100, 40, 2000);


        yield return null;
    }

    IEnumerator Blink()
    {
        titleHighlight.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        titleHighlight.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        titleHighlight.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        titleHighlight.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        titleHighlight.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        titleHighlight.SetActive(false);

    }

    IEnumerator GoalAchieved()
    {
        score += goalPoints;
        scoreText.text = score.ToString();
        goalHighlight.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        goalHighlight.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        goalHighlight.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        goalHighlight.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        goalHighlight.SetActive(true);
        generateGoal();
    }

    IEnumerator GoalFailed()
    {
        goalText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        goalText.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        goalText.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        goalText.color = Color.white;
        generateGoal();
    }

}
