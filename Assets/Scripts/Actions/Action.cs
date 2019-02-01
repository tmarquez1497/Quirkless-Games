using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script is a template for all the guard objectives.
 * They specifically tell the guard what to do while the game is running.
 */

public abstract class Action : ScriptableObject {
    public abstract void Act(StateController controller);
}
