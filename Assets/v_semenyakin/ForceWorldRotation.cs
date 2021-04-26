using UnityEngine;

public class ForceWorldRotation : MonoBehaviour
{
    private void FixedUpdate() {
        Quaternion currentRotation = transform.rotation;
        Vector3 currentEulerAngles = currentRotation.eulerAngles;

        if (_ignoreX)
            currentEulerAngles.x = _rotationToForce.x;
        if (_ignoreY)
            currentEulerAngles.y = _rotationToForce.y;
        if (_ignoreZ)
            currentEulerAngles.z = _rotationToForce.z;
    }

    [SerializeField] Vector3 _rotationToForce;
    [SerializeField] bool _ignoreX = false;
    [SerializeField] bool _ignoreY = false;
    [SerializeField] bool _ignoreZ = false;
}
