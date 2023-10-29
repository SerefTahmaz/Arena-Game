using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeKinematic : MonoBehaviour
{
    [ContextMenu("kine")]
    public void InitKinematic()
    {
        foreach (Transform VARIABLE in transform.GetComponentsInChildren<Transform>())
        {
            if (VARIABLE.TryGetComponent(out Rigidbody rigidbody))
            {
                rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
