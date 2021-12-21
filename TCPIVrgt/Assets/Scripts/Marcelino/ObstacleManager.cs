using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager get;
    [SerializeField] Obstacle drone, tronco, tiro;
    [SerializeField] PlayerController player;

    private void Awake()
    {
        get = this;
    }
    
    public void SpawnDrone()
    {
        Spawn(drone);
    }

    public void SpawnTronco()
    {
        Spawn(tronco);
    }

    public void SpawnTiro()
    {
        Spawn(tiro);
    }

    private void Spawn(Obstacle obstacle)
    {
        obstacle.gameObject.SetActive(true);
        obstacle.Spawn(player.transform.position);
    }
}
