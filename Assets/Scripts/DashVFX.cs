using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashVFX : MonoBehaviour
{
    public Animator south;
    public Animator southwest;
    public Animator west;
    public Animator east;
    public Animator southeast;
    public Animator north;
    public Animator northwest;
    public Animator northeast;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void playDashVFX(Vector3 direction){
        Debug.Log(direction.x > 0.7f && direction.y > 0.7f);
        if(direction == new Vector3(1.0f, 0.0f, 0.0f)){
            west.SetTrigger("Dash");
        } else if(direction.x > 0.7f && direction.y < -0.7f){
            northwest.SetTrigger("Dash");
        } else if(direction == new Vector3(0.0f, -1.0f, 0.0f)){
            north.SetTrigger("Dash");
        } else if(direction.x < -0.7f && direction.y < -0.7f){
            northeast.SetTrigger("Dash");
        } else if(direction == new Vector3(-1f, 0, 0)){
            east.SetTrigger("Dash");
        } else if(direction.x < -0.7f && direction.y > 0.7f){
            southeast.SetTrigger("Dash");
        } else if(direction == new Vector3(0.0f, 1.0f, 0.0f)){
            south.SetTrigger("Dash");
        } else if(direction.x > 0.7f && direction.y > 0.7f){
            southwest.SetTrigger("Dash");
        }
    }
}
