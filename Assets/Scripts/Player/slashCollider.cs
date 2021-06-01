using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class slashCollider : MonoBehaviour
{
    public PolygonCollider2D hurtBox;
    public PolygonCollider2D extendedHurtBox;

    public CircleCollider2D groundSmash;
    // Start is called before the first frame update
    void Start()
    {
        hurtBox.enabled = false;
        extendedHurtBox.enabled = false;
        groundSmash.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void enableHurtBox(){
        hurtBox.enabled = true;
    }

    public void disableHurtBox(){
        hurtBox.enabled = false;
    }

    public void enableSmashHurtBox(){
        hurtBox.enabled = true;
    }

    public void disableSmashHurtBox(){
        hurtBox.enabled = false;
    }
    
}
