using UnityEngine;
using UnityEngine.InputSystem;

public class SailsMovement : MonoBehaviour
{
    // [SAILS OPENING VARIABLES]
    private float sailsOpen = 0f; // % percentage
    public float openingRate = 20f;
    float minOpening = 0f;
    float maxOpening = 100f;

    // [SAILS ROTATION VARIABLES]
    private float sailsRotation = 0f; // * degree
    public float rotationRate = 20f;
    float maxLeftRotation = -45f;
    float maxRightRotation = 45f;
    Transform sailObject;
    Quaternion localRotationSail;

    // [MOVE RELATED VARIABLES]
    private Vector2 moveVector;
    private float steerAxis; // since it's q/e it's and axis
    private float currentSpeed = 0f;
    private InputAction moveInput;
    private InputAction steerInput;

    // [WIND VARIABLES]
    public Transform windTransform; // empty gameobject from the inspector
    [SerializeField] float windStrength = 10f;

    // [DRAGGING VARIABLE]
    // the resistance force that water and air exert against the craft as it moves forward, slowing it down
    public float dragCoeff = 0.5f;

    // [STEERING VARIABLE]
    public float steerRate = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        steerInput = InputSystem.actions.FindAction("Steer");
        sailObject = transform.Find("Sail");
        localRotationSail = sailObject.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>();
        steerAxis = steerInput.ReadValue<float>();

        HandleOpeningSails(moveVector);
        HandleRotatingSails(moveVector);
        HandleSteering(steerAxis);

        float propulsion = CalculatePropulsion();
        float resistance = CalculateResistance();

        currentSpeed += (Mathf.Max(propulsion, 0f) - resistance) * Time.deltaTime;

        Debug.Log($"current speeeed: {currentSpeed}");

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

    }

    // all the logic behind actually going forward
    float CalculatePropulsion()
    {
        float openCoeff = sailsOpen / maxOpening; // (0-1)

        Vector3 sailRight = sailObject.up; // local sail axis that is perpendicular to the wind forward (to simulate wind blowing on it)
        Vector3 windForward = windTransform.forward;
        float windSailDot = Vector3.Dot(windForward, sailRight); // measures how much the wind is blowing against the flat side of the sail
        float sailShipDot = Vector3.Dot(sailRight, transform.forward); // tells how the sail is positioned in regard to the ship.forward
        // ensuring the ship only accelerates in the direction its facing

        float propulsion = windStrength * openCoeff * windSailDot * sailShipDot;
        return propulsion;
    }

    // logic behind ship resistance (dragging)
    float CalculateResistance()
    {
        return dragCoeff * currentSpeed * currentSpeed; // so the drag will be more the more the ship accelerates
    }

    // input for opening sails [W, S]
    void HandleOpeningSails(Vector2 moveVector)
    {
        // moveVector.y -> W, S
        sailsOpen += moveVector.y * openingRate * Time.deltaTime;

        // keep the value between these min and max points
        sailsOpen = Mathf.Clamp(sailsOpen, minOpening, maxOpening);
        //Debug.Log($"sailsOpen: {sailsOpen}%");

        // actualSpeed = (sailsOpen / maxOpening) * maxSpeed; // (current percentage / maxOpening) times maxspeed (to make it: (0-1) * maxSpeed)
        //Debug.Log($"actual speed: {actualSpeed}");
    }

    // input for rotating the sail [A, D]
    void HandleRotatingSails(Vector2 moveVector)
    {
        // moveVector.x -> A, D
        sailsRotation += moveVector.x * rotationRate * Time.deltaTime;

        sailsRotation = Mathf.Clamp(sailsRotation, maxLeftRotation, maxRightRotation);
        //Debug.Log($"sailsrotation: {sailsRotation}*");

        // sailObject.Rotate(0f, 0f, sailsRotation * Time.deltaTime);
        sailObject.localRotation = localRotationSail * Quaternion.Euler(0f, 0f, sailsRotation);

    }

    // input for rotating the ship [Q, E] for testing wind
    void HandleSteering(float steerAxis)
    {
        float rotationAmount = steerAxis * steerRate * currentSpeed * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f, Space.World);
    }

    // getters for UILabels
    public float GetSailsOpening()
    {
        return sailsOpen;
    }

    public float GetSailsRotation()
    {
        return sailsRotation;
    }

}
