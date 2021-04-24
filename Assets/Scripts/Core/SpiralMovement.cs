using System.Collections.Generic;
using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    private const float Radius = 7.1f;
    
    private static List<float> Angles = new List<float>
    {
        230f,
        250f,
        270f,
        290f,
        310f,
        330f,
        350f
    };
    
    [SerializeField] private Transform _center;
    
    public Vector3 GetStartPosition(int index)
    {
        var angle = Angles[index];

        var centerPosition = _center.position;
        var centerPositionNoY = new Vector3(centerPosition.x, 0f, centerPosition.z);
        
        var x = centerPositionNoY.x +  Radius * Mathf.Cos(angle * Mathf.Deg2Rad);
        var z = centerPositionNoY.z + Radius * Mathf.Sin(angle * Mathf.Deg2Rad);
        
        return new Vector3(x, 0f, z);
    }

    public Vector3 GetNextDirection(Vector3 currentPosition, float speed, float deltaTime)
    {
        var nextDirection = GetNextDirection(currentPosition);
        
        var currentPositionNoY = new Vector3(currentPosition.x, 0f, currentPosition.z);

        var predictedPositionNoY = currentPositionNoY + nextDirection * speed * deltaTime;
        
        var centerPositionNoY = new Vector3(_center.position.x, 0f, _center.position.z);
        var nextPositionNoY = (predictedPositionNoY - centerPositionNoY).normalized * Radius;
        
        //-1 то костыль
        nextDirection = (-1) * (nextPositionNoY - currentPositionNoY).normalized;
        
        return nextDirection;
    }
    
    private Vector3 GetNextDirection(Vector3 currentPosition)
    {
        var centerPositionNoY = new Vector3(_center.position.x, 0f, _center.position.z);
        var currentPositionNoY = new Vector3(currentPosition.x, 0f, currentPosition.z);

        var direction = (currentPositionNoY - centerPositionNoY).normalized;

        var nextDirection =  Vector3.Cross(direction, Vector3.up).normalized;
        
        return nextDirection;
    }
}
