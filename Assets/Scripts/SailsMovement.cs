using UnityEngine;
using UnityEngine.InputSystem;

public class SailsMovement : MonoBehaviour
{
    // [SAILS OPENING VARIABLES]
    private float sailsOpen = 0f; // % percentage
    public float openingRate = 20f; // how fast the sail opens
    float minOpening = 0f;
    float maxOpening = 100f;

    // [SAILS ROTATION VARIABLES]
    private float sailsRotation = 0f; // * degree
    public float rotationRate = 20f; // how fast the sail rotates
    float maxLeftRotation = -45f;
    float maxRightRotation = 45f;
    Transform sailObject; // the transform of the seperate sail object in ship parent
    Quaternion localRotationSail;

    // [MOVE RELATED VARIABLES]
    private Vector2 moveVector; // W/S/A/D
    private float steerAxis; // Q/E axis
    private float currentSpeed = 0f;
    private InputAction moveInput;
    private InputAction steerInput;

    // [WIND VARIABLES]
    public Transform windTransform; // empty gameobject from the inspector
    [SerializeField] float windStrength = 20f;

    // [DRAGGING VARIABLE]
    // "the resistance force that water and air exert against the craft as it moves forward, slowing it down"
    [SerializeField] float dragCoeff = 0.2f;

    // [STEERING VARIABLE]
    [SerializeField] float steerRate = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
        steerInput = InputSystem.actions.FindAction("Steer");
        sailObject = transform.Find("Sail"); // returns found child transform
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

        // Debug.Log($"current speed: {currentSpeed}");

        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // visualisation of the angles
        /*
        Vector3 p = transform.position + Vector3.up * 3f;
        Debug.DrawRay(p, windTransform.forward * 5f, Color.cyan);
        Debug.DrawRay(p, sailObject.up * 5f, Color.yellow);
        Debug.DrawRay(p, transform.forward * 5f, Color.green);
        */
    }

    // all the logic behind going forward
    float CalculatePropulsion()
    {
        float openCoeff = sailsOpen / maxOpening; // (0-1) scale

        Vector3 sailNormal = sailObject.up; // local sail axis that is perpendicular to the ship
        Vector3 windForward = windTransform.forward;
        float windSailDot = Vector3.Dot(windForward, sailNormal); // measures how much the wind is blowing against the normal of the sail
        float sailShipDot = Vector3.Dot(sailNormal, transform.forward); // tells how the sail is positioned in regard to the ship.forward
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
        sailsOpen += moveVector.y * openingRate * Time.deltaTime;

        // keep the value between these min and max points
        sailsOpen = Mathf.Clamp(sailsOpen, minOpening, maxOpening);

        //Debug.Log($"sailsOpen: {sailsOpen}%");
    }

    // input for rotating the sail [A, D]
    void HandleRotatingSails(Vector2 moveVector)
    {
        sailsRotation += moveVector.x * rotationRate * Time.deltaTime;

        sailsRotation = Mathf.Clamp(sailsRotation, maxLeftRotation, maxRightRotation);
        //Debug.Log($"sailsrotation: {sailsRotation}*");

        sailObject.localRotation = localRotationSail * Quaternion.Euler(0f, 0f, sailsRotation);
    }

    // input for rotating the ship [Q, E] for testing wind
    void HandleSteering(float steerAxis)
    {
        float rotationAmount = steerAxis * steerRate * Time.deltaTime;
        transform.Rotate(0f, rotationAmount, 0f, Space.World);
    }

    // getters for UILabels
    public float GetSailsOpening()
    {
        return Mathf.Round(sailsOpen);
    }

    public float GetSailsRotation()
    {
        return Mathf.Round(sailsRotation);
    }

}
