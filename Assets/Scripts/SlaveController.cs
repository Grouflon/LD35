using UnityEngine;
using System.Collections;

public class SlaveController : MonoBehaviour
{
    public Vector3 slavePosition;
    public Vector3 targetPosition;
    public ChariotController masterChariot;
    public int strength;

    void Start()
    {
        strength = 1;
        targetPosition = masterChariot.buttZone.transform.position;
    }

    void Update()
    {
        MoveSlaveAss();
    }

    void MoveSlaveAss()
    {
        Vector3 newDirection = targetPosition - slavePosition;
        newDirection.Normalize();
        newDirection.z = newDirection.z * strength * 0.03f;
        newDirection.z += Random.Range(-1, 1) * newDirection.z * 0.3f;
        newDirection.x = newDirection.x * strength * 0.03f;
        newDirection.x += Random.Range(-1, 1) * newDirection.x * 0.3f;
        newDirection.y = 0f;
        slavePosition += newDirection;
        //vector3.Value = new Vector3(vector2.Value.x,vector2.Value.y,zValue.Value);
        this.transform.position = slavePosition;
    }

    void OnTriggerEnter(Collider powerCollider)
    {
        //
    }

    void DestroySlave()
    {
        Destroy(this.gameObject, 0.3f);
    }





}
