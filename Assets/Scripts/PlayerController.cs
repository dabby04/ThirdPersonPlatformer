using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 7.0f;  // Movement speed
    [SerializeField] private Transform cameraPlayer;  // Reference to the camera's transform (should be the FreeLook Camera's follow target)
    [SerializeField] private InputManager inputManager;  // Reference to the InputManager to get player input
    private Rigidbody rb;
    [SerializeField] float jumpForce;

    // Movement input
    private Vector2 moveInput;
    private bool isGrounded;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Subscribe to movement event
        inputManager.OnMove.AddListener(HandleMovement);

        // Subscribe to space bar event (e.g., jumping)
        inputManager.OnSpacePressed.AddListener(HandleJump);
    }

    // Handle movement input from the InputManager
    private void HandleMovement(Vector2 input)
    {
        moveInput = input;
    }

    private void OnCollisionEnter(Collision other)
    { //checking that when the ball enters, it is on the ground
        if(other.gameObject.CompareTag("Ground")){
            Vector3 normal=other.GetContact(0).normal;
            if(normal==Vector3.up){ //checking that when it hits an obstacle, it cannot double jump
                isGrounded=true;
            } 
        }
    }

    private void OnCollisionExit(Collision other)
    { //when the ball is off the ground, set the value to false
        if(other.gameObject.CompareTag("Ground")){
            isGrounded=false; 
        }
    }

    // Handle jump event (space bar)
    private void HandleJump()
    {
        Debug.Log("Jump triggered");
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    // Handle reset event
    private void HandleReset()
    {
        // Implement reset functionality here (e.g., reset position or game state)
        Debug.Log("Reset triggered");
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate player to face the camera's direction when there's movement input
        if (moveInput.magnitude > 0.1f)
        {
            // Get the camera's rotation along the y-axis
            Vector3 rotating = transform.rotation.eulerAngles;
            rotating.y = cameraPlayer.rotation.eulerAngles.y;  // Keep the rotation around the y-axis in sync with the camera
            transform.rotation = Quaternion.Euler(rotating);
        }
    }

    // FixedUpdate is called at fixed time intervals for physics calculations
    private void FixedUpdate()
    {
        // Calculate camera-relative movement
        Vector3 moveDirection = cameraPlayer.forward * moveInput.y + cameraPlayer.right * moveInput.x;
        moveDirection.y = 0f; // Make sure we don't apply any vertical movement

        // Apply movement velocity to the Rigidbody
        Vector3 velocity = rb.linearVelocity;

        // Adjust the velocity in the x and z axes based on the camera-relative move direction
        velocity.x = moveDirection.x * speed;
        velocity.z = moveDirection.z * speed;

        // Keep the current vertical velocity (e.g., for gravity or jumping)
        velocity.y = rb.linearVelocity.y;

        rb.linearVelocity = velocity;  // Apply the new velocity to the Rigidbody
    }
}
