using AdvanceUI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownPanel : AdvancePanel
{
    [SerializeField] private Image image;
    [SerializeField] private Sprite three;
    [SerializeField] private Sprite two;
    [SerializeField] private Sprite one;
    
    private void OnEnable()
    { 
        if (Managers.Instance == null) return;
        EventManager.OnPointReset.AddListener(InitiateCountdown);
        HidePanel();
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        EventManager.OnPointReset.RemoveListener(InitiateCountdown);
    }

    private void InitiateCountdown()
    {
        StartCoroutine(InitiateCountdownCo());
    }

    private IEnumerator InitiateCountdownCo()
    {
        ShowPanel();
        image.sprite = three;
        yield return new WaitForSeconds(1f);
        image.sprite = two;
        yield return new WaitForSeconds(1f);
        image.sprite = one;
        yield return new WaitForSeconds(1f);
        EventManager.OnPointStarted.Invoke();
        HidePanel();
    }
}
