using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    void Update()
    {
        //Mira si se esta tocando o no la pantalla. Si no se toca, mejor no hacer nada.
        if (!Touchscreen.current.primaryTouch.press.isPressed) { return; }

        //Leer la pos del touch y guardarla
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();  
        Debug.Log(touchPosition);
    }
}
