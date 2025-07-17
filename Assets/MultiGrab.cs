using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MultiGrab : MonoBehaviour
{
    public Transform Grip; // Точка для пистолетной рукоятки
    public Transform Mag;   // Точка для магазина
    public Transform Trigger;    // Точка для курка
    private XRGrabInteractable grabInteractable; // Компонент для захвата

    private Transform activeAttachPoint; // Текущая точка захвата

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null) Debug.LogError("XRGrabInteractable не найден!");
        if (Grip == null) Debug.LogError("Pistol Grip Point не привязан!");
        if (Mag == null) Debug.LogError("Magazine Point не привязан!");
        if (Trigger == null) Debug.LogError("Trigger Point не привязан!");

        // Подписываемся на события захвата
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRBaseInputInteractor interactor)
        {
            // Определяем ближайшую точку захвата
            float distPistolGrip = Vector3.Distance(interactor.transform.position, Grip.position);
            float distMagazine = Vector3.Distance(interactor.transform.position, Mag.position);
            float distTrigger = Vector3.Distance(interactor.transform.position, Trigger.position);

            if (distPistolGrip <= distMagazine && distPistolGrip <= distTrigger)
                activeAttachPoint = Grip;
            else if (distMagazine <= distTrigger)
                activeAttachPoint = Mag;
            else
                activeAttachPoint = Trigger;

            // Устанавливаем точку привязки
            grabInteractable.attachTransform = activeAttachPoint;
            Debug.Log("Захват за: " + activeAttachPoint.name);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        activeAttachPoint = null;
        grabInteractable.attachTransform = null; // Сбрасываем точку при отпускании
    }

    void Update()
    {
        // Если захвачено двумя руками, вычисляем среднюю позицию
        if (grabInteractable.interactorsSelecting.Count > 1)
        {
            Vector3 avgPos = (Grip.position + Mag.position + Trigger.position) / 3f;
            transform.position = Vector3.Lerp(transform.position, avgPos, Time.deltaTime * 5f);
        }
        else if (grabInteractable.isSelected && activeAttachPoint != null)
        {
            transform.position = Vector3.Lerp(transform.position, activeAttachPoint.position, Time.deltaTime * 5f);
        }
    }

    void OnDestroy()
    {
        if (grabInteractable != null)
        {
            grabInteractable.selectEntered.RemoveListener(OnGrab);
            grabInteractable.selectExited.RemoveListener(OnRelease);
        }
    }
}