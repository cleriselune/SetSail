using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class SailsMovement : MonoBehaviour
{
    Rigidbody rigidBody;
    Camera mainCamera;

    Vector2 moveVector;

    float actualSpeed;
    float maxSpeed = 20f;

    float sailsOpen = 0f; // % percentage
    public float openingRate = 20f;
    float minOpening = 0f;
    float maxOpening = 100f;

    float sailsRotation = 0f; // * degree
    public float rotationRate = 20f;
    float maxLeftRotation = -45f;
    float maxRightRotation = 45f;

    private InputAction moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        mainCamera = GetComponentInChildren<Camera>();

        moveInput = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        moveVector = moveInput.ReadValue<Vector2>();
        
        HandleOpeningSails(moveVector);
        HandleSteering(moveVector);
    }


    void FixedUpdate()
    {
        rigidBody.AddForce(transform.forward * actualSpeed);
    }

    void HandleOpeningSails(Vector2 moveVector)
    {
        sailsOpen += moveVector.y * openingRate * Time.deltaTime;

        // keep the value between these min and max points
        sailsOpen = Mathf.Clamp(sailsOpen, minOpening, maxOpening);
        //Debug.Log($"sailsOpen: {sailsOpen}%");

        actualSpeed = (sailsOpen / maxOpening) * maxSpeed; // (current percentage / maxOpening) times maxspeed (to make it: (0-1) * maxSpeed)
        Debug.Log($"actual speed: {actualSpeed}");
    }

    void HandleSteering(Vector2 moveVector)
    {
        // mouseVector.x
    }
}
