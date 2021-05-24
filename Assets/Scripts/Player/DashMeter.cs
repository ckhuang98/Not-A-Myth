using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashMeter : MonoBehaviour
{
    public PlayerController player;
    Vector3 localScale;
    // Start is called before the first frame update
    void Start()
    {
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(player.dashMeterEmpty){
            localScale.x = 0;
            transform.localScale = localScale;       
        }
        if(player.canDash == false){
            StartCoroutine(dashMeterFill());
        }
    }

    private IEnumerator dashMeterFill(){
        float tempTimer = 0;
        while(player.canDash == false)
        {
            localScale.x = tempTimer;
            transform.localScale = localScale;
            
            tempTimer += Time.deltaTime;
            yield return null;
        }
        localScale.x = 0.7f;
        transform.localScale = localScale;
    }
}
