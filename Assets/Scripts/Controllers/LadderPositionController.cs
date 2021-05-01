using UnityEngine;

public class LadderPositionController : MonoBehaviour
{
    [SerializeField] private Ladder _ladder;
    
    [SerializeField] private Transform _followCamera;

    private void Update()
    {
        _ladder.TestSetZPosition(_followCamera.position.y);
    }
}
