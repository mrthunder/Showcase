using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField, ChildGameObjectsOnly]
    private Transform _mainCamera;

    [SerializeField]
    private Vector3 _cameraOffset = new Vector3(0, 1, -5);

    [SerializeField]
    private Vector2 _pitchRotationConstraints = new Vector2(0, 30);

    private Vector2 _rotationDirection = Vector2.zero;

    [SerializeField,SceneObjectsOnly]
    private GameObject _player;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }
    private void Update()
    {
        FollowPlayer();
        RotateCamera(_rotationDirection);
    }

    public void GetRotationDirection(InputAction.CallbackContext context)
    {
        Vector2 rotationDirection = context.ReadValue<Vector2>();
        if (_rotationDirection != rotationDirection)
        {
            _rotationDirection = rotationDirection;
        }
    }

    public void RotateCamera(Vector2 rotationDirection)
    {
        Vector3 mainCameraEuler = _mainCamera.localEulerAngles;
        mainCameraEuler.x += _rotationDirection.y;
        mainCameraEuler.x = Mathf.Clamp(mainCameraEuler.x, _pitchRotationConstraints.x, _pitchRotationConstraints.y);
        _mainCamera.localEulerAngles = mainCameraEuler;

        //----------------------

        Vector3 cameraBoomEuler = transform.localEulerAngles;
        cameraBoomEuler.y += _rotationDirection.x;
        //cameraBoomEuler.y = Mathf.Clamp(cameraBoomEuler.y, -45.0f, 45.0f);
        transform.localEulerAngles = cameraBoomEuler;

    }

    public void FollowPlayer()
    {
        transform.position = _player.transform.position+_cameraOffset;
    }


}
