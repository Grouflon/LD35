using UnityEngine;
using System.Collections;

public class FireballProjectileController : MonoBehaviour {

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

        float a = (distanceToTarget * gravity) / Mathf.Sin(2.0f * Mathf.Deg2Rad * shootAngle);
        float initialVelocity = Mathf.Sqrt(a);
        m_velocity = directionToTarget.normalized * initialVelocity;
        m_velocity = Quaternion.AngleAxis(Mathf.Rad2Deg * shootAngle, new Vector3(-directionToTarget.z, 0.0f, directionToTarget.x)) * m_velocity;
	}
	
	// Update is called once per frame
	void Update () {
        m_velocity += new Vector3(0.0f, -gravity * Time.deltaTime, 0.0f);
        Vector3 position = transform.position;
        position += m_velocity * Time.deltaTime;
        transform.position = position;

        if (position.y < 0.0f)
        {
            GameObject.Destroy(gameObject);
        }
	}

    private Vector3 m_velocity;
}
