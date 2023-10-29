using System;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Transform _lookAtObject = null;

    public void OnInteractableObjectEnter(InteractionHelper interactedObject)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
    public void OnInteractableObjectExit(InteractionHelper interactedObject)
    {
        _lookAtObject = null;
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void Start()
    {
        _lookAtObject = Camera.main.transform;
    }

    private void Update()
    {
        if (_lookAtObject != null)
        {
             Vector3 relative = transform.InverseTransformPoint(_lookAtObject.position);
             float angle = Mathf.Atan2(relative.x, relative.z) * Mathf.Rad2Deg;
             transform.Rotate(0, angle, 0);
             // transform.GetChild(0).GetChild(0).LookAt(_lookAtObject);
        }
    }
}

