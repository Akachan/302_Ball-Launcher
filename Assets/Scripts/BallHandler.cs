using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
   
    [SerializeField] Rigidbody2D currentBallRigidbody;
    [SerializeField] SpringJoint2D currentBallSpringJoint;
    [SerializeField] float detachBallDelay = 0.1f;

    Camera mainCamera;

     bool isDragging;

    void Start() 
    {
        mainCamera = Camera.main;
        
        
    }
    void Update()
    {
        //Mira si se esta tocando o no la pantalla. Si no se toca, mejor no hacer nada.
        if (!Touchscreen.current.primaryTouch.press.isPressed) 
        { 
            if(isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidbody.isKinematic = true;
        //Leer la pos del touch y guardarla
        Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();  

        //Transformar de ScreenSpace to WorldSpace
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);

        currentBallRigidbody.position = worldPosition;
    }
    void LaunchBall()
    {
        //Lo vuelvo dinamico y elimino el objeto de la variable, para luego verificar
        //que no lo vuelva a hacer si volvemos a touchear y soltar cuando la pelota ya se fue.
        currentBallRigidbody.isKinematic = false;
        currentBallRigidbody = null;

        Invoke("DetachBall", detachBallDelay);

    }

    private void DetachBall()
    {
        //rompo la cuerda y  elimino el elemento de la variable
        currentBallSpringJoint.enabled = false;
        currentBallSpringJoint = null;
    }
}
