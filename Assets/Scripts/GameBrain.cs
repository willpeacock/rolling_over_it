using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBrain : MonoBehaviour {
    public PlayerController playerController;

    private Rigidbody2D playerRB;

    void Start() {
        playerRB = playerController.GetComponent<Rigidbody2D>();
    }

    public void PlayerPickedUpOrb() {
        playerController.SetPlayerCanMove(false);
        playerRB.velocity = Vector2.zero;
        playerRB.isKinematic = true;
    }
}
