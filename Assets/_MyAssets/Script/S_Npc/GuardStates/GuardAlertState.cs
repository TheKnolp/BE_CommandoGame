using UnityEngine;
using UnityEngine.SceneManagement;

public class GuardAlertState : GuardBaseState
{
    public GuardAlertState(GuardController guard) : base(guard)
    {
    }

    public override void Enter()
    {
        Debug.Log("I need to check position");
    }

    public override void Exit()
    {
        SceneManager.LoadScene("Main");
    }

    public override void Tick()
    {
        Guard.GoToLastPlPosition();
    }
}
