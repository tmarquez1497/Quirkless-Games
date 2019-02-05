using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script contains the chasing objective.
 * It only tells the guard where the player is to navigate to him.
 */

[CreateAssetMenu(menuName = "GuardController/Actions/Chase")]
public class ChaseAction : Action {
    public override void Act(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }
}
