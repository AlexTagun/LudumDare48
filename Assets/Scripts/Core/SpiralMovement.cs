using UnityEngine;

public class SpiralMovement : MonoBehaviour
{
    [SerializeField] private Transform _center;

    [SerializeField] private float _distanceFromCenter = 10f;

    [SerializeField] private float _ySpeed = 0.1f;

    [SerializeField] private float _rotationSpeed = 10f;

    [SerializeField] private float _radius = 10f;

    private Quaternion _currentRotation = Quaternion.identity;

    public Vector3 GetDirection(Vector3 currentPosition)
    {
        _currentRotation.eulerAngles += new Vector3(0f, _rotationSpeed, 0f);

        var centerPositionNoY = new Vector3(_center.position.x, 0f, _center.position.z);

        var rotationDirection = _currentRotation * Vector3.forward;
        
        var newPositionNoY = centerPositionNoY + rotationDirection * _distanceFromCenter;

        var newY = currentPosition.y += _ySpeed * Time.deltaTime;
        
        
        var newPosition = new  Vector3(newPositionNoY.x, newY, newPositionNoY.z);

        var newDirection = (newPosition - currentPosition).normalized;

        return newDirection;
    }
}
