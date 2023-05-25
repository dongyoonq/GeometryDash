using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public Gamemodes gamemode;
    public Speed speed;
    public bool gravity;
    public int State;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        try
        {
            PlayerMoveFix move = collision.gameObject.GetComponent<PlayerMoveFix>();

            move.ThroughPortal(gamemode, speed, gravity ? 1 : -1, State);
        }
        catch { }
    }
}
