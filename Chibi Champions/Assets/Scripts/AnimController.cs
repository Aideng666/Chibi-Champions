using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    //Characters
    [SerializeField] Animator drumstickAnimator;
    [SerializeField] Animator rolfeAnimator;
    [SerializeField] Animator potterAnimator;

    Animator playerAnimator;

    bool gatlingFiring;

    public static AnimController Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (playerAnimator == null)
        {
            playerAnimator = FindObjectOfType<PlayerController>().GetComponent<Animator>();
        }
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

    public void PlayFeatherBlasterShootAnim(Animator anim)
    {
        anim.SetTrigger("Shoot");
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

    public void SetGatlingDrummetFiring(Animator animator, bool isFiring)
    {
        if (gatlingFiring && isFiring)
        {
            return;
        }
        if (!gatlingFiring && !isFiring)
        {
            return;
        }

        if (isFiring)
        {
            animator.SetTrigger("Fire");
            gatlingFiring = true;
        }
        else
        {
            animator.SetTrigger("Stop");
            gatlingFiring = false;
        }
    }
}
