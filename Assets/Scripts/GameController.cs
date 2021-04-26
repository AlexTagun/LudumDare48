using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    [SerializeField] private Ladder _ladder;
    [SerializeField] private Transform _followCamera;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private GameObject _loseWindow;

    [SerializeField] private float _spawnStep;

    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ItemSpawner _itemSpawner;

    [SerializeField] private float _spawnOffset = 20f;

    [SerializeField] private List<Vector3> _firstRandomPoints;
    [SerializeField] private List<Vector3> _secondRandomPoints;

    public static int CurrentLevel = 0;
    public static int CurHeroCount = 0;

    private void Start() {
        EventManager.OnHpEnded += OnHeroDie;
    }

    private void Update()
    {
        _ladder.TestSetZPosition(_followCamera.position.y);
        
        TrySpawn();
    }

    private void TrySpawn()
    {
        if (_followCamera.position.y > GetNextSpawnPosition() + _spawnOffset)
        {
            return;
        }
        
        var drop = _dropGenerator.GetDropInfo(CurrentLevel);
        
        var stepPosition = GetStepPosition();
        var spawnOffset = drop?.SpawnOffset ?? Vector3.zero;
        
        stepPosition += spawnOffset;

        var randPosition = GetRandomPosition(true);
        stepPosition += randPosition;
        
        CurrentLevel++;
        _levelText.text = (Math.Max(CurrentLevel - 3, 0)).ToString();

        if (drop == null)
        {
            return;
        }
        
        _itemSpawner.SpawnObject(drop, stepPosition);
    }

    private float GetNextSpawnPosition()
    {
        return CurrentLevel * _spawnStep;
    }

    private Vector3 GetStepPosition()
    {
        return new Vector3(transform.position.x, GetNextSpawnPosition(), transform.position.z);
    }

    private Vector3 GetRandomPosition(bool isFirst)
    {
        var randIndex = 0;
        
        if (isFirst)
        {
            randIndex = Random.Range(0, _firstRandomPoints.Count);
            return _firstRandomPoints[randIndex];
        }

        randIndex = Random.Range(0, _secondRandomPoints.Count);
        return _secondRandomPoints[randIndex];
    }

    private void OnHeroDie(Hero hero) {
        if (CurHeroCount <= 1) {
            _loseWindow.SetActive(true);
        }
    }

    private void OnDestroy() {
        EventManager.OnHpEnded -= OnHeroDie;
    }
}
