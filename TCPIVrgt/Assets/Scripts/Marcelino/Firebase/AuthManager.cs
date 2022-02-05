using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Extensions; // for ContinueWithOnMainThread

public enum AuthState { login, logout, signin, logoutANDsignin }

public class AuthManager : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseUser user;

    public delegate void AuthEvent(AuthState state, FirebaseUser _user);
    public AuthEvent OnAuthStateChanged;

    public FirebaseUser GetUserData()
    {
        //FirebaseUser _user = auth.CurrentUser;
        /*if (_user != null)
        {
            //TODO:
            string name = _user.DisplayName;
            string email = _user.Email;
            System.Uri photo_url = _user.PhotoUrl;
            // The user's Id, unique to the Firebase project.
            // Do NOT use this value to authenticate with your backend server, if you
            // have one; use User.TokenAsync() instead.
            string uid = _user.UserId;
        }*/
        return (auth != null ? auth.CurrentUser : null);
    }

    private void Awake()
    {
        FirebaseManager.get.OnInitializeFirebase += OnInitializeHandler;
    }

    private void OnDisable()
    {
        FirebaseManager.get.OnInitializeFirebase -= OnInitializeHandler;
        auth.StateChanged -= AuthStateChanged;
        auth = null;
    }

    // Handle initialization of the necessary firebase modules:
    private void OnInitializeHandler()
    {
        auth = FirebaseAuth.DefaultInstance;
        Debug.Log("Sitema de autenticação ao Banco de Dados conectado.");
        //loginMessages = "Sitema de autenticação do Banco de Dados conectado.";

        auth.StateChanged += AuthStateChanged;
        AuthStateChanged(this, null);
    }

    public bool Connected { get { return auth != null && auth.CurrentUser != null && user == auth.CurrentUser; } }

    // Track state changes of the auth object.
    private void AuthStateChanged(object sender, System.EventArgs eventArgs)
    {
        /*Debug.Log("Auth: " + auth + ", U: " + user + ", CU == U: " + (auth.CurrentUser == user)
            + ", Uname: " + (user != null ? user.DisplayName : null)
            + ", CUname: " + (auth != null ? (auth.CurrentUser != null ? auth.CurrentUser.DisplayName : null) : null));*/

        bool signedIn = auth.CurrentUser != null &&
                    (auth.CurrentUser.DisplayName == null || auth.CurrentUser.DisplayName == "");
        if (auth.CurrentUser != null) Debug.Log("Provider: " + auth.CurrentUser.ProviderId);
        if (auth.CurrentUser != user)
        {
            bool loggedIn = auth.CurrentUser != null && !string.IsNullOrEmpty(auth.CurrentUser.DisplayName);

            user = auth.CurrentUser;

            if (!loggedIn && user != null)
            {
                Debug.Log("Signed ??? " + user.UserId);
                OnAuthStateChanged?.Invoke(AuthState.signin, user);
            }
            if (loggedIn)
            {
                Debug.Log("Logged in " + user.UserId);
                OnAuthStateChanged?.Invoke(AuthState.login, user);
            }
            if (!Connected)
            {
                Debug.Log("Logged out ");
                OnAuthStateChanged?.Invoke(AuthState.logout, user);
            }
        }
        else
        {
            if (signedIn)
            {
                Debug.Log("Signed in " + user.Email);
                OnAuthStateChanged?.Invoke(AuthState.logoutANDsignin, user);
            }
        }
    }

    public void UpdateUserProfile(string _username)
    {
        FirebaseUser _user = auth.CurrentUser;
        if (_user != null)
        {
            UserProfile profile = new UserProfile
            {
                DisplayName = _username,
                //PhotoUrl = new System.Uri("https://example.com/jane-q-user/profile.jpg"),
            };
            _user.UpdateUserProfileAsync(profile).ContinueWithOnMainThread(task => {
                if (task.IsCanceled)
                {
                    Debug.LogError("UpdateUserProfileAsync was canceled.");
                    FBLog.PrintAuth("Ação cancelada");
                    return;
                }
                if (task.IsFaulted)
                {
                    Debug.LogError("UpdateUserProfileAsync encountered an error: " + task.Exception);
                    FirebaseException ex = task.Exception.GetBaseException() as FirebaseException;
                    AuthError error = (AuthError)ex.ErrorCode;

                    switch(error)
                    {
                        case AuthError.AccountExistsWithDifferentCredentials:
                        case AuthError.CredentialAlreadyInUse:
                        case AuthError.EmailAlreadyInUse:
                            //
                            break;
                        case AuthError.InvalidActionCode:
                        case AuthError.InvalidApiKey:
                        case AuthError.InvalidAppCredential:
                        case AuthError.InvalidClientId:
                        case AuthError.InvalidCredential:
                        case AuthError.InvalidCustomToken:
                        case AuthError.InvalidEmail:
                        case AuthError.InvalidUserToken:
                            //
                            break;
                        case AuthError.MissingAppCredential:
                        case AuthError.MissingAppToken:
                        case AuthError.MissingClientIdentifier:
                        case AuthError.MissingContinueUri:
                        case AuthError.MissingEmail:
                            //
                            break;
                        case AuthError.MissingPassword:
                            //
                            break;
                        case AuthError.NetworkRequestFailed:
                            //
                            break;
                        case AuthError.UserNotFound:
                            //
                            break;
                        case AuthError.WeakPassword:
                            //
                            break;
                        case AuthError.WrongPassword:
                            //
                            break;
                        default:
                            //
                            break;
                    }
                    FBLog.PrintAuth("Error: " + error.ToString());

                    return;
                }

                Debug.Log("User profile updated successfully.");
                //
            });
        }
    }

    public void Register(string _username, string _tag, string _password)
    {
        auth.CreateUserWithEmailAndPasswordAsync($"{_username}@slimo.com", _password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync was canceled.");
                FBLog.PrintAuth("Ação cancelada");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("CreateUserWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                FBLog.PrintAuth("Error: " + task.Exception.Message);
                return;
            }

            // Firebase user has been created.
            FirebaseUser newUser = task.Result;
            Debug.Log($"Firebase user created successfully: {newUser.DisplayName} ({newUser.UserId})");
            UpdateUserProfile(_tag);
        });
    }

    public void Login(string _username, string _password)
    {
        auth.SignInWithEmailAndPasswordAsync($"{_username}@slimo.com", _password).ContinueWithOnMainThread(task => {
            if (task.IsCanceled)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync was canceled.");
                FBLog.PrintAuth("Ação cancelada");
                return;
            }
            if (task.IsFaulted)
            {
                Debug.LogError("SignInWithEmailAndPasswordAsync encountered an error: " + task.Exception);
                FBLog.PrintAuth("Error: " + task.Exception.Message);
                return;
            }

            FirebaseUser newUser = task.Result;
            Debug.Log($"User signed in successfully: {newUser.DisplayName} ({newUser.UserId})");
        });
    }

    public void SignOut()
    {
        auth.SignOut();
    }
}
