using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;          // There can only be one copy of this gameobject. This stores the copy.

    public Color spotColor;                             // Color of all patrol spots.
    [RangeAttribute(0f, 10f)] public float spotSize;    // Size of the patrol spots, ranges from 0 to 10 units. (In editor only).

    private void Awake()
    {
        /*
         If there is no other copy of the gameobject, this becomes that copy.
         If there already is one, destroy this extra copy.
         */
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        // Tell Unity to keep this gameobject when loading and unloading scenes.
        DontDestroyOnLoad(gameObject);
    }

    private void OnDrawGizmos()
    {
        /**
         * 1. Find all the patrol spots (taged with "PatrolSpot")
         * 2. Create a circle in the editor with the size and color you give it in the inspector.
         */
        GameObject[] points = GameObject.FindGameObjectsWithTag("PatrolSpot");

        Gizmos.color = spotColor;
        for (int i = 0; i < points.Length; i++)
            Gizmos.DrawSphere(points[i].transform.position, spotSize);
    }
}
