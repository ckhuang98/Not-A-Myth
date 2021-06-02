using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    // Start is called before the first frame update
Vector3 target = Vector3.zero;
    public Animator animator;

    public bool playing;

    public HitVFX vfx;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        playing = vfx.playing;
    }
    
    public void updateTarget(Vector3 transform){
        target = transform;
        Debug.Log(target);
        animator.SetTrigger("Hit");
        if(target != Vector3.zero && !playing)
        {
            Vector3 dir = target - this.transform.position;
            float angle = Mathf.Atan2(dir.y,dir.x) * Mathf.Rad2Deg;
            this.transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            target = Vector3.zero;
        }
    }

}
