using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBarrier : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            WaveManager.Instance.AddEnemyKilled();
            EnemyPool.Instance.AddToGruntPool(other.gameObject);
        }
    }
}
