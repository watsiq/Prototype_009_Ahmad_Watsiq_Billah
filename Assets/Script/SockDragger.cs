using UnityEngine;

public class SockDragger : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void OnMouseDown()
    {
        isDragging = true;
        Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
        Vector3 mousePoint = Input.mousePosition;
        offset = transform.position - cam.ScreenToWorldPoint(new Vector3(mousePoint.x, mousePoint.y, screenPoint.z));
    }

    void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePoint = Input.mousePosition;
            Vector3 screenPoint = cam.WorldToScreenPoint(transform.position);
            Vector3 targetPosition = cam.ScreenToWorldPoint(new Vector3(mousePoint.x, mousePoint.y, screenPoint.z)) + offset;
            transform.position = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);
        }
    }

    void OnMouseUp()
    {
        isDragging = false;
    }
}