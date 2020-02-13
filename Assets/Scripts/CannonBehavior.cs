using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonBehavior : MonoBehaviour {
    public GameObject bulletObject;
    public Transform bulletSpawnPosition;
    public Rigidbody2D parentRB;
    void Start() {
        
    }

    void Update() {
        if (Input.GetButtonDown("Fire1")) {
            BulletBehavior newBullet = Instantiate(bulletObject, bulletSpawnPosition.position, transform.rotation).GetComponent<BulletBehavior>();
            newBullet.FireBullet(parentRB.velocity.magnitude);
        }


        Vector3 mouseScreen = Input.mousePosition;
        Vector3 mouse = Camera.main.ScreenToWorldPoint(mouseScreen);

        transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x) * Mathf.Rad2Deg);
    }
}
