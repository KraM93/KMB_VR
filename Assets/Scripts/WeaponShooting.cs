using UnityEngine;
using UnityEngine.InputSystem;


public class AK47Shooting : MonoBehaviour
{
    private AudioSource audioSource;
    public Transform barrel; // Точка начала луча (дуло)
    public float maxDistance = 100f; // Дальность выстрела
    public LayerMask targetLayer; // Слой для мишеней
    public InputActionReference shootAction; // Действие для триггера
    public AudioClip shootSound; // Аудиоклип выстрела
    public ParticleSystem muzzleFlash; // Эффект вспышки дула

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable; // Компонент для захвата

    void Start()
    {
        grabInteractable = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Проверка компонентов
        if (grabInteractable == null) Debug.LogError("XRGrabInteractable не найден!");
        if (barrel == null) Debug.LogError("Barrel не привязан!");
        if (shootAction == null) Debug.LogError("Shoot Action не привязан!");
        if (shootSound == null) Debug.LogError("Shoot Sound не привязан!");
        if (muzzleFlash == null) Debug.LogError("Muzzle Flash не привязан!");
    }

    void Update()
    {
        if (grabInteractable.isSelected && shootAction.action != null && shootAction.action.triggered)
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Воспроизведение звука
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }

        // Воспроизведение эффекта вспышки
        if (muzzleFlash != null)
        {
            muzzleFlash.Play(); // Запуск эффекта
        }

        // Выполняем Raycast и уничтожаем мишень
        RaycastHit hit;
        if (Physics.Raycast(barrel.position, barrel.forward, out hit, maxDistance, targetLayer))
        {
            Debug.Log("Hit: " + hit.collider.name);
            Destroy(hit.collider.gameObject);
        }
    }

    void OnEnable() => shootAction?.action?.Enable();
    void OnDisable() => shootAction?.action?.Disable();
}