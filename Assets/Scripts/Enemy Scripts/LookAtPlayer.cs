using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{
    Transform target;
    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        target = GameMaster.instance.player.transform;
    }

    // Update is called once per frame
    void Update()
    {
        if(target != null)
        {
            Vector3 dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}
