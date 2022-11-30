using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Drone_Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private float _speed = 03f; // all directions
    private float horizontal_speed = 0.5f; // up, down
    private float rotation_speed = 1f; // (yaw) left, right
    private Vector3 target_dir = new Vector3(0, 0, 0); // Relative xyz-direction
    Vector3 left_stick_vec = new Vector3(0, 0, 0);
    Vector3 right_stick_vec = new Vector3(0, 0, 0);
    Vector3 rotation_vel_vec = new Vector3(0, 0, 0);

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {   
        target_dir = (left_stick_vec + right_stick_vec); // Combine left and right stick inputs
        // Rotate drone
        Quaternion deltaRotation = Quaternion.Euler(rotation_vel_vec);
        _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
        // Add velocity
        _rigidbody.velocity += (_rigidbody.rotation * target_dir) * _speed;
    }

    // Called when left stick input is perceived.
    private void OnLeftStick(InputValue value) {
        Vector2 input_vec = value.Get<Vector2>();
        left_stick_vec = new Vector3(0, input_vec[1] * horizontal_speed, 0);
        rotation_vel_vec = new Vector3(0, input_vec[0] * rotation_speed, 0);
    }

    // Called when right stick input is perceived.
    private void OnRightStick(InputValue value) {
        Vector2 input_vec = value.Get<Vector2>();
        right_stick_vec = new Vector3(input_vec[0], 0, input_vec[1]);
    }
}
