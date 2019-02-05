using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This scipt is a template for all decision assets used by the guards.
 * Decision assets are scipts that check to see if the objective needs to change.
 */

public abstract class Decision : ScriptableObject {
    public abstract bool Decide(StateController controller);
}
