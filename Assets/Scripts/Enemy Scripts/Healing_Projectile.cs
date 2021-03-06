using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Healing_Projectile : MonoBehaviour
{
    private float healingProjectileSpeed = 10f;
    private Transform target;
    private GameObject[] hammerGiants;
    // Start is called before the first frame update
    void Start()
    {
        hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
        target = hammerGiants[0].transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (hammerGiants.Length > 0) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, healingProjectileSpeed * Time.deltaTime);
        } 
        
        

        foreach (GameObject _hammerGiant in hammerGiants) {
            if (_hammerGiant != null) {
                var checkingDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                var targetDistance = Vector3.Distance(transform.position, target.position);
                if (checkingDistance < targetDistance)
                {
                    target = _hammerGiant.transform;
                } 
            }
        }
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Hammer Giant")) {
            Destroy(this.gameObject);
        }
    }
}
