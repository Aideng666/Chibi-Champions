using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;

    public static AnimController Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerIsWalking(bool walking)
    {
        playerAnimator.SetBool("IsWalking", walking);
    }

    public void SetEnemyIsWalking(Animator anim, bool walking)
    {
        anim.SetBool("IsWalking", walking);
    }

    public void PlayPlayerAttackAnim()
    {
        playerAnimator.SetTrigger("Attack");
    }

    public void PlayEnemyAttackAnim(Animator anim)
    {
        anim.SetTrigger("Attack");
    }

    public bool IsAnimatorPlaying(Animator animator)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            return true;
        }

        return false;
    }
}
