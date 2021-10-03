using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<T> : MonoBehaviour where T : StateMachine<T>
{
    protected State<T> State;
    public void SetState(State<T> state)
    {
        if(State != null)
            StartCoroutine(State.End());
        State = state;
        StartCoroutine(State.Start());
    }

    public void SetState<S>() where S : State<T>, new()
    {
        State<T> stateInstance = new S();
        stateInstance.StateMachine = (T)this;
        if (State != null)
            StartCoroutine(State.End());
        State = stateInstance;
        StartCoroutine(State.Start());
    }


    public void Update()
    {
        if(State != null)
            StartCoroutine(State.Tick());
    }

    private void OnDestroy()
    {
        this.StopAllCoroutines();
    }
}
