using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    private static PlayerController _get;
    public static PlayerController get { get { return _get; } }

    private UiController uiController;
    public Light2D pointLight;

    public AudioSource collectedSound;
    public AudioSource jumpSound;
    public AudioSource walkSound;
    public AudioSource dashSound;
    private Animator anim;
    private Rigidbody2D playerRB;
    public BoxCollider2D box2D;
    public BoxCollider2D box2DDown;

    [SerializeField] private float speed;
    [SerializeField] float speedConst=450;
    private float speedMult = 1;
    public float SpeedMult { get { return speedMult; } }
    public float jumpHeight; 
    public float dir = 1;

    private float tempAnimDown = 1f;
    private float tempSpeedIncrease = 10f;

    public float maxSwipetime;
    public float minSwipeDistance;
    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipelength;

    private bool hasJumped = false;
    bool animDown = false;
    [HideInInspector] public UnityEngine.Events.UnityEvent jumpEvent;
    [HideInInspector] public UnityEngine.Events.UnityEvent diveEvent;
    [HideInInspector] public UnityEngine.Events.UnityEvent crouchEvent;
    [HideInInspector] public UnityEngine.Events.UnityEvent hitEvent;

    [SerializeField] float lowlightRadius = 6f;
    [SerializeField] float lowlightIntensity = 1f;
    [SerializeField] float highlightRadius = 8f;
    [SerializeField] float highlightIntensity = 2f;

    void Start()
    {
        _get = this;

        uiController = GameObject.FindObjectOfType<UiController>(); 
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        box2D.enabled = true;
        box2DDown.enabled = false;

        walkSound.Play();

        speed = speedConst * speedMult;

        decreaseLight();

    }
    void FixedUpdate()
    {
        SwipeTest();
        transitionDownWalk();
        walk();
        speedIncrease();

        //teste pc
#if UNITY_EDITOR
        if ((/*Input.GetMouseButtonDown(0) || */Input.GetKeyDown(KeyCode.UpArrow)) && hasJumped == false)
        {
            jump();
        }
        if (/*Input.GetMouseButtonDown(1) || */Input.GetKeyDown(KeyCode.DownArrow))
        {
            down();
        }
#endif
    }
    void walk()
    {
        playerRB.velocity = new Vector2(dir * speed * Time.fixedDeltaTime, playerRB.velocity.y);
    }
    void transitionDownWalk()
    {
        if (animDown == true)
        {
            tempAnimDown -= Time.fixedDeltaTime;
            if (tempAnimDown < 0)
            {
                box2D.enabled = true;
                box2DDown.enabled = false;
                anim.SetBool("downU", false);
                walkSound.Play();
                tempAnimDown = 1f;
                animDown = false;
            }
        }
    }
    void SwipeTest()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                swipeStartTime = Time.time;
                startSwipePosition = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                swipeEndTime = Time.time;
                endSwipePosition = touch.position;
                swipeTime = swipeEndTime - swipeStartTime;
                swipelength = (endSwipePosition - startSwipePosition).magnitude;
                if(swipeTime<maxSwipetime&& swipelength > minSwipeDistance)
                {
                    swipeControl();
                }
            }
        }
    }
    void swipeControl()
    {
        Vector2 Distance = endSwipePosition - startSwipePosition;
        float xDistance = Mathf.Abs(Distance.x);
        float yDistance = Mathf.Abs(Distance.y);
        if (yDistance > xDistance)
        {
            if (Distance.y > 0 && hasJumped==false)
            {
                jump();
            }
        }
            if (Distance.y < 0)
            {
                down();
            }
        }
    void jump()
    {
        playerRB.velocity = /*new Vector2(playerRB.velocity.x, jumpHeight * Time.fixedDeltaTime);/*/ Vector2.up * jumpHeight * Time.deltaTime;
        hasJumped = true;
        anim.SetBool("jumped", true);
        anim.SetBool("downU", false);
        walkSound.Stop();
        jumpSound.Play();

        jumpEvent.Invoke();
    }
    void down()
    {
        if (hasJumped==true)
        {
            anim.SetBool("a", true);
            anim.SetBool("jumped", false);
            playerRB.velocity = /*new Vector2(playerRB.velocity.x, -jumpHeight * Time.fixedDeltaTime);/*/ Vector2.down * jumpHeight * Time.deltaTime;

            diveEvent.Invoke();
        }
        if (hasJumped == false)
        {
            anim.SetBool("downU",true);
            animDown = true;
            box2D.enabled = false;
            box2DDown.enabled = true;

            crouchEvent.Invoke();
        }
       
        //Debug.Log("abaixou");
        walkSound.Stop();
        dashSound.Play();
    }
    void speedIncrease()
    {
        //aumentar a cada 10 segundos o multiplicador da speed e do pontos de distancia

        speed = speedConst * speedMult;
        tempSpeedIncrease -= Time.fixedDeltaTime;
            if (tempSpeedIncrease < 0)
            {
            speedMult = speedMult + 0.05f;
            uiController.distanceIncrease = uiController.distanceIncrease + 0.05f;
                tempSpeedIncrease = 10f;

            RythmEvents.get.SetSpeed(speedMult);
        }
        
    }
    public void increaseLight()
    {
        pointLight.pointLightOuterRadius = highlightRadius;
        pointLight.intensity = highlightIntensity;
    }
    public void decreaseLight()
    {
        pointLight.pointLightOuterRadius = lowlightRadius;
        pointLight.intensity = lowlightIntensity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            anim.SetBool("jumped", false);
            anim.SetBool("a", false);
            hasJumped = false;
            if(animDown == false) walkSound.Play();
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "luz")
        {
            collectedSound.Play();
        }
        if (collision.gameObject.CompareTag("obstaculo"))
        {
            hitEvent.Invoke();
            UiController.get.pause();//TODO: game over
        }
    }
}





