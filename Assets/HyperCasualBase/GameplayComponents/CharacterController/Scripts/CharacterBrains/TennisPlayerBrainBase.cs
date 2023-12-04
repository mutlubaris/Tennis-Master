using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public abstract class TennisPlayerBrainBase : CharacterBrainBase
{
    protected CharacterType characterType;
    protected Vector3 targetPosition;
    protected bool isOnRightSide;

    private bool isBallHit;
    private bool isPaused;
    private float stoppingDistance = 1f;

    private Vector3 targetPositionOne;
    private Vector3 targetPositionTwo;
    private Vector3 targetPositionThree;
    
    protected virtual void OnEnable()
    {
        if (Managers.Instance == null) return;
        EventManager.OnTennisBallHit.AddListener(CalculateTargetPosition);
        EventManager.OnPointReset.AddListener(ResetBase);
        EventManager.OnPointFinished.AddListener(PausePlayer);
    }

    protected virtual void OnDisable()
    {
        if (Managers.Instance == null) return;
        EventManager.OnTennisBallHit.RemoveListener(CalculateTargetPosition);
        EventManager.OnPointReset.RemoveListener(ResetBase);
        EventManager.OnPointFinished.RemoveListener(PausePlayer);
    }

    protected virtual void CalculateTargetPosition(CharacterType _characterType, Vector3 initialPosition, Vector3 initialSpeed)
    {
        if (_characterType != characterType)
        {
            var timeUntilFirstPeak = initialSpeed.y / -Physics.gravity.y;

            targetPositionOne = new Vector3(initialPosition.x + timeUntilFirstPeak * initialSpeed.x, initialPosition.y +
                initialSpeed.y * timeUntilFirstPeak + (Physics.gravity.y * Mathf.Pow(timeUntilFirstPeak, 2) / 2), initialPosition.z + timeUntilFirstPeak * initialSpeed.z);

            var timeUntilFirstBounce = Mathf.Sqrt(2 * (0.5f - targetPositionOne.y) / Physics.gravity.y);

            targetPositionTwo = new Vector3(targetPositionOne.x + initialSpeed.x * timeUntilFirstBounce,
                targetPositionOne.y + (Physics.gravity.y * Mathf.Pow(timeUntilFirstBounce, 2) / 2), targetPositionOne.z + initialSpeed.z * timeUntilFirstBounce);

            var timeUntilSecondPeak = timeUntilFirstBounce * Mathf.Pow(0.6f, 2);

            targetPositionThree = new Vector3(targetPositionTwo.x + timeUntilSecondPeak * initialSpeed.x, targetPositionTwo.y, targetPositionTwo.z + initialSpeed.z * timeUntilSecondPeak);

            var totalTime = timeUntilFirstPeak + timeUntilFirstBounce + timeUntilSecondPeak;

            targetPosition = new Vector3(initialPosition.x + totalTime * initialSpeed.x, transform.position.y, initialPosition.z + totalTime * initialSpeed.z);

            var targetPositionX = transform.position.x > targetPosition.x ?
                targetPosition.x + stoppingDistance : targetPosition.x - stoppingDistance;

            isOnRightSide = targetPositionX < targetPosition.x ? true : false;

            targetPosition = new Vector3(targetPositionX, targetPosition.y, targetPosition.z);

            isBallHit = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isBallHit && !isPaused)
        {
            var tennisBall = other.GetComponent<TennisBall>();
            if (tennisBall != null)
            {
                StartHitSequence(tennisBall);
                isBallHit = true;
            }
        }
    }

    protected abstract void StartHitSequence(TennisBall tennisBall);

    public override void Logic()
    {
        if (targetPosition == Vector3.zero) return;

        CharacterController.Move(targetPosition);
    }

    private void PausePlayer(CharacterType type)
    {
        targetPosition = transform.position;
        DOTween.KillAll();
        isPaused = true;
    }

    private void ResetBase()
    {
        isPaused = false;
        isBallHit = false;
    }
}
