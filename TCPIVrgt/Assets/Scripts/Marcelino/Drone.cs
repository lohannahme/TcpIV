using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Obstacle
{
    public float atackHeight;
    public float fallSpeed = 1;

    private void Update()
    {
        transform.position = new Vector3(transform.position.x /*- speed * Time.deltaTime*/, Mathf.Lerp(transform.position.y, atackHeight, fallSpeed * speedFactor * Time.deltaTime), 0);
        /*if (Camera.main.ViewportToScreenPoint(transform.position).x < 0)
        {
            gameObject.SetActive(false);
        }*/
    }
}
