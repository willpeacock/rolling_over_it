using UnityEngine;

public class PlayerInput : MonoBehaviour {
    public float GetHorizontalInput() {
        return Input.GetAxis("Horizontal");
    }

    public bool GetJumpButtonDown() {
        return Input.GetButtonDown("Jump");
    }
}
