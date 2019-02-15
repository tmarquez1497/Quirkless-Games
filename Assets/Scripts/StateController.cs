using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

    public State currentState;                                  // The current objective of the guard.
    public State remainState;                                   // Placeholder for an empty objective.
    public FieldOfView eyes;                                    // The script of whatever object forms the guard's line of sight.

    [HideInInspector] public NavMeshAgent navMeshAgent;         // The NavMeshAgent component
    [HideInInspector] public List<Transform> wayPointList;      // The list of points for the guard to patrol around (Set in Unity)
    [HideInInspector] public int nextWayPoint;                  // The number of the next point to patrol to
    [HideInInspector] public Transform chaseTarget;             // The transform of the object to chase (when chasing)

    private bool isAiActive;                                    // Is the navigation system on
    private GameManager gameManager = null;                     // Reference to the GameManger script

    private void Awake()
    {
        // In here we grab the NavMeshAgent and store every patrol point (Taged with "PatrolSpot")
        navMeshAgent = GetComponent<NavMeshAgent>();
        GameObject[] wayPoints = GameObject.FindGameObjectsWithTag("PatrolSpot");
        wayPointList = new List<Transform>();
        isAiActive = true;
        if (GameObject.Find("GameManager") != null)
            gameManager = GameManager.instance;

        for (int i = 0; i < wayPoints.Length; i++)
            wayPointList.Add(wayPoints[i].transform);
    }

    void Update () {
        // If the GameManager exists and the game isn't paused . . .
        if (gameManager == null || !gameManager.isPaused)
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
                gameManager.GameOver();
                break;
            default:
                break;
        }
    }
}
