using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    
    public static GameEvents current;

    private void Awake() {
        current = this;
    }

    public event Action onPersonOverflown;
    public event Action onTargetAreaReached;
    
    public void OnPersonOverflown() {
        if (onPersonOverflown != null) {
            onPersonOverflown();
        }
    }

    public void OnTargetAreaReached() {
        if (onTargetAreaReached != null) {
            onTargetAreaReached();
        }
    }
}
