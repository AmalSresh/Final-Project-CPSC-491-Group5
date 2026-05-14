using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D body;
    public float speed = 5f;

    [Header("Footstep Audio")]
    [Tooltip("AudioSource on this GameObject whose clip is your footstep sound. " +
             "Add an AudioSource component to the Player prefab, set its clip to " +
             "your footstep .wav/.ogg, Play On Awake = OFF, Loop = OFF.")]
    public AudioSource footstepAudioSource;
    [Tooltip("Seconds between each footstep while moving")]
    public float footstepInterval = 0.35f;

    private Animator animator;
    private SpriteRenderer sprite;
    private float footstepTimer = 0f;

    void Awake()
    {
        if (body == null)
            body = GetComponent<Rigidbody2D>();

        animator = GetComponent<Animator>();
        sprite   = GetComponent<SpriteRenderer>();

        // Auto-grab if not assigned in Inspector
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

        // Flip sprite left/right
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
                footstepAudioSource.PlayOneShot(footstepAudioSource.clip);
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            // Reset so first step fires immediately when moving again
            footstepTimer = 0f;
        }
    }
}
