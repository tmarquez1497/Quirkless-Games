using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GuardController/Decisions/Patrol")]
public class PatrolDecision : Decision {
    public override bool Decide(StateController controller)
    {
        if (controller.eyes.canSeePlayer)
        {
            controller.chaseTarget = GameObject.FindGameObjectWithTag("Player").transform;
            return true;
        }
        else
            return false;
    }
}
