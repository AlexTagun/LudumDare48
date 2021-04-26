using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoseWindow : MonoBehaviour {
    [SerializeField] private Button restartBtn;
    [SerializeField] private TextMeshProUGUI coins;

    private void Awake() {
        coins.text = GameController.CollectedCoinsCount.ToString();
        
        restartBtn.onClick.AddListener(() => {
            GameController.CurrentLevel = 0;
            GameController.CollectedCoinsCount = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        StartCoroutine(Rebuild());
    }

    private IEnumerator Rebuild() {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(coins.transform.parent as RectTransform);
    }
}