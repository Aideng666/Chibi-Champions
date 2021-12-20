using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] Animator playerAnimator;
    [SerializeField] Animator enemyAnimator;

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

    public void PlayPlayerAttackAnim()
    {
        playerAnimator.SetTrigger("Attack");
    }

    public void PlayEnemyAttackAnim()
    {
        enemyAnimator.SetTrigger("Attack");
    }
}
