using UnityEngine;

public class SpawnRotator : MonoBehaviour
{
    [SerializeField] private bool IsPerpendicular;

    public void RotateForward(Vector3 centerPosition)
    {
        transform.forward = GetRotationForward(centerPosition, transform.position);
    }
    
    private Vector3 GetRotationForward(Vector3 centerPosition, Vector3 currentPosition)
    {
        var centerNoY = new Vector3(centerPosition.x, 0f, centerPosition.z);
        var currentPositionNoY = new Vector3(currentPosition.x, 0f, currentPosition.z);

        var directionToCenter = (centerNoY - currentPositionNoY).normalized;

        if (IsPerpendicular)
        {
            return (-1) * directionToCenter;
        }
        
        var targetDirection = Vector3.Cross(directionToCenter, Vector3.up).normalized;
        return targetDirection;
    }
}
