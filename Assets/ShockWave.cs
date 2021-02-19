using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWave : MonoBehaviour
{
    private float timer = 5.0f;
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

    // Update is called once per frame
    void Update()
    {
        /*
        if (timer >= 0.0f) {
            timer -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
        */
        Vector3 newScale = transform.localScale;
        newScale *= 1.01f;
        transform.localScale = newScale;
        if (transform.localScale.x >= 15.0f && called == false) {
            Debug.Log("Called");
            called = true;
            StartCoroutine("FadeOut");
        }

    }

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