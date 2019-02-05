using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script checks to see if the guard needs to change to the chasing objective.
 * If the player is inside the view cone, then the objective changes.
 */

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
