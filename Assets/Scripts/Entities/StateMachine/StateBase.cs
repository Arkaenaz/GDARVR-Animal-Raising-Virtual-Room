using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateBase<TEntity> where TEntity : StateMachine<TEntity>
{
    public readonly TEntity Entity;

    protected StateBase(TEntity entity)
    {
        Entity = entity;
    }
    public virtual void Enter()
    {
    }
    public virtual void Update()
    {
    }
    public virtual void Exit()
    {
    }
}
