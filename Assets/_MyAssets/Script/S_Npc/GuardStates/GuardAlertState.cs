using UnityEngine;

public class GuardAlertState : GuardBaseState
{
    public GuardAlertState(GuardController guard) : base(guard)
    {
    }

    public override void Enter()
    {
        throw new System.NotImplementedException();
    }

    public override void Exit()
    {
        throw new System.NotImplementedException();
    }

    public override void Tick()
    {
        Guard.FaceTarget();
    }
}
