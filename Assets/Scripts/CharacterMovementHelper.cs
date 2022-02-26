using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class CharacterMovementHelper : MonoBehaviour
{
    private XROrigin _xrOrigin;
    private CharacterController _characterController;
    private CharacterControllerDriver _driver;

    void Update()
    {
        UpdateCharacterController();
    }
    
    /// <summary>
    /// Updates the <see cref="CharacterController.height"/> and <see cref="CharacterController.center"/>
    /// based on the camera's position.
    /// </summary>
    protected virtual void UpdateCharacterController()
    {
        if (_xrOrigin == null || _characterController == null)
            return;

        var height = Mathf.Clamp(_xrOrigin.CameraInOriginSpaceHeight, _driver.minHeight, _driver.maxHeight);

        Vector3 center = _xrOrigin.CameraInOriginSpacePos;
        center.y = height / 2f + _characterController.skinWidth;

        _characterController.height = height;
        _characterController.center = center;
    }
}
