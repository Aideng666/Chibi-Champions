using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TennisBall : MonoBehaviour
{
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] float explosionRadius;
    [SerializeField] AudioSource fuze;

    Tower tower;
    float fuseDuration;

    bool fuseEnded = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Fuse());        
        fuze.volume = FindObjectOfType<AudioManager>().GetSFXVolume();

    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<AudioManager>().dirtyBal)
        {
            if (FindObjectOfType<AudioManager>().isMute() == true)
            {
                fuze.mute = true;
            }
            else
            {
                fuze.mute = false;
            }

            fuze.volume = FindObjectOfType<AudioManager>().GetSFXVolume();
            FindObjectOfType<AudioManager>().dirtyBal= false;

        }



        if (fuseEnded)
        {
            Explode();
        }
    }

    void Explode()
    {
        ParticleManager.Instance.SpawnParticle(ParticleTypes.Explosion, transform.position);

        Collider[] enemiesHit = Physics.OverlapSphere(transform.position, explosionRadius, enemyLayer);

        foreach (Collider enemy in enemiesHit)
        {
            if (enemy.tag == "Enemy")
            {
                enemy.gameObject.GetComponentInParent<Health>().ModifyHealth(-tower.GetDamage());
                enemy.GetComponentInParent<Enemy>().Knockback(40, transform);
            }
        }

        Destroy(gameObject);
    }

    IEnumerator Fuse()
    {
        yield return new WaitForSeconds(fuseDuration);

        fuseEnded = true;
    }

    public void SetFuseDuration(float duration)
    {
        fuseDuration = duration;
    }

    public void SetTower(Tower t)
    {
        tower = t;
    }
}
