using UnityEngine;
using System.Collections;

public class GamepadInputController : InputController {

    public int gamepadId = 0;

    override public Vector2 GetDirection()
    {
        Vector2 direction = new Vector2();
        direction.x = Input.GetAxis("Horizontal" + gamepadId);
        direction.y = Input.GetAxis("Vertical" + gamepadId);

        direction.Normalize();
        return direction;
    }
}
