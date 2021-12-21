using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 startPosition;
    public float speed;
    public AudioSource warning;
    
    public void Spawn(Vector3 playerPosition)
    {
        transform.position = new Vector3(playerPosition.x + startPosition.x, startPosition.y, 0f);
        warning.Play();
    }
}
