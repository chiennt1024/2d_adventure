using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Projectile : MonoBehaviour
{
    Rigidbody2D rigidbody2d;
    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
    }
    void Start()
    {

    }
    void Update()
    {
        GameObject playerCharacter = GameObject.Find("PlayerCharacter");
        if(playerCharacter != null) {
            Rigidbody2D playerRigidbody2d = playerCharacter.GetComponent<Rigidbody2D>();
            float distance = Vector3.Distance(rigidbody2d.position, playerRigidbody2d.position);
            Debug.Log(distance);
            if(distance > 10.0f) {
                Destroy(gameObject);
            }
        }        
        // if(transform.position.magnitude > 100.0f)
        // {
        //     Destroy(gameObject);
        // }
    }
    public void Launch(Vector2 direction, float force)
    {
        rigidbody2d.AddForce(direction * force);
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController enemy = other.collider.GetComponent<EnemyController>();
        if(enemy != null) {
            enemy.Fix();
        }
        Destroy(gameObject);
    }
}
