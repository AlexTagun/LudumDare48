using System;
using TMPro;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Ladder _ladder;
    [SerializeField] private Transform _followCamera;
    [SerializeField] private TextMeshProUGUI _levelText;

    [SerializeField] private float _spawnStep;

    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ItemSpawner _itemSpawner;

    [SerializeField] private float _startSpawnPosition;
    [SerializeField] private float _offsetSpawnPosition = 0;
    private float _lastSpawnPosition;
    public static int CurrentLevel = 0;

    private void Awake()
    {
        _lastSpawnPosition = _startSpawnPosition;
    }

    private void Update()
    {
        _ladder.TestSetZPosition(_followCamera.position.y);
        
        TrySpawn();
    }

    private void TrySpawn()
    {
        var nextSpawnPosition = GetNextSpawnPosition();

        if (_followCamera.position.y > nextSpawnPosition)
        {
            return;
        }
        
        var drop = _dropGenerator.GetDropInfo(CurrentLevel);
        
        var stepPosition = GetStepPosition();
        var spawnOffset = drop?.SpawnOffset ?? Vector3.zero;
        
        stepPosition += spawnOffset;
        
        _lastSpawnPosition = nextSpawnPosition;
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
        return _lastSpawnPosition + _spawnStep;
    }

    private Vector3 GetStepPosition()
    {
        return new Vector3(transform.position.x, GetNextSpawnPosition() + _offsetSpawnPosition, transform.position.z);
    }
}
