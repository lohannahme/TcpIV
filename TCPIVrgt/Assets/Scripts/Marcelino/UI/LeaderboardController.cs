using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Firebase.Database;
using Firebase.Auth;
using System.Linq;

[System.Serializable]
public class UserData
{
    public string username;
    public int score;
}
[System.Serializable]
public class ScoreBoard
{
    public Text pos;
    public Text text;
    public bool user;

    public void Update(string username, int score, int pos, bool sameAsUser = false)
    {
        if (this.pos != null)
            this.pos.text = (pos > 0 ? pos.ToString("D2") : "--") + ".";

        this.text.text = username + (score > 0 ? "-" + score : "");

        if (user)
            this.text.color = Color.yellow;
        else
            this.text.color = (sameAsUser ? Color.yellow : Color.white);
    }
}

public class LeaderboardController : MonoBehaviour
{
    /*[Header("User")]
    [SerializeField] Text usernameText;
    [SerializeField] Text scoreText;
    [SerializeField] Text msgText;
    [SerializeField] InputField scoreInput;*/

    //[Header("Leaderboard")]
    //[SerializeField] GameObject[] LineContainers;
    [SerializeField] ScoreBoard[] leaderBoard;
    [SerializeField] ScoreBoard userScores;
    /*private ScoreBoard[] LeaderBoard
    {
        get
        {
            if (leaderBoard == null || leaderBoard.Length != LineContainers.Length)
            {
                leaderBoard = new ScoreBoard[LineContainers.Length];
                for (int i = 0; i < leaderBoard.Length; i++)
                {
                    Text[] line = LineContainers[i].GetComponentsInChildren<Text>();
                    leaderBoard[i] = new ScoreBoard
                    {
                        position = line[0],
                        username = line[1],
                        score = line[2]
                    };
                }
            }
            return leaderBoard;
        }
    }*/

    DatabaseManager database { get {
            return FirebaseManager.get != null ? FirebaseManager.get.DatabaseManager : null; } }
    AuthManager auth { get {
            return FirebaseManager.get != null ? FirebaseManager.get.AuthManager : null; } }
    FirebaseUser User { get {
            return FirebaseManager.get != null ? FirebaseManager.get.AuthManager.GetUserData() : null; } }

    // Start is called before the first frame update
    void Start()
    {
        //database.OnUserUpdate += OnUserUpdateHandler;
        database.OnLeaderboardUpdate += OnLeaderboardUpdateHandler;
        auth.OnAuthStateChanged += ClearLeaderboardUIHandler;
    }

    private void OnDisable()
    {
        if (FirebaseManager.get != null)
        {
            //database.OnUserUpdate -= OnUserUpdateHandler;
            database.OnLeaderboardUpdate -= OnLeaderboardUpdateHandler;
            auth.OnAuthStateChanged -= ClearLeaderboardUIHandler;
        }
    }
    
    public void OpenLeaderboard()
    {
        //
    }
    
    /*private void OnUserUpdateHandler(DataSnapshot data)
    {
        if (data != null && data.HasChildren)
        {
            DataSnapshot nameValue = data.Child("username");
            if (nameValue == null)
            {
                Debug.LogError(message: "No username child.");
                return;
            }
            string _username = nameValue.Value.ToString();
            usernameText.text = _username ?? "null";

            DataSnapshot scoreValue = data.Child("score");
            if (scoreValue == null)
            {
                Debug.LogError(message: "No score child.");
                return;
            }
            string _prescore = scoreValue.Value.ToString();
            int _score = 0;
            if (int.TryParse(_prescore, out _score))
                scoreText.text = _score.ToString();
            else
                Debug.LogError(message: "Invalid score data");
        }
        else
            Debug.LogError(message: "No data.");
    }*/
    private void OnUserUpdateHandler(UserData data, int pos)
    {
        /*usernameText.text = data.username;
        scoreText.text = data.score.ToString();

        LeaderBoard[4].Update(pos.ToString(), data.username, data.score,
                            true);*/
        userScores.Update(data.username, data.score, pos, true);
    }

    public void OnUpdateLeaderboard()
    {
        database.GetLeaderboardData();
    }
    private void OnLeaderboardUpdateHandler(DataSnapshot data)
    {
        Debug.Log("Update Leaderboard; " + data.ChildrenCount);
        ClearLeaderboardUI();
        int i = 0;
        foreach (DataSnapshot dataChild in data.Children.Reverse())
        {
            bool sameAsUser = User != null ? dataChild.Key == User.UserId : false;
            if (i < leaderBoard.Length || sameAsUser)
            {
                string log = "data: " + dataChild.Key + ", child count: " + dataChild.ChildrenCount;
                UserData userData = new UserData();
                DataSnapshot nameValue = dataChild.Child("username");
                log += ", nameV: " + nameValue;
                if (nameValue == null)
                {
                    Debug.LogError(message: "No leaderboard username child.");
                    return;
                }
                //Debug.Log("Raw leaderboard value: " + userData.Value.ToString());
                string _username = nameValue.Value.ToString();
                userData.username = _username ?? "null";
                log += ", tag: " + _username;

                DataSnapshot scoreValue = dataChild.Child("score");
                if (scoreValue == null)
                {
                    Debug.LogError(message: "No score child.");
                    return;
                }
                string _prescore = scoreValue.Value.ToString();
                int _score = 0;
                if (int.TryParse(_prescore, out _score))
                    userData.score = _score;
                else
                    Debug.LogError(message: "Invalid score data");
                log += "score string: " + _prescore + ", score int: " + _score + ", i: " + i;
                Debug.Log(log);
                if (sameAsUser)
                {
                    Debug.Log("same as user");
                    OnUserUpdateHandler(userData, i + 1);
                }
                if (i < leaderBoard.Length && i >= 0)
                {
                    leaderBoard[i].Update(userData.username, userData.score,
                                i + 1, sameAsUser);
                    Debug.Log(i);
                }
            }
            i++;
        }
    }

    private void ClearLeaderboardUIHandler(AuthState state, FirebaseUser _user)
    {
        ClearLeaderboardUI();
    }
    public void ClearLeaderboardUI()
    {
        for (int i = 0; i < leaderBoard.Length; i++)
        {
            /*leaderBoard[i].text.text = "-";

            if (leaderBoard[i].user)
            {
                leaderBoard[i].text.color = Color.yellow;
                leaderBoard[i].pos.text = "--.";
            }

            leaderBoard[i].text.color = Color.white;*/
            leaderBoard[i].Update("", 0, i + 1);
        }
        userScores.Update("", 0, 0, true);
        //userScores.text.text = "-";
    }
}
