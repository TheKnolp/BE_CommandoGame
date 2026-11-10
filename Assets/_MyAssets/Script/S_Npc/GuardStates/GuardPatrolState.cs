using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GuardPatrolState : GuardBaseState
{
    public GuardPatrolState(GuardController guard) : base(guard)
    {
        
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        Guard.StopMoving();
    }

    public override void Tick()
    {
        if(Guard.HasReachedCurrentDestination())
        {
            
            Guard.AdvanceToNextWaypoint();
            Guard.MoveTo(Guard.GetCurrentWaypoint().position);
        }
    }
    
}
