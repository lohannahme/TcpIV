using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzColetável : MonoBehaviour
{
    
    private PlayerController slime;
    private UiController uiController;
    void Start()
    {
        slime = GameObject.FindObjectOfType<PlayerController>();
        uiController = GameObject.FindObjectOfType<UiController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((float)transform.position.x < slime.transform.position.x -10)
        {
            Destroy(this.gameObject);
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "slime")
        {
            Destroy(this.gameObject);
            uiController.points++;
        }
    }
}
