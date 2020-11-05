using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public Action<string> GoalScored;

    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Ball")
        {
            if (GoalScored != null)
            {
                GoalScored(transform.tag);
              
            }
        }
    }
}
