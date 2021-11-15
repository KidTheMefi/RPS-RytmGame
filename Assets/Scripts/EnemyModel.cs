using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(BloodParticleFabric))]

public class EnemyModel : MonoBehaviour
{
    
    private Animator EnemyAnimator;
    private BloodParticleFabric bloodParticle; 

    void Start()
    {
        EnemyAnimator = GetComponent<Animator>();
        bloodParticle = GetComponent<BloodParticleFabric>();
    }

    public void AnimationSetTrigger(string trigger)
    {
        EnemyAnimator.SetTrigger(trigger);
    }

    public void playBloodParticle()
    {
        bloodParticle.CreateBlood();
    }
}
