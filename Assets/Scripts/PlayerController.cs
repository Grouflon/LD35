﻿using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject chariotGO;
    public InputController inputController;
    public float recoverTime;
    public int id = -1;

	void Start ()
    {
        m_powerManager = FindObjectOfType<PowerManager>();
	}
	
	void Update ()
    {
        if (m_currentPower != null)
        {
            if (m_currentPower.GetCastState() != Power.CastState.Finished)
            {
                m_currentPower.direction = inputController.GetDirection();
            }
            else
            {
                GameObject.Destroy(m_currentPower.gameObject);
                m_currentPower = null;
            }
        }
        else
        {
            m_recoverTimer += Time.deltaTime;

            if (m_recoverTimer > recoverTime)
            {
                m_currentPower = GameObject.Instantiate(m_powerManager.powerTemplates[0]);

                m_currentPower.source = chariotGO;
                m_recoverTimer = 0.0f;
            }
        }
	}

    private float m_recoverTimer = 0.0f;
    private PowerManager m_powerManager;
    private Power m_currentPower = null;
}
