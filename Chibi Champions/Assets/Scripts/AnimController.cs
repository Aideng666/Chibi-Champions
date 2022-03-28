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

    public void SetPlayerWalking(Animator anim, bool isWalking, bool forward, bool sendToOtherClients = true)
    {
        if (anim == drumstickAnimator)
        {
            if (forward)
            {
                drumstickAnimator.SetBool("WalkForward", isWalking);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Drumstick", AnimationTypes.WalkForward, isWalking);
                }
            }
            else
            {
                drumstickAnimator.SetBool("WalkBackward", isWalking);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Drumstick", AnimationTypes.WalkBackward, isWalking);
                }
            }
        }
        if (anim == rolfeAnimator)
        {
            if (forward)
            {
                rolfeAnimator.SetBool("WalkForward", isWalking);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Rolfe", AnimationTypes.WalkForward, isWalking);
                }
            }
            else
            {
                rolfeAnimator.SetBool("WalkBackward", isWalking);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Rolfe", AnimationTypes.WalkBackward, isWalking);
                }
            }
        }
        if (anim == potterAnimator)
        {
            if (forward)
            {
                potterAnimator.SetBool("WalkForward", isWalking);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Potter", AnimationTypes.WalkForward, isWalking);
                }
            }
            else
            {
                potterAnimator.SetBool("WalkBackward", isWalking);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Potter", AnimationTypes.WalkBackward, isWalking);
                }
            }
        }
    }

    public void SetPlayerStrafing(Animator anim, bool isStrafing, int direction = 0, bool sendToOtherClients = true) //0 = left 1 = right
    {
        if (anim == drumstickAnimator)
        {
            if (direction == 0)
            {
                drumstickAnimator.SetBool("StrafeLeft", isStrafing);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Drumstick", AnimationTypes.WalkLeft, isStrafing);
                }
            }
            else if (direction == 1)
            {
                drumstickAnimator.SetBool("StrafeRight", isStrafing);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Drumstick", AnimationTypes.WalkRight, isStrafing);
                }
            }
        }
        if (anim == rolfeAnimator)
        {
            if (direction == 0)
            {
                rolfeAnimator.SetBool("StrafeLeft", isStrafing);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Rolfe", AnimationTypes.WalkLeft, isStrafing);
                }
            }
            else if (direction == 1)
            {
                rolfeAnimator.SetBool("StrafeRight", isStrafing);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Rolfe", AnimationTypes.WalkRight, isStrafing);
                }
            }
        }
        if (anim == potterAnimator)
        {
            if (direction == 0)
            {
                potterAnimator.SetBool("StrafeLeft", isStrafing);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Potter", AnimationTypes.WalkLeft, isStrafing);
                }
            }
            else if (direction == 1)
            {
                potterAnimator.SetBool("StrafeRight", isStrafing);

                if (sendToOtherClients)
                {
                    UDPClient.Instance.SendAnimationUpdates("Potter", AnimationTypes.WalkRight, isStrafing);
                }
            }
        }
    }

    public void PlayPlayerJumpAnim(Animator anim, bool sendToOtherClients = true)
    {
        anim.SetTrigger("Jump");

        if (sendToOtherClients)
        {
            UDPClient.Instance.SendAnimationUpdates(anim.GetComponentInParent<PlayerController>().GetName(), AnimationTypes.Jump);
        }
    }

    public void PlayPlayerAttackAnim(Animator anim, bool sendToOtherClients = true)
    {
        anim.SetTrigger("Attack");

        if (sendToOtherClients)
        {
            UDPClient.Instance.SendAnimationUpdates(anim.GetComponentInParent<PlayerController>().GetName(), AnimationTypes.BasicAttack);
        }
    }

    public void PlayPlayerDeathAnim(Animator anim, bool sendToOtherClients = true)
    {
        anim.SetTrigger("Died");

        if (sendToOtherClients)
        {
            UDPClient.Instance.SendAnimationUpdates(anim.GetComponentInParent<PlayerController>().GetName(), AnimationTypes.Death);
        }
    }

    public void SetPlayerRespawn(Animator anim, bool sendToOtherClients = true)
    {
        anim.SetTrigger("Respawn");

        if (sendToOtherClients)
        {
            UDPClient.Instance.SendAnimationUpdates(anim.GetComponentInParent<PlayerController>().GetName(), AnimationTypes.Respawn);
        }
    }

    public void PlayPlayerAbilityAnim(Animator anim, bool sendToOtherClients = true)
    {
        anim.SetTrigger("Ability");

        if (sendToOtherClients)
        {
            UDPClient.Instance.SendAnimationUpdates(anim.GetComponentInParent<PlayerController>().GetName(), AnimationTypes.UseAbility);
        }
    }
}
