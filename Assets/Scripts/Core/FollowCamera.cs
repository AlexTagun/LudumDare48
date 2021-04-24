using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Transform _center;
    [SerializeField] private Transform _targetForY;

    [SerializeField] private float _speed = 10f;

    private void Update()
    {
        UpdateRotation();
    }

    private void LateUpdate()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        transform.position = GetTargetPosition();
    }

    private void UpdateRotation()
    {
        if (Input.GetMouseButton(0))
        {
            transform.eulerAngles += _speed * new Vector3(0f, Input.GetAxis("Mouse X"), 0f);
        }
    }
    
    private Vector3 GetTargetPosition()
    {
        return new Vector3(_center.position.x, _targetForY.position.y, _center.position.z);
    }
}
