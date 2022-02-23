using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem healingParticle;
    [SerializeField] ParticleSystem knockbackParticle;
    [SerializeField] ParticleSystem jumpParticle;
    [SerializeField] ParticleSystem hurtParticle;
    [SerializeField] ParticleSystem sporeParticle;
    ParticleSystem.ShapeModule particleShape;
    float shapeRadius;

    public static ParticleManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public void SpawnParticle(ParticleTypes type, Vector3 position, float radius = 0)
    {
        shapeRadius = radius;

        switch (type)
        {
            case ParticleTypes.Explosion:
                
               Instantiate(explosionParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.Healing:

                Instantiate(healingParticle, position, Quaternion.Euler(-90, 0, 0));

                particleShape = healingParticle.shape;

                particleShape.radius = shapeRadius;

                break;

            case ParticleTypes.Knockback:

                Instantiate(knockbackParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.JumpLanding:

                Instantiate(jumpParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.Hurt:

                Instantiate(hurtParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.Spore:

                Instantiate(sporeParticle, position, Quaternion.Euler(-90, 0, 0));

                break;
        }
    }

    public void SetShapeRadius(float radius)
    {
        shapeRadius = radius;
    }
}
