using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class HeliController : MonoBehaviour {

    [SerializeField] private Rigidbody heliRigidbody;
    [SerializeField] private Vector3 torqueModefier;

    private PlayerInputActions playerInputActions;


    private float mouseSensitivity = 0.3f;




    private void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();

        Cursor.lockState = CursorLockMode.Locked;
    }


    private void Start() {
        SendTransformToUI();
    }

    private void Update() {
        HandleOrientation();
    }

    private void FixedUpdate() {
        HandleTiltPhysics();
    }




    private void HandleOrientation() {


        // Constances
        float powerForce = 3f;
        float tiltForce = 0.6f;


        // Power
        float powerNormalized = 0;
        if(Input.GetKey(KeyCode.W)) powerNormalized += 1;
        if(Input.GetKey(KeyCode.S)) powerNormalized -= 1;

        // Tilt to the side
        float sideTiltNormalized = 0;
        if(Input.GetKey(KeyCode.A)) sideTiltNormalized += 1;
        if(Input.GetKey(KeyCode.D)) sideTiltNormalized -= 1;

        // Yaw
        float yawNormalized = mouseSensitivity * playerInputActions.Player.Yaw.ReadValue<float>();
        yawNormalized = Mathf.Clamp(yawNormalized, -1, 1);

        // Pitch
        float pitchNormalized = mouseSensitivity * playerInputActions.Player.Pitch.ReadValue<float>();
        pitchNormalized = Mathf.Clamp(pitchNormalized, -1, 1);

        // Modefied by heli collider size -> Vector3 torqueModefier
        Vector3 torque =
            transform.right * pitchNormalized * (torqueModefier.y * torqueModefier.z) +
            transform.up * yawNormalized * (torqueModefier.x * torqueModefier.z) +
            transform.forward * sideTiltNormalized * (torqueModefier.x * torqueModefier.y);

        heliRigidbody.AddTorque(torque * tiltForce);
        heliRigidbody.AddForce(transform.up * powerForce * powerNormalized);
    }

    private void HandleTiltPhysics() {
        // Counter gravity Force
        heliRigidbody.AddForce(transform.up * heliRigidbody.mass * 9.8f);

        // Lending Force
        float pullingDownForce = 0.5f;
        float effectDistance = 5f;
        if(Physics.Raycast(transform.position, Vector3.down, effectDistance)) {
            heliRigidbody.AddForce(-transform.up * heliRigidbody.mass * pullingDownForce);
        }
    }


    private void SendTransformToUI() {
        if(HeliUI.Instance == null) return;

        HeliUI.Instance.SetHeliTransform(transform);
    }

}
