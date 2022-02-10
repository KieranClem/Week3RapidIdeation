using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    //variables used for movement and jumping
    [Header("Movement and Jumping variables")]
    Vector3 Movement;
    Vector3 InitialPosition;
    public float MovementSpeed = 5f;
    Vector3 Jump = new Vector3(0f, 2f, 0f);
    public float JumpForce = 5f;
    public Rigidbody rb;
    private bool isGrounded = true;
    [Header("Morph Ball variables")]
    //Variables used to morph between morph ball
    private bool isMorphCollected = false;
    public GameObject morphBall;
    private bool isMorphActive = false;
    public GameObject MainBody;
    [Header("Speed Booster variables")]
    //variables used for speed booster
    private bool isBoosterCollected = false;
    public float timeToStart = 10f;
    private float normalSpeed;
    private float timer;
    private bool isBoosterActive = false;
    //space jump variables
    private bool isSpaceCollected = false;
    private float ApexOfJump;

    public bool isFeezeCollected = false;

    public Material BaseColour;
    public Material BoostColour;
    public MeshRenderer PlayerColour;


    // Start is called before the first frame update
    void Start()
    {
        InitialPosition = transform.position;
        morphBall.SetActive(false);
        normalSpeed = MovementSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        //Player movement
        Movement.x = Input.GetAxisRaw("Horizontal");
        //Jumping; addes force upwards onto rigibody
        if (Input.GetKeyDown(KeyCode.E) && isGrounded && !isMorphActive)
        {
            rb.AddForce(Jump * JumpForce, ForceMode.Impulse);
            isGrounded = false;
            ApexOfJump = (transform.position.y + JumpForce)/2;
        }

        //activate morph ball
        if (Input.GetKeyDown(KeyCode.Q) && isMorphCollected)
        {
            MorphBall(isMorphActive);
        }

        //counts down how long the button is held down, used to determine if speed booster activates
        if((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) && isBoosterCollected)
        {
            timer += Time.deltaTime;
        }

        //Resets timeer when button is no longer being pressed
        if(Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A))
        {
            timer = 0;
            MovementSpeed = normalSpeed;
            isBoosterActive = false;
            PlayerColour.material = BaseColour;
        }

        //If the timer reaches the appropriate time the player's movement speed will be increased
        if(timer >= timeToStart && !isBoosterActive)
        {
            MovementSpeed = MovementSpeed * 3;
            isBoosterActive = true;
            PlayerColour.material = BoostColour;
        }

        //Allows the player to jump if they reach the apex of the jump
        if(((Mathf.Round(transform.position.y * 100)/100.0) >= ApexOfJump) && isSpaceCollected)
        {
            isGrounded = true;
            Debug.Log("activate");
        }

    }

    private void FixedUpdate()
    {
        //Player movement
        rb.MovePosition(rb.position + Movement * MovementSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //If player touches floor they can jump again
        if(other.tag == "Floor")
        {
            isGrounded = true;
        }    

        if(other.tag == "Enemy")
        {
            if (!other.GetComponent<EnemyFrozen>().isFrozen)
            {
                this.transform.position = InitialPosition;
            }
            else
            {
                isGrounded = true;
            }
        }

        if(other.name == "MorphBallPowerUp")
        {
            isMorphCollected = true;
            Destroy(other.gameObject);
        }

        if (other.name == "IceBeamPowerUp")
        {
            isFeezeCollected = true;
            Destroy(other.gameObject);
        }

        if (other.name == "SpeedBoosterPowerUp")
        {
            isBoosterCollected = true;
            Destroy(other.gameObject);
        }

        if (other.name == "SpaceJumpPowerUp")
        {
            isSpaceCollected = true;
            Destroy(other.gameObject);
        }

        if (other.tag == "SpeedBlock")
        {
            Destroy(other.gameObject);
        }
    }

    private void MorphBall(bool inMorph)
    {
        if(!inMorph)
        {
            isMorphActive = !isMorphActive;
            morphBall.SetActive(true);
            MainBody.SetActive(false);
        }
        else
        {
            isMorphActive = !isMorphActive;
            MainBody.SetActive(true);
            morphBall.SetActive(false);
        }
    }
}
