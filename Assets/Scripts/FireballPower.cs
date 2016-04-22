using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireballPower : Power {

    public float radius = 2.0f;
    public float inputAcceleration = 1.0f;
    public float inputDeceleration = 1.0f;
    public float maxSpeed = 10.0f;
    public float minSpeed = 2.0f;
    public float castTime = 3.0f;
    public float targetStartSpeed = 10.0f;
    public float targetEndSpeed = 60.0f;

    public FireballProjectileController projectilePrefab;
    public Text textPrefab;
    public GroundTargetController groundTargetPrefab;

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

        m_groundTarget = (GroundTargetController)Instantiate(groundTargetPrefab, m_target, Quaternion.identity);
        Vector3 scale = m_groundTarget.transform.localScale;
        scale.x = scale.z = radius;
        m_groundTarget.transform.localScale = scale * 2.0f;
        m_groundTarget.color = playerDescription.color;

        GameObject UI = GameObject.Find("_UI");
        m_castTimerText = Instantiate(textPrefab);
        m_castTimerText.rectTransform.SetParent(UI.transform);
        //m_castTimerText.fontSize = 30;
        m_castTimerText.fontStyle = FontStyle.Bold;

        m_headAnimator = playerDescription.chariot.transform.Find("_graphic").Find("_head").GetComponent<Animator>();
        m_timer = 0.0f;
    }
	
	void Update()
    {
        // UPDATE TIME
        m_timer += Time.deltaTime;
        
        switch (m_state)
        {
            case CastState.Preparing:
                {
                    Vector2 direction = playerDescription.input.GetDirection();

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

                    if (m_castTimerText)
                    {
                        Vector3 screenPoint = Camera.main.WorldToScreenPoint(m_target);
                        m_castTimerText.rectTransform.position = screenPoint;
                        m_castTimerText.text = "" + (int)Mathf.Ceil(castTime - m_timer);
                    }

                    m_groundTarget.angularSpeed = (targetEndSpeed - targetStartSpeed) * (m_timer / castTime);
                    Vector3 targetPosition = m_groundTarget.transform.position;
                    targetPosition = m_target;
                    targetPosition.y = 0.01f;
                    m_groundTarget.transform.position = targetPosition;

                    if (m_timer > castTime - 2.0f)
                    {
                        m_headAnimator.SetTrigger("Fireball");
                    }

                    if (m_timer > castTime)
                    {
                        m_timer = 0.0f;
                        if (m_castTimerText != null)
                        {
                            GameObject.Destroy(m_castTimerText.gameObject);
                            m_castTimerText = null;
                        }

                        Vector3 sourcePosition = source.transform.position;
                        sourcePosition.y = 0.0f;
                        GameObject projectileInstance = (GameObject)GameObject.Instantiate(projectilePrefab.gameObject, sourcePosition, Quaternion.identity);
                        FireballProjectileController projectileController = projectileInstance.GetComponent<FireballProjectileController>();
                        projectileController.target = m_target;
                        projectileInstance.transform.localScale = new Vector3(radius, radius, radius) * 2.0f;

                        m_groundTarget.FadeOut(projectileController.flightTime + 0.5f);
                        m_state = CastState.Casting;
                        m_headAnimator.ResetTrigger("Fireball");
                    }
                }
                break;

            case CastState.Casting:
                {
                    if (m_timer > projectilePrefab.flightTime)
                    {
                        m_state = CastState.Finished;
                        m_headAnimator.SetTrigger("Idle");
                    }
                }
                break;

            default:
                break;
        }
	}


    private float m_timer = 0.0f;
    private Vector2 m_targetVelocity;
    private Vector3 m_target;
    private Text m_castTimerText;
    private GroundTargetController m_groundTarget;
    private Animator m_headAnimator;
}
