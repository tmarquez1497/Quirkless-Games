using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

    /** NOTE:
     * This script should be attached to the viewcone.
     * Anytime the player enters the cone the boolean will say it can see him.
     * Once the player leaves, the boolean will switch back.
     */

    [HideInInspector] public bool canSeePlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canSeePlayer = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canSeePlayer = false;
    }
}
