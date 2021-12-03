using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    public float jumpHeight; 
    public float dir = 1;
    private Rigidbody2D playerRB;
    public float maxSwipetime;
    public float minSwipeDistance;

    private float swipeStartTime;
    private float swipeEndTime;
    private float swipeTime;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipelength;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        SwipeTest();
        //transform.Translate(Vector2.right * 10 * Time.deltaTime);
        playerRB.velocity= new Vector2(dir * speed * Time.deltaTime, playerRB.velocity.y);
        //playerRB.velocity = Vector2.up * jumpHeight * Time.deltaTime;
       // if (Input.GetMouseButtonDown(0)) //(Input.GetKeyDown(KeyCode.Space))
      //  {
           
            //transform.Translate(Vector2.up * 100 * Time.deltaTime);
           // playerRB.velocity = Vector2.up * jumpHeight * Time.deltaTime;
      //  }
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
            if (Distance.y > 0)
            {
                playerRB.velocity = Vector2.up * jumpHeight * Time.deltaTime;
            }
            if (Distance.y < 0)
            {
                Debug.Log("abaixou");
            }
        }
    }

}
