using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    private static UiController _get;
    public static UiController get { get { return _get; } }
    public PlayerController slime;
    public Text pointsUi;
    public Text distanceUi;
    public GameObject pausedUi;
    public int points;
    private float distance;
    private float distanceConst;
    public float distanceIncrease;
    public int dist;
    public int minPowerUp;

    //contagem de coletavel pego
    public int contCol;
    void Start()
    {
        _get = this;

        points = 0;
        dist= Mathf.RoundToInt(distance);
        slime = GameObject.FindObjectOfType<PlayerController>();
        distanceConst = 8f;
        distanceIncrease = 1;
    }

    void Update()
    {
        distanceCount();
        pointsUi.text = points.ToString();
        distanceUi.text = dist.ToString();
        lightCheck();

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
            pause();
#endif
    }
    public void pause()
    {
        Time.timeScale = Time.timeScale == 1.0f ? 0.0f : 1.0f;
        if (Time.timeScale == 1.0f) pausedUi.SetActive(false);
        if (Time.timeScale == 0.0f) pausedUi.SetActive(true);
        slime.walkSound.Stop();

    }
    void distanceCount()
    {
        distance += distanceConst * Time.deltaTime* distanceIncrease;
        dist = Mathf.RoundToInt(distance);
    }
    void lightCheck()
    {
        if (contCol >= minPowerUp)
        {
            slime.increaseLight();
        }
        else
        {
            slime.decreaseLight();
        }
    }
}
