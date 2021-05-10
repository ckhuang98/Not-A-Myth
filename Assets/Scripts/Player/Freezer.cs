using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Freezer : MonoBehaviour
{
    [Range(0f, 1f)]
    public float duration = 0.1f;

    float pendingFreezeDuration = 0f;

    bool isFrozen = false;
    // Update is called once per frame
    void Update()
    {
        duration = GameMaster.instance.playerStats.freezeDuration.Value;
        if(pendingFreezeDuration > 0 && !isFrozen){
            StartCoroutine(DoFreeze());
            Debug.Log("Froze");
        }
    }

    public void Freeze(){
        pendingFreezeDuration = duration;
    }

    IEnumerator DoFreeze(){
        isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;

        yield return new WaitForSecondsRealtime(duration);

        Time.timeScale = original;
        pendingFreezeDuration = 0;
        isFrozen = false;
    }
}
