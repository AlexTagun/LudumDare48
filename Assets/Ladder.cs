using UnityEngine;

public class Ladder : MonoBehaviour
{
    private void Start() {
        updateLadder();
    }
    private void updateLadder() {
        float anglePerStep = 360f / _stepsInCircle;
        float zAngleRadians = _zAngle * Mathf.Deg2Rad;

        Quaternion stepRotator = Quaternion.Euler(0f, anglePerStep, 0f);

        float stepSize = _radius * Mathf.Sin(anglePerStep / 2f) * 2;
        float deltaZ = stepSize * Mathf.Sin(zAngleRadians / 2f);

        //TODO: Make height-based
        Vector3 stepZPoint = Vector3.zero;
        Vector3 stepPointDirection = Vector3.right;
       
        for (int stepIndex = 0; stepIndex < _stepsNum; ++stepIndex) {

            Vector3 nextStepPointDirection = stepRotator * stepPointDirection;

            Vector3 stepNormal = (nextStepPointDirection - stepPointDirection).normalized;
            Vector3 additionalStepSizeDelta = stepNormal * _additionalStepSize * 0.5f;

            Vector3 stepPoint =
                stepZPoint +
                stepPointDirection * _radius -
                additionalStepSizeDelta;

            stepZPoint.y -= deltaZ;
            Vector3 nextStepPoint =
                stepZPoint +
                nextStepPointDirection * _radius +
                additionalStepSizeDelta;

            placeRampStep(stepPoint, nextStepPoint, _stepWidth, _stepHeight);

            stepPointDirection = nextStepPointDirection;
        }
    }

    void placeRampStep(Vector3 inPoint, Vector3 inNextPoint, float inWidth, float inHeight) {
        Ramp rampObject = Instantiate(_rampPrefab, transform);
        rampObject.placeLocal(inPoint, inNextPoint, inWidth, inHeight);
    }

    [SerializeField] float _radius = 5f;
    [SerializeField] float _stepWidth = 4f;
    [SerializeField] float _stepHeight = 1f;
    [SerializeField] float _zAngle = 10;
    [SerializeField] int _stepsInCircle = 10;
    [SerializeField] int _stepsNum = 100;
    [SerializeField] float _additionalStepSize = 0f;

    [SerializeField] Ramp _rampPrefab = null;
}
