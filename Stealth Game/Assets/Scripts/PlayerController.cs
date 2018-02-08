using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour {

    [Header("Transform of the camera")]
    public Transform cameraTransform;
    Vector3 cameraInitialPosition;

    [Header("Transform of the hand")]
    public Transform hand;

    public GameObject lampPrefab;

    [Header("Movemnet/Rotation speed")]
    // movement
    public float movementSpeed = 5f;
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

    AudioSource footSteps;
    bool moving = true;
    bool running = false;
    float speedMultiplier = 1.9f;
    float normalizedFootStep = 3f;

    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Singleton;

        if (cameraTransform == null)
        {
            Debug.LogError("No Camera attached");
            return;
        }
        tag = "Player";
        playerRigidBody = GetComponent<Rigidbody>();
        playerRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        cameraInitialPosition = cameraTransform.localPosition;

        footSteps = AudioManager.InstantiateAudioSource(transform.position, "Foot Steps", transform);

        CreateLamp();
    }

    void CreateLamp ()
    {
        GameObject lamp = Instantiate(lampPrefab, hand);
        LightController lightController = lamp.GetComponent<LightController>();
        lightController.SetupLights(transform, 30f, true);
    }

    void Update ()
    {
        // input from keys
        Vector3 inputDirection = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (!running)
            {
                movementSpeed *= speedMultiplier;
                running = true;
            }          
        } else
        {
            if (running)
            {
                movementSpeed /= speedMultiplier;
                running = false;
            }
        }

        if (inputDirection.x != 0 || inputDirection.z != 0)
        {
            if (!moving)
            {
                moving = true;
                StopCoroutine("Breathing");
                cameraTransform.localPosition = cameraInitialPosition;
                StartCoroutine("FootStep");
            }
        } else
        {
            if (moving)
            {
                moving = false;
                StopCoroutine("FootStep");
                StartCoroutine("Breathing");
            }
        }

        // caluclate velocity
        targetVelocity = inputDirection * movementSpeed;
        velocity = Vector3.SmoothDamp(velocity, targetVelocity, ref currentVelocitySmoothDamp, velocitySmoothTime);

        // calculate camera movement
        Vector2 angles = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        // rotate camera and hand vertically
        targetYRotationAngle -= angles.y * rotationSpeedY;
        targetYRotationAngle = Mathf.Clamp(targetYRotationAngle, minRotationDown, maxRotationUp);
        yRotationAngle = Mathf.SmoothDampAngle(yRotationAngle, targetYRotationAngle, ref currentVelocityAngleSmoothDampY, rotationSmoothTimeY);

        cameraTransform.localEulerAngles = new Vector3(yRotationAngle, 0f, 0f);

        // rotate player on own y axis
        targetXRotationAngle += angles.x * rotationSpeedX;
        xRotationAngle = Mathf.SmoothDampAngle(xRotationAngle, targetXRotationAngle, ref currentVelocityAngleSmoothDampX, rotationSmoothTimeX);

        transform.localEulerAngles = new Vector3(0f, xRotationAngle, 0f);
    }

    void FixedUpdate()
    {
        // move player
        playerRigidBody.MovePosition(playerRigidBody.position + transform.TransformDirection(velocity) * Time.fixedDeltaTime);
    }

    
    IEnumerator FootStep ()
    {
        while (moving)
        {
            footSteps.Play();

            yield return new WaitForSeconds(normalizedFootStep / movementSpeed);
        }
    }

    IEnumerator Breathing ()
    {
        while (!moving)
        {
            Vector3 targetPosition = cameraTransform.position + new Vector3(0f, .1f * Mathf.Sin(Time.time), 0f);
            cameraTransform.position = Vector3.Lerp(cameraTransform.position, targetPosition, 2 * Time.deltaTime);

            yield return null;
        }
    }
}
