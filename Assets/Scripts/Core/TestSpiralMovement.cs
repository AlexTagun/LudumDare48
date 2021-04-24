using UnityEngine;

public class TestSpiralMovement : MonoBehaviour
{
    [SerializeField] private Transform _center;

    [SerializeField] private float _distanceFromCenter = 10f;

    [SerializeField] private float _ySpeed = 0.1f;

    [SerializeField] private float _rotationSpeed = 10f;

    private Quaternion _currentRotation = Quaternion.identity;

    private void Update()
    {
        var currentPosition = transform.position;
        
        _currentRotation.eulerAngles += new Vector3(0f, _rotationSpeed, 0f);

        var centerPositionNoY = new Vector3(_center.position.x, 0f, _center.position.z);

        Vector3 rotationDirection = _currentRotation * Vector3.forward;
        
        var newPositionNoY = centerPositionNoY + rotationDirection * _distanceFromCenter;

        var newY = currentPosition.y += _ySpeed * Time.deltaTime;
        
        
        var newPosition = new  Vector3(newPositionNoY.x, newY, newPositionNoY.z);
        transform.position = newPosition;
    }
}
