using UnityEngine;

public class ObtainableItem : MonoBehaviour
{
    public string itemID;

    void Start()
    {
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
