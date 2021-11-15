using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodParticleFabric : MonoBehaviour
{
    [SerializeField] private ParticleSystem playerBloodParticlePrefab; 

    public void CreateBlood()
    {
        if (playerBloodParticlePrefab != null)
        {
            Instantiate(playerBloodParticlePrefab, gameObject.transform);
        }
        
    }
}
