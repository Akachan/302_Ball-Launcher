using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class BallHandler : MonoBehaviour
{
   
    [SerializeField] GameObject ballPrefab;
    [SerializeField] Rigidbody2D pivot;
    [SerializeField] float detachBallDelay = 0.1f;
    [SerializeField] float respawnDelay = 2f;

    Camera mainCamera;
    Rigidbody2D currentBallRigidbody;
    SpringJoint2D currentBallSpringJoint;
    public int cantTouch;

    bool isDragging;
    private void Awake() 
    {
        currentBallRigidbody = ballPrefab.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballPrefab.GetComponent<SpringJoint2D>();
        
    }

    void Start() 
    {
        mainCamera = Camera.main;
        SpawnNewBall();
        
        
    }
    void Update()
    {
        if (currentBallRigidbody == null) {return;}
         cantTouch = Touch.activeTouches.Count;
        //Mira  cuantos touch se estan haciendo. Si no hay ninguno, mejor no hacer nada.
        if (Touch.activeTouches.Count == 0) 
        { 
            if(isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
            
        }

        //cacular la posicion media entre varios touches
        Vector2 touchPosition = new Vector2();
      
        foreach(Touch touch in Touch.activeTouches)
        {
          
                touchPosition +=touch.screenPosition;
             
            
        }
        touchPosition/= Touch.activeTouches.Count;
        

        isDragging = true;
        currentBallRigidbody.isKinematic = true;

        //Transformar de ScreenSpace to WorldSpace
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(touchPosition);
        currentBallRigidbody.position = worldPosition;
    }

    void OnEnable() 
    {
        EnhancedTouchSupport.Enable();
    }
    void OnDisable() 
    {
        EnhancedTouchSupport.Disable();
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

        Invoke(nameof(SpawnNewBall), respawnDelay);
    }

    void SpawnNewBall()
    {
        //creo el objeto
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);
        //guardo sus características
        currentBallRigidbody = ballInstance.GetComponent<Rigidbody2D>();
        currentBallSpringJoint = ballInstance.GetComponent<SpringJoint2D>();
        //conecto la bola nueva con el pivote
        currentBallSpringJoint.connectedBody = pivot;
    }
}
