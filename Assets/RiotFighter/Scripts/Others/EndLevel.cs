using UnityEngine;

public class EndLevel : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        var interactiveObject = other.gameObject.GetComponent<IInteractiveObject>();
    }
}
