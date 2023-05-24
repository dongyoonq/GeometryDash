using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundMake : MonoBehaviour
{
    [SerializeField] private float distanceRange;
    [SerializeField] private GameObject prevGround;
    [SerializeField] private GameObject groundPrefabs;
    [SerializeField] private LayerMask boundaryMask;

    private GameObject newInstance;

    private void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector3.right, distanceRange, boundaryMask);
        Debug.DrawRay(transform.position, Vector3.right * distanceRange, Color.red);
        if (hit)
        {
            newInstance = Instantiate(groundPrefabs,
                new Vector3(prevGround.transform.position.x + 24.28f,
                prevGround.transform.position.y, 0f),
                prevGround.transform.rotation);
            newInstance.SetActive(false);
            StartCoroutine(OnableGround(newInstance));
            StartCoroutine(DisableGround(prevGround));
            prevGround.transform.GetChild(1).GetComponent<BoxCollider2D>().enabled = false;
            // Destroy(prevGround, 4f);
            prevGround = newInstance;
        }
    }

    IEnumerator OnableGround(GameObject newInstance)
    {
        yield return new WaitForSeconds(0.01f);
        newInstance.SetActive(true);
    }

    IEnumerator DisableGround(GameObject prevInstance)
    {
        yield return new WaitForSeconds(4f);
        prevInstance.SetActive(false);
    }
}
