using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : MonoBehaviour {
    [SerializeField] private Button restartBtn;

    private void Awake() {
        restartBtn.onClick.AddListener(() => {
            GameController.CurrentLevel = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }
}