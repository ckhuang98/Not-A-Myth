using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopLeft : MonoBehaviour
{
    // Start is called before the first frame update
    private Enemy enemyScript;
    // Start is called before the first frame update
    void Start()
    {
        enemyScript = GetComponentInParent<Enemy>();
    }

    void OnTriggerEnter2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            enemyScript.SetAttackDir("TopLeft");
        }
    }

    void OnTriggerStay2D(Collider2D col) {
        if (col.CompareTag("Player")) {
            enemyScript.SetAttackDir("TopLeft");
        }
    }

    void OnTriggerExit2D(Collider2D col) {
        enemyScript.SetAttackDir("Not Set");
    }
}
