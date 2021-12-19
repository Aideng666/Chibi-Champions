using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;

    bool playerIsWalking;

    public static AnimController Instance { get; set; }


    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        playerAnimator.SetBool("IsWalking", playerIsWalking);
    }

    public void SetPlayerIsWalking(bool walking)
    {
        playerIsWalking = walking;
    }

    public void PlayAttackAnim()
    {
        playerAnimator.SetTrigger("Attack");
    }
}
