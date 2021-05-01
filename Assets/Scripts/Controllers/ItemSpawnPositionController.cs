using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnPositionController : MonoBehaviour
{

    [SerializeField] private float _spawnStep;
    
    [SerializeField] private List<Vector3> _firstRandomPoints;
    [SerializeField] private List<Vector3> _secondRandomPoints;

    public Vector3 GetSpawnPosition(Vector3? dropSpawnOffset, bool isFirstPoint)
    {
        var randPositions = isFirstPoint ? _firstRandomPoints : _secondRandomPoints;
        
        var spawnPosition = GetStepPosition();
        var spawnOffset = dropSpawnOffset ?? Vector3.zero;
        
        spawnPosition += spawnOffset;

        var randPosition = GetRandomPosition(randPositions);
        
        spawnPosition += randPosition;

        return spawnPosition;
    }
    
    private Vector3 GetStepPosition()
    {
        return new Vector3(transform.position.x, GetNextSpawnPosition(), transform.position.z);
    }
    
    public float GetNextSpawnPosition()
    {
        return LadderLevelManager.CurrentLevel * _spawnStep;
    }
    
    private Vector3 GetRandomPosition(List<Vector3> randPositions)
    {
        var randIndex = Random.Range(0, randPositions.Count);
        
        return randPositions[randIndex];
    }
}
