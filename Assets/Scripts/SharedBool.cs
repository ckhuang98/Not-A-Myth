using System;
using UnityEngine;

[CreateAssetMenu]
public class SharedBool : ScriptableObject
{
	[SerializeField]
	private bool initialValue, currentValue;

	public event Action OnValueChanged;

	private void OnEnable()
	{
		currentValue = initialValue;
	}

	public bool Value
	{
		get => currentValue;
		set
		{
			if (value != currentValue)
			{
				currentValue = value;
				OnValueChanged?.Invoke();
			}
		}
	}
}