using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GuardController/Actions/Chase")]
public class ChaseAction : Action {
    public override void Act(StateController controller)
    {
        controller.navMeshAgent.isStopped = true;
        controller.navMeshAgent.destination = controller.chaseTarget.position;
        controller.navMeshAgent.isStopped = false;
    }
}
