using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Rolfe : PlayerController
{
    [SerializeField] GameObject beaconPrefab;
    [SerializeField] int maxBeacons = 2;

    [SerializeField] AudioSource scratch;
    [SerializeField] AudioSource set;

    int currentBeacons = 0;

    bool beaconActivated = false;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        scratch.volume = AudioManager.Instance.GetSFXVolume();
        set.volume = AudioManager.Instance.GetSFXVolume();
    }

    // Update is called once per frame
    void Update()
    {
        if (AudioManager.Instance.dirtyRol)
        {
            if (AudioManager.Instance.isMute() == true)
            {
                scratch.mute = true;
                set.mute = true;
            }
            else
            {
                scratch.mute = false;
                set.mute = false;
            }

            scratch.volume = AudioManager.Instance.GetSFXVolume();
            set.volume = AudioManager.Instance.GetSFXVolume();
            AudioManager.Instance.dirtyRol = false;

        }


        if (isPlayerCharacter)
        {
            base.Update();

            AbilityCooldown(beaconActivated);
        }
    }

    protected override void Attack()
    {
        if (!CanvasManager.isGamePaused && !CanvasManager.isMultiplayerPaused)
        {
            if (Input.GetMouseButton(0) && CanLightAttack())
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Attack, GetCharacterNameEnum());
                }

                AnimController.Instance.PlayPlayerAttackAnim(GetComponentInChildren<Animator>());

                Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

                foreach (Collider enemy in hitEnemies)
                {
                    if (enemy.tag == "Enemy")
                    {
                        enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                        enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                        enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                        enemy.GetComponentInParent<Enemy>().HitSound();

                        ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
                    }
                }
                scratch.Play();

                StartCoroutine(WaitForSecondSwipe());
            }
            if (Input.GetMouseButtonDown(1) && CanHeavyAttack() && currentBeacons < maxBeacons)
            {
                if (FindObjectOfType<UDPClient>() != null)
                {
                    UDPClient.Instance.SendPlayerUpdates(ActionTypes.Ability, GetCharacterNameEnum());
                }

                AnimController.Instance.PlayPlayerAbilityAnim(GetComponentInChildren<Animator>());

                var beacon = Instantiate(beaconPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                currentBeacons++;

                beaconActivated = true;
                set.Play();
            }
            else
            {
                beaconActivated = false;
            }
        }      
    }

    public override void ReceiveAttackTrigger()
    {
        AnimController.Instance.PlayPlayerAttackAnim(GetComponentInChildren<Animator>());

        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);

                ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
            }
        }

        StartCoroutine(WaitForSecondSwipe());
    }

    public override void ReceiveAbilityTrigger()
    {
        AnimController.Instance.PlayPlayerAbilityAnim(GetComponentInChildren<Animator>());

        var beacon = Instantiate(beaconPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
    }

    void SecondSwipeAttack()
    {
        Collider[] hitEnemies = Physics.OverlapSphere(attackPoint.position, lightAttackRange, enemyLayer);

        foreach (Collider enemy in hitEnemies)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-lightAttackDamage);
                enemy.GetComponentInParent<Enemy>().Knockback(20, transform);
                enemy.GetComponentInParent<Enemy>().SetLastHit(this);
                //GetComponent<PointsManager>().AddPoints(20);
                enemy.GetComponentInParent<Enemy>().HitSound(); ;

                ParticleManager.Instance.SpawnParticle(ParticleTypes.Hurt, enemy.transform.position);
            }
        }
    }

    public void RemoveBeacon()
    {
        if (currentBeacons > 0)
        {
            currentBeacons--;         
        }
    }

    IEnumerator WaitForSecondSwipe()
    {
        yield return new WaitForSeconds(0.4f);

        SecondSwipeAttack();
    }
}
