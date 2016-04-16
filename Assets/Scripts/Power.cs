using UnityEngine;
using System.Collections;

public class Power : MonoBehaviour {

    public enum CastState
    {
        NotStarted,
        Preparing,
        Casting,
        Finished
    };

    public GameObject source;
    public Vector2 direction;

    public CastState GetCastState() { return m_state; }
    protected CastState m_state;
}
