using UnityEngine;
using UnityEngine.XR;

public class ToggleStatsCanvas : MonoBehaviour
{
    [SerializeField] private GameObject statsCanvas;

    void Update()
    {
        InputDevice leftDevice = InputDevices.GetDeviceAtXRNode(XRNode.LeftHand);

        if (leftDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool isPressed))
        {
            statsCanvas.SetActive(isPressed);
        }
    }
}
