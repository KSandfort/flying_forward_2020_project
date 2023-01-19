using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTargetArea : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.OnTargetAreaReached();
    }
}
