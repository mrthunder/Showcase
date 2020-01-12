using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class MovementComponent : MonoBehaviour
{
    [SerializeField]
    private float _MovementSpeed = 2f;

    private Vector2 _MovementDirection = Vector2.zero;

    private Transform _MainCamera;

    private Rigidbody _Rb;

    [SerializeField]
    private Animator _Anim;

    // Start is called before the first frame update
    void Start()
    {
        this._MainCamera = Camera.main.transform;
        this._Rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveGameObject();
    }

    private void MoveGameObject()
    {
        Vector3 direction3D = new Vector3(this._MovementDirection.x, 0, this._MovementDirection.y);
        Vector3 transformedDirection = this._MainCamera.TransformDirection(direction3D);
        transformedDirection.y = 0;
        if (direction3D != Vector3.zero)
        {
            transform.forward = transformedDirection;
        }
        transformedDirection *= this._MovementSpeed;
        transformedDirection.y = this._Rb.velocity.y;
        this._Rb.velocity = transformedDirection;

        if (this._Anim)
        {
            float speed = (Mathf.Abs(this._MovementDirection.x) + Mathf.Abs(this._MovementDirection.y)) / 2;
            this._Anim.SetFloat(Animator.StringToHash("MovementSpeed"), speed);
        }
    }

    public void GetMovementDirection(InputAction.CallbackContext context)
    {
        Vector2 direction = context.ReadValue<Vector2>();
        this._MovementDirection = direction;
    }

    public void GetMovementDirection(Vector2 direction)
    {
        this._MovementDirection = direction;
    }

}
