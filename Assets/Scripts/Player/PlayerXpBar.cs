using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerXpBar : BarScript
{
    public PlayerStats playerStats;

	private void Start()
	{
		if (!playerStats) playerStats = ScriptableObject.CreateInstance("PlayerStats") as PlayerStats;

		slider.maxValue = 5;
		slider.value = playerStats.currentXp.Value;

		playerStats.currentXp.OnValueChanged += SetV;
	}

	private void SetV()
	{
		slider.value = playerStats.currentXp.Value;
	}

}