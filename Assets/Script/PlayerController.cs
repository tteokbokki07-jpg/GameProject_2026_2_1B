using NUnit.Framework.Constraints;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform cameraTransform;

    [Header("이동설정")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("바닥 설정")]
    [SerializeField] private float gravity = -20f;

    private CharacterController controller;
    private float verticalVelocity;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }
    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }
        //이동
        Vector2 input = Vector2.zero;

        if (keyboard.aKey.isPressed)
            input.x -= 1f;
        if (keyboard.dKey.isPressed)
            input.x += 1f;
        if (keyboard.sKey.isPressed)
            input.y -= 1f;
        if (keyboard.wKey.isPressed)
            input.y += 1f;

        input = Vector2.ClampMagnitude(input, 1f);

        //카메라
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        //위아래 기울기 방지
        cameraForward.y = 0f;
        cameraRight.y = 0f;
        cameraForward.Normalize();
        cameraRight.Normalize();
        //카메라 기준 이동
        Vector3 moveDirection = cameraForward * input.y + cameraRight * input.x;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f);

        //shift 달리기
        bool isRunning = keyboard.leftShiftKey.isPressed;
        float currentSpeed = isRunning ? runSpeed : walkSpeed;
        //수평이동
        controller.Move(moveDirection * currentSpeed * Time.deltaTime);
        //이동방향으로 회전
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        //중력
        if (controller.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);

        //애니메이션
        float animationSpeed = 0f;
        if (moveDirection.sqrMagnitude > 0.001f)
        {
            animationSpeed = isRunning ? 1f : 0.5f;
        }
        animator.SetFloat("speed", animationSpeed, 0.1f, Time.deltaTime);
    }


}
