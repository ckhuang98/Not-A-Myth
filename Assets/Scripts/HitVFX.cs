using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitVFX : MonoBehaviour
{
    public bool playing;
    // Start is called before the first frame update
    void Start()
    {
        playing = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void isPlaying(){
        playing = true;
    }

    public void stopPlaying(){
        playing = false;
    }


}
