using UnityEngine;

public class FollowCameraSimple : MonoBehaviour
{
    public Camera mainCamera; // ������ �� ������
    public Vector3 offset = new Vector3(-0.5f, -0.5f, 1f); // �������� ��� ������ ������� ����

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // ������������� ����� ������
            if (mainCamera == null)
            {
                Debug.LogError("Main Camera not found!");
            }
        }
    }

    void Update()
    {
        if (mainCamera != null)
        {
            // ����������� ������ ����� ���� � ������� ����������
            Vector3 screenBottomLeft = new Vector3(0f, 0f, mainCamera.nearClipPlane + offset.z);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenBottomLeft) +
                                   mainCamera.transform.right * offset.x +
                                   mainCamera.transform.up * offset.y;
            transform.position = worldPosition;
        }
    }
}