using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StateMachine<TEntity> : MonoBehaviour where TEntity : StateMachine<TEntity>
{
    protected virtual StateBase<TEntity> CurrentState { get; private set; }

    protected virtual void Update()
    {
        CurrentState?.Update();
    }

    public virtual void SwitchState(StateBase<TEntity> state)
    {
        CurrentState?.Exit();
        CurrentState = state;
        CurrentState.Enter();
    }
}
