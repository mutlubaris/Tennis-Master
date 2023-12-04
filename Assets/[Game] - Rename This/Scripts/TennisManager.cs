using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisManager : Singleton<TennisManager>
{
    private string[] opponentNames = { "Roger", "Rafael", "Novak", "Andy", "Dominic", "Alex", "Naomi",
        "Gael", "Fabio", "Milos", "Serena", "Venus", "Petra", "Simona", "Victoria" };

    private int[] scoreValues = { 0, 15, 30, 40 };

    private int playerScore;
    private int opponentScore;
    private int playerGamesWon;
    private int opponentGamesWon;

    private int gamesToWin;

    public string OpponentName;
    
    private void OnEnable()
    { 
        if (Managers.Instance == null) return;
        LevelManager.Instance.OnLevelStart.AddListener(InvokePointReset);
        EventManager.OnPointFinished.AddListener(UpdateScoreInManager);
        SceneController.Instance.OnSceneStartedLoading.AddListener(SetOpponentName);
        gamesToWin = LevelManager.Instance.CurrentLevel.TennisData.GamesToWin;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        LevelManager.Instance.OnLevelStart.RemoveListener(InvokePointReset);
        EventManager.OnPointFinished.RemoveListener(UpdateScoreInManager);
        SceneController.Instance.OnSceneStartedLoading.RemoveListener(SetOpponentName);
    }

    private void SetOpponentName()
    {
        OpponentName = opponentNames[Random.Range(0, opponentNames.Length - 1)];
    }

    private void InvokePointReset()
    {
        EventManager.OnPointReset.Invoke();

        playerScore = 0;
        opponentScore = 0;
        playerGamesWon = 0;
        opponentGamesWon = 0;
    }

    public void UpdateScoreInManager(CharacterType pointWinner)
    {
        if (pointWinner == CharacterType.Player)
        {
            playerScore++;
            if (playerScore >= 4)
            {
                playerGamesWon++;

                playerScore = 0;
                opponentScore = 0;
            }
        }

        if (pointWinner == CharacterType.AI)
        {
            opponentScore++;
            if (opponentScore >= 4)
            {
                opponentGamesWon++;

                playerScore = 0;
                opponentScore = 0;
            }
        }

        var scorePanel = UIIDHolder.Panels[UIIDHolder.ScorePanel] as ScorePanel;
        scorePanel.UpdateScoreInPanel(scoreValues[playerScore], scoreValues[opponentScore], playerGamesWon, opponentGamesWon, pointWinner);
        StartCoroutine(ResetLevelCo(pointWinner));
    }

    private IEnumerator ResetLevelCo(CharacterType pointWinner)
    {
        yield return new WaitForSeconds(2.5f);

        var matchEndPanel = UIIDHolder.Panels[UIIDHolder.MatchEndPanel] as MatchEndPanel;

        if (playerGamesWon >= gamesToWin || opponentGamesWon >= gamesToWin)
        {
            matchEndPanel.ActivatePanel(pointWinner);
        }
        else
        {
            EventManager.OnPointReset.Invoke();
        } 
    }
}
