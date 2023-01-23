using UnityEngine;

public class CenterOfMass : MonoBehaviour
{
    public Transform centerOfMass;

    void Awake()
    {
        GetComponent<Rigidbody>().centerOfMass = Vector3.Scale(centerOfMass.localPosition, transform.localScale);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(GetComponent<Rigidbody>().worldCenterOfMass, 0.1f);
    }
}
