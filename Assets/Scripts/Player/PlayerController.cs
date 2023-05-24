using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // 오른쪽으로 이동하는 속도
    public float jumpForce = 5f;        // 점프하는 힘
    public float rotationSpeed = 360f;  // 한 바퀴 회전하는 속도

    private Rigidbody2D player;
    private bool isJumping = false;

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void OnJump(InputValue value)
    {
        Jump();
    }

    void Jump()
    {
        isJumping = true;
        player.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);

        // 한 바퀴 회전
        if (isJumping)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // 바닥에 닿으면 점프 상태 초기화
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJumping = false;
        }
    }
}
