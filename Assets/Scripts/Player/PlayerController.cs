using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;        // ���������� �̵��ϴ� �ӵ�
    public float jumpForce = 5f;        // �����ϴ� ��
    public float rotationSpeed = 360f;  // �� ���� ȸ���ϴ� �ӵ�

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

        // �� ���� ȸ��
        if (isJumping)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            transform.Rotate(Vector3.forward, rotationAmount);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // �ٴڿ� ������ ���� ���� �ʱ�ȭ
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isJumping = false;
        }
    }
}
