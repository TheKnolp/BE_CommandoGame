using UnityEngine;

public class GuardPatrolState : GuardBaseState
{
    public GuardPatrolState(GuardController guard) : base(guard)
    { }
    public override void Enter()
    {
        Debug.Log("Entering Patrol");
    }

    public override void Exit()
    {
        Debug.Log("I saw Something");
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
