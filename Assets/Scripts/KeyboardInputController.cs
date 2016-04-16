using UnityEngine;
using System.Collections;

public class KeyboardInputController : InputController {

    public KeyCode upKey = KeyCode.UpArrow;
    public KeyCode rightKey = KeyCode.RightArrow;
    public KeyCode downKey = KeyCode.DownArrow;
    public KeyCode leftKey = KeyCode.LeftArrow;

	override public Vector2 GetDirection()
    {
        Vector2 direction = new Vector2();
        if (Input.GetKey(upKey))
        {
            direction.y -= 1.0f;
        }

        if (Input.GetKey(rightKey))
        {
            direction.x += 1.0f;
        }

        if (Input.GetKey(downKey))
        {
            direction.y += 1.0f;
        }

        if (Input.GetKey(leftKey))
        {
            direction.x -= 1.0f;
        }

        direction.Normalize();
        return direction;
    }
}
