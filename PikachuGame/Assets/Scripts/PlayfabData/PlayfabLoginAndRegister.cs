using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PlayfabLoginAndRegister : MonoBehaviour
{
    [SerializeField] private TMP_InputField emailInput;
    [SerializeField] private TMP_InputField passwordInput;

    [Header("Config")]
    [SerializeField] private int startCoin = 0;
    [SerializeField] private int startRocket = 0;
    [SerializeField] private int startSwap = 0;
    [SerializeField] private int startFind = 0;
    [SerializeField] private int startAddTime = 0;

    [SerializeField] private DataLocal dataLocal;

    public void Login()
    {
        SoundManager.Instance.PlayOnClickButton();
        var request = new LoginWithEmailAddressRequest
        {
            Email = emailInput.text,
            Password = passwordInput.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true,
                GetUserReadOnlyData = true
            }
        };

        PlayFabClientAPI.LoginWithEmailAddress(
            request,
            OnLoginSuccess,
            OnError
        );
    }

    public void Register()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        string displayName = email.Split('@')[0];

        var request = new RegisterPlayFabUserRequest
        {
            Email = email,
            Password = password,
            DisplayName = displayName,
            RequireBothUsernameAndEmail = false
        };
        SoundManager.Instance.PlayOnClickButton();
        PlayFabClientAPI.RegisterPlayFabUser(
            request,
            result =>
            {
                Debug.Log("Register success!");
                Login(); 
            },
            OnError
        );
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("Login success!");
        Debug.Log("PlayFabId: " + result.PlayFabId);

        string displayName = result.InfoResultPayload?.PlayerProfile?.DisplayName;
        Debug.Log("DisplayName: " + displayName);
        if (result.InfoResultPayload.UserReadOnlyData == null ||
            !result.InfoResultPayload.UserReadOnlyData.ContainsKey("Coin")
            || !result.InfoResultPayload.UserReadOnlyData.ContainsKey("Rocket")
            || !result.InfoResultPayload.UserReadOnlyData.ContainsKey("Swap")
            || !result.InfoResultPayload.UserReadOnlyData.ContainsKey("Find")
            || !result.InfoResultPayload.UserReadOnlyData.ContainsKey("AddTime"))
        {
            CreateStartCoin();
        }
        else
        {
            int coin = int.Parse(result.InfoResultPayload.UserReadOnlyData["Coin"].Value);
            Debug.Log("Coin: " + coin);
        }
        dataLocal.SetLevelData();
        dataLocal.displayName = displayName;
    }

    private void CreateStartCoin()
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Coin", startCoin.ToString() },
                { "Rocket", startRocket.ToString() },
                { "Swap", startSwap.ToString() },
                { "Find", startFind.ToString() },
                { "AddTime", startAddTime.ToString() }
            }
        };

        PlayFabClientAPI.UpdateUserData(
            request,
            r => Debug.Log("Create start coin success"),
            OnError
        );
    }


    private void OnError(PlayFabError error)
    {
        Debug.LogError("PlayFab Error:");
        Debug.LogError(error.GenerateErrorReport());
    }
}
