using UnityEngine;
using UnityEngine.InputSystem;

public enum Speed { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 }
public enum Gamemodes { Cube = 0, Ship = 1 }

public class PlayerMoveFix : MonoBehaviour
{
    public float jumpForce = 6f;        // 점프하는 힘
    public float rotationSpeed = 360f;  // 한 바퀴 회전하는 속도
    public Transform Sprite;

    private Rigidbody2D player;
    private bool isJumping = false;
    private float currentRotation = 0f;
    private bool isRotating = false;

    public Speed currentSpeed;
    public Gamemodes currentGamemode;
    public float[] SpeedValues = { 9f, 11f, 13f, 15f, 19f };
    int gravity = 1;

    void Awake()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 오른쪽으로 이동
        Move();

        Invoke(currentGamemode.ToString(), 0);
    }

    void Cube()
    {
        // 회전
        if (isRotating)
        {
            float rotationAmount = rotationSpeed * Time.deltaTime;
            Sprite.Rotate(Vector3.forward, -rotationAmount * gravity);
            currentRotation += -rotationAmount;

            /*Vector3 Rotation = Sprite.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 90) * 90;
            Sprite.rotation = Quaternion.Euler(Rotation * gravity);*/

            // 360도 회전 후 점프 상태 초기화
            if (currentRotation <= -180f)
            {
                isRotating = false;
                currentRotation = 0f;
            }
        }
    }

    void Ship()
    {
        Sprite.rotation = Quaternion.Euler(0, 0, player.velocity.y * 2);

        if(Input.GetMouseButtonDown(0))
        {
            player.gravityScale = -4.314969f;
        }
        else
        {
            player.gravityScale = 4.314969f;
        }

        player.gravityScale *= gravity;
    }

    void Move()
    {
        player.velocity = new Vector2(SpeedValues[(int)currentSpeed], player.velocity.y);
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
        // Cube
        if(currentGamemode == (Gamemodes)0)
        {
            if (value.isPressed && !isJumping)
            {
                isJumping = true;
                isRotating = true;
                player.AddForce(Vector2.up * jumpForce * gravity, ForceMode2D.Force);
            }
        }
        // Ship
        else if(currentGamemode == (Gamemodes)1)
        {
            if (value.isPressed)
            {
                player.gravityScale = -4.314969f;
                isJumping = true;
                isRotating = true;
                player.AddForce(Vector2.up * jumpForce * gravity, ForceMode2D.Force);
            }
            else
            {
                player.gravityScale = 4.314969f;
                isJumping = true;
                isRotating = true;
                player.AddForce(Vector2.up * jumpForce * gravity, ForceMode2D.Force);
            }
        }
    }

    public void ThroughPortal(Gamemodes Gamemode, Speed Speed, int Gravity, int State)
    {
        switch (State)
        {
            case 0:
                currentSpeed = Speed;
                break;
            case 1:
                currentGamemode = Gamemode;
                player.AddForce(Vector2.up * jumpForce * gravity * 2, ForceMode2D.Force);
                break;
            case 2:
                gravity = Gravity;
                player.gravityScale = Mathf.Abs(player.gravityScale) * Gravity;
                break;
        }
    }
}
