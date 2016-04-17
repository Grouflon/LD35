using UnityEngine;
using System.Collections;

public class SlaveController : MonoBehaviour
{
    public GameObject targetZone;
    public ChariotController masterChariot;
    public int strength;
    public int isWorking;
    public float deathTime = 3.0f;
    public float deathStrength = 5.0f;

    void Start()
    {
        strength = 1;
        targetZone = masterChariot.buttZone.gameObject;
    }

    void Update()
    {
        // Determine target zone
        if (isWorking == 0)
        {
            targetZone = masterChariot.buttZone.gameObject;
        }
        else if (isWorking == -1 && targetZone.GetComponent<Zone>()!=null)// worker is not assigned to a spot yet
        {
            targetZone = masterChariot.leftZone.gameObject;
        }
        else if (isWorking == 1 && targetZone.GetComponent<Zone>() != null)// worker is not assigned to a spot yet
        {
            targetZone = masterChariot.rightZone.gameObject;
        }
        // else if the slave is already assigned to a spot, its target does not change

        // Move
        Vector3 target = targetZone.transform.position;
        MoveSlaveAss(target);
        
    }

    void MoveSlaveAss(Vector3 target)
    {
        if (!m_isDead)
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
    }


    void DestroySlave(Vector3 _direction)
    {
        m_isDead = true;
        Rigidbody rigidbody = GetComponent<Rigidbody>();
        rigidbody.isKinematic = false;
        rigidbody.useGravity = true;
        rigidbody.AddForceAtPosition(_direction * deathStrength, _direction * 0.01f, ForceMode.Impulse);

        gameObject.GetComponentInChildren<Billboard>().enabled = false;

        Destroy(this.gameObject, deathTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Lethal")
        {
            Vector3 direction = transform.position - other.transform.position;
            if (direction.y < 0) direction.y = -direction.y;
            direction.Normalize();
            DestroySlave(direction);
        }
    }

    private bool m_isDead = false;
}
