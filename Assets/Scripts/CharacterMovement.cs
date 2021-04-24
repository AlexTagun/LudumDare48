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

    private void FixedUpdate()
    {
        _input = _spiralMovement.GetNextDirection(transform.position);
        
        if (_input == Vector3.zero)
        {
            return;
        }
        
        _body.transform.forward = _input;

        var deltaPosition = (int)_direction * Speed * _input * Time.fixedDeltaTime;
        
        _body.MovePosition(_body.position + deltaPosition);
    }
}
