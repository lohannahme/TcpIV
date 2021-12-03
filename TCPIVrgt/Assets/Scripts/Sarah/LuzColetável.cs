using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzColetável : MonoBehaviour
{
    
    private PlayerController slime;
    void Start()
    {
        slime = GameObject.FindObjectOfType<PlayerController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((float)transform.position.x < slime.transform.position.x -10)
        {
            Debug.Log("entrou");
            Destroy(this.gameObject);
        }

    }
}
