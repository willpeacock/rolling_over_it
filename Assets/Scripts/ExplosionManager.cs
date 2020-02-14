using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionManager : MonoBehaviour {
    public PhysicsMaterial2D bouncyMaterial;

    private Dictionary<Rigidbody2D, Coroutine> bouncyCoroutineByRB;
    private Dictionary<Rigidbody2D, PhysicsMaterial2D> defaultMaterialByRB;
    
    private const float bouncyMaterialActiveTime = 0.2f;

    void Start() {
        bouncyCoroutineByRB = new Dictionary<Rigidbody2D, Coroutine>();
        defaultMaterialByRB = new Dictionary<Rigidbody2D, PhysicsMaterial2D>();
    }

    public void StartBouncyCoForRigidbody(Rigidbody2D rb) {
        // if there is an active coroutine, end it so it doesn't interfere with the new one starting
        if (bouncyCoroutineByRB.ContainsKey(rb) && bouncyCoroutineByRB[rb] != null) {
            StopCoroutine(bouncyCoroutineByRB[rb]);
            bouncyCoroutineByRB.Remove(rb);
            rb.sharedMaterial = defaultMaterialByRB[rb];
        }
        bouncyCoroutineByRB[rb] = StartCoroutine(BouncyCoForRigidbody(rb));
    }

    IEnumerator BouncyCoForRigidbody(Rigidbody2D rb) {
        PhysicsMaterial2D defaultMaterial = rb.sharedMaterial;
        if (!defaultMaterialByRB.ContainsKey(rb)) {
            defaultMaterialByRB[rb] = defaultMaterial;
        }

        // apply the bouncy physics material to all colliders attatched to the rigidbody
        rb.sharedMaterial = bouncyMaterial;

        // give it some time to bounce
        yield return new WaitForSeconds(bouncyMaterialActiveTime);

        // then return to the old material
        rb.sharedMaterial = defaultMaterial;

        // indicate that the coroutine for this rigibody is finished
        bouncyCoroutineByRB.Remove(rb);
    }
}
