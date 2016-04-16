using UnityEngine;
using System.Collections;

public class SlaveController : MonoBehaviour
{
    public Zone targetZone;
    public ChariotController masterChariot;
    public int strength;
    public int isWorking;

    void Start()
    {
        strength = 1;
        targetZone = masterChariot.buttZone;
    }

    void Update()
    {
        // Determine target zone
        if (isWorking == 0)
        {
            targetZone = masterChariot.buttZone;
        }
        else if (isWorking == -1)
        {
            targetZone = masterChariot.leftZone;
        }
        else if (isWorking == 1)
        {
            targetZone = masterChariot.rightZone;
        }

        // Move
        Vector3 target = targetZone.transform.position;
        MoveSlaveAss(target);
        
    }

    void MoveSlaveAss(Vector3 target)
    {
        // Set Move direction
        Vector3 slavePosition = this.transform.position;
        Vector3 newDirection = target - slavePosition;
        newDirection.y = 0f;

        // Compute new position
        newDirection.Normalize();
        newDirection.z = newDirection.z * strength * Time.deltaTime;
        newDirection.z += Random.Range(-0.6f, 0.6f) * newDirection.z;
        newDirection.x = newDirection.x * strength * Time.deltaTime;
        newDirection.x += Random.Range(-0.6f, 0.6f) * newDirection.x;
        slavePosition += newDirection;

        // Go there
        this.transform.position = slavePosition;
    }


    void DestroySlave()
    {
        Destroy(this.gameObject, 0.3f);
    }





}
