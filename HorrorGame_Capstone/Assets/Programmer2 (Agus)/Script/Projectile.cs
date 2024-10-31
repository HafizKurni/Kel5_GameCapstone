using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private bool hit;
    private Animator anim;
    private BoxCollider2D boxCollider;
    private float projectile_lifetime;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hit) return;
        float movementSpeed = speed*Time.deltaTime * direction;
        transform.Translate(movementSpeed, 0, 0);

        projectile_lifetime += Time.deltaTime;
        if (projectile_lifetime > 4) gameObject.SetActive(false);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleCollision(collision);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider2D collision)
    {
        if (hit) return; // Cegah tabrakan ganda

        hit = true;
        boxCollider.enabled = false;
        anim.SetTrigger("Explode");
        Invoke(nameof(Deactive), 0.5f); // Hancurkan setelah durasi animasi ledakan
    }
    public void SetDirection(float projectile_direction)
    {
        projectile_lifetime = 0;
        direction = projectile_direction;
        gameObject.SetActive(true);
        hit = false;
        boxCollider.enabled = true;

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != projectile_direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }
    private void Deactive()
    {
        gameObject.SetActive(false);
    }
}
