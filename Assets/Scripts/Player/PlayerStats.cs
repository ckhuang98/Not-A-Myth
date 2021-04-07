using System;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
	public SharedFloat currentHealth;
	public SharedFloat maxHealth;

	public SharedFloat attackPower;

	public SharedFloat speed;

	public SharedFloat dashSpeed;

	[Space]
	public SharedBool unlockedDoubleDash;

	public event Action OnStatsChanged;

	private void OnEnable()
	{
		SharedFloat[] SharedFloats =
		{
			currentHealth,
			maxHealth,
			attackPower,
			speed,
			dashSpeed,
		};

		foreach (SharedFloat sf in SharedFloats) {
			sf.OnValueChanged += CallOnStatsChanged;
		}

		SharedBool[] SharedBools =
		{
			unlockedDoubleDash,
		};

		foreach (SharedBool sb in SharedBools)
		{
			sb.OnValueChanged += CallOnStatsChanged;
		}
	}

	private void CallOnStatsChanged()
	{
		OnStatsChanged?.Invoke();
	}
}
