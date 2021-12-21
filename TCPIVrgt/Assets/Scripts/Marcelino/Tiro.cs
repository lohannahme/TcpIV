using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiro : Obstacle
{
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, 0f);
        /*if (Camera.main.ViewportToScreenPoint(transform.position).x > Screen.width)
        {
            gameObject.SetActive(false);
        }*/
    }
}
