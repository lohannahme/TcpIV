using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class seguirSlimo : MonoBehaviour
{
    public GameObject Personagem;
    
    void Update()
    {
        float Posy=transform.position.y;
        float x = Personagem.transform.position.x ;
        transform.position = new Vector3(x, Posy, 0);
    }

}
