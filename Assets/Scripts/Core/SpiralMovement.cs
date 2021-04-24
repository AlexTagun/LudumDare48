using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    [SerializeField] private Transform _center;

    public Vector3 GetNextDirection(Vector3 currentPosition)
    {
        var centerPositionNoY = new Vector3(_center.position.x, 0f, _center.position.z);
        var currentPositionNoY = new Vector3(currentPosition.x, 0f, currentPosition.z);

        var direction = (currentPositionNoY - centerPositionNoY).normalized;

        var nextDirection =  Vector3.Cross(direction, Vector3.up).normalized;
        
        return nextDirection;
    }
}
