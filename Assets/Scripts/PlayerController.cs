using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float recoverTime;
    public PlayerDescription playerDescription;

	void Start ()
    {
        m_powerManager = FindObjectOfType<PowerManager>();
        m_headAnimator = playerDescription.chariot.transform.Find("_graphic").Find("_head").GetComponent<Animator>();
    }
	
	void Update ()
    {
        if (m_currentPower != null)
        {
            if (m_currentPower.GetCastState() == Power.CastState.Finished)
            {
                GameObject.Destroy(m_currentPower.gameObject);
                m_currentPower = null;
                m_headAnimator.ResetTrigger("Idle");
            }
        }
        else
        {
            m_recoverTimer += Time.deltaTime;

            if (m_recoverTimer > recoverTime)
            {
                int powerId = Random.Range(0, m_powerManager.powerTemplates.Length - 1);
                m_currentPower = GameObject.Instantiate(m_powerManager.powerTemplates[powerId]);
                m_currentPower.playerDescription = playerDescription;
                m_currentPower.source = playerDescription.chariot.gameObject;
                m_recoverTimer = 0.0f;
            }
        }
	}

    private float m_recoverTimer = 0.0f;
    private PowerManager m_powerManager;
    private Power m_currentPower = null;
    private Animator m_headAnimator;
}
