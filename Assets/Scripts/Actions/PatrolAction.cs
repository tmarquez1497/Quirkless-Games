using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
