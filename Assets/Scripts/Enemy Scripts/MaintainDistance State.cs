using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class MaintainDistanceState : BaseState
{
    private Enemy _enemy;
    private Transform target;

    private float speed = 3f;
    private float stoppingDistance = 4f;
    private float retreatDistance = 2.5f;

    private float timeBtwShots = 2f;

    public MaintainDistanceState(Enemy enemy) : base(enemy.gameObject) {
        _enemy = enemy;
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override Type Tick() {
        if (Vector2.Distance(transform.position, target.position) > stoppingDistance) {
            transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        } else if (Vector2.Distance(transform.position, target.position) < stoppingDistance && Vector2.Distance(transform.position, target.position) > retreatDistance) {
            transform.position = this.transform.position;
        } else if (Vector2.Distance(transform.position, target.position) < retreatDistance) {
             transform.position = Vector2.MoveTowards(transform.position, target.position, -speed * Time.deltaTime);
        }

        if (timeBtwShots <= 0) {
            timeBtwShots = 2f;
            return typeof(FireProjectileState);
        } else {
            timeBtwShots -= Time.deltaTime;
        }
        return typeof(MaintainDistanceState);
    }
}
