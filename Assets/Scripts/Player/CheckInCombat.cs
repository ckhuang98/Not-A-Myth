using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckInCombat : MonoBehaviour
{
    [SerializeField]
    private PlayerStats stats;


    private void Update() {
        if(GameMaster.instance.numOfEnemies > 0){
            if(FindClosestEnemy() < 50){
                stats.inCombat.Value = true;
            } else{
                stats.inCombat.Value = false;
            }
        } else{
            stats.inCombat.Value = false;
        }
    }


    float FindClosestEnemy() {
        float distance = Mathf.Infinity;
        Vector3 position = transform.position;
        foreach (GameObject enemy in GameMaster.instance.enemyList) {
            if(enemy != null){
                Vector3 diff = enemy.GetComponent<Transform>().position - position;
                float curDistance = diff.sqrMagnitude;
                if (curDistance < distance) {
                    distance = curDistance;
                }
            }

        }
        return distance;
    }
}
