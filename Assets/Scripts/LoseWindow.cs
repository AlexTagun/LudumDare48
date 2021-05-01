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
    [SerializeField] private TextMeshProUGUI currentFloorText;
    [SerializeField] private TextMeshProUGUI maxFloorText;
    [SerializeField] private TextMeshProUGUI newRecordText;
    
    private const string MaxFloorResult = "MaxFloorResult";

    private void Awake() {
        coins.text = GameController.CollectedCoinsCount.ToString();

        var currentFloor = LadderLevelManager.GetDisplayCurrentLevel();

        var oldMaxFloor = PlayerPrefs.GetInt(MaxFloorResult, 0);
        var isNewRecord = currentFloor > oldMaxFloor;
        
        var maxFloor = Mathf.Max(currentFloor, oldMaxFloor);
        PlayerPrefs.SetInt(MaxFloorResult, maxFloor);
        PlayerPrefs.Save();

        currentFloorText.text = currentFloor.ToString();

        if (isNewRecord)
        {
            maxFloorText.transform.parent.gameObject.SetActive(false);
            newRecordText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            maxFloorText.text = maxFloor.ToString();
            maxFloorText.transform.parent.gameObject.SetActive(true);
            newRecordText.transform.parent.gameObject.SetActive(false);
        }
        
        restartBtn.onClick.AddListener(() => {
            LadderLevelManager.Clear();
            GameController.CollectedCoinsCount = 0;
            GameController.CurHeroCount = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });

        StartCoroutine(Rebuild());
    }

    private IEnumerator Rebuild() {
        yield return null;
        LayoutRebuilder.ForceRebuildLayoutImmediate(coins.transform.parent as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(currentFloorText.transform.parent as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(maxFloorText.transform.parent as RectTransform);
        LayoutRebuilder.ForceRebuildLayoutImmediate(newRecordText.transform.parent as RectTransform);
    }
}