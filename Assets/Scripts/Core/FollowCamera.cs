using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowCamera : MonoBehaviour
{
    private const float MinDeltaAnglesMagnitude = 1f; 
    
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _targetForY;

    [SerializeField] private float _manualSpeed = 10f;
    
    [SerializeField] private float _startAutoFollowTime = 2f;

    [SerializeField] private float _autoReturnTime = 0.5f;
    
    private float _currentIdleTime = 0f;

    private float _currentAutoReturnTime;
    private Quaternion? _oldRotation;

    private void Start() {
        EventManager.OnHpEnded += hero => {
            _targetForY = InventoryManager.Instance.GetFirstHero();
        };
    }

    private void UpdateRotation(float delta)
    {
        if (TryUpdateManualRotation() && !EventManager.IsDragging)
        {
            _currentIdleTime = 0;
            _currentAutoReturnTime = 0f;
            _oldRotation = null;
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
        UpdateRotation(Time.fixedDeltaTime);
    }

    private void UpdateCameraPosition()
    {
        transform.position = GetTargetPosition();
    }

    private bool TryUpdateManualRotation() {

        if (!Input.GetMouseButton(0))
        {
            return false;
        }
        
        transform.eulerAngles += _manualSpeed * new Vector3(0f, Input.GetAxis("Mouse X"), 0f);

        return true;
    }

    private void UpdateAutoRotation(float deltaTime)
    {
        if (_oldRotation == null)
        {
            _oldRotation = transform.rotation;
        }
        
        var targetNoY = new Vector3(_targetForY.position.x, 0f, _targetForY.position.z);
        var centerNoY = new Vector3(_center.position.x, 0f, _center.position.z);
        var needDirection = (targetNoY - centerNoY).normalized;
        var needAngle = Mathf.Atan2(needDirection.x, needDirection.z) * Mathf.Rad2Deg;
        Quaternion needRotation = Quaternion.Euler(new Vector3(0, needAngle, 0));

        
        var deltaAnglesMagnitude = Mathf.Abs((transform.rotation.eulerAngles - needRotation.eulerAngles).magnitude);
        
        if (deltaAnglesMagnitude > MinDeltaAnglesMagnitude)
        {
            transform.rotation = Quaternion.Slerp(_oldRotation.Value, needRotation, _currentAutoReturnTime / _autoReturnTime);
        }
        else
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, needRotation, 1f);
        }

        _currentAutoReturnTime += deltaTime;
    }
    
    private Vector3 GetTargetPosition()
    {
        return new Vector3(_center.position.x, _targetForY.position.y, _center.position.z);
    }
}
