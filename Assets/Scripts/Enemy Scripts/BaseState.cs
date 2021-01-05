using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.Reflection;

public abstract class BaseState
{
    public BaseState(GameObject gameObject)
    {
        this.gameObject = gameObject;
        this.transform = gameObject.transform;
    }
    protected GameObject gameObject;
    protected Transform transform;

    public abstract Type Tick();
}
