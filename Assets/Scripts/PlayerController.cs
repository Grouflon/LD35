using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject chariotGO;
    public InputController inputController;
    public Power[] powerTemplates;

	void Start ()
    {
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
            m_currentPower = GameObject.Instantiate(powerTemplates[0]);
            m_currentPower.source = chariotGO;
        }
	}

    private Power m_currentPower = null;
}
