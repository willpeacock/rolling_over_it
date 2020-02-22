using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInput : MonoBehaviour {
    public float GetHorizontalInput() {
        return Input.GetAxis("Horizontal");
    }

    public bool GetJumpButtonDown() {
        return Input.GetButtonDown("Jump");
    }

    public bool GetResetButtonDown() {
        return Input.GetKeyDown("escape");
    }
}
