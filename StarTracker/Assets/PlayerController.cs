using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MassiveObject
{
    [SerializeField] Camera cam;
    [SerializeField] float normalForceMinMagnitude, normalForceMultiplier;
    [SerializeField] CharacterController charController;
    [SerializeField] float sensitivity;
    [SerializeField] public GameObject ghost;
    float verticalLookRotation;
    float horizontalLookRotation;
    // Vector3 velocity = new Vector3();

    void Start() {
        ghost.transform.parent = null;
        ghost.transform.position = this.transform.position;
    }

    void Update() {
        if(!charController.isGrounded) {
            // Debug.Log("not grounded");
            base.Gravity();
        }
        else {
            // Debug.Log("grounded");
        }
        
        Look();

        // velocity += transform.parent.
        // charController.Move(velocity * Time.deltaTime);
        // transform.position += velocity * Time.deltaTime;
    }

    private void LateUpdate() {
        Vector3 translation = ghost.transform.position - transform.position;
        // Debug.Log("translation" + translation);
        // Debug.Log("move " + (translation + velocity * Time.deltaTime));
        // charController.Move(translation + velocity * Time.deltaTime);
        charController.Move(translation);
        ghost.transform.position = transform.position;
        transform.rotation = ghost.transform.rotation;
        transform.localEulerAngles += new Vector3(0, horizontalLookRotation, 0);
    }

    void Look()
    {
        // Debug.Log(SensitivitySettings.Instance.sensitivity);
        // cam.transform.Rotate(Vector3.up * Input.GetAxisRaw("Mouse X") * sensitivity);
        horizontalLookRotation += Input.GetAxisRaw("Mouse X") * sensitivity % 360;

        verticalLookRotation += Input.GetAxisRaw("Mouse Y") * sensitivity;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90, 90);
        cam.transform.localEulerAngles = Vector3.left * verticalLookRotation;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log("HERE");
        NormalForce(hit);
        // TODO: get normal force actually working
    }

    void NormalForce(ControllerColliderHit hit)
    {
        // slowly remove the part of globalVelocity that goes in the direction of hit.normal
        float normalForceMagnitude = Mathf.Abs(Vector3.Dot(velocity, hit.normal));
        // Debug.Log(normalForceMagnitude);
        if(normalForceMagnitude >= normalForceMinMagnitude){
            // Debug.Log("Normal force");
            Debug.DrawRay(hit.point, hit.normal * Vector3.Dot(velocity, hit.normal), Color.red, 1f);
            velocity -= hit.normal * Vector3.Dot(velocity, hit.normal) * normalForceMultiplier * Time.deltaTime;
        }
    }
}
