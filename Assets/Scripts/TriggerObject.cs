using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour
{
    public UnityEngine.Events.UnityEvent<Collider> onEnter;
    public UnityEngine.Events.UnityEvent<Collider> onExit;

    private void OnTriggerEnter(Collider other) {
    }

    private void OnTriggerExit(Collider other) {
    }
}
