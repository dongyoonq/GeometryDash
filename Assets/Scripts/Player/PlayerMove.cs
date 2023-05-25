using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 5f;        // 오른쪽으로 이동하는 속도
    public float jumpForce = 6f;        // 점프하는 힘
    public float rotationSpeed = -180f;  // 한 바퀴 회전하는 속도

    private Rigidbody2D player;
    private bool isJumping = false;
    private float currentRotation = 0f;
    private bool isRotating = false;

    void Awake()
    {
        player = GetComponent<Rigidbody2D>();
        player.gravityScale = 4f; // 중력을 활성화
    }

    void Update()
    {
        // 오른쪽으로 이동
        player.velocity = new Vector2(moveSpeed, player.velocity.y);

        // 회전
        if (isRotating)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, -rotationAmount);
            currentRotation += -rotationAmount;

            // 360도 회전 후 점프 상태 초기화
            if (currentRotation <= -180f)
            {
                isRotating = false;
                currentRotation = 0f;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿으면 점프 상태 초기화
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJumping = false;
            isRotating = false;
            currentRotation = 0f;
        }
    }

    void OnJump(InputValue value)
    {
        if (value.isPressed && !isJumping)
        {
            isJumping = true;
            isRotating = true;
            player.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Force);
        }
    }
}
