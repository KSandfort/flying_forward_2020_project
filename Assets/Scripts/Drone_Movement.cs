using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class Drone_Movement : MonoBehaviour
{
    private Rigidbody _rigidbody;
    public float horizontal_speed = 2f;
    public float vertical_speed = 1f;
    public float rotation_speed = 4f; // (yaw) left, right
    private float max_tilt = 1.5f;
    private Vector3 target_dir = new Vector3(0, 0, 0); // Relative xyz-direction
    Vector3 left_stick_vec = new Vector3(0, 0, 0);
    Vector3 right_stick_vec = new Vector3(0, 0, 0);
    Vector3 rotation_vel_vec = new Vector3(0, 0, 0);
    GameObject tilt_controller;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        tilt_controller = GameObject.Find("Tilt_Controller");
    }

    // Update is called once per frame
    void FixedUpdate()
    {   
        // Combine both stick inputs for target direction (xyz)
        target_dir = (left_stick_vec + right_stick_vec); // Combine left and right stick inputs
        
        Vector3 x_z_rotation = new Vector3(right_stick_vec[2] * max_tilt, 0, -right_stick_vec[0] * max_tilt); // TODO: Find current y rotation
        Quaternion x_z_quaternion = Quaternion.Euler(x_z_rotation);
        Quaternion deltaRotation = Quaternion.Euler(rotation_vel_vec);
        
        if (Data_Tracking_2.enable_controls) {
            // Tilt drone
            tilt_controller.transform.localRotation = x_z_quaternion;
            // Rotate drone (yaw)
            _rigidbody.MoveRotation(_rigidbody.rotation * deltaRotation);
            // Add velocity
            _rigidbody.velocity += (_rigidbody.rotation * target_dir);
        }
    }

    // Called when left stick input is perceived.
    private void OnLeftStick(InputValue value) {
        Vector2 input_vec = value.Get<Vector2>();
        left_stick_vec = new Vector3(0, input_vec[1] * vertical_speed, 0);
        rotation_vel_vec = new Vector3(0, input_vec[0] * rotation_speed, 0);
    }

    // Called when right stick input is perceived.
    private void OnRightStick(InputValue value) {
        Vector2 input_vec = value.Get<Vector2>();
        right_stick_vec = new Vector3(input_vec[0] * horizontal_speed, 0, input_vec[1] * horizontal_speed);
    }

    private void OnEscape() {
        Application.Quit();
    }
}
