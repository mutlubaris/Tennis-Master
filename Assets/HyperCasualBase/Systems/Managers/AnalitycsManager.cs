using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Firebase.Analytics;
using Facebook.Unity;
using ElephantSDK;

public class AnalitycsManager : Singleton<AnalitycsManager>
{
    private void Start()
    {
        FB.Init();
        if (!PlayerPrefs.HasKey(PlayerPrefKeys.UserID))
        {
            PlayerPrefs.SetString(PlayerPrefKeys.UserID, System.Guid.NewGuid().ToString());
            PlayerPrefs.SetInt(PlayerPrefKeys.LoginCount, 1);
            FirebaseAnalytics.LogEvent("Login_Event", "FirstLogin", "NewUserLogin");
        }
        else
        {
            int loginCount = PlayerPrefs.GetInt(PlayerPrefKeys.LoginCount, 1);
            loginCount++;
            PlayerPrefs.SetInt(PlayerPrefKeys.LoginCount, loginCount);
            FirebaseAnalytics.LogEvent("Login_Event", "ExistingUserLogedin", "LoginCount_" + loginCount);
        }
    }


    private void OnEnable()
    {
        GameManager.Instance.OnStageFail.AddListener(SendFailEvent);
        GameManager.Instance.OnStageSuccess.AddListener(SendLevelEvent);
        LevelManager.Instance.OnLevelStart.AddListener(SendScreenEvent);

    }

    private void OnDisable()
    {
        GameManager.Instance.OnStageFail.RemoveListener(SendFailEvent);
        GameManager.Instance.OnStageSuccess.RemoveListener(SendLevelEvent);
        LevelManager.Instance.OnLevelStart.RemoveListener(SendScreenEvent);
    }

    private void FacebookInitCallback()
    {
        if (FB.IsInitialized)
        {
            FB.ActivateApp();
        }
    }

    public void LogEvent(string name, string paramName, string paramValue)
    {
        Elephant.Event(name, LevelManager.Instance.LevelIndex, Params.New().Set(paramName, paramValue));
    }

    private void SendScreenEvent()
    {
        Elephant.LevelStarted(LevelManager.Instance.LevelIndex);
    }

    private void SendFailEvent()
    {
        Elephant.LevelFailed(LevelManager.Instance.LevelIndex);
    }

    private void SendLevelEvent()
    {
        Elephant.LevelCompleted(LevelManager.Instance.LevelIndex);
    }

    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (FB.IsInitialized)
                FB.ActivateApp();
        }
    }
}
