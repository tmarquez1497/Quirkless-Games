using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This is the script that contains the entire objective.
 */

[CreateAssetMenu(menuName = "GuardController/State")]
public class State : ScriptableObject {
    public Action[] actions;                // A list of actions that tell the guard what to do
    public Color gizmoColor = Color.cyan;   // Placeholder for any editor gizmos (if needed later)
    public Transition[] transitions;        // A list of changes that move to new objectives.

    public void UpdateState(StateController controller)
    {
        // When called, perform all the saved actions and check to see if it should change to a new objective.
        DoActions(controller);
        CheckTransitions(controller);
    }

    private void DoActions(StateController controller)
    {
        // This loops through each action from the list.
        for (int i = 0; i < actions.Length; i++)
            actions[i].Act(controller);
    }

    private void CheckTransitions(StateController controller)
    {
        // This loops through all saved changes to set a new objective (if it's time to).
        for (int i = 0; i < transitions.Length; i++)
        {
            if (transitions[i].decision.Decide(controller))
                controller.TransitionToState(transitions[i].trueState);
            else
                controller.TransitionToState(transitions[i].falseState);
        }
    }
}
