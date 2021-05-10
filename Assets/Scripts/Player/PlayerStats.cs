using System;
using UnityEngine;

[CreateAssetMenu]
public class PlayerStats : ScriptableObject
{
	public SharedFloat currentHealth;
	public SharedFloat maxHealth;

	public SharedFloat currentXp;

	public SharedFloat skillPoints;

	public SharedFloat attackPower;

	public SharedFloat speed;

	public SharedFloat maxSpeed;

	public SharedFloat sprintSpeed;

	public SharedFloat dashSpeed;

	public SharedFloat knockBackForce;
	public SharedFloat cameraOffsetX;
	public SharedFloat cameraOffsetY;

	public SharedFloat dashCooldown;
	public SharedFloat healAmount;
	public SharedFloat weaponRange;
	public SharedFloat freezeDuration;

	[Space]
	public SharedBool unlockedDoubleDash;

	public SharedBool unlockedHealthDash;
	public SharedBool unlockedRegen;
	public SharedBool unlockedDashAttack;
	public SharedBool unlockedAttackRegen;
	public SharedBool unlockedGroundSmash;
	public SharedBool unlockedDashMovement;
	public SharedBool unlockedPlantDrop;

	public SharedBool inCombat;
	public SharedBool attackRegenHit;
	public SharedBool inBossFight;
	public SharedBool isAttacking;
	public SharedBool toggleMovement;
	public SharedBool increaseWeaponScale;

	

	[Space]
	public SharedStringList keys;

	public event Action OnStatsChanged;

	private void OnEnable()
	{

		SharedFloat[] SharedFloats =
		{
			currentHealth,
			maxHealth,
			currentXp,
			skillPoints,
			attackPower,
			speed,
			maxSpeed,
			sprintSpeed,
			dashSpeed,
			knockBackForce,
			cameraOffsetX,
			cameraOffsetY,
			dashCooldown,
			healAmount,
			weaponRange,
			freezeDuration,
		};

		foreach (SharedFloat sf in SharedFloats) {
			sf.OnValueChanged += CallOnStatsChanged;
		}

		SharedBool[] SharedBools =
		{
			unlockedDoubleDash,
			unlockedHealthDash,
			unlockedRegen,
			unlockedDashAttack,
			unlockedAttackRegen,
			unlockedGroundSmash,
			unlockedDashMovement,
			unlockedPlantDrop,
			inCombat,
			attackRegenHit,
			inBossFight,
			isAttacking,
			toggleMovement,
			increaseWeaponScale,
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
