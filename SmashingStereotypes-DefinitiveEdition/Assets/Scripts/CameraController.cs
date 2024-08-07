using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    public List<Transform> targets;
    public Vector3 offset;
    private Vector3 velocity;
    public float smoothTime = 0.5f, minZoom = 40f, maxZoom = 10f, zoomLimiter = 50f, minX = -50, maxX = 50, minY = -18, maxY = 18;

    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void LateUpdate()
    {
        if (targets == null || targets.Count == 0)
            return;

        targets.RemoveAll(target => target == null);

        if (targets.Count == 0)
            return;

        Vector3 position = transform.position;
        position = new Vector3(Mathf.Clamp(position.x, minX, maxX), Mathf.Clamp(position.y, minY, maxY), position.z);
        transform.position = position;

        Move();
        Zoom();

        cam.fieldOfView = Mathf.Clamp(cam.fieldOfView, 10, 60);
    }

    void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position, newPosition, ref velocity, smoothTime);
    }

    void Zoom()
    {
        float newZoomX = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance().x / zoomLimiter);
        float newZoomY = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance().y / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoomX, Time.deltaTime);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoomY, Time.deltaTime);
    }

    Vector3 GetCenterPoint()
    {
        if (targets.Count == 1)
        {
            return targets[0].position;
        }

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 1; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].position);
            }
        }

        return bounds.center;
    }

    Vector3 GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 1; i < targets.Count; i++)
        {
            if (targets[i] != null)
            {
                bounds.Encapsulate(targets[i].position);
            }
        }

        return bounds.size;
    }
}
