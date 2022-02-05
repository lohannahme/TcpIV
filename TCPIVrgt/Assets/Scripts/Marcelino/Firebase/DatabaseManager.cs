using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Database;
using Firebase.Extensions; // for ContinueWithOnMainThread

public class DatabaseManager : MonoBehaviour
{
    DatabaseReference DBReference;
    AuthManager authManager { get { return FirebaseManager.get.AuthManager; } }

    public delegate void GenericEvent(DataSnapshot data);
    public GenericEvent OnUserUpdate;
    public GenericEvent OnLeaderboardUpdate;

    private void Awake()
    {
        FirebaseManager.get.OnInitializeFirebase += OnInitializeHandler;
    }

    private void OnInitializeHandler()
    {
        DBReference = FirebaseDatabase.DefaultInstance.RootReference;
        Debug.Log("Banco de Dados conectado.");

        /*DBReference.Child("users").OrderByChild("score")/*.LimitToLast(5)* /.ValueChanged +=
                    OnLeaderboardValueChangedHandler;

        if (authManager.GetUserData() != null)
            DBReference.Child("users").Child(authManager.GetUserData().UserId).ValueChanged +=
                        OnUserValueChangedHandler;*/
    }

    private void OnDisable()
    {
        FirebaseManager.get.OnInitializeFirebase -= OnInitializeHandler;

        /*DBReference.Child("users").OrderByChild("score")/*.LimitToLast(5)* /.ValueChanged -=
                    OnLeaderboardValueChangedHandler;

        if (authManager.GetUserData() != null)
            DBReference.Child("users").Child(authManager.GetUserData().UserId).ValueChanged -=
                        OnUserValueChangedHandler;*/
    }

    public void UpdateField<T>(T data, string _field)
    {
        /*DBReference.Child("users").OrderByChild("score")/*.LimitToLast(5)* /.ValueChanged +=
                    OnLeaderboardValueChangedHandler;

        if (authManager.GetUserData() != null)
            DBReference.Child("users").Child(authManager.GetUserData().UserId).ValueChanged +=
                        OnUserValueChangedHandler;*/

        string uid = authManager.GetUserData().UserId;
        DBReference.Child("users").Child(uid).Child(_field).SetValueAsync(data).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SetValueAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SetValueAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Value updated successfully.");
                // O tratamento é feito por evento
            }
        );
    }

    public void UpdateData<T>(T data)
    {
        /*DBReference.Child("users").OrderByChild("score")/*.LimitToLast(5)* /.ValueChanged +=
                    OnLeaderboardValueChangedHandler;

        if (authManager.GetUserData() != null)
            DBReference.Child("users").Child(authManager.GetUserData().UserId).ValueChanged +=
                        OnUserValueChangedHandler;*/

        string uid = authManager.GetUserData().UserId;
        string json = JsonUtility.ToJson(data);
        DBReference.Child("users").Child(uid).SetRawJsonValueAsync(json).ContinueWithOnMainThread(
            task =>
            {
                if (task.IsCanceled)
                {
                    Debug.LogError("SetRawJsonValueAsync was canceled.");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("SetRawJsonValueAsync encountered an error: " + task.Exception);
                    return;
                }

                Debug.Log("Data updated successfully.");
                // O tratamento é feito por evento
            }
        );
    }
    
    private void OnLeaderboardValueChangedHandler(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        /*DBReference.Child("users").OrderByChild("score")/*.LimitToLast(5)* /.ValueChanged -=
                    OnLeaderboardValueChangedHandler;
        if (authManager.GetUserData() != null)
            DBReference.Child("users").Child(authManager.GetUserData().UserId).ValueChanged -=
                        OnUserValueChangedHandler;*/

        // Do something with the data in args.Snapshot
        OnLeaderboardUpdate?.Invoke(args.Snapshot);
    }

    private void OnUserValueChangedHandler(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }
        
        /*DBReference.Child("users").OrderByChild("score")/*.LimitToLast(5)* /.ValueChanged -=
                    OnLeaderboardValueChangedHandler;
        if (authManager.GetUserData() != null)
            DBReference.Child("users").Child(authManager.GetUserData().UserId).ValueChanged -=
                        OnUserValueChangedHandler;*/

        // Do something with the data in args.Snapshot
        OnUserUpdate?.Invoke(args.Snapshot);
    }

    public void GetUserData()
    {
        DBReference.Child("users").Child(authManager.GetUserData().UserId).Child("score").GetValueAsync().ContinueWithOnMainThread(
            task => {
                if (task.IsFaulted)
                {
                    Debug.LogError(task.Exception.Message);
                    FBLog.PrintAuth("Error: " + task.Exception.Message);
                    return;
                }
                if (task.IsCompleted)
                {
                    Debug.Log("task is completed");
                    OnUserUpdate?.Invoke(task.Result);
                }
            }
        );
    }

    public void GetLeaderboardData()
    {
        DBReference.Child("users").OrderByChild("score").GetValueAsync().ContinueWithOnMainThread(task => {
            if (task.IsFaulted)
            {
                // Handle the error...
                Debug.LogError(task.Exception.Message);
                FBLog.PrintAuth("Error: " + task.Exception.Message);
                return;
            }
            if (task.IsCompleted)
            {
                Debug.Log("task is completed");
                DataSnapshot snapshot = task.Result;
                // Do something with snapshot...
                OnLeaderboardUpdate?.Invoke(snapshot);
            }
        });
    }
}
