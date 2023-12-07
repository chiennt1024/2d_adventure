using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float timeInvincible = 2.0f;
    public int maxHealth = 5;
    public int health { get {return currentHealth;} }
    public GameObject projectilePrefab;
    int currentHealth = 1;
    bool isInvincible;
    float damageCooldown;
    public InputAction MoveAction;
    public InputAction launchAction;
    public InputAction talkAction;
    Rigidbody2D rigidbody2d;
    Vector2 move;
    Vector2 moveDirection = new Vector2(1, 0);
    Animator animator;
    [SerializeField] float speed = 5.0f;
    void Start()
    {
        MoveAction.Enable();
        launchAction.Enable();
        talkAction.Enable();
        launchAction.performed += Launch;
        talkAction.performed += FindFriend;
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }
    void Update()
    {
        move = MoveAction.ReadValue<Vector2>();
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) {
            moveDirection.Set(move.x, move.y);
            moveDirection.Normalize();
        }
        animator.SetFloat("Look X", moveDirection.x);
        animator.SetFloat("Look Y", moveDirection.y);
        animator.SetFloat("Speed", move.magnitude);
        if(isInvincible) {
            damageCooldown -= Time.deltaTime;
            if(damageCooldown < 0) {
                isInvincible = false;
            }
        }
        // if(Input.GetKeyDown(KeyCode.C)) {
        //     Launch();
        // }
    }
    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position + move * speed * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }
    public void ChangeHealth(int amount)
    {
        if(amount < 0) {
            if(isInvincible) {
                return;
            }
            isInvincible = true;
            damageCooldown = timeInvincible;
            animator.SetTrigger("Hit");
        }
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        UIHandler.instance.SetHealthValue(currentHealth / (float)maxHealth);
    }
    void Launch(InputAction.CallbackContext context)
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(moveDirection, 300);
        animator.SetTrigger("Launch");
    }
    void FindFriend(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, moveDirection, 1.5f, LayerMask.GetMask("NPC"));
        if(hit.collider != null)
        {
            NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
            if (character != null)
            {
            UIHandler.instance.DisplayDialogue();
            }
        }
    }
}
