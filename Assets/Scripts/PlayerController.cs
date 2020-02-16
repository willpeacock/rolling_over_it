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
    public bool delayBeforeStartOn = true;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    private PlayerVisualsHandler visualsHandler;
    private PlayerInput playerInput;
    private bool playerCanMove = false;
    private Transform mainCamTransform;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
        visualsHandler = GetComponent<PlayerVisualsHandler>();
        playerInput = GetComponent<PlayerInput>();

        mainCamTransform = Camera.main.transform;
        StartCoroutine(DelayBeforeMoveEnableCo());
    }

    void Update() {
        if (CheckForGround()) {
            visualsHandler.SetPlayerColorsIfNeeded("green");
            if (playerCanMove && playerInput.GetJumpButtonDown()) {
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
}
