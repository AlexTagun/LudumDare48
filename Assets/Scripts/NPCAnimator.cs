using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimator : MonoBehaviour {
    [SerializeField] private Animator _animator = null;

    private void Awake() {
        Run();
    }

    public void Run() {
        _animator.Play("run");
    }
}
