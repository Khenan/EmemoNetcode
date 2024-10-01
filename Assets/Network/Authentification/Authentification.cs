using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave.Models.Data.Player;
using Unity.Services.Core;
using Unity.VisualScripting;
using UnityEngine;

public class Authentification : MonoBehaviour
{
    [SerializeField] private GameObject root;

    // Sign In and Sign Up fields
    private string usernameSignIn = "";
    private string passwordSignIn = "";
    private string usernameSignUp = "";
    private string passwordSignUp = "";
    private string usernameAdmin = "admin";
    private string passwordAdmin = "Admin123!";
    [SerializeField] private GameObject authentificationPanel;

    // PlayerName fields
    private string playerName = "";
    [SerializeField] private GameObject playerNamePanel;

    private void Awake()
    {
        ShowAuthentificationPanel(true);
        ShowPlayerNamePanel(false);
    }

    private async Task SignUpWithUsernamePasswordAsync(string _username, string _password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(_username, _password);
            Debug.Log("SignUp is successful.");
            // if (CheckIfPlayerNameExist().Result)
            // {
            //     Debug.Log(AuthenticationService.Instance.PlayerName);
            //     CloseAuthentification();
            // }
            // else Debug.Log("PlayerName is null");
        }
        catch (AuthenticationException ex)
        {
            ShowAuthentificationPanel(true);
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            ShowAuthentificationPanel(true);
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    private async Task SignInWithUsernamePasswordAsync(string _username, string _password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(_username, _password);
            Debug.Log("SignIn is successful.");
            CloudManager.CloudResultString _resultString = await CheckIfPlayerNameExist();
            if (_resultString.success)
            {
                Debug.Log(_resultString.value);
                CloseAuthentification();
            }
            else
            {
                Debug.Log("PlayerName is null");
                ShowPlayerNamePanel(true);
            }
        }
        catch (AuthenticationException ex)
        {
            ShowAuthentificationPanel(true);
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            ShowAuthentificationPanel(true);
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }
    private async Task SignPlayerNameAsync(string _username)
    {
        try
        {
            await CloudManager.Instance.SaveData("PlayerName", _username);
            Debug.Log("Sign PlayerName is successful: " + AuthenticationService.Instance.PlayerName);
            CloseAuthentification();
        }
        catch (AuthenticationException ex)
        {
            ShowPlayerNamePanel(true);
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            ShowPlayerNamePanel(true);
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
    }

    public void SetUsernameSignIn(string _value)
    {
        usernameSignIn = _value;
    }
    public void SetPasswordSignIn(string _value)
    {
        passwordSignIn = _value;
    }
    public void SetUsernameSignUp(string _value)
    {
        usernameSignUp = _value;
    }
    public void SetPasswordSignUp(string _value)
    {
        passwordSignUp = _value;
    }
    public void SetPlayerName(string _value)
    {
        playerName = _value;
    }

    public async void SubmitSignIn()
    {
        ShowAuthentificationPanel(false);
        await SignInWithUsernamePasswordAsync(usernameSignIn, passwordSignIn);
    }
    public async void SubmitSignUp()
    {
        ShowAuthentificationPanel(false);
        await SignUpWithUsernamePasswordAsync(usernameSignUp, passwordSignUp);
    }
    public async void SubmitPlayerName()
    {
        ShowPlayerNamePanel(false);
        await SignPlayerNameAsync(playerName);
    }
    public async void AdminConnexion()
    {
        ShowAuthentificationPanel(false);
        await SignInWithUsernamePasswordAsync(usernameAdmin, passwordAdmin);
    }

    private void ShowAuthentificationPanel(bool _value)
    {
        authentificationPanel.SetActive(_value);
    }
    private void ShowPlayerNamePanel(bool _value)
    {
        playerNamePanel.SetActive(_value);
    }
    private async Task<CloudManager.CloudResultString> CheckIfPlayerNameExist()
    {
        var _result = await CloudManager.Instance.LoadDataString("PlayerName");
        return _result;
    }

    private void CloseAuthentification()
    {
        root.SetActive(false);
    }
}
