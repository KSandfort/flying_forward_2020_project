using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Drone_Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnLeftStick(InputValue value) {
        Vector2 input_vec = value.Get<Vector2>();
        Debug.Log(input_vec);
        Vector3 target_dir = new Vector3(0, input_vec[1], 0);
        _rigidbody.velocity = target_dir * _speed;
        Vector3 m_EulerAngleVelocity = new Vector3(0, input_vec[0] * 20, 0);
        Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
    }

    private void OnRightStick(InputValue value) {
        Vector2 input_vec = value.Get<Vector2>();
        Debug.Log(input_vec);
        Vector3 target_dir = new Vector3(input_vec[0], 0, input_vec[1]);
        _rigidbody.velocity = target_dir * _speed;
    }
}
