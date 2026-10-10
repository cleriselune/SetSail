using UnityEngine;
using UnityEngine.InputSystem;

public class SailsMovement : MonoBehaviour
{
    Vector2 moveVector;

    float actualSpeed;
    float maxSpeed = 20f;

    private float sailsOpen = 0f; // % percentage
    public float openingRate = 20f;
    float minOpening = 0f;
    float maxOpening = 100f;

    private float sailsRotation = 0f; // * degree
    public float rotationRate = 20f;
    float maxLeftRotation = -45f;
    float maxRightRotation = 45f;

    private InputAction moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveInput = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>();
        
        HandleOpeningSails(moveVector);
        HandleSteering(moveVector);
    }

    void HandleOpeningSails(Vector2 moveVector)
    {
        // moveVector.y -> W, S
        sailsOpen += moveVector.y * openingRate * Time.deltaTime;

        // keep the value between these min and max points
        sailsOpen = Mathf.Clamp(sailsOpen, minOpening, maxOpening);
        //Debug.Log($"sailsOpen: {sailsOpen}%");

        actualSpeed = (sailsOpen / maxOpening) * maxSpeed; // (current percentage / maxOpening) times maxspeed (to make it: (0-1) * maxSpeed)
        //Debug.Log($"actual speed: {actualSpeed}");

        transform.position += transform.forward * actualSpeed * Time.deltaTime;
    }

    void HandleSteering(Vector2 moveVector)
    {
        // moveVector.x -> A, D
        sailsRotation += moveVector.x * rotationRate * Time.deltaTime;

        sailsRotation = Mathf.Clamp(sailsRotation, maxLeftRotation, maxRightRotation);
        Debug.Log($"sailsrotation: {sailsRotation}*");

        transform.Rotate(0f, sailsRotation * Time.deltaTime, 0f);
    }

    public float GetSailsOpening()
    {
        return sailsOpen;
    }

    public float GetSailsRotation()
    {
        return sailsRotation;
    }

}
