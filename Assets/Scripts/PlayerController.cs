using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float collisionOffset = 0.05f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    public int maxHealth = 100;
    public int currentHealth;

    public HealthBar healthBar;

    Vector2 movementInput;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Animator animator;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();

    bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    [System.Obsolete]
    private void FixedUpdate() 
    {
        if (maxHealth <= 0)
        {
            Application.LoadLevel(Application.loadedLevel);
        }

        if (canMove) 
        {
            if(movementInput != Vector2.zero)
            {
               
                bool success = TryMove(movementInput);

                if(!success) 
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }

                if(!success) 
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
               
                animator.SetBool("isMoving", success);
            } 
            else 
            {
                animator.SetBool("isMoving", false);
            }

            if(movementInput.x < 0) 
            {
                spriteRenderer.flipX = true;
            } 
            else if (movementInput.x > 0) 
            {
                spriteRenderer.flipX = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Enemy")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                TakeDamage(4);
            }
        }

        if (other.tag == "Boss")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                TakeDamage(10);
            }
        }

        if (other.tag == "Slime")
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null)
            {
                TakeDamage(1);
            }
        }
    }

    private bool TryMove(Vector2 direction) 
    {
        if(direction != Vector2.zero) 
        {
            int count = rb.Cast(
                direction,
                movementFilter,
                castCollisions,
                moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if(count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            } 
            else 
            {
                return false;
            }
        } 
        else 
        {
            return false;
        }
       
    }

    void OnMove(InputValue movementValue) 
    {
        movementInput = movementValue.Get<Vector2>();
    }
    
    void OnFire() 
    {
        animator.SetTrigger("swordAttack");
    }

    public void SwordAttack() 
    {
        LockMovement();

        if(spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        } 
        else 
        {
            swordAttack.AttackRight();
        }
    }

    public void EndSwordAttack() 
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    public void LockMovement() 
    {
        canMove = false;
    }

    public void UnlockMovement() 
    {
        canMove = true;
    }

    void TakeDamage(int damage)
    {
        currentHealth -= damage;

        healthBar.SetHealth(currentHealth);
    }
}