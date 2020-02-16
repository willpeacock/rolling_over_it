using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TheOrbBehavior : MonoBehaviour {
    public GameBrain gameBrain;
    public GameObject orbInstructionGraphics;

    private Transform mainCamTransform;

    void Start() {
        mainCamTransform = Camera.main.transform;
        StartCoroutine(DisplayOrbInstructionCo());
        orbInstructionGraphics.SetActive(true);
    }

    void Update() {
        transform.Rotate(Vector3.right * Time.deltaTime * 100.0f, Space.World);
    }
    void OnTriggerEnter2D(Collider2D coll) {
        // If the player comes into contact with the orb...
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player")) {
            gameBrain.PlayerPickedUpOrb();
            // after sending signal, destroy the orb
            Destroy(gameObject);
        }
    }

    IEnumerator DisplayOrbInstructionCo() {
        while (mainCamTransform.position.y > 0.5f)  {
            yield return null;
        }
        orbInstructionGraphics.SetActive(false);
    }
}
