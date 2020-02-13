using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour {
    public GameObject bulletObject;
    public Transform bulletSpawnPosition;
    public Rigidbody2D parentRB;
    public Transform playerTransform;
    void Start() {
        StartCoroutine(FireBulletCo());
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            FireBullet();
        }


        Vector3 playerPosition = playerTransform.position;
        Vector2 targetLocation = new Vector2(playerPosition.x, playerPosition.y+2.0f);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetLocation.y - transform.position.y, targetLocation.x - transform.position.x) * Mathf.Rad2Deg);
    }

    void OnBecameVisible() {
        BulletBehavior newBullet = Instantiate(bulletObject, bulletSpawnPosition.position, transform.rotation).GetComponent<BulletBehavior>();
        newBullet.FireBullet(parentRB.velocity.magnitude);
    }

    void FireBullet() {
        BulletBehavior newBullet = Instantiate(bulletObject, bulletSpawnPosition.position, transform.rotation).GetComponent<BulletBehavior>();
        newBullet.FireBullet(parentRB.velocity.magnitude);
    }

    IEnumerator FireBulletCo() {
        while (isActiveAndEnabled) {
            yield return new WaitForSeconds(2.0f);
            FireBullet();
        }
    }
}
