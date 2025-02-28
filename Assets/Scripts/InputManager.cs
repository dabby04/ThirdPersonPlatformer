using UnityEngine;
using UnityEngine.Events;

public class InputManager : MonoBehaviour
{
    // Event for movement input
    public UnityEvent<Vector2> OnMove = new UnityEvent<Vector2>();

    // Event for space bar (e.g., jumping)
    public UnityEvent OnSpacePressed = new UnityEvent();

    // Update is called once per frame
    void Update()
    {
        // Check for space key press (e.g., jump)
        if (Input.GetKeyDown(KeyCode.Space))
        {
            OnSpacePressed?.Invoke();
        }

        // Capture movement input (WASD or Arrow Keys)
        Vector2 input = Vector2.zero;

        // Check for horizontal movement (A/D or Left/Right arrow keys)
        if (Input.GetKey(KeyCode.A))
        {
            input += Vector2.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            input += Vector2.right;
        }

        // Check for vertical movement (W/S or Up/Down arrow keys)
        if (Input.GetKey(KeyCode.W))
        {
            input += Vector2.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            input += Vector2.down;
        }
            OnMove?.Invoke(input);
    }
}
