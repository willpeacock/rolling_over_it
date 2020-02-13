using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {
    public PointEffector2D explosionEffector;
    public LayerMask collideLayerMask;
    public Transform backOfBullet;
    public Rigidbody2D rb;

    private bool hasExploded = false;

    void Update() {
        // Get angle based off rigidbody's velocity
        float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
        // Set the sprite's angle so it looks to be following the trajectory
        transform.GetChild(0).rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    public void FireBullet(float currentPlayerSpeed, float launchSpeed = 50.0f) {
        rb.velocity = transform.right * currentPlayerSpeed;
        rb.AddForceAtPosition(transform.right * launchSpeed, backOfBullet.position, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D coll) {
        // If the object that the bullet collided with is within the specified layer mask...
        if (!hasExploded && collideLayerMask == (collideLayerMask | (1 << coll.gameObject.layer))) {
            StartCoroutine(PlayExplosionThenDie());
            hasExploded = true;
        }
    }

    IEnumerator PlayExplosionThenDie() {
        // disable physics/gravity on bullet
        rb.isKinematic = true;
        // reset velocity so explosion stays in place
        rb.velocity = Vector2.zero;
        // then disable sprites of bullet
        transform.GetChild(0).gameObject.SetActive(false);

        // then enable the explosion force
        explosionEffector.gameObject.SetActive(true);

        // wait a bit for explosion force's effect
        yield return new WaitForFixedUpdate();
        yield return new WaitForFixedUpdate();

        // then destroy the bullet object
        explosionEffector.enabled = false;
        Destroy(gameObject, 0.2f);
    }
}
