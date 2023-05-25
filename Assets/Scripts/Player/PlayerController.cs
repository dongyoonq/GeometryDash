using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D player;
    [SerializeField] private float jumpForce;

    private void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }

    private void OnJump(InputValue value)
    {
        Jump();
    }

    private void Jump()
    {
        // มกวม
        player.AddForce(Vector2.up * jumpForce, ForceMode2D.Force);
    }
}
