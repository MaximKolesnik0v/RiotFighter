using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoyStickController : MonoBehaviour
{
    public GameObject touchMarker;
    public TargetController targetController;

    private Vector3 _target;

    void Start()
    {
        touchMarker.transform.position = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 touchPosition = Input.mousePosition;
            _target = touchPosition - transform.position;

            if (_target.magnitude < 100)
            {
                touchMarker.transform.position = touchPosition;
                targetController.TargetMove = _target;
            }
        } else
        {
            touchMarker.transform.position = transform.position;
            targetController.TargetMove = new Vector3(0, 0, 0);
        }
    }
}
