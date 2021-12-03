using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class luzController : MonoBehaviour
{
    public GameObject Personagem;
    public GameObject prefabLuz;
    private bool instanciou= false;

    private float temp1;
    void Start()
    {
        temp1 = Random.Range(2, 6);
        Instantiate(prefabLuz, new Vector3(Personagem.transform.position.x + 20, Random.Range(-3.5f, -1.5f), 0), Quaternion.identity);
    }
    void Update()
    {
        Debug.Log(temp1);
        temp1 -= Time.deltaTime;
        if (temp1 < 0)
        {
            if (instanciou == false)
            {
                Instantiate(prefabLuz, new Vector3(Personagem.transform.position.x + 20, Random.Range(-3.5f, -1.5f), 0), Quaternion.identity);
                instanciou = true;
            }
            if (instanciou == true)
            {
                instanciou = false;
                temp1 = Random.Range(2,6);
            }
            //temp1 = 5f;
        }
    }
}
