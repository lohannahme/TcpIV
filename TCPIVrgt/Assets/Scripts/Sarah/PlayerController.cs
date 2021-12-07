﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //public BoxCollider2D slimeDownCollider;
    //public GameObject slimeCollider;
    private Animator anim;
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
    bool animWalking = false;
    public BoxCollider2D box2D;
    public BoxCollider2D box2DDown;
    private float tempAnim =1f;

    private Vector2 startSwipePosition;
    private Vector2 endSwipePosition;
    private float swipelength;

    void Start()
    {
        playerRB = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        box2D.enabled = true;
        box2DDown.enabled = false;
    }
    void FixedUpdate()
    {
        SwipeTest();
        transitionDownWalk();
        walk();
        
        //teste pc
       /* if (Input.GetMouseButtonDown(0) && hasJumped == false)
        {
            jump();
        }
        if (Input.GetMouseButtonDown(1))
        {
            down();
        }*/
    }
    void walk()
    {
        playerRB.velocity = new Vector2(dir * speed * Time.deltaTime, playerRB.velocity.y);
    }
    void transitionDownWalk()
    {
        if (animWalking == true)
        {
            tempAnim -= Time.deltaTime;
            if (tempAnim < 0)
            {
                box2D.enabled = true;
                box2DDown.enabled = false;
                anim.SetTrigger("walking");
                animWalking = false;
                tempAnim = 1f;
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
        playerRB.velocity = Vector2.up * jumpHeight * Time.deltaTime;
        hasJumped = true;
        anim.SetBool("jumped", true);
        // transform.Translate(Vector2.up * 100 * Time.deltaTime);
        //playerRB.AddForce(new Vector2(0, 400));
    }
    void down()
    {
        if (hasJumped==true)
        {
            anim.SetBool("a", true);
            anim.SetBool("jumped", false);
            playerRB.velocity = Vector2.down * jumpHeight * Time.deltaTime;
            
            
        }
        if (hasJumped == false)
        {
            anim.SetTrigger("downD");
            animWalking = true;
            box2D.enabled = false;
            box2DDown.enabled = true;
        }
       
        Debug.Log("abaixou");

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "chao")
        {
            anim.SetBool("jumped", false);
            anim.SetBool("a", false);
            hasJumped = false;
        }
    }
}





