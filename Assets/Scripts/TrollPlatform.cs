using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollPlatform : MonoBehaviour
{
    public Transform player;
    private Vector2 originalPos;
    public float distance;
    public float minDistanceFromPlayer;
    public float moveSpeed;

    void Start()
    {
        originalPos = transform.position;
    }

    void Update()
    {
        float distanceFromPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceFromPlayer <= minDistanceFromPlayer) {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(originalPos.x + distance, transform.position.y), moveSpeed * Time.deltaTime);
        }

        else {
            transform.position = Vector2.MoveTowards(transform.position, originalPos, moveSpeed * Time.deltaTime);
        }
    }
}
