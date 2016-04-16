using UnityEngine;
using System.Collections;

public class FireballPower : Power {

    public float radius = 2.0f;
    public float inputAcceleration = 1.0f;
    public float inputDeceleration = 1.0f;
    public float maxSpeed = 10.0f;
    public float minSpeed = 2.0f;
    public float castTime = 3.0f;

    public FireballProjectileController projectile;

    public FireballPower()
    {
        m_state = CastState.NotStarted;
    }

    void Start()
    {
        m_state = CastState.Preparing;
        m_target = source.transform.position;
        m_target.y = 0.0f;
        float randAngle = Random.Range(0.0f, Mathf.PI * 2.0f);
        m_targetVelocity.x = Mathf.Cos(randAngle) * minSpeed;
        m_targetVelocity.y = Mathf.Sin(randAngle) * minSpeed;
    }
	
	void Update()
    {
        // UPDATE TIME
        m_castTimer += Time.deltaTime;
        if (m_castTimer > castTime)
        {
            m_state = CastState.Casting;
        }


        switch (m_state)
        {
            case CastState.Preparing:
                {
                    // UPDATE TARGET
                    if (direction.magnitude > Mathf.Epsilon)
                    {
                        m_targetVelocity.x += direction.x * inputAcceleration * Time.deltaTime;
                        m_targetVelocity.y += -direction.y * inputAcceleration * Time.deltaTime;

                        float velocity = m_targetVelocity.magnitude;
                        velocity = Mathf.Min(velocity, maxSpeed);
                        m_targetVelocity = m_targetVelocity.normalized * velocity;
                    }
                    else if (m_targetVelocity.magnitude > minSpeed)
                    {
                        
                        Vector2 breakDirection = -m_targetVelocity.normalized;
                        m_targetVelocity.x += breakDirection.x * inputDeceleration * Time.deltaTime;
                        m_targetVelocity.y += breakDirection.y * inputDeceleration * Time.deltaTime;

                        float velocity = m_targetVelocity.magnitude;
                        velocity = Mathf.Max(velocity, minSpeed);
                        m_targetVelocity = m_targetVelocity.normalized * velocity;
                    }
                    m_target.x += m_targetVelocity.x * Time.deltaTime;
                    m_target.z += m_targetVelocity.y * Time.deltaTime;

                    

                    // CAST
                    if (m_castTimer > castTime)
                    {

                    }
                }
                break;

            case CastState.Casting:
                {
                    Vector3 sourcePosition = source.transform.position;
                    sourcePosition.y = 0.0f;
                    GameObject projectileInstance = (GameObject)GameObject.Instantiate(projectile.gameObject, sourcePosition, Quaternion.identity);
                    projectileInstance.GetComponent<FireballProjectileController>().target = m_target;

                    m_state = CastState.Finished;
                }
                break;

            default:
                break;
        }
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(m_target, radius);
        Debug.DrawLine(m_target, m_target + new Vector3(m_targetVelocity.x, 0.0f, m_targetVelocity.y));
    }

    private float m_castTimer = 0.0f;
    private Vector2 m_targetVelocity;
    private Vector3 m_target;
}
