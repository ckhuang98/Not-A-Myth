using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyHealth : MonoBehaviour
{
    Vector3 localScale;
    Transform transform;

    public Dummy dummy;
    // Start is called before the first frame update
    void Start()
    {
        transform = this.GetComponent<Transform>();
        localScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        localScale.x = dummy.currentHealth;
        transform.localScale = localScale;
    }
}
