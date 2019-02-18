using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class StateController : MonoBehaviour {

    public State currentState;                                  // The current objective of the guard.
    public State remainState;                                   // Placeholder for an empty objective.
    public FieldOfView eyes;                                    // The script of whatever object forms the guard's line of sight.
    public Color patrolSpotColor;                               // Color of the guard's patrol spots
    public Transform patrolSpots;                               // The child object that holds all of its patrol spots.
    [RangeAttribute(0.4f, 10f)] public float patrolSpotSize;    // Size of the patrol spots, ranges from 0.4 to 10 units. (In editor only).
    public float minSoundDistance;                              // How far should the guard be to the player before the electricty can't be heard.
    public float maxSoundDistance;                              // How close should the guard be to the player before the electricity is max volume.

    [HideInInspector] public NavMeshAgent navMeshAgent;         // The NavMeshAgent component
    [HideInInspector] public List<Vector3> wayPointList;        // The list of points for the guard to patrol around (Set in Unity)
    [HideInInspector] public int nextWayPoint;                  // The number of the next point to patrol to
    [HideInInspector] public Transform chaseTarget;             // The transform of the object to chase (when chasing)

    private bool isAiActive;                                    // Is the navigation system on
    private AudioSource audioSource;
    private GameObject playerRef;

    private void Awake()
    {
        // In here we grab the NavMeshAgent and store every patrol point (Taged with "PatrolSpot")
        navMeshAgent = GetComponent<NavMeshAgent>();
        wayPointList = new List<Vector3>();
        isAiActive = true;

        /*for (int i = 0; i < wayPoints.Length; i++)
            wayPointList.Add(wayPoints[i].transform);*/

        for (int i = 0; i < patrolSpots.childCount; i++)
            if (patrolSpots.GetChild(i).tag.Equals("PatrolSpot"))
                wayPointList.Add(patrolSpots.GetChild(i).position);
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerRef = GameObject.FindGameObjectWithTag("Player");
    }

    void Update () {
        float distToPlayer = Vector3.Distance(transform.position, playerRef.transform.position);

        audioSource.volume = LinMap(distToPlayer);

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

    private float LinMap(float value)
    {
        return (value - minSoundDistance) / (maxSoundDistance - minSoundDistance) * (0f - 1f) + 1f;
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

    private void OnDrawGizmos()
    {
        /**
         * 1. Find all the patrol spots (taged with "PatrolSpot")
         * 2. Create a sphere in the editor with the size and color you give it in the inspector.
         */
        Gizmos.color = patrolSpotColor;

        if(patrolSpots != null)
        {
            for (int i = 0; i < patrolSpots.childCount; i++)
                if (patrolSpots.GetChild(i).tag.Equals("PatrolSpot"))
                    Gizmos.DrawSphere(patrolSpots.GetChild(i).position, patrolSpotSize);
        }
    }
}
