using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerBrain : TennisPlayerBrainBase
{
    private float hitSpeedX = 6f;
    private float hitSpeedY = 7f;
    private float hitSpeedZ = 6f;

    private bool canHitTheBall;
    private bool isPrepared;
    private TennisBall tennisBall;
    private GameObject mainCamera;
    private Vector3 startingPosition;
    private Vector3 cameraFirstRotVector;
    private Quaternion cameraFirstRotation;
    private Tweener rotateCamera;
    private Animator animator;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (Managers.Instance == null) return;

        hitSpeedX = LevelManager.Instance.CurrentLevel.TennisData.PlayerHitX;
        hitSpeedY = LevelManager.Instance.CurrentLevel.TennisData.PlayerHitY;
        hitSpeedZ = LevelManager.Instance.CurrentLevel.TennisData.PlayerHitZ;

        characterType = CharacterType.Player;
        mainCamera = Character.MainCamera;
        mainCamera.SetActive(true);
        Character.Arms.SetActive(true);
        Character.Body.SetActive(false);
        animator = Character.Arms.GetComponent<Animator>();
        cameraFirstRotVector = new Vector3(mainCamera.transform.rotation.x * 90, 
            mainCamera.transform.rotation.y * 90, mainCamera.transform.rotation.z * 90);
        cameraFirstRotation = mainCamera.transform.rotation;
        startingPosition = transform.position;
        var tennisController = GetComponent<ICharacterController>();
        if (tennisController is TennisCharacterController)
        {
            (tennisController as TennisCharacterController).moveSpeed = LevelManager.Instance.CurrentLevel.TennisData.PlayerSpeed;
        }

        InputManager.Instance.OnSwipeDetected.AddListener(HitTheBall);
        EventManager.OnPointReset.AddListener(ResetPlayer);
    }

    protected override void OnDisable()
    {
        base.OnDisable();

        if (Managers.Instance == null) return;

        ResetPlayer();

        InputManager.Instance.OnSwipeDetected.RemoveListener(HitTheBall);
        EventManager.OnPointReset.RemoveListener(ResetPlayer);
    }

    protected override void CalculateTargetPosition(CharacterType _characterType, Vector3 initialPosition, Vector3 initialSpeed)
    {
        base.CalculateTargetPosition(_characterType, initialPosition, initialSpeed);

        if (_characterType != characterType && !isPrepared)
        {
            if (isOnRightSide) animator.SetTrigger("PrepareForehand");
            else animator.SetTrigger("PrepareBackhand");

            isPrepared = true;
        }
    }

    protected override void StartHitSequence(TennisBall _tennisBall)
    {
        if (isOnRightSide) rotateCamera = mainCamera.transform.DORotate(cameraFirstRotVector + Vector3.up * 50, 0.3f, RotateMode.Fast);
        if (!isOnRightSide) rotateCamera = mainCamera.transform.DORotate(cameraFirstRotVector + Vector3.up * -50, 0.3f, RotateMode.Fast);

        Time.timeScale = 0.05f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        canHitTheBall = true;
        tennisBall = _tennisBall;
    }

    private void OnTriggerExit(Collider other)
    {
        if (canHitTheBall)
        {
            var tennisBall = other.GetComponent<TennisBall>();
            if (tennisBall != null)
            {
                Time.timeScale = 1f;
                Time.fixedDeltaTime = 0.02F * Time.timeScale;
                canHitTheBall = false;
            }
        }
    }

    private void HitTheBall(Swipe swipe, Vector2 velocity)
    {
        if (canHitTheBall && (swipe == Swipe.UpLeft || swipe == Swipe.Up || swipe == Swipe.UpRight) || swipe == Swipe.Left || swipe == Swipe.Right)
        {
            if (velocity.y <= 0) return;
            
            Time.timeScale = 1f;
            Time.fixedDeltaTime = 0.02F * Time.timeScale;
            rotateCamera.SmoothRewind();
            tennisBall.SetBallVelocity(characterType, Vector3.right * Mathf.Clamp(velocity.x, -50, 50) * hitSpeedX / 20
                + Vector3.up * hitSpeedY + Vector3.forward * (hitSpeedZ + Mathf.Clamp(velocity.y/ 5, 0, 20)));
            HapticManager.Haptic(HapticTypes.SoftImpact);
            canHitTheBall = false;

            if (isOnRightSide) animator.SetTrigger("HitForehand");
            else animator.SetTrigger("HitBackhand");

            targetPosition = startingPosition;
            isPrepared = false;
        }
    }

    private void ResetPlayer()
    {
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02F * Time.timeScale;
        targetPosition = Vector3.zero;
        transform.position = startingPosition;
        canHitTheBall = false;
        mainCamera.transform.rotation = cameraFirstRotation;
        animator.SetTrigger("ResetPlayer");
        isPrepared = false;
    }
}
