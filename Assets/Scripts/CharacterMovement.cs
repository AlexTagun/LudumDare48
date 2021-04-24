using UnityEngine;

public class CharacterMovement : MonoBehaviour
{

    [SerializeField] private Rigidbody _body;
    
    [SerializeField] private float Speed = 5f;
    
    [SerializeField] private Vector3 _input = Vector3.zero;
    
    public void SetInput(Vector3 input)
    {
        _input = input;
    }

    private void FixedUpdate()
    {
        _body.transform.forward = _input;
        _body.MovePosition(_body.position + _input * Speed * Time.fixedDeltaTime);
    }
}
