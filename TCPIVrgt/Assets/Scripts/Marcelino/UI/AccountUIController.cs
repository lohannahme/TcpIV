using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AccountUIController : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] Text usernameText;
    ButtonClick mainUIControl;

    [Header("Login")]
    [SerializeField] GameObject loginPanel;
    [SerializeField] InputField loginUsernameInput, loginPasswordInput;
    [SerializeField] Button loginEnterButton/*, loginSignOutButton*/;
    [SerializeField] Text loginStatusMsg;

    [Header("Register")]
    [SerializeField] GameObject registerPanel;
    [SerializeField] InputField registerUsernameInput, registerTagInput, registerPasswordInput;
    [SerializeField] Button registerSignInButton;
    [SerializeField] Text registerStatusMsg;

    // Start is called before the first frame update
    void Start()
    {
        FBLog.PrintAuth("");

        mainUIControl = GetComponent<ButtonClick>();
        loginPanel.transform.localScale = Vector3.zero;
        registerPanel.transform.localScale = Vector3.zero;

        FirebaseManager.get.AuthManager.OnAuthStateChanged += OnAuthStateChangedHandler;
        StartCoroutine(InitUpdate());
    }

    private void OnDisable()
    {
        if (FirebaseManager.get != null)
            FirebaseManager.get.AuthManager.OnAuthStateChanged -= OnAuthStateChangedHandler;
    }

    private void Update()
    {
        loginStatusMsg.text = FBLog.authPrint;
    }

    private void OnAuthStateChangedHandler(AuthState state, Firebase.Auth.FirebaseUser _user)
    {
        Debug.Log($"OnAuthStateChangeHandler {state}, {_user}, name: " + (_user != null ? _user.DisplayName : "null"));
        switch (state)
        {
            case AuthState.login:
                //loginStatusMsg.text = "Usuário conectado";
                FBLog.PrintAuth("Usuário conectado");
                OnCloseLogin();
                break;
            case AuthState.logout:
                //loginStatusMsg.text = "Usuário desconectado";
                FBLog.PrintAuth("Usuário desconectado");
                break;
            case AuthState.signin:
                //registerStatusMsg.text = "Novo usuário criado";
                FBLog.PrintAuth("Novo usuário criado");
                OnCloseRegister();
                break;
            case AuthState.logoutANDsignin:
                //registerStatusMsg.text = "Novo usuário criado";
                FBLog.PrintAuth("Novo usuário criado");
                OnCloseRegister();
                break;
        }
        StartCoroutine(InitUpdate());
    }
    private IEnumerator InitUpdate()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("yeld return coroutine init update.");
        UpdateLoginPanel();
    }

    public void OnPlay()
    {
        var _user = FirebaseManager.get.AuthManager.GetUserData();
        if (_user != null)
        {
            if (!string.IsNullOrEmpty(_user.UserId))
            {
                SceneManager.LoadScene(1);
                return;
            }
        }
        OnOpenLogin();
    }

    /*private void OpenMain()
    {
        mainPanel.SetActive(true);
    }
    private void CloseMain()
    {
        mainPanel.SetActive(false);
    }*/

    #region LOGIN
    public void OnOpenLogin()
    {
        //CloseMain();
        //loginPanel.SetActive(true);
        mainUIControl.OpenPannel(loginPanel);
        UpdateLoginPanel();
    }

    private void UpdateLoginPanel()
    {
        AuthManager auth = FirebaseManager.get.AuthManager;
        var _user = auth.GetUserData();
        loginUsernameInput.text = (auth.Connected ? (_user.DisplayName ?? "") : "");
        loginPasswordInput.text = "";
        //loginSignOutButton.interactable = auth.Connected;
        //loginSignInButton.interactable = !auth.Connected;
        if (auth.Connected)
            loginEnterButton.GetComponentInChildren<Text>().text = "Sair";
        else
            loginEnterButton.GetComponentInChildren<Text>().text = "Entrar";
        usernameText.text = (auth.Connected ? _user.DisplayName : "Desconectado");
    }

    public void OnCloseLogin()
    {
        //OpenMain();
        //loginPanel.SetActive(false);
        mainUIControl.ClosePannel(loginPanel);
    }

    public void OnSignIn()
    {
        AuthManager auth = FirebaseManager.get.AuthManager;
        if (auth.Connected)
            auth.SignOut();
        else
        {
            string _name = loginUsernameInput.text;
            string _pass = loginPasswordInput.text;

            bool invalid = false;
            if (_name.Length < 6 || _name.Length > 12)
            {
                FBLog.PrintAuth("O Usuário deve ter entre 6 e 12 caracteres");
                invalid = true;
            }
            if (_pass.Length < 6 || _pass.Length > 12)
            {
                FBLog.PrintAuth("A senha deve ter entre 6 e 12 caracteres");
                invalid = true;
            }
            if (invalid)
                return;

            auth.Login(_name, _pass);
        }
    }

    public void OnSignOut()
    {
        FirebaseManager.get.AuthManager.SignOut();
    }

    private void SetTextsLogin(string _usename, string _password)
    {
        loginUsernameInput.text = _usename;
        loginPasswordInput.text = _password;
    }
    #endregion
    #region REGISTER
    public void OnOpenRegister()
    {
        //CloseMain();
        //registerPanel.SetActive(true);
        mainUIControl.OpenPannel(registerPanel);
        
        // clear
        SetTextsRegister("", "", "");
    }

    public void OnCloseRegister()
    {
        //OpenMain();
        //registerPanel.SetActive(false);
        mainUIControl.ClosePannel(registerPanel);
    }

    public void OnRegister()
    {
        string _name = registerUsernameInput.text;
        string _tag = registerTagInput.text;
        string _pass = registerPasswordInput.text;

        bool invalid = false;
        if (_name.Length < 6 || _name.Length > 12)
        {
            FBLog.PrintAuth("O Usuário deve ter entre 6 e 12 caracteres");
            invalid = true;
        }
        if (_tag.Length != 3)
        {
            FBLog.PrintAuth("A TAG deve ter 3 caracteres");
            invalid = true;
        }
        if (_pass.Length < 6 || _pass.Length > 12)
        {
            FBLog.PrintAuth("A senha deve ter entre 6 e 12 caracteres");
            invalid = true;
        }
        if (invalid)
            return;

        FirebaseManager.get.AuthManager.Register(
                    registerUsernameInput.text, registerTagInput.text, registerPasswordInput.text);
    }

    private void SetTextsRegister(string _usename, string _tag, string _password)
    {
        registerUsernameInput.text = _usename;
        registerTagInput.text = _tag;
        registerPasswordInput.text = _password;
    }
    #endregion
}
