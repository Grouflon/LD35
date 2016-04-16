using UnityEngine;
using System.Collections;

public class ChariotController : MonoBehaviour {

    public Vector2 chariotPosition;
    private float forwardSpeed;
    private float lateralSpeed;

    public Collider buttZone;
    public Collider leftZone;
    public Collider rightZone;

    public int leftWorkForce;
    public int rightWorkForce;

    public Vector2 initialPosition;

    // Use this for initialization

    void Start ()
    {
        chariotPosition = initialPosition;
        forwardSpeed = 0f;
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // Evaluate Chariot Speed
        forwardSpeed = leftWorkForce + rightWorkForce;
        lateralSpeed = rightWorkForce - leftWorkForce;

        if (forwardSpeed > 100)
        {
            forwardSpeed = 100;
        }

        if (lateralSpeed > 50)
        {
            lateralSpeed = 50;
        }
        else if (lateralSpeed < -50)
        {
            lateralSpeed = -50;
        }

        // Move Chariot according to speed  
        Vector2 velocity = new Vector2(forwardSpeed / 10f, lateralSpeed / 10f);
        MoveChariot(velocity);
	}
 
    void MoveChariot(Vector2 chariotDirection)
    {
        chariotPosition += chariotDirection;
        this.transform.position = chariotPosition;
    }

    public void SendSlaveToWork(SlaveController sc)
    {
        // Determine wether the slave should be sent left or right
        if (leftWorkForce < rightWorkForce)
        {
            sc.targetPosition = leftZone.transform.position;
        }
        else
        {
            sc.targetPosition = rightZone.transform.position;
        }
    }

}
