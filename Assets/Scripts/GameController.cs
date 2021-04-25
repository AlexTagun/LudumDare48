using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _offset = 0;
    
    [SerializeField] private Ladder _ladder;
    [SerializeField] private Transform _followCamera;

    [SerializeField] private float _spawnStep;

    [SerializeField] private DropGenerator _dropGenerator;
    [SerializeField] private ItemSpawner _itemSpawner;
    
    private float _lastSpawnPosition = 0;
    private int _currentDropIndex = 0;

    private void Update()
    {
        _ladder.TestSetZPosition(_offset + _followCamera.position.y);
        
        TrySpawn();
    }

    private void TrySpawn()
    {
        var nextSpawnPosition = GetNextSpawnPosition();

        if (_followCamera.position.y > nextSpawnPosition)
        {
            return;
        }

        var stepPosition = GetStepPosition();
        
        var drop = _dropGenerator.GetDropInfo(_currentDropIndex);
        
        _lastSpawnPosition = nextSpawnPosition;
        _currentDropIndex++;

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
        return new Vector3(transform.position.x, GetNextSpawnPosition(), transform.position.z);
    }
}
