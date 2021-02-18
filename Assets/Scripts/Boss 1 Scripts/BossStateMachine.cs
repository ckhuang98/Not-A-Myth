using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.Reflection;

public class BossStateMachine : MonoBehaviour
{
    // This timer controls when to select a new state
    public float timer = 0;

    protected Dictionary<Type, BaseState> availableStates;

    //Holds the current state the enemy is in
    public BaseState CurrentState { get; private set; }
    //Triggers an event when the state is changed
    public event Action<BaseState> OnStateChanged;

    /*
    Purpose: sets all the states available in the enemy script file
    from the passed dictionary to the state machine's dictionary
    Recieves: Dictionary containg all states form the enemy.cs file
    Returns: nothig
    */
    public void SetStates(Dictionary<Type, BaseState> states)
    {
        availableStates = states;
    }

    /*
    Purpose: Checks every frame what state the enemy is currently in and 
    when is should be changed. If no state is set, the first state of 
    dictionary will be activated. 
    Recieves: nothing
    Returns: nothing
    */
    public void Update()
    {
        if (CurrentState == null)
        {
            CurrentState = availableStates.Values.First();
        }

        var nextState = CurrentState?.Tick();
        //Debug.Log("working");

        if (nextState != null &&
            nextState != CurrentState?.GetType())
        {
            SwitchToNewState(nextState);
        }
        /*
        if (CurrentState == null)
        {
            Debug.Log("Working");
            CurrentState = availableStates.Values.First();
        }
        else {
            timer += Time.deltaTime;
            if (timer >= 3.0f) {
                
                timer = 0;
                var randomIndex = UnityEngine.Random.Range(1,4);
                CurrentState = availableStates.Values.ElementAt(randomIndex);
            }
        }
        */
    }

    /*
    Purpose: switches the current state of the enemy to whatever is
    returned by it.
    Recieves: nothing
    Returns: nothing
    */
    private void SwitchToNewState(Type nextState)
    {
        CurrentState = availableStates[nextState];
        OnStateChanged?.Invoke(CurrentState);
    }
}
