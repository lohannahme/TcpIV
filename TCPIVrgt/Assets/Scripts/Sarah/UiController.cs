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
    public GameObject luzSlime;
    public GameObject vitoriaUi;
    public GameObject derrotaUi;
    public GameObject botoesUi;
    public Text recordeUi;
    public Text pointsUiDerrota;
    public Text recordeTextDerrota;

    public AudioClip vitoriaAudio;
    public AudioClip derrotaAudio;
    public AudioSource endGame;
    //
    public Text pointsUi;
    public Text distanceUi;
    public GameObject pausedUi;
    public Image pauseButtonImg;
    public Sprite pauseSprite;
    public Sprite playSprite;

    public int points;
    private float distance;
    private float distanceConst;
    public float distanceIncrease;
    public int dist;
    public int scoreTotal;
    public int minPowerUp;
    public bool gameOverCheck = false;
    private int prevRecord;
    private string tag;

    //contagem de coletavel pego
    public int contCol;
    void Start()
    {
        _get = this;
        luzSlime.SetActive(true);
        points = 0;
        dist = Mathf.RoundToInt(distance);
        slime = GameObject.FindObjectOfType<PlayerController>();
        distanceConst = 8f;
        distanceIncrease = 1;

        SetScaleZero();

        FirebaseManager.get.DatabaseManager.OnUserUpdate += OnUserScoreUpdate;
        FirebaseManager.get.DatabaseManager.GetUserData();
    }

    void Update()
    {

        distanceCount();

        //Debug.Log(PlayerPrefs.GetInt("recorde"));
        pointsUi.text = points.ToString();
        distanceUi.text = dist.ToString();
        lightCheck();
        scoreTotal = (points * 5) + dist;

#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape))
            pause();
#endif
    }
    public void pause()
    {
        if (!gameOverCheck)
        {
            bool paused = Time.timeScale == 1.0f;
            Time.timeScale = /*Time.timeScale == 1.0f*/paused ? 0.0f : 1.0f;
            /*if (Time.timeScale == 1.0f) */pausedUi.SetActive(paused);
            //if (Time.timeScale == 0.0f) pausedUi.SetActive(true);
            slime.walkSound.Stop();
            pauseButtonImg.sprite = paused ? playSprite : pauseSprite;
        }

    }
    public void gameOver()
    {
        gameOverCheck = true;
        int aux = prevRecord;//PlayerPrefs.GetInt("recorde");
        luzSlime.SetActive(false);
        if (scoreTotal > aux)
        {
            vitoriaUi.transform.LeanScale(Vector3.one, .2f).setIgnoreTimeScale(true);
            endGame.clip = vitoriaAudio;
            endGame.Play();
            //vitoriaUi.SetActive(true);
            UserData userData = new UserData { username = FirebaseManager.get.AuthManager.GetUserData().DisplayName, score = scoreTotal };
            FirebaseManager.get.DatabaseManager.UpdateData(userData);// PlayerPrefs.SetInt("recorde", scoreTotal);
            recordeUi.text = scoreTotal.ToString();// PlayerPrefs.GetInt("recorde").ToString();
        }
        else if (scoreTotal <= aux)
        {
            derrotaUi.transform.LeanScale(Vector3.one, .2f).setIgnoreTimeScale(true);
            endGame.clip = derrotaAudio;
            endGame.Play();
            //derrotaUi.SetActive(true);
            pointsUiDerrota.text = scoreTotal.ToString();
            recordeTextDerrota.text = aux.ToString();// PlayerPrefs.GetInt("recorde").ToString();
        }
        botoesUi.transform.LeanScale(Vector3.one, .2f).setIgnoreTimeScale(true);
        Time.timeScale = 0;

    }
    public void restart()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }
    void distanceCount()
    {
        distance += distanceConst * Time.deltaTime * distanceIncrease;
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

    private void SetScaleZero()
    {
        derrotaUi.transform.localScale = Vector3.zero;
        vitoriaUi.transform.localScale = Vector3.zero;
        botoesUi.transform.localScale = Vector3.zero;
    }

    private void OnUserScoreUpdate(Firebase.Database.DataSnapshot data)
    {
        Debug.Log("" + data);
        if (data != null)
        {
            string _prescore = data.Value.ToString();
            int _score = 0;
            if (int.TryParse(_prescore, out _score))
                //scoreText.text = _score.ToString();
                prevRecord = _score;
            else
                Debug.LogError(message: "Invalid score data");
        }
    }
    private void OnUserDataUpdate(Firebase.Database.DataSnapshot data)
    {
        Debug.Log("" + data + ", " + data.ChildrenCount);
        if (data != null && data.HasChildren)
        {
            Firebase.Database.DataSnapshot nameValue = data.Child("username");
            if (nameValue == null)
            {
                Debug.LogError(message: "No username child.");
                return;
            }
            tag = nameValue.Value.ToString();

            Firebase.Database.DataSnapshot scoreValue = data.Child("score");
            if (scoreValue == null)
            {
                Debug.LogError(message: "No score child.");
                return;
            }
            string _prescore = scoreValue.Value.ToString();
            int _score = 0;
            if (int.TryParse(_prescore, out _score))
                //scoreText.text = _score.ToString();
                prevRecord = _score;
            else
                Debug.LogError(message: "Invalid score data");
        }
        else
            Debug.LogError(message: "No data.");
    }

    private void OnDestroy()
    {
        if (FirebaseManager.get)
        {
            if (FirebaseManager.get.DatabaseManager)
                FirebaseManager.get.DatabaseManager.OnUserUpdate -= OnUserScoreUpdate;
        }
    }
}
