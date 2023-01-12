using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAreaController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        GameEvents.current.OnPersonOverflown();
    }
}
