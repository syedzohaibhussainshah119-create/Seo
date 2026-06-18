using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class VehicleController : MonoBehaviour
{
    [Header("Wheels (WheelCollider order should match wheel meshes)")]
    public WheelCollider[] drivenWheels; // set in inspector (e.g., FL, FR, RL, RR)
    public Transform[] wheelMeshes;      // corresponding visual meshes

    [Header("Performance")]
    public float maxMotorTorque = 1600f;
    public float maxBrakeTorque = 4000f;
    public float maxSteerAngle = 30f;
    public float topSpeedKph = 240f;

    [Header("Drift")]
    [Range(0.6f, 1f)]
    public float driftFactor = 0.92f; // lower = more drift
    public float handbrakeBrakeMultiplier = 0.95f;

    [Header("Feedback")]
    public AudioSource crashSfx;
    public ParticleSystem skidParticles;

    Rigidbody rb;
    bool isHandbrake;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0.2f);
    }

    void Update()
    {
        HandleInputAndVisuals();
    }

    void HandleInputAndVisuals()
    {
        float steer = Input.GetAxis("Horizontal");
        float accel = Input.GetAxis("Vertical");
        isHandbrake = Input.GetKey(KeyCode.Space);

        float speed = rb.velocity.magnitude * 3.6f;
        float steerAngle = maxSteerAngle * steer * Mathf.Clamp01(1f - (speed / topSpeedKph));

        // Apply motor/steer/brake to wheel colliders
        for (int i = 0; i < drivenWheels.Length; i++)
        {
            WheelCollider wc = drivenWheels[i];

            // Simple drivetrain: distribute torque equally to motorized wheels
            if (Mathf.Abs(accel) > 0.01f)
                wc.motorTorque = accel * (maxMotorTorque / drivenWheels.Length);
            else
                wc.motorTorque = 0f;

            // Heuristic: first two wheels are front for steering
            wc.steerAngle = steerAngle;

            // Handbrake: apply strong brake to rear wheels only (set rear wheels in inspector)
            if (isHandbrake)
                wc.brakeTorque = maxBrakeTorque * handbrakeBrakeMultiplier;
            else
                wc.brakeTorque = 0f;

            // Sync visual mesh
            if (i < wheelMeshes.Length && wheelMeshes[i] != null)
            {
                Quaternion quat;
                Vector3 pos;
                wc.GetWorldPose(out pos, out quat);
                wheelMeshes[i].position = pos;
                wheelMeshes[i].rotation = quat;
            }
        }

        // Drifting: reduce lateral velocity
        bool isDrifting = isHandbrake || Mathf.Abs(Vector3.Dot(transform.right, rb.velocity.normalized)) > 0.35f;
        if (isDrifting)
        {
            Vector3 lateral = transform.right * Vector3.Dot(rb.velocity, transform.right);
            rb.AddForce(-lateral * (1f - driftFactor) * 10f, ForceMode.Acceleration);
            if (skidParticles && !skidParticles.isPlaying) skidParticles.Play();
        }
        else
        {
            if (skidParticles && skidParticles.isPlaying) skidParticles.Stop();
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        float impulse = collision.impulse.magnitude / Time.fixedDeltaTime;
        // threshold empirically tuned
        if (impulse > 1200f)
        {
            OnCrash(impulse);
        }
    }

    void OnCrash(float intensity)
    {
        if (crashSfx) crashSfx.Play();
        // Camera shake and damage systems would be triggered here
        StartCoroutine(TemporaryDisable(0.4f));
    }

    System.Collections.IEnumerator TemporaryDisable(float t)
    {
        enabled = false;
        yield return new WaitForSeconds(t);
        enabled = true;
    }
}
