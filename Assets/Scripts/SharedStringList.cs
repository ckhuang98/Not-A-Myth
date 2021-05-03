using System;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public class SharedStringList : ScriptableObject
{
    [SerializeField]
    private List<string> initialValue, currentValue;

	public event Action OnValueChanged;

	private void OnEnable()
	{
        currentValue.Clear();
        initialValue.Clear();
		currentValue = initialValue;
	}

	public List<string> Value
	{
        get => currentValue;
        set
		{
            if (value != currentValue)
			{
				currentValue = value;
                Debug.Log("shared string list set");
				OnValueChanged?.Invoke();
			}
		}
	}
}
