﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class CannonBehavior : MonoBehaviour {
    [Range(0.5f,5.0f)]
    public float timeBetweenFiring = 2.0f;
    public GameObject bulletObject;
    public Transform bulletSpawnPosition;
    public Rigidbody2D parentRB;
    public Transform playerTransform;
    public SpriteShapeRenderer cannonColorSR;
    public float bulletFireForce = 100.0f;

    private Animator cannonAnim;
    private bool fireBulletCoOn = true;
    void Start() {
        cannonAnim = GetComponent<Animator>();
        StartCoroutine(FireBulletCo());
    }

    void Update() {
        Vector3 playerPosition = playerTransform.position;
        float distanceToPlayer = Mathf.Abs(playerPosition.x-transform.position.x);
        float yOffset = distanceToPlayer / Mathf.Clamp(1.0f / Mathf.Pow(0.07f * distanceToPlayer, 2), 1f, 10.0f);
        Vector2 targetLocation = new Vector2(playerPosition.x, playerPosition.y+yOffset);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(targetLocation.y - transform.position.y, targetLocation.x - transform.position.x) * Mathf.Rad2Deg);

        if (!fireBulletCoOn && VisibleByCamera()) {
            fireBulletCoOn = true;
            StartCoroutine(FireBulletCo());
        }
    }

    public void FireBullet() {
        BulletBehavior newBullet = Instantiate(bulletObject, bulletSpawnPosition.position, transform.rotation).GetComponent<BulletBehavior>();
        newBullet.SetBulletColor(cannonColorSR.color);
        newBullet.FireBullet(bulletFireForce);
    }

    private bool VisibleByCamera() {
        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        return screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
    }

    IEnumerator FireBulletCo() {
        while (VisibleByCamera()) {
            yield return new WaitForSeconds(timeBetweenFiring);
            cannonAnim.Play("cannon_fire");
        }
        fireBulletCoOn = false;
    }
}
