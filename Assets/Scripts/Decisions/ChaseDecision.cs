using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GuardController/Decisions/Chase")]
public class ChaseDecision : Decision {
    public override bool Decide(StateController controller)
    {
        // If the guard can't see the player then it's time to switch to a new objective.
        // Otherwise, just stay on the say same objective.
        if (!controller.eyes.canSeePlayer)
        {
            controller.chaseTarget = null;
            return true;
        }
        else
            return false;
    }
}
