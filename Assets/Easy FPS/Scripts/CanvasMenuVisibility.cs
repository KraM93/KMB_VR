using UnityEngine;

public class CanvasMenuVisibility : MonoBehaviour
{
    public Canvas MenuCanvas;
    public Transform palmTransform;
    public Transform headTransform;
    public float showAngle = 40f;
    public float hideAngle = 55f;
    public float showDelay = 0.3f;
    public float hideDelay = 0.3f;

    private float timer = 0f;
    private bool isMenuVisible = false;

    private void Start()
    {
        MenuCanvas.enabled = false;
        isMenuVisible = false;
        timer = 0f;
    }

    void Update()
    {
        Vector3 toHead = (headTransform.position - palmTransform.position).normalized;
        Vector3 palmNormal = palmTransform.right;

        float angle = Vector3.Angle(palmNormal, toHead);

        Debug.Log("Angle: " + angle);

        if (!isMenuVisible && angle < showAngle)
        {
            timer += Time.deltaTime;
            if (timer >= showDelay)
            {
                MenuCanvas.enabled = true;
                isMenuVisible = true;
                timer = 0f;
            }
        }
        else if (isMenuVisible && angle > hideAngle)
        {
            timer += Time.deltaTime;
            if (timer >= hideDelay)
            {
                MenuCanvas.enabled = false;
                isMenuVisible = false;
                timer = 0f;
            }
        }
        else
        {
            timer = 0f;
        }
    }
}