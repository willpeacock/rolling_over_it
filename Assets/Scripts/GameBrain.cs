using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameBrain : MonoBehaviour {
    public PlayerController playerController;
    public Transform[] elevatorTransforms;
    public PlayerInput playerInput;
    public CinemachineVirtualCamera followCam;

    private Rigidbody2D playerRB;

    void Start() {
        playerRB = playerController.GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (playerInput.GetResetButtonDown()) {
            ResetGame();
        }
    }

    private void ResetGame() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void PlayerPickedUpOrb() {
        playerController.SetPlayerCanMove(false);
        playerRB.velocity = Vector2.zero;
        playerRB.isKinematic = true;
        followCam.m_Follow = null;

        StartCoroutine(EndGame());
    }

    IEnumerator EndGame() {
        yield return new WaitForSeconds(2.0f);
        float counter = 0;
        while (counter < 5.0f) {
            foreach(Transform elevatorTransform in elevatorTransforms) {
                elevatorTransform.Translate(Vector2.up * Time.deltaTime * 4.0f, Space.World);
            }
            counter += Time.deltaTime;
            yield return null;
        }

        ResetGame();
    }
}
