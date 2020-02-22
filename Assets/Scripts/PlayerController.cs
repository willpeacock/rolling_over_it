using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float groundThrust = 10f;
    public float airThrust = 5f;
    public float jumpThrust = 10f;
    public float maxThrustVelocity = 50.0f;
    public float distanceToGround = 0.85f;
    public LayerMask groundLayerMask;
    public AudioSource rollSound;
    public AudioSource jumpSound;
    public AudioSource hitSound;
    public bool delayBeforeStartOn = true;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private PlayerVisualsHandler visualsHandler;
    private PlayerInput playerInput;
    private bool playerCanMove = false;
    private Transform mainCamTransform;
    private bool rollingAudioIsPlaying = false;
    private bool rollingAudioFadingActive = false;
    private bool canPlayWhack = false;
    private bool playerMadeCollision = false;
    private Animator mainAnim;
    private float defaultRollVolume;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        visualsHandler = GetComponent<PlayerVisualsHandler>();
        playerInput = GetComponent<PlayerInput>();
        mainAnim = GetComponent<Animator>();

        defaultRollVolume = rollSound.volume;

        mainCamTransform = Camera.main.transform;
        if (delayBeforeStartOn)
            StartCoroutine(DelayBeforeMoveEnableCo());
        else
            playerCanMove = true;

        StartCoroutine(CheckForDelayInCollisionsForWhack());
    }

    void Update() {
        if (CheckForGround()) {
            visualsHandler.SetPlayerColorsIfNeeded("green");
            if (playerCanMove && playerInput.GetJumpButtonDown()) {
                if (rollingAudioIsPlaying && !rollingAudioFadingActive) {
                    rollingAudioFadingActive = true;
                    StartCoroutine(FadeRollingSound());
                }
                jumpSound.pitch = Random.Range(0.8f, 1.2f);
                jumpSound.Play();
                mainAnim.Play("wilbur_jump");
                rb.AddForce(Vector2.up * jumpThrust, ForceMode2D.Impulse);
            }
        }
        else {
            visualsHandler.SetPlayerColorsIfNeeded("orange");
        }
    }

    void FixedUpdate() {
        if (playerCanMove) {
            HandleHorizontalMovement();
        }
    }

    void HandleHorizontalMovement() {
        float horizontalInput = playerInput.GetHorizontalInput();

        if (!rollingAudioFadingActive && rollingAudioIsPlaying) {
            rollSound.volume = Mathf.Clamp(rb.velocity.magnitude * (defaultRollVolume/10.0f),0, defaultRollVolume);
        }

        if (!rollingAudioIsPlaying && CheckForGround()) {
            if (Mathf.Abs(horizontalInput) > 0 || rb.velocity.magnitude > 1.0f) {
                rollSound.Play();
                rollingAudioIsPlaying = true;
            }
        }
        else if (rollingAudioIsPlaying && !rollingAudioFadingActive) {
            if (!CheckForGround() || Mathf.Abs(horizontalInput) == 0 && rb.velocity.magnitude < 1.0f) {
                rollingAudioFadingActive = true;
                StartCoroutine(DelayBeforeFadeRollingSound(horizontalInput));
            }
        }

        Vector2 moveDirection = new Vector2(horizontalInput, 0);

        float thrust = CheckForGround() ? groundThrust : airThrust;
        
        if (rb.velocity.magnitude <= maxThrustVelocity || Mathf.Sign(rb.velocity.x) != Mathf.Sign(horizontalInput)) {
            rb.AddForce(moveDirection * thrust);
        }
        else {
            Debug.Log("can't add force");
        }
    }

    public void SetPlayerCanMove(bool canMove) {
        playerCanMove = canMove;
    }

    private bool CheckForGround() {
        bool isGrounded = false;

        for (int i = -1; i < 2; i++) {
            Vector3 rayStartPos = new Vector3(transform.position.x + i * (circleCollider.radius/1.5f), transform.position.y, transform.position.z);
            Vector3 rayEndPos = new Vector3(transform.position.x + i * (circleCollider.radius/1.5f), transform.position.y - distanceToGround, transform.position.z);
            Debug.DrawLine(rayStartPos, rayEndPos, Color.red);
            RaycastHit2D hit = Physics2D.Raycast(rayStartPos, Vector2.down, distanceToGround, groundLayerMask);
            if (hit.collider != null) {
                isGrounded = true;
            }
        }

        return isGrounded;
    }

    IEnumerator DelayBeforeMoveEnableCo()  {
        while (mainCamTransform.position.y > 0.5f) {
            yield return null;
        }
        playerCanMove = true;
    }

    IEnumerator DelayBeforeFadeRollingSound(float horizontalInput)  {
        yield return new WaitForSeconds(0.5f);
        // if the rolling sound should still stop
        if (!CheckForGround() || Mathf.Abs(horizontalInput) == 0 && rb.velocity.magnitude < 1.0f) {
            while (rollSound.volume > 0) {
                rollSound.volume -= Time.deltaTime * 5.0f;
                yield return null;
            }
            rollSound.Stop();
            rollSound.volume = defaultRollVolume;
            rollingAudioIsPlaying = false;
        }
        rollingAudioFadingActive = false;
    }

    IEnumerator FadeRollingSound()  {
        while (rollSound.volume > 0) {
            rollSound.volume -= Time.deltaTime * 5.0f;
            yield return null;
        }
        rollSound.Stop();
        rollSound.volume = defaultRollVolume;
        rollingAudioIsPlaying = false;
        rollingAudioFadingActive = false;
    }

    void OnCollisionEnter2D(Collision2D coll) {
        playerMadeCollision = true;
        if (canPlayWhack) {
            hitSound.pitch = Random.Range(0.8f, 1.2f);
            hitSound.Play();
            canPlayWhack = false;
        }
    }

    void OnCollisionStay2D(Collision2D coll) {
        playerMadeCollision = true;
    }

    IEnumerator CheckForDelayInCollisionsForWhack()  {
        while (gameObject.activeSelf) {
            float counter = 0;
            while (counter < 0.25f) {
                // reset timer every time there is a recent collision
                if (playerMadeCollision || visualsHandler.CheckForVisualsGroundedState()) {
                    counter = 0;
                    playerMadeCollision = false;
                }
                counter += Time.deltaTime;
                yield return null;
            }
            canPlayWhack = true;
            // wait for whack sound to play and for canPlayWhack to be reset
            while (canPlayWhack)
                yield return null;
            
            // then reset again with the counter
        }
    }
}
