using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _targetForY;

    [SerializeField] private float _manualSpeed = 10f;
    
    [SerializeField] private float _autoRotationSpeed = 2f;

    [SerializeField] private float _startAutoFollowTime = 2f;

    private float _currentIdleTime = 0f;

    private void UpdateRotation(float delta)
    {
        if (TryUpdateManualRotation())
        {
            _currentIdleTime = 0;
            return;
        }

        if (IsAutoControl())
        {
            UpdateAutoRotation(delta);
        }
        
        _currentIdleTime += delta;
    }

    private bool IsAutoControl()
    {
        return _currentIdleTime > _startAutoFollowTime;
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
        UpdateRotation(Time.deltaTime);
    }

    private void UpdateCameraPosition()
    {
        transform.position = GetTargetPosition();
    }

    private bool TryUpdateManualRotation()
    {
        if (!Input.GetMouseButton(0))
        {
            return false;
        }
        
        transform.eulerAngles += _manualSpeed * new Vector3(0f, Input.GetAxis("Mouse X"), 0f);

        return true;
    }

    private void UpdateAutoRotation(float deltaTime)
    {
        var targetNoY = new Vector3(_targetForY.position.x, 0f, _targetForY.position.z);
        var centerNoY = new Vector3(_center.position.x, 0f, _center.position.z);
        var needDirection = (targetNoY - centerNoY).normalized;
        var needAngle = Mathf.Atan2(needDirection.x, needDirection.z) * Mathf.Rad2Deg;
        Quaternion needRotation = Quaternion.Euler(new Vector3(0, needAngle, 0));

        transform.rotation = Quaternion.Slerp(transform.rotation, needRotation, _autoRotationSpeed * deltaTime);
    }
    
    private Vector3 GetTargetPosition()
    {
        return new Vector3(_center.position.x, _targetForY.position.y, _center.position.z);
    }
}
