using UnityEngine;

public class TPScamera : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = new Vector3(0, 5f, -10f);

    [SerializeField] private float positionSmoothTime = 0.3f;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private float ZoomDistance = 5f;

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        Vector3 targetPosition = target.position + (target.rotation * offset.normalized * ZoomDistance) + Vector3.up * offset.y;

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, positionSmoothTime);

        Vector3 look = (target.position - transform.position).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(look);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
