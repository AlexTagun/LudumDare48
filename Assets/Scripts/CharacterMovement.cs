using UnityEngine;

public enum Direction
{
    Left = -1,
    Right = 1
}

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody _body;
    
    [SerializeField] private float Speed = 5f;
    
    [SerializeField] private Vector3 _input = Vector3.zero;
    
    [SerializeField] private SpiralMovement _spiralMovement;
    
    [SerializeField] private Direction _direction;

    [SerializeField] private int Index;

    private void Awake()
    {
        _body.position = _spiralMovement.GetStartPosition(Index);
    }

    private void FixedUpdate()
    {
        var deltaTime = Time.fixedDeltaTime;
        
        _input = _spiralMovement.GetNextDirection(_body.position, Speed, deltaTime, _direction);
        
        if (_input == Vector3.zero)
        {
            return;
        }
        
        _body.transform.forward = _input;
        
        var directionSpeed = (int) _direction * Speed;


        var deltaPosition = _input * directionSpeed * deltaTime;
        
        _body.MovePosition(_body.position + deltaPosition);
    }
}
