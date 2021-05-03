using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class key : MonoBehaviour
{
    public string keyName;
    public PlayerStats playerStats;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerStats.keys.Value.Add(keyName);
            Destroy(gameObject);
        }
    }
}
