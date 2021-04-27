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

    [SerializeField] private float _spawnStep;

    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ItemSpawner _itemSpawner;

    [SerializeField] private float _spawnOffset = 20f;

    [SerializeField] private List<Vector3> _firstRandomPoints;
    [SerializeField] private List<Vector3> _secondRandomPoints;

    [SerializeField] private Transform _center;
    
    

    public static int CurrentLevel = 0;
    public static int CurHeroCount = 0;
    public static int CollectedCoinsCount = 0;

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
        
        RollDrop(_firstRandomPoints, true);
        RollDrop(_secondRandomPoints, false);
        
        CurrentLevel++;
        _levelText.text = (GetDisplayCurrentLevel()).ToString();
        TrySpeedUpHeroes(CurrentLevel);
    }

    public static int GetDisplayCurrentLevel()
    {
        return Math.Max(CurrentLevel - 3, 0);
    }

    private float GetNextSpawnPosition()
    {
        return CurrentLevel * _spawnStep;
    }

    private Vector3 GetStepPosition()
    {
        return new Vector3(transform.position.x, GetNextSpawnPosition(), transform.position.z);
    }

    private void RollDrop(List<Vector3> randPositions, bool isFirst)
    {
        var drop = _dropGenerator.GetDropInfo(CurrentLevel);
        
        var stepPosition = GetStepPosition();
        var spawnOffset = drop?.SpawnOffset ?? Vector3.zero;
        
        stepPosition += spawnOffset;

        Vector3 randPosition = Vector3.zero;
        randPosition = GetRandomPosition(randPositions);
        
        stepPosition += randPosition;
        

        if (drop == null)
        {
            return;
        }

        var spawnGo =  _itemSpawner.SpawnObject(drop, stepPosition);

        var spawnRotator = spawnGo.GetComponent<SpawnRotator>();
        
        if (spawnRotator != null)
        {
            spawnGo.transform.forward = GetRotationForward(spawnGo.transform.position,spawnRotator.IsPerpendicular);
        }
    }

    private Vector3 GetRandomPosition(List<Vector3> randPositions)
    {
        var randIndex = Random.Range(0, randPositions.Count);
        
        return randPositions[randIndex];
    }

    private Vector3 GetRotationForward(Vector3 currentPosition, bool isPerpendicular)
    {
        var centerNoY = new Vector3(_center.position.x, 0f, _center.position.z);
        var currentPositionNoY = new Vector3(currentPosition.x, 0f, currentPosition.z);

        var directionToCenter = (centerNoY - currentPositionNoY).normalized;

        if (isPerpendicular)
        {
            return (-1) * directionToCenter;
        }
        
        var targetDirection = Vector3.Cross(directionToCenter, Vector3.up).normalized;
        return targetDirection;
    }

    private void TrySpeedUpHeroes(int currentLevel)
    {
        if (!InventoryManager.Instance.IsHaveHero())
        {
            return;
        }

        if (5 <= currentLevel && currentLevel < 10)
        {
            InventoryManager.Instance.SetSpeedAllHeroes(6);
        }else if (10 <= currentLevel && currentLevel < 15)
        {
            InventoryManager.Instance.SetSpeedAllHeroes(7);
        }
        else if (15 <= currentLevel && currentLevel < 20)
        {
            InventoryManager.Instance.SetSpeedAllHeroes(8);
        }else if (20 <= currentLevel && currentLevel < 25)
        {
            InventoryManager.Instance.SetSpeedAllHeroes(9);
        }
        else if (25 <= currentLevel)
        {
            InventoryManager.Instance.SetSpeedAllHeroes(10);
        }
    }
}
