using UnityEngine;

public class GameManager : MonoBehaviour
{
    private StateMachine stateMachine;
    private GMMenu menuState;
    private GMPlaying playingState;
    private GMDefeat defeatState;
    private GMVictory victoryState;

    public string CurrentStateName => stateMachine != null && stateMachine.CurrentState != null
        ? stateMachine.CurrentState.GetType().Name
        : "Uninitialized";
   
    void Awake()
    {
        stateMachine = new StateMachine();
        playingState = new GMPlaying(this);
        defeatState = new GMDefeat(this);
        victoryState = new GMVictory(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
