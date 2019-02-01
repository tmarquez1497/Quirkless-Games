using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GuardController/Decisions/Chase")]
public class ChaseDecision : Decision {
    public override bool Decide(StateController controller)
    {
        if (!controller.eyes.canSeePlayer)
        {
            controller.chaseTarget = null;
            return true;
        }
        else
            return false;
    }
}
