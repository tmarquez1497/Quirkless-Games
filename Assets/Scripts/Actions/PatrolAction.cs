using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script holds the patrolling objective.
 * It tells the guard to move linearly from one patrol spot to the next.
 * It will stand still if there is one patrol spot.
 * It will throw an error if there are no patrol spots.
 */

[CreateAssetMenu(menuName = "GuardController/Actions/Patrol")]
public class PatrolAction : Action {
    public override void Act(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.navMeshAgent.destination = controller.wayPointList[controller.nextWayPoint].position;
        controller.navMeshAgent.isStopped = false;

        if (controller.navMeshAgent.remainingDistance <= controller.navMeshAgent.stoppingDistance && !controller.navMeshAgent.pathPending)
            controller.nextWayPoint = (++controller.nextWayPoint) % controller.wayPointList.Count;
    }
}
