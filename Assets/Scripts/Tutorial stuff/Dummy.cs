using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dummy : MonoBehaviour
{
    public float currentHealth;
    public float timer = 0;

    public Gate gate;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = 3f;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if(currentHealth <=0){
            Destroy(this.gameObject);
            gate.unlocked = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.name.Equals("SlashSpriteSheet_0") && timer >= .5)
        {
            currentHealth -= GameMaster.instance.playerStats.attackPower.Value;
            timer = 0;
        }
    }
}
