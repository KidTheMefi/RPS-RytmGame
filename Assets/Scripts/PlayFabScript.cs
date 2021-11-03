using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabScript : MonoBehaviour
{
    private string myID;
    //[SerializeField] private GameObject loginPanel;


    public event Action NoCompletedLevelData = delegate { };
    public event Action<string> CompletedLevelDataLoaded = delegate { };

    // Start is called before the first frame update
    void Start()
    {

#if UNITY_ANDROID
        var requestAndroid = new LoginWithAndroidDeviceIDRequest { AndroidDeviceId = ReturnMobileID(), CreateAccount = true };
        PlayFabClientAPI.LoginWithAndroidDeviceID(requestAndroid, OnLoginAndroidSuccess, OnLoginAndroidFailure);
#endif

    }


    #region LoginID
    public static string ReturnMobileID()
    {
        string deviceID = SystemInfo.deviceUniqueIdentifier;
        return deviceID;
    }

    private void OnLoginAndroidFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnLoginAndroidSuccess(LoginResult result)
    {
        myID = result.PlayFabId;
        Debug.Log("Congratulations, you made successful API call!");
        //loginPanel.SetActive(false);
        GetCompletedLevelData();
    }
    #endregion LoginID

    #region CompletedLevelData

    public void GetCompletedLevelData()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest()
        {
            PlayFabId = myID,
            Keys = null
        }, CompletedLevelDataSuccess, CompletedLevelDataFailed);

    }


    private void CompletedLevelDataSuccess(GetUserDataResult result)
    {
        if (result.Data == null || !result.Data.ContainsKey("CompletedLevelData"))
        {
            Debug.LogError("CompletedLevelData not set");
            NoCompletedLevelData();
        }

        else
        {
            Debug.Log("Loaded success");
            CompletedLevelDataLoaded(result.Data["CompletedLevelData"].Value);
        }
    }


    private void CompletedLevelDataFailed(PlayFabError error)
    {
        Debug.LogWarning(error.GenerateErrorReport());
    }


    public void SetComlpetedLevelData(string completedLevelDataJSON)
    {
        PlayFabClientAPI.UpdateUserData(new UpdateUserDataRequest()
        {
            Data = new Dictionary<string, string>()
            {
                {"CompletedLevelData", completedLevelDataJSON }
            }
        }, SetDataSuccess, CompletedLevelDataFailed);

    }

    private void SetDataSuccess(UpdateUserDataResult result)
    {
        //Debug.Log(result.DataVersion);
    }





    #endregion CompletedLevelData
}
