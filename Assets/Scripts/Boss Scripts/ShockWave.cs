using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private Color col;
    //public float alphaLevel = 1.01f;
    SpriteRenderer rend;
    private bool called = false;
    //Transform transform = GameObject.GetComponent<Transform>();
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
    }

    
    void FixedUpdate()
    {
        Vector3 newScale = transform.localScale;
        newScale *= 1.04f;
        transform.localScale = newScale;
        //Calls the IEnumerator for FadeOut
        if (transform.localScale.x >= 15.0f && called == false) {
            called = true;
            StartCoroutine("FadeOut");
        }

    }

    /*
    Purpose: Slowly decrease the alpha for 5 seconds and destroys the object.
    Receives: nothing
    Returns: nothing
    */
    IEnumerator FadeOut() {
        for (float f = 1f; f >= -0.05f; f -= 0.05f) {
            Color c = rend.material.color;
            c.a = f;
            rend.material.color = c;
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(this.gameObject);
    }
}