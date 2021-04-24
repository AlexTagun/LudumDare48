using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager {
    public static Action OnItemSwapped;

    public static void HandleOnItemSwapped() {
        OnItemSwapped?.Invoke();
    }
}