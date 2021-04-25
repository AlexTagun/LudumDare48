using UnityEngine;

public class HintProjectile : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        UpdateRotation();
    }

    private void UpdateRotation()
    {
        transform.LookAt(Camera.main.transform);
    }
}
