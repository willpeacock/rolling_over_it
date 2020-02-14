using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour {
    public GameObject bulletObject;
    public Transform bulletSpawnPosition;
    public Rigidbody2D parentRB;
    public Transform playerTransform;
    public float bulletFireForce = 100.0f;
    void Start() {
        StartCoroutine(FireBulletCo());
    }

    void Update() {
        Vector3 playerPosition = playerTransform.position;
        float distanceToPlayer = Mathf.Abs(playerPosition.x-transform.position.x);
        float yOffset = distanceToPlayer / Mathf.Clamp(1.0f / Mathf.Pow(0.07f * distanceToPlayer, 2), 1f, 10.0f);
        Vector2 targetLocation = new Vector2(playerPosition.x, playerPosition.y+yOffset);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetLocation.y - transform.position.y, targetLocation.x - transform.position.x) * Mathf.Rad2Deg);
    }

    void OnBecameVisible() {
        BulletBehavior newBullet = Instantiate(bulletObject, bulletSpawnPosition.position, transform.rotation).GetComponent<BulletBehavior>();
        newBullet.FireBullet(parentRB.velocity.magnitude);
    }

    void FireBullet() {
        BulletBehavior newBullet = Instantiate(bulletObject, bulletSpawnPosition.position, transform.rotation).GetComponent<BulletBehavior>();
        newBullet.FireBullet(bulletFireForce);
    }

    IEnumerator FireBulletCo() {
        while (isActiveAndEnabled) {
            yield return new WaitForSeconds(2.0f);
            FireBullet();
        }
    }
}
