using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{
    [SerializeField] private Rigidbody2D currentBallRigidBody2D;
    [SerializeField] private SpringJoint2D currentBallSpringJoint;
    [SerializeField] private float delayTime = 1f;

    private Camera mainCamera;
    private bool isDragging = false; // means we touched once.

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentBallRigidBody2D == null) { return; }

        if (!Touchscreen.current.primaryTouch.press.isPressed) // if you don't have primaryTouch it does not work.
        {
            if (isDragging)
            {
                LaunchBall();
            }
            isDragging = false;
            return;
        }

        isDragging = true;
        currentBallRigidBody2D.isKinematic = true; // puts the ball under physics control
        
        Vector2 position = Touchscreen.current.primaryTouch.position.ReadValue();
            
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(position);

        currentBallRigidBody2D.position = worldPosition;
            
//            Debug.Log("pos["+vector3+"]");
    }

    private void LaunchBall()
    {
        currentBallRigidBody2D.isKinematic = false; // takes the ball out of physics control
        currentBallRigidBody2D = null; // we forget about the this ball and stop controlling its state.
        Debug.Log("Before Invoke:" + DateTime.Now);
        Invoke(nameof(DetachBall), delayTime);
    }
    private void DetachBall()
    {
        Debug.Log("In DetachBall:" + DateTime.Now);
        currentBallSpringJoint.enabled = false; // prevent the spring from interacting with the ball
        currentBallSpringJoint = null; // and forget about the joint
    }
}
