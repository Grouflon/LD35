using UnityEngine;
using System.Collections;

public class SlaveController : MonoBehaviour
{
    public GameObject SlaveGO;
    public Vector2 slavePosition;
    public Vector2 targetPosition;
    public ChariotController masterChariot;
    public int strength;
    public const int MAX_SLAVES = 150;

    void Start()
    {
        strength = 1;
        targetPosition = masterChariot.buttZone.transform.position;
    }

    void Update()
    {
        MoveSlaveAss();
    }

    void SpawnSlave()
    {
        // spawn slave in spawn zone 
    }

    void MoveSlaveAss()
    {
        Vector2 newDirection = targetPosition - slavePosition;
        newDirection.Normalize();
        slavePosition += newDirection;
        SlaveGO.transform.position = slavePosition;
    }




}
