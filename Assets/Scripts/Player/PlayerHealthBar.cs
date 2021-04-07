using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : BarScript
{
    public PlayerStats playerStats;

	private void Start()
	{
		if (!playerStats) playerStats = ScriptableObject.CreateInstance("PlayerStats") as PlayerStats;

		slider.maxValue = playerStats.maxHealth.Value;
		slider.value = playerStats.currentHealth.Value;

		playerStats.maxHealth.OnValueChanged += SetMV;
		playerStats.currentHealth.OnValueChanged += SetV;
	}

	private void SetV()
	{
		slider.value = playerStats.currentHealth.Value;
		Debug.Log("Player Health Bar: SetV");
	}

	private void SetMV()
	{
		slider.maxValue = playerStats.maxHealth.Value;
		Debug.Log("Player Health Bar: SetMV");
	}
}
