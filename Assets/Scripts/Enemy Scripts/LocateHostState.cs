using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class LocateHostState : BaseState
{
    private Enemy _enemy;
    private Dictionary<string, float> badGuys = new Dictionary<string, float>();
    private GameObject[] hammerGiants = GameObject.FindGameObjectsWithTag("Hammer Giant");
    private GameObject[] fireImps = GameObject.FindGameObjectsWithTag("Fire Imp");
    private GameObject[] fireEels = GameObject.FindGameObjectsWithTag("Fire Eel");
    private GameObject[] swordGiants = GameObject.FindGameObjectsWithTag("Sword Giant");
    private Transform closestHammer, closestImp, closestEel, closestSword;

    //private GameObject[] fullList;
    public LocateHostState(Enemy enemy) : base(enemy.gameObject)
    {
        _enemy = enemy;
        badGuys["Hammer Giant"] = Mathf.Infinity;
        badGuys["Fire Imp"] = Mathf.Infinity;
        badGuys["Fire Eel"] = Mathf.Infinity;
        badGuys["Sword Giant"] = Mathf.Infinity;
    }

    public override Type Tick() {
        return typeof(LocateHostState);
    }
    
    private void NPCDetection() { 
        foreach (GameObject _hammerGiant in hammerGiants) {
            if (_hammerGiant != null) {
                var currentDistance = Vector3.Distance(transform.position, _hammerGiant.transform.position);
                if (currentDistance < badGuys["Hammer Giant"])
                {
                    badGuys["Hammer Giant"] = currentDistance;
                    closestHammer = _hammerGiant.transform;
                } 
            }
        }

        foreach (GameObject _fireImp in fireImps) {
            if (_fireImp != null) {
                var currentDistance = Vector3.Distance(transform.position, _fireImp.transform.position);
                if (currentDistance < badGuys["Fire Imp"])
                {
                    badGuys["Fire Imp"] = currentDistance;
                    closestImp = _fireImp.transform;
                } 
            }
        }

        foreach(GameObject _fireEel in fireEels) {
            if (_fireEel != null) {
                float currentDistance = Vector3.Distance(transform.position, _fireEel.transform.position);
                if (currentDistance < badGuys["Fire Eel"]) {
                    badGuys["Fire Eel"] = currentDistance;
                    closestImp = _fireEel.transform;
                }
            }
        }

        foreach(GameObject _swordGiant in swordGiants) {
            if (_swordGiant != null) {
                float currentDistance = Vector3.Distance(transform.position, _swordGiant.transform.position);
                if (currentDistance < badGuys["Sword Giant"]) {
                    badGuys["Sword Giant"] = currentDistance;
                    closestHammer = _swordGiant.transform;
                }
            }
        }
    }
}
