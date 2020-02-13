using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    public float thrust = 10f;
    public float jumpThrust = 10f;
    public float maxThrustVelocity = 50.0f;
    public float distanceToGround = 0.85f;
    public LayerMask groundLayerMask;

    private Rigidbody2D rb;
    private CircleCollider2D circleCollider;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void Update() {
        if (CheckForGround()) {
            if (Input.GetKeyDown(KeyCode.Space)) {
                rb.AddForce(Vector2.up * jumpThrust, ForceMode2D.Impulse);
            }
        }
    }

    void FixedUpdate() {
        float horizontalInput = Input.GetAxis("Horizontal");

        Vector2 moveDirection = new Vector2(horizontalInput, 0);

        if (rb.velocity.magnitude <= maxThrustVelocity || Mathf.Sign(rb.velocity.x) != Mathf.Sign(horizontalInput)) {
            rb.AddForce(moveDirection * thrust);
        }
        else {
            Debug.Log("can't add force");
        }
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
}
