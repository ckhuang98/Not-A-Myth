using System;
using UnityEngine;

[CreateAssetMenu]
public class SharedFloat : ScriptableObject
{
    [SerializeField]
    private float initialValue, currentValue;

	public event Action OnValueChanged;

	private void OnEnable()
	{
		currentValue = initialValue;
	}

	public float Value
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
