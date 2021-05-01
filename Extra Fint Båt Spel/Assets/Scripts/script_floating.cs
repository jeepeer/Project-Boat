using UnityEngine;

public class script_floating : MonoBehaviour
{
    private Rigidbody m_rigidbody;

    //private float waterSurfaceHeight = 0f;
    [SerializeField] private float depthUntilBuoyancy = 1f; // Depth where buoyancy kicks in
    [SerializeField] private float depthLimit = 3f;         // Depth where max buyancy force as being applied, beyond this will still apply the same amount of force
    [SerializeField] private float byouancyForce = 0f;
    [SerializeField] private float waterDrag = 0.5f;
    [SerializeField] private float waterAngularDrag = 0.5f;

    [SerializeField] private int floatingAmount = 4;
    [SerializeField] private float gravityMultiplier = 1f;

    [SerializeField] private bool isTesting = false;
    private float waveHeight = 0f;

    private script_waveGenerator Waves;

    private void Start()
    {
        Waves = FindObjectOfType<script_waveGenerator>();

        m_rigidbody = GetComponentInParent<Rigidbody>();
        if (!m_rigidbody) Debug.LogWarning("Unsuccessfully assigned Rigidbody");

        if (!isTesting) waveHeight = 0f;
    }

    private void FixedUpdate()
    {
        // Apply an extra gravitational force on the boat distributed eqaully among all floaters
        m_rigidbody.AddForceAtPosition((Physics.gravity / floatingAmount) * gravityMultiplier, transform.position, ForceMode.Acceleration);
        //if(isTesting) waveHeight = script_waveAlgorithm.instance.GetWaveHeight(transform.position.x);
        if(isTesting) waveHeight = Waves.GetHeight(transform.position);
        if (transform.position.y < waveHeight)
        {
            // Approximates in percentage how far below the water surface the object is
            // Then use that percentage to apply a lagom amount of force to the floating object (lagom being determined by the depth)
            //float byouancuForceMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthUntilBuoyancy) * depthLimit;

            byouancyForce = Mathf.Clamp01((waveHeight - transform.position.y) / depthUntilBuoyancy) * depthLimit;
            m_rigidbody.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y * byouancyForce), 0f), transform.position, ForceMode.Acceleration);
            m_rigidbody.AddForce(byouancyForce * -m_rigidbody.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            m_rigidbody.AddTorque(byouancyForce * -m_rigidbody.angularVelocity * waterAngularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}