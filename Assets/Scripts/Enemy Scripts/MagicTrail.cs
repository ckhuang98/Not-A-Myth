using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicTrail : MonoBehaviour
{
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        var main = ps.main;
        if (transform.parent != null && transform.parent.tag == "Healing Projectile") {
            main.startColor = new Color(0.0f, 1.0f, 0.0f, 1.0f);
        } else if (transform.parent != null && transform.parent.tag == "Imp Damage Projectile") {
            main.startColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
        }
    }

    // Update is called once per frame
    /*
    void Update()
    {
        
    }
    */
}
