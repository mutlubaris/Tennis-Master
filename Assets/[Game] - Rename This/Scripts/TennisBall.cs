using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TennisBallHitEvent : UnityEvent<CharacterType, Vector3, Vector3> { }

public class TennisBall : MonoBehaviour
{
    [SerializeField] private Rigidbody rigid;
    [SerializeField] private ParticleSystem hitEffectParticle;
    [SerializeField] private ParticleSystem bounceEffectParticle;

    private CharacterType lastPlayerThatHit = CharacterType.Player;
    private CharacterType playerToReceive = CharacterType.AI;
    private bool hasBounced;
    private bool pointEnded;
    private Vector3 startingPosition;
    private TrailRenderer trailRenderer;

    private void OnEnable()
    {
        if (Managers.Instance == null) return;
        EventManager.OnPointReset.AddListener(ResetBall);
        EventManager.OnPointStarted.AddListener(InitiateMovement);

        trailRenderer = GetComponent<TrailRenderer>();
        startingPosition = transform.position;
        rigid.useGravity = false;
        pointEnded = false;
    }

    private void OnDisable()
    {
        if (Managers.Instance == null) return;
        EventManager.OnPointReset.RemoveListener(ResetBall);
        EventManager.OnPointStarted.RemoveListener(InitiateMovement);
    }

    private void InitiateMovement()
    {
        rigid.velocity = new Vector3(0f, 2f, 5f);
        rigid.useGravity = true;
    }

    public void SetBallVelocity(CharacterType characterType, Vector3 ballSpeed)
    {
        rigid.velocity = ballSpeed;
        EventManager.OnTennisBallHit.Invoke(characterType, transform.position, ballSpeed);
        playerToReceive = characterType == CharacterType.Player? CharacterType.AI : CharacterType.Player;
        lastPlayerThatHit = characterType;
        hasBounced = false;

        if (characterType == CharacterType.Player) hitEffectParticle.Play();
    }

    private void OnCollisionEnter(Collision collision)
    {
        var surface = collision.gameObject.GetComponent<Surface>();

        if (!pointEnded && surface != null)
        {
            if (hasBounced)
            {
                pointEnded = true;
                trailRenderer.enabled = false;
                EventManager.OnPointFinished.Invoke(lastPlayerThatHit);
                return;
            }

            

            if ((lastPlayerThatHit == CharacterType.Player && surface.SurfaceType == SurfaceType.OpponentCourt) ||
                lastPlayerThatHit == CharacterType.AI && surface.SurfaceType == SurfaceType.PlayerCourt)
            {
                if (surface.SurfaceType != SurfaceType.TennisNet)
                {
                    var main = bounceEffectParticle.main;
                    main.startColor = Color.green;
                    bounceEffectParticle.Play();
                }

                hasBounced = true;
            }

            else
            {
                if (surface.SurfaceType != SurfaceType.TennisNet)
                {
                    var main = bounceEffectParticle.main;
                    main.startColor = Color.red;
                    bounceEffectParticle.Play();
                }

                pointEnded = true;
                trailRenderer.enabled = false;
                EventManager.OnPointFinished.Invoke(playerToReceive);
            }
        }
    }

    private void ResetBall()
    {
        hasBounced = false;
        pointEnded = false;
        transform.position = startingPosition;
        rigid.velocity = Vector3.zero;
        rigid.useGravity = false;
        trailRenderer.enabled = true;
    }
}
