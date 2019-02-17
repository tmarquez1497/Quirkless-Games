using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

    public State currentState;                                  // The current objective of the guard.
    public State remainState;                                   // Placeholder for an empty objective.
    public FieldOfView eyes;                                    // The script of whatever object forms the guard's line of sight.
    public Color spotColor;                                     // Color of the guard's patrol spots
    [RangeAttribute(0.4f, 10f)] public float spotSize;            // Size of the patrol spots, ranges from 0 to 10 units. (In editor only).

    [HideInInspector] public NavMeshAgent navMeshAgent;         // The NavMeshAgent component
    [HideInInspector] public List<Vector3> wayPointList;      // The list of points for the guard to patrol around (Set in Unity)
    [HideInInspector] public int nextWayPoint;                  // The number of the next point to patrol to
    [HideInInspector] public Transform chaseTarget;             // The transform of the object to chase (when chasing)

    private bool isAiActive;                                    // Is the navigation system on
    //private GameManager gameManager = null;                     // Reference to the GameManger script

    private void Awake()
    {
        // In here we grab the NavMeshAgent and store every patrol point (Taged with "PatrolSpot")
        navMeshAgent = GetComponent<NavMeshAgent>();
        wayPointList = new List<Vector3>();
        isAiActive = true;

        /*for (int i = 0; i < wayPoints.Length; i++)
            wayPointList.Add(wayPoints[i].transform);*/

        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).tag.Equals("PatrolSpot"))
                wayPointList.Add(transform.GetChild(i).position);
    }

    void Update () {
        // If the GameManager exists and the game isn't paused . . .
        if (GameManager.instance == null || !GameManager.instance.isPaused)
        {
            // If the navigation system is not on, do nothing.
            if (!isAiActive)
                return;

            // Otherwise, keep performing the current objective.
            currentState.UpdateState(this);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            navMeshAgent.isStopped = !navMeshAgent.isStopped;
	}

    public void TransitionToState(State nextState)
    {
        // If the next objective is not empty, change to the new objective.
        if (nextState != remainState)
        {
            OnExitState(currentState, nextState);
            currentState = nextState;
        }
    }

    private void OnExitState(State current, State next)
    {
        switch (current.name)
        {
            case "GuardPatrol":
                GameManager.instance.GameOver();
                break;
            default:
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        /**
         * 1. Find all the patrol spots (taged with "PatrolSpot")
         * 2. Create a sphere in the editor with the size and color you give it in the inspector.
         */
        Gizmos.color = spotColor;

        for (int i = 0; i < transform.childCount; i++)
            if (transform.GetChild(i).tag.Equals("PatrolSpot"))
                Gizmos.DrawSphere(transform.GetChild(i).position, spotSize);
    }
}
