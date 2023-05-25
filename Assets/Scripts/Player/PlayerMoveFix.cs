using UnityEngine;
using UnityEngine.InputSystem;

public enum Speed { Slow = 0, Normal = 1, Fast = 2, Faster = 3, Fastest = 4 }
public enum Gamemodes { Cube = 0, Ship = 1 }

public class PlayerMoveFix : MonoBehaviour
{
    public float jumpForce = 27f;        // 점프하는 힘
    private Rigidbody2D player;

    public Transform GroundCheckTransform;
    public float GroundCheckRadius;
    public LayerMask GroundMask;
    public Transform Sprite;

    public Speed currentSpeed;
    public Gamemodes currentGamemode;
    public float[] SpeedValues = { 9f, 10f, 13f, 16f, 19f };
    int gravity = 1;

    private void Start()
    {
        player = GetComponent<Rigidbody2D>();
        player.gravityScale = 12f;
    }

    private void Update()
    {
        // 오른쪽으로 이동
        Move();

        Invoke(currentGamemode.ToString(), 0);
    }

    private void Cube()
    {
        if (OnGround())
        {
            Vector3 Rotation = Sprite.rotation.eulerAngles;
            Rotation.z = Mathf.Round(Rotation.z / 90) * 90;
            Sprite.rotation = Quaternion.Euler(Rotation * gravity);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                player.velocity = Vector2.zero;
                player.AddForce(Vector2.up * jumpForce * gravity, ForceMode2D.Impulse);
            }
        }
        else
        {
            Sprite.Rotate(Vector3.back, 400f * Time.deltaTime);
        }
    }

    private void Ship()
    {
        Sprite.rotation = Quaternion.Euler(0, 0, player.velocity.y * 2);

        if(Input.GetKeyDown(KeyCode.Space))
        {
            player.gravityScale = -4f;
        }
        else
        {
            player.gravityScale = 4f;
        }

        player.gravityScale *= gravity;
    }

    private void Move()
    {
        player.velocity = new Vector2(SpeedValues[(int)currentSpeed], player.velocity.y);
    }

    private bool OnGround()
    {
        return Physics2D.OverlapBox(transform.position + Vector3.down * gravity * 0.5f,
            Vector2.right * 1.1f + Vector2.up * GroundCheckRadius, 0, GroundMask);
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
