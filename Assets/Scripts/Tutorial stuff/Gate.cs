using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public bool unlocked;

    BoxCollider2D box;

    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        unlocked = false;
        animator = this.GetComponent<Animator>();
        box = this.GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("Unlocked", unlocked);
        if(unlocked){
            //box.enabled = false;
            Destroy(this.gameObject);
        }
    }
}
