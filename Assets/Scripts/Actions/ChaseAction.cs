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
        Vector3 dest = new Vector3(controller.chaseTarget.position.x, controller.transform.position.y, controller.chaseTarget.position.z);

        controller.navMeshAgent.isStopped = true;
        controller.navMeshAgent.destination = dest;
        controller.navMeshAgent.isStopped = false;
    }
}
