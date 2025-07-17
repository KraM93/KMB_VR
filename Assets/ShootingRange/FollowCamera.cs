using UnityEngine;

public class FollowCameraSimple : MonoBehaviour
{
    public Camera mainCamera; // Ссылка на камеру
    public Vector3 offset = new Vector3(-0.5f, -0.5f, 1f); // Смещение для левого нижнего угла

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Автоматически найти камеру
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
            // Преобразуем нижний левый угол в мировые координаты
            Vector3 screenBottomLeft = new Vector3(0f, 0f, mainCamera.nearClipPlane + offset.z);
            Vector3 worldPosition = mainCamera.ScreenToWorldPoint(screenBottomLeft) +
                                   mainCamera.transform.right * offset.x +
                                   mainCamera.transform.up * offset.y;
            transform.position = worldPosition;
        }
    }
}