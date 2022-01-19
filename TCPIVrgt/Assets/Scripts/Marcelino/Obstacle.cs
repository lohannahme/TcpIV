using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public Vector3 startPosition;
    public float speed;
    public AudioSource warning;

    [SerializeField] float _speedFactor;
    protected float speedFactor
    {
        get
        {
            _speedFactor = PlayerController.get.SpeedMult;

            return _speedFactor;
        }
    }
    
    public virtual void Spawn()
    {
        Vector3 playerPos = PlayerController.get.transform.position;
        //float distanceAhead;//
        transform.position = new Vector3(playerPos.x + startPosition.x /* speedFactor*/, startPosition.y, 0f);
        warning.Play();
    }
}
