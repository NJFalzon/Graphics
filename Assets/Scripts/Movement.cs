using UnityEngine;

[RequireComponent(typeof(Rigidbody))]

public class Movement : MonoBehaviour
{
    [SerializeField] KeyCode forward;
    [SerializeField] KeyCode forwardAlternate;
    [Space]
    [SerializeField] KeyCode backward;
    [SerializeField] KeyCode backwardAlternate;
    [Space]
    [SerializeField] KeyCode left;
    [SerializeField] KeyCode leftAlternate;
    [Space]
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode rightAlternate;

    private Rigidbody rb;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Look();
    }

    void FixedUpdate()
    {
        Move();
    }

    void MoveForward()
    {
        if (Input.GetKey(forward) || Input.GetKey(forwardAlternate))
        {
            rb.AddForce(transform.forward * 5, ForceMode.Impulse);
        }
    }

    void MoveBackward()
    {
        if (Input.GetKey(backward) || Input.GetKey(backwardAlternate))
        {
            rb.AddForce(-transform.forward * 5, ForceMode.Impulse);
        }
    }

    void MoveRight()
    {
        if (Input.GetKey(right) || Input.GetKey(rightAlternate))
        {
            rb.AddForce(transform.right * 5, ForceMode.Impulse);
        }
    }

    void MoveLeft()
    {
        if (Input.GetKey(left) || Input.GetKey(leftAlternate))
        {
            rb.AddForce(-transform.right * 5, ForceMode.Impulse);
        }
    }


    void Look()
    {
        float x = Input.GetAxis("Mouse X") * 2;

        transform.Rotate(new Vector3(0, x, 0));
    }

    void Move()
    {
        MoveForward();
        MoveBackward();
        MoveLeft();
        MoveRight();
        LimitVelocity();
    }

    void LimitVelocity()
    {
        if(rb.velocity.magnitude > 5)
        {
            rb.velocity = rb.velocity.normalized*5;
        }
    }
}
