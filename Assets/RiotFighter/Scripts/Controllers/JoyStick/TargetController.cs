using UnityEngine;

public class TargetController : MonoBehaviour
{
    public float speed;
    public Vector3 TargetMove { get; set; }

    void Update()
    {
        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, 0, Screen.width), transform.position.y, transform.position.z);
        transform.position = new Vector3(
            transform.position.x, Mathf.Clamp(transform.position.y, Screen.height / 2, Screen.height), transform.position.z);

        transform.Translate(TargetMove * speed * Time.deltaTime);
    }
}
