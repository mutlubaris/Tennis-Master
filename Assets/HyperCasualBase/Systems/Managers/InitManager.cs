﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class InitManager : Singleton<InitManager>
{

    private void Start()
    {
        Invoke("LoadMenuScene", 0.5f);
    }
    
    public void LoadMenuScene()
    {
        StartCoroutine(LoadMenuSceneCo());
    }

    private IEnumerator LoadMenuSceneCo()
    {
        yield return SceneManager.LoadSceneAsync("UI", LoadSceneMode.Additive);
        yield return SceneManager.LoadSceneAsync("Effects", LoadSceneMode.Additive);
        LevelManager.Instance.LoadLastLevel();
        Destroy(gameObject);
    }
}
