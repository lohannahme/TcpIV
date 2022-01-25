using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzColetável : MonoBehaviour
{
    private PlayerController slime;
    private UiController uiController;
    private float temp = 0.5f;
    private bool luzCol=true;
    void Start()
    {
        slime = GameObject.FindObjectOfType<PlayerController>();
        uiController = GameObject.FindObjectOfType<UiController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((float)transform.position.x < slime.transform.position.x -5)
        {
            uiController.contCol=0;
            Destroy(this.gameObject);
        }
        if (luzCol == false)
        {
            temp -= Time.deltaTime;
        }
        if (temp < 0)
        {
            luzCol = true;
            temp = 0.5f;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "slime"&&luzCol==true)
        {
            uiController.contCol++;
            uiController.points++;
            Destroy(this.gameObject);
            luzCol = false;
            
        }
    }
}
