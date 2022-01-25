using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiController : MonoBehaviour
{
    private static UiController _get;
    public static UiController get { get { return _get; } }
    public PlayerController slime;
    //gameover
    public GameObject vitoriaUi;
    public GameObject derrotaUi;
    public GameObject botoesUi;
    public Text recordeUi;
    public Text pointsUiDerrota;
    public Text recordeTextDerrota;
    //
    public Text pointsUi;
    public Text distanceUi;
    public GameObject pausedUi;
    public int points;
    private float distance;
    private float distanceConst;
    public float distanceIncrease;
    public int dist;
    public int scoreTotal;
    public int minPowerUp;
    public bool gameOverCheck=false;

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
       
        Debug.Log(PlayerPrefs.GetInt("recorde"));
        pointsUi.text = points.ToString();
        distanceUi.text = dist.ToString();
        lightCheck();
        scoreTotal=(points*5) +dist;

       // Debug.Log("scoreTotal :" + scoreTotal);
       // Debug.Log("prefsrecorde :" + PlayerPrefs.GetInt("recorde"));

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
            pause();
#endif
    }
    public void pause()
    {
        if (!gameOverCheck)
        {
            Time.timeScale = Time.timeScale == 1.0f ? 0.0f : 1.0f;
            if (Time.timeScale == 1.0f) pausedUi.SetActive(false);
            if (Time.timeScale == 0.0f) pausedUi.SetActive(true);
            slime.walkSound.Stop();
        }
       
    }
    public void gameOver()
    {
        gameOverCheck = true;
        int aux=PlayerPrefs.GetInt("recorde");
        if (scoreTotal >= aux)
        {
            vitoriaUi.SetActive(true);
            PlayerPrefs.SetInt("recorde", scoreTotal);
           recordeUi.text = PlayerPrefs.GetInt("recorde").ToString();
        }else if (scoreTotal < aux){
            derrotaUi.SetActive(true);
            pointsUiDerrota.text = scoreTotal.ToString();
            recordeTextDerrota.text = PlayerPrefs.GetInt("recorde").ToString();
        }
        botoesUi.SetActive(true);
        Time.timeScale = 0;

    }
    public void restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex );

    }
    public void menu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
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
