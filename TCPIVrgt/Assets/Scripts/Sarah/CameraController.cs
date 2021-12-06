using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject Personagem;
    
        void Update()
        {
        float x = Personagem.transform.position.x+5f;
        float z = Personagem.transform.position.z - 10.0f;
        transform.position = new Vector3(x, 0, z);
    }
    
}
