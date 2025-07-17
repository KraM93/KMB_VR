using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class MultiGrab : MonoBehaviour
{
    public Transform Grip; // ����� ��� ����������� ��������
    public Transform Mag;   // ����� ��� ��������
    public Transform Trigger;    // ����� ��� �����
    private XRGrabInteractable grabInteractable; // ��������� ��� �������

    private Transform activeAttachPoint; // ������� ����� �������

    void Start()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        if (grabInteractable == null) Debug.LogError("XRGrabInteractable �� ������!");
        if (Grip == null) Debug.LogError("Pistol Grip Point �� ��������!");
        if (Mag == null) Debug.LogError("Magazine Point �� ��������!");
        if (Trigger == null) Debug.LogError("Trigger Point �� ��������!");

        // ������������� �� ������� �������
        grabInteractable.selectEntered.AddListener(OnGrab);
        grabInteractable.selectExited.AddListener(OnRelease);
    }

    void OnGrab(SelectEnterEventArgs args)
    {
        if (args.interactorObject is XRBaseInputInteractor interactor)
        {
            // ���������� ��������� ����� �������
            float distPistolGrip = Vector3.Distance(interactor.transform.position, Grip.position);
            float distMagazine = Vector3.Distance(interactor.transform.position, Mag.position);
            float distTrigger = Vector3.Distance(interactor.transform.position, Trigger.position);

            if (distPistolGrip <= distMagazine && distPistolGrip <= distTrigger)
                activeAttachPoint = Grip;
            else if (distMagazine <= distTrigger)
                activeAttachPoint = Mag;
            else
                activeAttachPoint = Trigger;

            // ������������� ����� ��������
            grabInteractable.attachTransform = activeAttachPoint;
            Debug.Log("������ ��: " + activeAttachPoint.name);
        }
    }

    void OnRelease(SelectExitEventArgs args)
    {
        activeAttachPoint = null;
        grabInteractable.attachTransform = null; // ���������� ����� ��� ����������
    }

    void Update()
    {
        // ���� ��������� ����� ������, ��������� ������� �������
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