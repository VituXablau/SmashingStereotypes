using UnityEngine;

public class DummyBehaviour : MonoBehaviour
{
    [Header("Character Identification")]
    public string characterID;

    [Header("Base Components")]
    public Rigidbody rb;
    public Collider col;
    public Animator anim;

    private int hitPoints, maxHitPoints;

    private Vector2 startPos;
    
    [Header("Others")]
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Vector3 footSize;
    [SerializeField] private float footOffsetY;
    [HideInInspector] public float hitForce, baseHitForce;

    void Start()
    {
        baseHitForce = 20;
        hitPoints = 0;
        maxHitPoints = 100;

        startPos = transform.position;
    }

    void Update()
    {
        if (IsGrounded())
            anim.SetBool("Grounded", true);
        else
            anim.SetBool("Grounded", false);

        if (transform.position.x >= 120 || transform.position.x <= -120 || transform.position.y >= 80 || transform.position.y <= -80)
            Knockout();
    }

    public void TakeHit(int damageTaken)
    {
        hitPoints = Mathf.Clamp(hitPoints + damageTaken, 0, 200);
        anim.SetTrigger("Hurt");
    }

    public float ScaleHitForce()
    {
        float knockbackMultiplier = Mathf.Pow(2f, (float)hitPoints / (float)maxHitPoints) - 1f;
        knockbackMultiplier = Mathf.Clamp(knockbackMultiplier, 0f, 2.5f);
        hitForce = baseHitForce * knockbackMultiplier;
        float knockbackPercentage = knockbackMultiplier * 100f;
        return (int)knockbackPercentage;
    }

    public void Knockout()
    {
        transform.position = startPos;
        rb.velocity = Vector2.zero;
        hitPoints = 0;
    }

    private bool IsGrounded()
    {
        if (Physics.BoxCast(col.bounds.center, footSize, -transform.up, transform.rotation, footOffsetY, groundLayer))
            return true;
        else
            return false;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center - transform.up * footOffsetY, footSize);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer.Equals(6))
        {
            switch (other.gameObject.tag)
            {
                case "Player":
                    PlayerBehaviour enemyPlayer = other.gameObject.transform.parent.parent.GetComponent<PlayerBehaviour>();
                    AttackColliders attackEnemy = other.gameObject.GetComponent<AttackColliders>();

                    if (enemyPlayer != null)
                    {
                        TakeHit(attackEnemy.damage);
                        ScaleHitForce();
                        rb.AddForce(new Vector2(enemyPlayer.transform.localScale.x, enemyPlayer.transform.localScale.y) * hitForce, ForceMode.Impulse);
                        Debug.Log("hitForce: " + hitForce);
                        Debug.Log("took " + attackEnemy.damage);
                        Debug.Log("knockBackMultiplier: " + ScaleHitForce() + "%");
                    }
                    break;

                case "OffensiveItem":
                    CombatItem harmfulItem = other.gameObject.transform.root.GetComponent<CombatItem>();

                    if (harmfulItem != null)
                    {
                        TakeHit(harmfulItem.damage);
                        ScaleHitForce();
                        rb.AddForce(new Vector2(harmfulItem.transform.localScale.x, harmfulItem.transform.localScale.y) * hitForce, ForceMode.Impulse);
                        Debug.Log("hitForce: " + hitForce);
                        Debug.Log("took " + harmfulItem.damage);
                        Debug.Log("knockBackMultiplier: " + ScaleHitForce() + "%");
                    }
                    break;

                case "Obstacle":
                    TakeHit(10);
                    ScaleHitForce();
                    rb.AddForce(Vector3.up * 50, ForceMode.Impulse);
                    Debug.Log("hitForce: " + hitForce);
                    Debug.Log("took " + 10);
                    Debug.Log("knockBackMultiplier: " + ScaleHitForce() + "%");
                    break;
            }
        }
    }
}
