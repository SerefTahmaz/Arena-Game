using UnityEngine;
using Cinemachine;
 
public class InputOnMouseDown : MonoBehaviour, AxisState.IInputAxisProvider
{
    public string HorizontalInput = "Mouse X";
    public string VerticalInput = "Mouse Y";

    private bool _isMovement = false;
 
    public float GetAxisValue(int axis)
    {
        // No input unless right mouse is down
        if (_isMovement)
            return 0;
 
        switch (axis)
        {
            case 0: return Input.GetAxis(HorizontalInput);
            case 1: return Input.GetAxis(VerticalInput);
            default: return 0;
        }
    }

    public void StopCamMovement()
    {
        _isMovement = true;
    }
    
    public void StartCamMovement()
    {
        _isMovement = false;
    }
}