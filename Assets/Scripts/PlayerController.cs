using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float timeInvincible = 2.0f;
    public int maxHealth = 5;
    public int health { get {return currentHealth;} }
    int currentHealth = 1;
    bool isInvincible;
    float damageCooldown;
    public InputAction MoveAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    [SerializeField] float speed = 5.0f;
    void Start()
    {
        MoveAction.Enable();
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        damageCooldown -= Time.deltaTime;
        if(damageCooldown < 0) {
            isInvincible = false;
        }
    }
    void FixedUpdate()
    {
        Vector2 position = (Vector2)transform.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if(isInvincible) {
            return;
        }
        isInvincible = true;
        damageCooldown = timeInvincible;
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
        Debug.Log(currentHealth + "/" + maxHealth);
    }
}
