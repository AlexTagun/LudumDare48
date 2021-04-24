using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody _body;
    
    [SerializeField] private float Speed = 5f;
    
    [SerializeField] private Vector3 _input = Vector3.zero;
    
    [SerializeField] private SpiralMovement _spiralMovement;

    private void FixedUpdate()
    {
        _input = _spiralMovement.GetDirection(transform.position);
        
        if (_input == Vector3.zero)
        {
            return;
        }
        
        _body.transform.forward = _input;
        _body.MovePosition(_body.position + _input * Speed * Time.fixedDeltaTime);
    }
}
