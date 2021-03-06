using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BorderVisibility : MonoBehaviour
{
    private float armor = 0;
    SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        armor = gameObject.transform.parent.gameObject.GetComponent<Enemy>().armorAmount;

        if (armor <= 0) {
            sr.enabled = false;
        } else {
            sr.enabled = true;
        }
    }
}
