using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public BoxCollider2D slimeDownCollider;
    //public GameObject slimeCollider;
    public float speed;
    public float jumpHeight; 
    public float dir = 1;
    private Rigidbody2D playerRB;
    public float maxSwipetime;
    public float minSwipeDistance;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private bool hasJumped = false;


    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipelength;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        //slimeDownCollider.
        //slimeDownCollider.transform.position(-Vector3.up, 0, 0);
        //slimeCollider.SetActive(true);
    }
    void FixedUpdate()
    {
        SwipeTest();
        playerRB.velocity= new Vector2(dir * speed * Time.deltaTime, playerRB.velocity.y);
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
            playerRB.velocity = Vector2.down * jumpHeight * Time.deltaTime;
            Debug.Log("abaixou");
            }
        }
    void jump()
    {
        playerRB.velocity = Vector2.up * jumpHeight * Time.deltaTime;
        hasJumped = true;
        // transform.Translate(Vector2.up * 100 * Time.deltaTime);
        //playerRB.AddForce(new Vector2(0, 400));
    }
    void down()
    {
       // slimeDownCollider.SetActive(true);
        //slimeCollider.SetActive(false);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            hasJumped = false;
        }
    }
}





