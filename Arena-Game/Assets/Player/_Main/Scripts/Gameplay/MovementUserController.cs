
using PlayerCharacter;
using UnityEngine;

public class MovementUserController : MonoBehaviour
{
    public IInputManager InputManager
    {
        get => _inputManager;
        set => _inputManager = value;
    }

    private Transform _target => Camera.main.transform;
    
    private float _horizontalAxis;
    private float _verticalAxis;
    private float _moveScale = 1;
    private float _moveActivateScale = 1;

    [SerializeField] private MovementController _movementController;
    private IInputManager _inputManager;

    private void Start()
    {
        _movementController = GetComponent<MovementController>();
        _inputManager.AddListenerToOnWalkSpeedUpEvent((() =>
        {
            _moveScale = .5f;
        }));
        _inputManager.AddListenerToOnWalkSpeedNormalEvent((() =>
        {
            _moveScale = 1;
        }));
    }

    public void Movement()
    {
        _horizontalAxis = _inputManager.HorizontalAxis;
        _verticalAxis = _inputManager.VerticalAxis;
    
        Vector3 cameraForward = _target.forward;
        cameraForward = Vector3.Scale(cameraForward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraDirection = cameraForward * _verticalAxis + _target.right * _horizontalAxis;
    
        _movementController.Move(cameraDirection * _moveScale * _moveActivateScale);
    }

    public void StopMovement()
    {
        _moveActivateScale = 0;
    }

    public void StartMovement()
    {
        _moveActivateScale = 1;
    }

    public void ActivateRotation()
    {
        _movementController.ActivateRotation();
    }
    
    public void DisableRotation()
    {
        _movementController.DisableRotation();
    }
}
