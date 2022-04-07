using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    //Characters
    [SerializeField] Animator drumstickAnimator;
    [SerializeField] Animator rolfeAnimator;
    [SerializeField] Animator potterAnimator;

    bool gatlingFiring;

    public static AnimController Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SetEnemyIsWalking(Animator anim, bool walking)
    {
        anim.SetBool("IsWalking", walking);
    }

    public void PlayTowerShootAnim(Animator anim)
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

    public void SetPlayerWalking(Animator anim, bool isWalking, bool forward)
    {
        if (anim == drumstickAnimator)
        {
            if (forward)
            {
                drumstickAnimator.SetBool("WalkForward", isWalking);
            }
            else
            {
                drumstickAnimator.SetBool("WalkBackward", isWalking);
            }
        }
        if (anim == rolfeAnimator)
        {
            if (forward)
            {
                rolfeAnimator.SetBool("WalkForward", isWalking);
            }
            else
            {
                rolfeAnimator.SetBool("WalkBackward", isWalking);
            }
        }
        if (anim == potterAnimator)
        {
            if (forward)
            {
                potterAnimator.SetBool("WalkForward", isWalking);
            }
            else
            {
                potterAnimator.SetBool("WalkBackward", isWalking);
            }
        }
    }

    public void SetPlayerStrafing(Animator anim, bool isStrafing, int direction = 0) //0 = left 1 = right
    {
        if (anim == drumstickAnimator)
        {
            if (direction == 0)
            {
                drumstickAnimator.SetBool("StrafeLeft", isStrafing);
            }
            else if (direction == 1)
            {
                drumstickAnimator.SetBool("StrafeRight", isStrafing);
            }
        }
        if (anim == rolfeAnimator)
        {
            if (direction == 0)
            {
                rolfeAnimator.SetBool("StrafeLeft", isStrafing);
            }
            else if (direction == 1)
            {
                rolfeAnimator.SetBool("StrafeRight", isStrafing);
            }
        }
        if (anim == potterAnimator)
        {
            if (direction == 0)
            {
                potterAnimator.SetBool("StrafeLeft", isStrafing);
            }
            else if (direction == 1)
            {
                potterAnimator.SetBool("StrafeRight", isStrafing);
            }
        }
    }

    public void PlayPlayerJumpAnim(Animator anim)
    {
        anim.SetTrigger("Jump");
    }

    public void PlayPlayerAttackAnim(Animator anim)
    {
        anim.SetTrigger("Attack");
    }

    public void PlayPlayerDeathAnim(Animator anim)
    {
        anim.SetTrigger("Died");
    }

    public void SetPlayerRespawn(Animator anim)
    {
        anim.SetTrigger("Respawn");

    }

    public void PlayPlayerAbilityAnim(Animator anim)
    {
        anim.SetTrigger("Ability");
    }
}
