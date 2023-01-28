using UnityEngine;
public class FSMClearSignal : StateMachineBehaviour
{
    public string[] clearAtEnter;
    public string[] cleatAtExit;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);//÷ÿ÷√trigger
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (var signal in clearAtEnter)
        {
            animator.ResetTrigger(signal);//÷ÿ÷√trigger
        }
    }
}