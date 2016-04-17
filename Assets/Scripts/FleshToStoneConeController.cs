using UnityEngine;
using System.Collections;

public class FleshToStoneConeController : MonoBehaviour {

    public float shootAngle;
    public float flightTime;
    public float gravity;
    public Vector3 target;

	// Use this for initialization
	void Start () {
        Vector3 position = transform.position;
        position.y = 0.0f;
        target.y = 0.0f;
        Vector3 directionToTarget = target - position;
        float distanceToTarget = directionToTarget.magnitude;
        directionToTarget.Normalize();

        //float a = (distanceToTarget * gravity) / Mathf.Sin(2.0f * Mathf.Deg2Rad * shootAngle);
        //float initialVelocity = Mathf.Sqrt(a);
        m_velocity.x = directionToTarget.x * distanceToTarget / flightTime;
        m_velocity.z = directionToTarget.z * distanceToTarget / flightTime;
        m_velocity.y = gravity * flightTime * 0.5f;

        m_collider = GetComponent<SphereCollider>();
        m_collider.enabled = false;
        m_initialScale = transform.localScale;
        transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
	}
	
	// Update is called once per frame
	void Update ()
    {

        m_lifeTime += Time.deltaTime;
        float scaleRatio = Mathf.Clamp(m_lifeTime / (flightTime / 2.0f), 0.0f, 1.0f);
        transform.localScale = m_initialScale * scaleRatio;

        m_velocity += new Vector3(0.0f, -gravity * Time.deltaTime, 0.0f);
        Vector3 position = transform.position;
        position += m_velocity * Time.deltaTime;
        transform.position = position;

        m_collider.enabled = m_velocity.y < 0.0f;

        if (position.y < -4.0f)
        {
            GameObject.Destroy(gameObject);
        }
	}

    private Vector3 m_velocity;
    private SphereCollider m_collider;
    private float m_lifeTime;
    private Vector3 m_initialScale;
}
