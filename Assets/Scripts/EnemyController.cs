using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Animator animator;
    public float speed = 5.0f;
    public float changeTime = 3.0f;
    public float timer = 3.0f;
    public bool vertical;
    int direction = 1;
    bool aggressive = true;
    Rigidbody2D rigidbody2d;
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        timer = changeTime;
    }
    void Update()
    {
        if(!aggressive) {
            return;
        }
        timer -= Time.deltaTime;
        if(timer < 0) {
            int randomValue = Random.Range(0, 2);
            vertical = randomValue != 0;
            timer = changeTime;
            direction = -direction;
        }
        if(vertical) {
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        } else {
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
    }
    void FixedUpdate()
    {
        if(!aggressive) {
            return;
        }
        Vector2 position = rigidbody2d.position;
        if(vertical) {
            position.y = position.y + speed * direction * Time.deltaTime;
        } else {
            position.x = position.x + speed * direction * Time.deltaTime;
        }
        rigidbody2d.MovePosition(position);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        PlayerController player = other.gameObject.GetComponent<PlayerController>();
        if(player != null) {
            player.ChangeHealth(-1);
        }
    }
    public void Fix()
    {
        aggressive = false;
        rigidbody2d.simulated = false;
        animator.SetTrigger("Fixed");
    }
}
