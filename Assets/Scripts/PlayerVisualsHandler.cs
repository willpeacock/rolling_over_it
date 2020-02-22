using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisualsHandler : MonoBehaviour {
    public Color[] colorStates;
    public Gradient[] trailGradients;
    public SpriteRenderer outlineColor;
    public TrailRenderer trailRenderer;

    private string currentColorType = "green";
    private string currentTransitionColor = "none";
    private Coroutine colorChangeDelayCo;
    private bool colorChangeDelayCoOn = false;

    public void SetPlayerColorsIfNeeded(string colorType) {
        // ignore the call if the color type was already set or is being transitioned into
        if (colorType.Equals(currentColorType) ||
            colorType.Equals(currentTransitionColor)) {
            return;
        }

        // if a color is still being transitioned, stop it and allow for the new color
        if (colorChangeDelayCoOn && colorChangeDelayCo != null) {
            StopCoroutine(colorChangeDelayCo);
            colorChangeDelayCoOn = false;
            currentTransitionColor = "none";
        }

        if (colorType.Equals("green") || colorType.Equals("orange")) {
            colorChangeDelayCoOn = true;
            currentTransitionColor = colorType;
            colorChangeDelayCo = StartCoroutine(DelayBeforeColorChange(colorType));
        }
        else if (colorType.Equals("red")) {
            outlineColor.color = colorStates[1];
            trailRenderer.colorGradient = trailGradients[1];
            currentColorType = colorType;
        }
        else {
            Debug.LogError("Did not recognize player color type: " + colorType);
        }
    }

    public bool CheckForVisualsGroundedState() {
        return currentColorType.Equals("green");
    }

    IEnumerator DelayBeforeColorChange(string colorType) {
        yield return new WaitForSeconds(0.1f);

        if (colorType.Equals("green")) {
            outlineColor.color = colorStates[0];
            trailRenderer.colorGradient = trailGradients[0];
        }
        else if (colorType.Equals("orange")) {
            outlineColor.color = colorStates[1];
            trailRenderer.colorGradient = trailGradients[1];
        }

        currentColorType = colorType;
        currentTransitionColor = "none";
        colorChangeDelayCoOn = false;
    }
}
