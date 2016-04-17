using UnityEngine;
using System.Collections;

public class ChariotController : MonoBehaviour {

    public Vector3 chariotPosition;
    private float forwardSpeed;
    private float lateralSpeed;

    public ButtZone buttZone;
    public Zone leftZone;
    public Zone rightZone;

    public const int MAX_SPOTS = 20; // on each side of the chariot, has to be a multiple of 4

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

        if (forwardSpeed > MAX_SPOTS*2)
        {
            forwardSpeed = MAX_SPOTS*2;
        }

        if (lateralSpeed > MAX_SPOTS)
        {
            lateralSpeed = MAX_SPOTS;
        }
        else if (lateralSpeed < -MAX_SPOTS)
        {
            lateralSpeed = -MAX_SPOTS;
        }

        forwardSpeed = forwardSpeed / (MAX_SPOTS*2);
        lateralSpeed = lateralSpeed / (MAX_SPOTS*2);

        // Move Chariot according to speed  
        Vector3 velocity = new Vector3(lateralSpeed *Time.deltaTime, 0f, forwardSpeed *Time.deltaTime );
        MoveChariot(velocity);
	}
 
    void MoveChariot(Vector3 chariotDirection)
    {
        chariotPosition = transform.position;
        chariotPosition += chariotDirection;
        this.transform.position = chariotPosition;
    }

    public void SendSlaveToWork(SlaveController sc)
    {
        // Determine wether the slave should be sent left or right
        if (leftZone.slaveCount < rightZone.slaveCount)
        {
            //sc.targetZone = leftZone;
            sc.isWorking = -1;
        }
        else
        {
            //sc.targetZone = rightZone;
            sc.isWorking = 1;
        }
    }

}
