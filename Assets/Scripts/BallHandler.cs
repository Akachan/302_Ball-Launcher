using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] Rigidbody2D currentBallRigidbody;

    Camera mainCamera;

    void Start() 
    {
        mainCamera = Camera.main;
        
    }
    void Update()
    {
        //Mira si se esta tocando o no la pantalla. Si no se toca, mejor no hacer nada.
        if (!Touchscreen.current.primaryTouch.press.isPressed) 
        { 
            currentBallRigidbody.isKinematic = false;
            return;
        }
        currentBallRigidbody.isKinematic = true;
        //Leer la pos del touch y guardarla
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();  

        //Transformar de ScreenSpace to WorldSpace
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;
    }
}
