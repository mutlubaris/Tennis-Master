using AdvanceUI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorePanel : AdvancePanel
{
    [SerializeField] private TextMeshProUGUI playerScoreText;
    [SerializeField] private TextMeshProUGUI opponentScoreText;
    [SerializeField] private TextMeshProUGUI playerGamesWonText;
    [SerializeField] private TextMeshProUGUI opponentGamesWonText;
    [SerializeField] private TextMeshProUGUI opponentName;

    private Color textColor;

    private void OnEnable()
    {
        if (Managers.Instance == null) return;

        textColor = playerScoreText.color;

        ResetPanel();

        EventManager.OnPointReset.AddListener(ResetTextColors);
        SceneController.Instance.OnSceneLoaded.AddListener(ResetPanel);
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        EventManager.OnPointReset.RemoveListener(ResetTextColors);
        SceneController.Instance.OnSceneLoaded.RemoveListener(ResetPanel);
    }

    public void UpdateScoreInPanel(int playerScore, int opponentScore, int playerGamesWon, int opponentGamesWon, CharacterType pointWinner)
    {
        playerScoreText.SetText(playerScore.ToString());
        opponentScoreText.SetText(opponentScore.ToString());
        playerGamesWonText.SetText(playerGamesWon.ToString());
        opponentGamesWonText.SetText(opponentGamesWon.ToString());

        if (pointWinner == CharacterType.Player)
        {
            if (playerScore == 0) playerGamesWonText.color = Color.green;
            else playerScoreText.color = Color.green;
        }
        if (pointWinner == CharacterType.AI)
        {
            if (opponentScore == 0) opponentGamesWonText.color = Color.red;
            else opponentScoreText.color = Color.red;
        }
    }

    private void ResetTextColors()
    {
        playerScoreText.color = textColor;
        opponentScoreText.color = textColor;
        playerGamesWonText.color = textColor;
        opponentGamesWonText.color = textColor;
    }

    private void ResetPanel()
    {
        opponentName.text = TennisManager.Instance.OpponentName;
        
        ResetTextColors();

        playerScoreText.SetText("0");
        opponentScoreText.SetText("0");
        playerGamesWonText.SetText("0");
        opponentGamesWonText.SetText("0");
    }
}
