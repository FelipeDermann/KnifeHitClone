using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum KnifeState
{
    NONE,
    FLYING,
    ATTACHED,
    INACTIVE
}

public class Knife : MonoBehaviour
{
    public static event Action AttachedToTarget;
    public static event Action GameOver;

    [SerializeField]
    LayerMask mask;
    Animator anim;
    Rigidbody2D rb;
    Collider2D col;
    KnifeState state;
    Transform targetOriginPos;

    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        targetOriginPos = GameManager.Instance.targetOriginPoint;
    }

    IEnumerator DeactivateAfterTime(float _time)
    {
        yield return new WaitForSeconds(_time);
        Deactivate();
    }
    public void Deactivate()
    {
        rb.isKinematic = true;
        col.enabled = false;
        state = KnifeState.NONE;
        KnifePool.Instance.ReturnKnife(gameObject);
        gameObject.SetActive(false);
    }

    public void Activate()
    {
        transform.eulerAngles = Vector3.zero;
        gameObject.SetActive(true);
    }

    public void Throw()
    {
        if (state == KnifeState.ATTACHED || state == KnifeState.FLYING) return;
        state = KnifeState.FLYING;

        StartCoroutine(FlyToTarget());
    }

    IEnumerator FlyToTarget()
    {
        Vector3 myPos = transform.position;
        Vector3 targetPos = GameManager.Instance.knifeTargetPoint.transform.position;
        Vector3 originPos = GameManager.Instance.raycastOriginPoint.position;

        float timer = 0;
        float _timeToReachTarget = GameManager.Instance.knifeTimeToReachTarget;

        bool hitSomething = false;

        while (timer < _timeToReachTarget)
        {
            timer += Time.deltaTime;
            transform.position = Vector3.Lerp(myPos, targetPos, timer / _timeToReachTarget);

            if (timer > _timeToReachTarget * 0.8f)
            {
                if (Physics2D.Raycast(originPos, Vector3.up, 5, mask))
                {
                    hitSomething = true;
                }

                //RaycastHit2D hit = Physics2D.Raycast(originPos, Vector3.up, 5, mask);

                //if (hit.collider != null)
                //{
                //    if(hit.transform.CompareTag("Apple")) hit.transform.GetComponent<Apple>()

                //}
            }

            yield return null;
        }
        
        if(!hitSomething) AttachToTarget();
        else Fall();
    }

    public void Fall()
    {
        GameOver?.Invoke();
        GameManager.Instance.GameOver();
        GameInterface.Instance.GameOver();

        GetNextKnife();

        rb.isKinematic = false;
        rb.AddForce(Vector3.down * 15, ForceMode2D.Impulse);
        rb.AddTorque(20, ForceMode2D.Impulse);

        StopAllCoroutines();
        StartCoroutine(DeactivateAfterTime(2));
    }

    public void LevelCleared()
    {
        transform.parent = null;
        rb.isKinematic = false;
        col.enabled = false;

        Vector3 impulseDir = transform.position - targetOriginPos.position;

        rb.AddForce(impulseDir * 5, ForceMode2D.Impulse);
        rb.AddTorque(20, ForceMode2D.Impulse);

        StopAllCoroutines();
        StartCoroutine(DeactivateAfterTime(2));
    }

    public void AttachToTarget()
    {
        transform.SetParent(GameManager.Instance.currentTarget.transform);
        state = KnifeState.ATTACHED;
        AttachedToTarget?.Invoke();

        GameManager.Instance.IncreaseScore();

        col.enabled = true;

        GetNextKnife();
    }

    void GetNextKnife()
    {
        GameManager.Instance.GetNextKnife();
    }
}
