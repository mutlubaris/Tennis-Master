using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AITennisBrain : TennisPlayerBrainBase
{
    private float hitSpeedX = 2f;
    private float hitSpeedY = 7f;
    private float hitSpeedZ = 11f;

    private float hitSpeedXMin;
    private float hitSpeedXMax;

    private bool IsSmartAI = true;

    private Vector3 startingPosition;
    private Quaternion racketStartingRotation;
    private Animator animator;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Managers.Instance == null) return;

        hitSpeedX = LevelManager.Instance.CurrentLevel.TennisData.OpponentHitX;
        hitSpeedY = LevelManager.Instance.CurrentLevel.TennisData.OpponentHitY;
        hitSpeedZ = LevelManager.Instance.CurrentLevel.TennisData.OpponentHitZ;
        IsSmartAI = LevelManager.Instance.CurrentLevel.TennisData.IsSmartAI;

        characterType = CharacterType.AI;
        startingPosition = transform.position;
        Character.Trigger.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + 2.5f);
        animator = Character.AIAnimator;

        var tennisController = GetComponent<ICharacterController>();
        if (tennisController is TennisCharacterController)
        {
            (tennisController as TennisCharacterController).moveSpeed = LevelManager.Instance.CurrentLevel.TennisData.OpponentSpeed;
        }

        EventManager.OnPointReset.AddListener(ResetAI);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (Managers.Instance == null) return;

        EventManager.OnPointReset.RemoveListener(ResetAI);
    }

    protected override void StartHitSequence(TennisBall tennisBall)
    {
        hitSpeedXMin = -transform.position.x - hitSpeedX;
        hitSpeedXMax = -transform.position.x + hitSpeedX;
        
        float dist = Mathf.Min (startingPosition.z - transform.position.z, 6);
        var ballVelocity = new Vector3(Random.Range(hitSpeedXMin, hitSpeedXMax), hitSpeedY, -(hitSpeedZ * (((6-dist) / 40) + 0.85f) * Random.Range(0.95f, 1.1f)));
        tennisBall.SetBallVelocity(characterType, ballVelocity);

        if (!isOnRightSide) animator.SetTrigger("HitForehand");
        else animator.SetTrigger("HitBackhand");

        if (IsSmartAI) targetPosition = startingPosition;
    }

    private void Update()
    {
        animator.SetFloat("MoveSpeed", CharacterController.CurrentSpeed());
    }

    private void ResetAI()
    {
        targetPosition = Vector3.zero;
        transform.position = startingPosition;
    }
}
