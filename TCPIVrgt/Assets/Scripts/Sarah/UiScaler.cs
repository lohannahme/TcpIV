using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiScaler : MonoBehaviour
{
    // Start is called before the first frame update
    public float resoX;
    public float resoY;

    private CanvasScaler can;
    void Start()
    {
        can = GetComponent<CanvasScaler>();
        SetInfo();
    }
    void SetInfo()
    {
        resoX = (float)Screen.currentResolution.width;
        resoY = (float)Screen.currentResolution.height;


        can.referenceResolution = new Vector2(resoX, resoY);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
