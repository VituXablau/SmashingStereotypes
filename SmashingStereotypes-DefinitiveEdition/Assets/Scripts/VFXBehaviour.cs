using UnityEngine;

public class VFXBehaviour : MonoBehaviour
{
    [SerializeField] private float screenTime;

    void Start()
    {
        Destroy(gameObject, screenTime);
    }
}
