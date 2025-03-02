using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed = 7.0f;  // Movement speed
    [SerializeField] private Transform cameraPlayer;  // Reference to the camera's transform (should be the FreeLook Camera's follow target)
    [SerializeField] private InputManager inputManager;  // Reference to the InputManager to get player input
    private Rigidbody rb;
    [SerializeField] float jumpForce;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDuration;
    [SerializeField] float dashCooldown;

    // Movement input
    private Vector2 moveInput;
    private bool isGrounded;
    private bool doubleJump;
    public float delayBeforeDoubleJump;

    // Dash variables
    private bool isDashing = false;
    private float dashTime = 0f;
    private float lastDashTime = -999f;
    private Vector3 dashDirection;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Subscribe to movement event
        inputManager.OnMove.AddListener(HandleMovement);

        // Subscribe to space bar event (e.g., jumping)
        inputManager.OnSpacePressed.AddListener(HandleJump);

        inputManager.OnDash.AddListener(HandleDash);
    }

    // Handle movement input from the InputManager
    private void HandleMovement(Vector2 input)
    {
        moveInput = input;
    }

    private void OnCollisionEnter(Collision other)
    { //checking that when the ball enters, it is on the ground
        Debug.Log(other.gameObject.name);
        if (other.gameObject.CompareTag("Ground"))
        {
            Vector3 normal = other.GetContact(0).normal;
            if (normal == Vector3.up)
            { //checking that when it hits an obstacle, it cannot double jump
                isGrounded = true;
                doubleJump = false;
            }
        }
        if (other.gameObject.tag == "Coin")
        { //refering to a specific gameobject
            Destroy(other.gameObject); //refers to the gameObject that you want to destroy
        }
    }

    private void OnCollisionExit(Collision other)
    { //when the ball is off the ground, set the value to false
        if (other.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

    // Handle jump event (space bar)
    private void HandleJump()
    {
        Debug.Log("Jump triggered");
        if (isGrounded)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z); //ensures jumps are consistent
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Invoke("EnableDoubleJump", delayBeforeDoubleJump);//invoke calls enable double jump with a delay
        }
        if (doubleJump)
        {
            doubleJump = false;
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    void EnableDoubleJump()
    {
        doubleJump = true; //set double jumping to true
    }

    private void HandleDash()
    {
        Debug.Log("Dash Triggered");
        if (Time.time > lastDashTime + dashCooldown && !isDashing)
        {
            isDashing = true;
            dashTime = dashDuration;
            lastDashTime = Time.time;

            // Get the dash direction based on camera and player facing
            dashDirection = cameraPlayer.forward;

            // Ensure we don't have any vertical movement during the dash
            dashDirection.y = 0;

            // Apply dash velocity
            rb.linearVelocity = dashDirection * dashSpeed;
        }
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
        if (isDashing)
        {
            dashTime -= Time.deltaTime;
            if (dashTime <= 0)
            {
                EndDash();
            }
        }

        // Calculate camera-relative movement
        Vector3 moveDirection = cameraPlayer.forward * moveInput.y + cameraPlayer.right * moveInput.x;
        moveDirection.y = 0f; // Make sure we don't apply any vertical movement

        if (!isDashing)
        {
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

    private void EndDash()
    {
        isDashing = false;
        rb.linearVelocity = Vector3.zero; // Stop the dash velocity
    }

}
