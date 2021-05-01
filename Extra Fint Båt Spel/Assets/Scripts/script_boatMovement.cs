using UnityEngine;

public class script_boatMovement : MonoBehaviour
{
    private Rigidbody m_rigidbody;
    [SerializeField] private Transform motor;
    [SerializeField] private float speed;
    [SerializeField] private float turnSpeed;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

        // steering
        float turn = 0f;
        if (Input.GetKey(KeyCode.A)) { turn = 1f; }
        if (Input.GetKey(KeyCode.D)) { turn = -1f; }

        // rotates boat by applying sideways force to the motors position
        m_rigidbody.AddForceAtPosition(turn * -transform.forward * turnSpeed, motor.position);

        // obsolete?
        //Vector3 forwardScaled = Vector3.Scale(new Vector3(1, 0, 1), transform.right);
        //Vector3 targetVelocity = Vector3.zero;

        // forwards & backwards movement
        if (Input.GetKey(KeyCode.W)) { m_rigidbody.AddForceAtPosition(transform.right * speed * 100f * Time.deltaTime, motor.position, ForceMode.Force); }
        if (Input.GetKey(KeyCode.S)) { m_rigidbody.AddForceAtPosition(transform.right * -speed * 100f * Time.deltaTime, motor.position, ForceMode.Force); }

    }
}
