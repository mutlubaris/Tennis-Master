using AdvanceUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MatchEndPanel : AdvancePanel
{
    [SerializeField] private GameObject winTextObject;
    [SerializeField] private GameObject nextLevelButton;
    [SerializeField] private GameObject replayButton;

    private TextMeshProUGUI winText;
    
    private void OnEnable()
    { 
        if (Managers.Instance == null) return;
        SceneController.Instance.OnSceneLoaded.AddListener(ResetPanel);
        winText = winTextObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        SceneController.Instance.OnSceneLoaded.RemoveListener(ResetPanel);
    }

    public void ActivatePanel(CharacterType matchWinner)
    {
        var matchWinnerName = matchWinner == CharacterType.Player ? "Player" : TennisManager.Instance.OpponentName;
        
        winText.text = $"{matchWinnerName} won!";
        
        if (matchWinner == CharacterType.Player)
        {
            winTextObject.SetActive(true);
            nextLevelButton.SetActive(true);
        }

        else if (matchWinner == CharacterType.AI)
        {
            winTextObject.SetActive(true);
            replayButton.SetActive(true);
        }
    }

    private void ResetPanel()
    {
        winTextObject.SetActive(false);
        nextLevelButton.SetActive(false);
        replayButton.SetActive(false);
    }

    public void NextLevel()
    {
        LevelManager.Instance.LoadNextLevel();
    }

    public void ReplayLevel()
    {
        LevelManager.Instance.LoadLastLevel();
    }
}
