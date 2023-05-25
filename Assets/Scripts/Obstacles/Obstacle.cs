using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float speed = 3f; // Speed at which the object moves
    public Transform endpoint;

    public enum State { Idle, Launch };
    public State curState;

    private void Start()
    {
        endpoint = GetComponent<Transform>();
        curState = State.Idle;
        //�̺�Ʈ ���� �߰� 
        GameManager.Pool.Trigger += SwithState;
    }
    private void Update()
    {
        // Translate the object to the left
        switch (curState)
        {
            case State.Idle:
                IdleState();
                break;
            case State.Launch:
                LaunchState();
                break;
        };
    }

    private void SwithState()
    {
        curState = State.Launch;
    }

    private void IdleState()
    {
        //��⸸ �Ѵ�. 
        return;
    }

    private void LaunchState()
    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);

        // Check if the object reaches the endpoint
        if (transform.position.x <= endpoint.position.x)
        {
            GameManager.Pool.ReturnBlockToPool(gameObject);
        }
    }
}