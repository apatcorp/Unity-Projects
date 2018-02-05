using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    public Transform cameraTransform;

    // movement
    public float speed = 5f;
    Vector3 targetVelocity;
    Vector3 velocity;
    Vector3 currentVelocitySmoothDamp;
    float velocitySmoothTime = 0.2f;

    // camera rotation
    public float rotationSpeedY = 10f;
    public float rotationSpeedX = 2f;
    float minRotationDown = -60f;
    float maxRotationUp = 70f;

    float targetYRotationAngle = 0f;
    float yRotationAngle = 0f;
    float rotationSmoothTimeY = 0.1f;
    float currentVelocityAngleSmoothDampY;

    float targetXRotationAngle = 0f;
    float xRotationAngle = 0f;
    float rotationSmoothTimeX = 0.05f;
    float currentVelocityAngleSmoothDampX;

    Rigidbody playerRigidBody;


    private void Start()
    {
        tag = "Player";
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        cameraTransform = transform.GetChild(0);
    }

    void Update ()
    {
        // input from keys
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        // caluclate velocity
        targetVelocity = inputDirection * speed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref currentVelocitySmoothDamp, velocitySmoothTime);

        // calculate camera movement
        Vector2 angles = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // rotate camera vertically
        targetYRotationAngle -= angles.y * rotationSpeedY;
        targetYRotationAngle = Mathf.Clamp(targetYRotationAngle, minRotationDown, maxRotationUp);

        yRotationAngle = Mathf.SmoothDampAngle(yRotationAngle, targetYRotationAngle, ref currentVelocityAngleSmoothDampY, rotationSmoothTimeY);

        cameraTransform.localEulerAngles = new Vector3(yRotationAngle, 0f, 0f);

        // rotate player horizontally
        targetXRotationAngle += angles.x * rotationSpeedX;
        xRotationAngle = Mathf.SmoothDampAngle(xRotationAngle, targetXRotationAngle, ref currentVelocityAngleSmoothDampX, rotationSmoothTimeX);

        transform.localEulerAngles = new Vector3(0f, xRotationAngle, 0f);
    }

    void FixedUpdate()
    {
        // move player
        playerRigidBody.MovePosition(playerRigidBody.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
    }
}
