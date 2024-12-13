using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [Header("Movement Parameters")]
    public float moveSpeed = 3f;
    public float chaseRange = 5f;
    public LayerMask playerLayer;

    [Header("Scale Parameter")]
    public Vector3 scale = new Vector3(1, 1, 1);

    [Header("Life Parameters")]
    public float lifeTime = 10f;

    private Transform player;

    private void Start()
    {
        StartCoroutine(DestroyAfterTime(lifeTime));
    }

    private void Update()
    {
        if (PlayerInRange())
        {
            ChasePlayer();
        }
        else
        {
            MoveForward();
        }
    }

    private bool PlayerInRange()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, chaseRange, playerLayer);
        if (hit != null)
        {
            player = hit.transform;
            return true;
        }
        return false;
    }

    private void ChasePlayer()
    {
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 movement = direction * moveSpeed * Time.deltaTime;
        transform.Translate(movement);

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(Mathf.Abs(scale.x), scale.y, scale.z);
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-Mathf.Abs(scale.x), scale.y, scale.z);
        }
    }

    private void MoveForward()
    {
        Vector2 movement = Vector2.right * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
    }
}