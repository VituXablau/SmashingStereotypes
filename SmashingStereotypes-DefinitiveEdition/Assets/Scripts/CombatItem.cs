using System.Collections;
using UnityEngine;

public class CombatItem : MonoBehaviour
{
    [SerializeField] private string itemID;

    private Rigidbody rb;

    public int damage;
    [SerializeField] private bool freeze;
    [SerializeField] private float timeToHarm;

    private Vector2 direction;
    [SerializeField] private GameObject damageCollider, vfx;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (!freeze)
            StartCoroutine(InitializeBehaviour(timeToHarm));

        Debug.Log(itemID + ": " + damage);
    }

    IEnumerator InitializeBehaviour(float waitTime)
    {
        switch (itemID)
        {
            case "BRA":
                rb.velocity = new Vector2(direction.x * 10, rb.velocity.y);
                Destroy(gameObject, 10);
                break;

            case "CHN":
                rb.velocity = Vector2.up * 3;
                yield return new WaitForSeconds(waitTime);
                rb.velocity = Vector2.zero;
                freeze = true;
                damageCollider.SetActive(true);
                Instantiate(vfx, damageCollider.transform.position, Quaternion.identity);
                Destroy(gameObject, 0.2f);
                break;

            case "IND":
                Destroy(gameObject, 8);
                break;

            case "MEX":
                rb.velocity = new Vector2(direction.x * 30, rb.velocity.y);
                Destroy(gameObject, 6);
                break;
        }

        if (!freeze)
        {
            yield return new WaitForSeconds(waitTime);
            damageCollider.SetActive(true);
        }
    }

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }
}
