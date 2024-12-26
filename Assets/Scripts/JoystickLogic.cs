using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickLogic : MonoBehaviour
{
    public float speed;
    [SerializeField] VariableJoystick variableJoystick;
    Animator animator;
    CharacterController characterController;
    public float gravity = 20.0F;
    public bool canMove = true;

    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(canMove)
        {
            var x = variableJoystick.Horizontal;
            var z = variableJoystick.Vertical;
            Vector3 move = new Vector3(x, 0f, z);
            if(move.magnitude > 0.15)
            {
                if (x != 0 || z != 0)
                {

                    transform.rotation = Quaternion.LookRotation(move);
                    move.y -= gravity * Time.deltaTime;
                    characterController.Move(move * speed * Time.deltaTime);
                }
                else
                {
                    move.y -= gravity * Time.deltaTime;
                    characterController.Move(move * speed * Time.deltaTime);
                }
                move = new Vector3(x, 0f, z);
                animator.SetFloat("speed", move.magnitude);
            }
            else
                animator.SetFloat("speed", 0);

        }
    }
}
