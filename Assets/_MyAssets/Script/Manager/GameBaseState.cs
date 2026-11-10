
public abstract class GameBaseState : IState
{
    protected readonly GameManager Gmanage;
    protected GameBaseState(GameManager gm) => Gmanage = gm;
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}
