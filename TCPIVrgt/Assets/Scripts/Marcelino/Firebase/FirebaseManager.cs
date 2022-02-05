using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;

[RequireComponent(typeof(AuthManager)), RequireComponent(typeof(DatabaseManager))]
public class FirebaseManager : MonoBehaviour
{
    private static FirebaseManager _get;
    public static FirebaseManager get
    {
        get
        {
            if (_get == null)
            {
                _get = FindObjectOfType<FirebaseManager>();
                if (_get == null)
                    return null;
                _get.AuthManager = _get.GetComponent<AuthManager>();
                _get.DatabaseManager = _get.GetComponent<DatabaseManager>();

                DontDestroyOnLoad(_get);
            }
            return _get;
        }
    }

    public AuthManager AuthManager { get; private set; }
    public DatabaseManager DatabaseManager { get; private set; }

    public delegate void InitializeEvent();
    public InitializeEvent OnInitializeFirebase;

    private void Awake()
    {
        if (_get == null)
        {
            _get = this;
            AuthManager = GetComponent<AuthManager>();
            DatabaseManager = GetComponent<DatabaseManager>();

            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        DependencyStatus dependency;

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {

            Debug.Log("Conectando ao Banco de Dados...");

            dependency = task.Result;
            if (dependency == DependencyStatus.Available)
            {
                Debug.Log("Dependencias do Firebase resolvidas: dependency = " + dependency);

                get.OnInitializeFirebase?.Invoke();
            }
            else
            {
                Debug.LogError("Dependencias do Firebase não resolvidas: " + dependency);
            }
        });
    }

    private void Update()
    {
        FBLog.Update(Time.deltaTime);
    }
}

public class FBLog
{
    public enum FirebaseTool { Auth, RTDataBase }

    public static string dbPrint;
    public static string authPrint;
    private static Queue<string> _dbQ;
    private static Queue<string> dbQ
    {
        get
        {
            if (_dbQ == null)
                _dbQ = new Queue<string>();
            return _dbQ;
        }
    }
    private static Queue<string> _authQ;
    private static Queue<string> authQ
    {
        get
        {
            if (_authQ == null)
                _authQ = new Queue<string>();
            return _authQ;
        }
    }

    private static float stdTime = 5f;
    private static float flexTimeAu { get { return authQ.Count > 0 ? stdTime / authQ.Count : stdTime; } }
    private static float flexTimeDB { get { return dbQ.Count > 0 ? stdTime / dbQ.Count : stdTime; } }
    private static float countAu, countDB;
    private static bool flagAu = false;


    public static void Print(string msg, FirebaseTool tool)
    {
        switch (tool)
        {
            case FirebaseTool.Auth:
                PrintAuth(msg);
                break;
            case FirebaseTool.RTDataBase:
                PrintDB(msg);
                break;
        }
    }
    public static void PrintAuth(string msg)
    {
        if (authQ.Count == 0)
            countAu *= 0.5f;
        authQ.Enqueue(msg);
    }
    public static void PrintDB(string msg)
    {
        if (dbQ.Count == 0)
            countDB *= 0.5f;
        dbQ.Enqueue(msg);
    }

    public static void Update(float deltaTime)
    {
        UpdateAuth(deltaTime);
    }
    private static void UpdateAuth(float deltaTime)
    {
        /*if (authQ.Count == 0)
        {
            countAu = flexTimeAu;
            return;
        }*/
        countAu -= deltaTime;
        if (countAu <= 0f)
        {
            countAu += flexTimeAu;
            PopLogAuth();
        }
    }
    private static void UpdateDB(float deltaTime)
    {
        /*if (dbQ.Count == 0)
        {
            countDB = flexTimeDB;
            return;
        }*/
        countDB -= deltaTime;
        if (countDB <= 0f)
        {
            countDB += flexTimeDB;
            PopLogDB();
        }
    }
    private static void PopLogAuth()
    {
        authPrint = (authQ.Count > 0 ? authQ.Dequeue() : "");
    }
    private static void PopLogDB()
    {
        dbPrint = (dbQ.Count > 0 ? dbQ.Dequeue() : "");
    }
}