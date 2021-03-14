using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armor_Health_Bar : MonoBehaviour
{
    Vector3 localScale;
    Transform transform;
    // Start is called before the first frame update
    void Start()
    {
        transform = GetComponent<Transform>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = gameObject.transform.parent.gameObject.GetComponent<Enemy>().armorAmount;
        transform.localScale = localScale;
    }
}
