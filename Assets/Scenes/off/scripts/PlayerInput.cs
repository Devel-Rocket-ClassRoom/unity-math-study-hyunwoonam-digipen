using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float rotationSpeed = 2f;

    private Camera mainCamera; // 씬의 Main Camera를 인스펙터에서 할당해주세요.

    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 camForward = mainCamera.transform.forward;
        Vector3 camRight = mainCamera.transform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * v + camRight * h).normalized;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime, Space.World);

            Vector3 lookDirection = moveDirection;

            if (v < 0)
            {
                lookDirection = camForward;
            }

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
