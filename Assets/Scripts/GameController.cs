using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _offset = 0;
    
    [SerializeField] private Ladder _ladder;

    private void Update()
    {
        _ladder.TestSetZPosition(_offset + transform.position.y);
    }
}
