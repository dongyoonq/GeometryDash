using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    private float speed = 2.5f; // Speed at which the object moves
    public Transform endpoint;
    public enum State
    {
        Idle, Launch
    }
    public State curState;

    private void Start()
    {
        endpoint = GetComponent<Transform>();
        curState = State.Idle;
        //이벤트 구문 추가 
        GameManager.Pool.Trigger += SwitchState;
    }
    private void Update()
    {
        if (curState == State.Idle)
            return;
        LaunchState(); 
    }

    private void SwitchState()
    {
        curState = State.Launch;
    }

    private void IdleState()
    {
        //대기만 한다. 
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