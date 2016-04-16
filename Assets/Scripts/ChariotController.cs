using UnityEngine;
using System.Collections;

public class ChariotController : MonoBehaviour {

    public Vector3 chariotPosition;
    private float forwardSpeed;
    private float lateralSpeed;

    public ButtZone buttZone;
    public Zone leftZone;
    public Zone rightZone;

    // Use this for initialization

    void Start ()
    {
        forwardSpeed = 0f;
	}

	
	// Update is called once per frame
	void FixedUpdate ()
    {
        // Evaluate Chariot Speed
        forwardSpeed = leftZone.slaveCount + rightZone.slaveCount;
        lateralSpeed = rightZone.slaveCount - leftZone.slaveCount;

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
        Vector3 velocity = new Vector3(lateralSpeed / 10f, 0, forwardSpeed / 10f );
        MoveChariot(velocity);
	}
 
    void MoveChariot(Vector3 chariotDirection)
    {
        chariotPosition += chariotDirection;
        this.transform.position = chariotPosition;
    }

    public void SendSlaveToWork(SlaveController sc)
    {
        // Determine wether the slave should be sent left or right
        if (leftZone.slaveCount < rightZone.slaveCount)
        {
            sc.targetPosition = leftZone.transform.position;
        }
        else
        {
            sc.targetPosition = rightZone.transform.position;
        }
    }

}
