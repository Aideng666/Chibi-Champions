using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] ParticleSystem explosionParticle;
    [SerializeField] ParticleSystem healingParticle;
    [SerializeField] ParticleSystem knockbackParticle;
    [SerializeField] ParticleSystem landingParticle;
    [SerializeField] ParticleSystem hurtParticle;
    [SerializeField] ParticleSystem sporeParticle;
    [SerializeField] ParticleSystem groundPoundParticle;
    [SerializeField] ParticleSystem speedParticle;
    [SerializeField] ParticleSystem highJumpParticle;
    ParticleSystem.ShapeModule particleShape;
    float shapeRadius;

    ParticleSystem currentParticle;

    public static ParticleManager Instance { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnParticle(ParticleTypes type, Vector3 position, float radius = 0)
    {
        shapeRadius = radius;

        switch (type)
        {
            case ParticleTypes.Explosion:
                
              currentParticle = Instantiate(explosionParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.Healing:

                currentParticle = Instantiate(healingParticle, position, Quaternion.Euler(-90, 0, 0));

                particleShape = healingParticle.shape;

                particleShape.radius = shapeRadius;

                break;

            case ParticleTypes.Knockback:

                currentParticle = Instantiate(knockbackParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.JumpLanding:

                currentParticle = Instantiate(landingParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.Hurt:

                currentParticle = Instantiate(hurtParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.Spore:

                currentParticle = Instantiate(sporeParticle, position, Quaternion.Euler(-90, 0, 0));

                break;

            case ParticleTypes.GroundPound:

                currentParticle = Instantiate(groundPoundParticle, position, Quaternion.Euler(-90, 0, 0));

                particleShape = groundPoundParticle.shape;

                particleShape.radius = shapeRadius;

                break;

            case ParticleTypes.Speed:

                currentParticle = Instantiate(speedParticle, position, Quaternion.Euler(90, 0, 0));

                break;

            case ParticleTypes.HighJump:

                currentParticle = Instantiate(highJumpParticle, position, Quaternion.Euler(-90, 0, 0));

                break;
        }

        return currentParticle.gameObject;
    }

    public void SetShapeRadius(float radius)
    {
        shapeRadius = radius;
    }
}
