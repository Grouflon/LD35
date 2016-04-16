using UnityEngine;
using System.Collections;

public class SlaveController : MonoBehaviour
{
    public GameObject SlaveGO;
    public Vector2 slavePosition;
    public Vector2 targetPosition;
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
        Vector2 newDirection = targetPosition - slavePosition;
        newDirection.Normalize();
        slavePosition += newDirection;
        SlaveGO.transform.position = slavePosition;
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
