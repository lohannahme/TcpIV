using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    public static ObstacleManager get;
    [SerializeField] Obstacle drone, tronco, tiro;
    [SerializeField] GameObject coletavel;

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
        //Vector3 spawnPos = PlayerController.get.transform.position;
        obstacle.Spawn();
    }

    public void SpawnColetavel()
    {
        Instantiate(coletavel, new Vector3(PlayerController.get.transform.position.x + 20, Random.Range(-3.5f, -1.5f), 0), Quaternion.identity);
        //coletavel.transform.position = new Vector3(PlayerController.get.transform.position.x + 20, Random.Range(-3.5f, -1.5f));
    }
}
