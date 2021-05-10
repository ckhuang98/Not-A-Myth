using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponRange : MonoBehaviour
{
    Vector3 scale = new Vector3();
    public Transform pos;

    // Start is called before the first frame update
    void Start()
    {
        scale = new Vector3(1.5f, 1.5f , 0);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameMaster.instance.playerStats.increaseWeaponScale.Value)
            transform.localScale = scale;
    }
}
