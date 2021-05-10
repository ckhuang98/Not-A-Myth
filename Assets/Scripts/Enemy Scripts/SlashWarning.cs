using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWarning : MonoBehaviour
{
    private float destroyTime = 1f;
    private Color colorChange;
    private SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        sr = gameObject.GetComponent<SpriteRenderer>();
        colorChange = gameObject.GetComponent<SpriteRenderer>().color;
        StartCoroutine("TurnWhite");
    }

    // Update is called once per frame
    void Update()
    {
        if (destroyTime >= 0f) {
            destroyTime -= Time.deltaTime;
        } else {
            Destroy(this.gameObject);
        }
    }

    private IEnumerator TurnWhite() {
        colorChange = Color.white;
        sr.color = colorChange;
        yield return new WaitForSeconds(.2f);
        StartCoroutine("TurnRed");
    }

    private IEnumerator TurnRed() {
        colorChange = Color.red;
        sr.color = colorChange;
        yield return new WaitForSeconds(.2f);
        StartCoroutine("TurnWhite");
    }
}
