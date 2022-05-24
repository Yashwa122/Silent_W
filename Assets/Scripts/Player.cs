using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;
    Animator animator;
    string currentState;
    bool isMoving = false;

    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }

        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        
        moveDelta = new Vector3(x,y,0);

        //moving
        if (Mathf.Abs(x) + Mathf.Abs(y) != 0)
        {
            isMoving = true;

        }
        else
        {
            isMoving = false;
            PlayAnim("idle down");
            
        }

        if (moveDelta.x > 0)
        {
            //transform.localScale = Vector3.one;
            PlayAnim("right walk");

        }
        else if (moveDelta.x < 0)
        {
            PlayAnim("left walk");
        }
        else if (moveDelta.y > 0)
        {
            //transform.localScale = Vector3.one;
            PlayAnim("up walk");

        }
        else if (moveDelta.y < 0)
        {
            PlayAnim("down walk");
        }

        //transform.localScale = new Vector3(-1, 1, 1);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
            
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x,0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }

    //animation
    void PlayAnim(string newState)
    {
        if (currentState == newState) return;
        animator.Play(newState);
            
        currentState = newState;
       
    }
}
