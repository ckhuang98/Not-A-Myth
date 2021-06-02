using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleEffects : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        ParticleSystem.EmissionModule em = ps.emission;
        em.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        ps.Play();
    }
}
