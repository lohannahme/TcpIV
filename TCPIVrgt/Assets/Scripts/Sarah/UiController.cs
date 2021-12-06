using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Text pointsUi;
    public Text distanceUi;
    public GameObject pausedUi;
    public int points;
    public float distance;
    public float a;
    public int dist;
    void Start()
    {
        points = 0;
         dist= Mathf.RoundToInt(distance);
}

    void Update()
    {
        distanceCount();
        pointsUi.text = points.ToString();
        distanceUi.text = dist.ToString();
    }
    public void pause()
    {
        Time.timeScale = Time.timeScale == 1.0f ? 0.0f : 1.0f;
        if (Time.timeScale == 1.0f) pausedUi.SetActive(false);
        if (Time.timeScale == 0.0f) pausedUi.SetActive(true);
    }
    void distanceCount()
    {
        distance += Time.deltaTime*1.5f;
        dist = Mathf.RoundToInt(distance);
    }
}
