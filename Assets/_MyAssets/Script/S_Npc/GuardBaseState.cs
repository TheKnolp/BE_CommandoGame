/// <summary>Base class for states that share one guard context.</summary>
public abstract class GuardBaseState : IState
{
    protected readonly GuardController Guard;
    protected GuardBaseState(GuardController guard) => Guard = guard;
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}
