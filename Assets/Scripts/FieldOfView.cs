using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour {

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
