using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * This script holds the skeleton for the transitions from one objective to another. 
 */

[System.Serializable]
public class Transition {
    public Decision decision;
    public State trueState;
    public State falseState;
}
