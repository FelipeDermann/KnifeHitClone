using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    Animator anim;
    AnimatorOverrideController animatorOverrideController;


    void Awake()
    {
        Knife.AttachedToTarget += PlayHitAnim;
    }

    void OnDestroy()
    {
        Knife.AttachedToTarget -= PlayHitAnim;
    }

    void PlayHitAnim()
    {
        Debug.Log("TARGET GOT A NEW KNIFE");
        anim.SetTrigger("Hit");
    }

    public void StartRotation(AnimationClip _clip)
    {
        anim = GetComponent<Animator>();

        animatorOverrideController = new AnimatorOverrideController(anim.runtimeAnimatorController);
        anim.runtimeAnimatorController = animatorOverrideController;

        animatorOverrideController["Empty"] = _clip;

        anim.SetTrigger("Start");
    }

    public void EndLevel()
    {
        anim.SetTrigger("End");
    }

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
