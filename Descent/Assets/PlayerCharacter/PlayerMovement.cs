using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed = 5f;

    [Header("Footstep Audio")]
    public AudioSource footstepAudioSource;
    [Tooltip("Seconds between each footstep while moving")]
    public float footstepInterval = 0.35f;
    [Tooltip("Volume of footstep sounds (0 to 1). Lowered to 0.3 by default.")]
    [Range(0f, 1f)]
    public float footstepVolume = 0.3f;

    private Animator animator;
    private SpriteRenderer sprite;
    private float footstepTimer = 0f;

    void Awake()
    {
        if (body == null)
            body = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        sprite   = GetComponent<SpriteRenderer>();

        if (footstepAudioSource == null)
            footstepAudioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        Vector2 move = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed     || Keyboard.current.leftArrowKey.isPressed)  move.x -= 1;
            if (Keyboard.current.dKey.isPressed     || Keyboard.current.rightArrowKey.isPressed) move.x += 1;
            if (Keyboard.current.sKey.isPressed     || Keyboard.current.downArrowKey.isPressed)  move.y -= 1;
            if (Keyboard.current.wKey.isPressed     || Keyboard.current.upArrowKey.isPressed)    move.y += 1;
        }

        move = move.normalized;
        body.linearVelocity = move * speed;

        if (animator != null)
            animator.SetFloat("Speed", move.magnitude);

        if (move.x < 0)       sprite.flipX = true;
        else if (move.x > 0)  sprite.flipX = false;

        HandleFootsteps(move);
    }

    private void HandleFootsteps(Vector2 move)
    {
        if (footstepAudioSource == null || footstepAudioSource.clip == null) return;

        if (move.magnitude > 0f)
        {
            footstepTimer -= Time.fixedDeltaTime;
            if (footstepTimer <= 0f)
            {
                // Use footstepVolume instead of full volume
                footstepAudioSource.PlayOneShot(footstepAudioSource.clip, footstepVolume);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f;
        }
    }
}
