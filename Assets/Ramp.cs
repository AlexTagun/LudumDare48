using UnityEngine;
public class Ramp : MonoBehaviour
{
    public void placeLocal(Vector3 inBasePointA, Vector3 inBasePointB, float inWidth, float inHeight)
    {
        Vector3 inBaseDirection = inBasePointB - inBasePointA;

        Vector3 inWidthNormal = Vector3.Cross(inBaseDirection, Vector3.up).normalized;
        Vector3 inHeightNormal = Vector3.Cross(inWidthNormal, inBaseDirection).normalized;

        transform.localPosition = inBasePointA -
            (inHeightNormal * inHeight / 2f) +
            (inBaseDirection / 2f) +
            (inWidthNormal * inWidth / 2f);
        transform.localRotation = Quaternion.LookRotation(inBaseDirection);
        transform.localScale = new Vector3(inWidth, inHeight, inBaseDirection.magnitude);
    }
}
