using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Character Identification")]
    public string characterID;

    [Header("Base Components")]
    public Rigidbody rb;
    public Collider col;
    public Animator anim;
    private PhotonView view;

    private Vector2 direction;
    private float moveSpeed, jumpForce, dashForce;
    [HideInInspector] public float hitForce, baseHitForce;
    private int jumpCount, dashCount, hitPoints, maxHitPoints;
    private bool isFacingRight, isDashing, isAttacking, isHit;
    [HideInInspector] public bool hasItem;
    private string heldItem;

    [HideInInspector] public int lives;
    private int playerNum;

    [Header("Player Actions")]
    public InputActionReference move, lightAttack, heavyAttack, jump, dash, useItem;

    [HideInInspector] public Vector2 startPos;
    [Header("Others")]
    [SerializeField] private Vector3 footSize;
    [SerializeField] private float footOffsetY;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject[] combatItems;

    void Start()
    {
        view = GetComponent<PhotonView>();
        if (view.IsMine)
            playerNum = PhotonNetwork.LocalPlayer.ActorNumber;

        Camera.main.GetComponent<CameraController>().targets.Add(this.transform);

        moveSpeed = 20f;
        jumpForce = 20f;
        dashForce = 30f;
        baseHitForce = 25f;
        jumpCount = 0;
        dashCount = 0;
        hitPoints = 0;
        maxHitPoints = 100;
        lives = 3;
        isDashing = false;
        isAttacking = false;
        isHit = false;
        hasItem = false;

        startPos = transform.position;
        Debug.Log(gameObject.name + ": " + startPos);
    }

    private void OnEnable()
    {
        jump.action.performed += OnJumpPerformed;
        dash.action.performed += OnDashPerformed;
        lightAttack.action.performed += OnLightAttackPerformed;
        heavyAttack.action.performed += OnHeavyAttackPerformed;
        useItem.action.performed += OnUseItemPerformed;
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJumpPerformed;
        dash.action.performed -= OnDashPerformed;
        lightAttack.action.performed -= OnLightAttackPerformed;
        heavyAttack.action.performed -= OnHeavyAttackPerformed;
        useItem.action.performed -= OnUseItemPerformed;
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                Jump();
            }
    }

    void OnDashPerformed(InputAction.CallbackContext context)
    {
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                StartCoroutine(Dash());
            }
    }

    void OnLightAttackPerformed(InputAction.CallbackContext context)
    {
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                if (IsGrounded())
                    StartCoroutine(Hit("LightGround", 0.4f));
                else
                    StartCoroutine(Hit("LightAir", 0.4f));
            }
    }

    void OnHeavyAttackPerformed(InputAction.CallbackContext context)
    {
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                if (IsGrounded())
                    StartCoroutine(Hit("HeavyGround", 0.7f));
                else
                    StartCoroutine(Hit("HeavyAir", 0.7f));
            }
    }

    void OnUseItemPerformed(InputAction.CallbackContext context)
    {
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                UseItem();
            }
    }

    void Update()
    {
        direction = move.action.ReadValue<Vector2>();

        if (IsGrounded())
        {
            jumpCount = 0;
            dashCount = 0;
            anim.SetBool("Grounded", true);
        }
        else
        {
            anim.SetBool("Grounded", false);
        }

        if (transform.position.x >= 120 || transform.position.x <= -120 || transform.position.y >= 80 || transform.position.y <= -80)
            Knockout();

        SendInfo();
    }

    void FixedUpdate()
    {
        if (view.IsMine)
        {
            if (isDashing || isAttacking || isHit)
                return;

            Movement(direction.x, direction.y);
        }
    }

    void Movement(float dirX, float dirY)
    {
        if (!IsGrounded() && dirY < 0)
        {
            rb.AddForce(new Vector2(0, dirY * (moveSpeed * 1.5f)), ForceMode.Force);
        }

        if (IsGrounded())
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        else
        {
            rb.AddForce(new Vector2(dirX * (moveSpeed * 1.5f), 0), ForceMode.Force);

            float velocityX = Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed);
            rb.velocity = new Vector2(velocityX, rb.velocity.y);
        }

        if (dirX != 0)
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);

        Flip(dirX);
    }

    void Jump()
    {
        if (IsGrounded() || jumpCount < 2)
        {
            rb.velocity = Vector2.zero;
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            jumpCount += 1;
        }
    }

    IEnumerator Dash()
    {
        if (dashCount < 1)
        {
            if (!IsGrounded())
                dashCount += 1;

            isDashing = true;
            gameObject.layer = 8;
            rb.useGravity = false;
            rb.AddForce(new Vector2(transform.localScale.x * dashForce, 0f), ForceMode.Impulse);
            anim.SetTrigger("Dash");

            yield return new WaitForSeconds(0.4f);

            rb.useGravity = true;
            gameObject.layer = 7;
            isDashing = false;
        }
    }

    IEnumerator Hit(string attackID, float cooldownTime)
    {
        isAttacking = true;
        anim.SetTrigger(attackID);

        switch (attackID)
        {
            case "LightGround":
                rb.velocity = new Vector2(transform.localScale.x * 5, 0);
                break;
            case "HeavyGround":
                rb.velocity = Vector2.zero;
                break;
            case "HeavyAir":
                rb.velocity = Vector2.zero;
                rb.AddForce(new Vector2(0, -1 * (moveSpeed * 2)), ForceMode.Impulse);
                break;
        }

        yield return new WaitForSeconds(cooldownTime);

        isAttacking = false;
    }

    void UseItem()
    {
        if (hasItem)
        {
            Vector2 direction = transform.localScale;
            GameObject deployedItem = null;

            switch (heldItem)
            {
                case "BRA":
                    deployedItem = PhotonNetwork.Instantiate(combatItems[0].name, transform.position, transform.rotation);
                    break;

                case "CHN":
                    deployedItem = PhotonNetwork.Instantiate(combatItems[1].name, transform.position, transform.rotation);
                    break;

                case "IND":
                    GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

                    foreach (GameObject player in players)
                    {
                        if (player != gameObject && player.layer.Equals(7))
                        {
                            deployedItem = PhotonNetwork.Instantiate(combatItems[2].name, new Vector2(player.transform.position.x, player.transform.position.y + 10), transform.rotation);
                            break;
                        }
                    }
                    break;

                case "MEX":
                    deployedItem = PhotonNetwork.Instantiate(combatItems[3].name, transform.position, transform.rotation);
                    break;
            }

            if (deployedItem != null)
            {
                CombatItem combatItem = deployedItem.GetComponent<CombatItem>();
                combatItem.SetDirection(direction);
            }

            hasItem = false;
            Debug.Log(direction);
        }
    }

    void Flip(float dirX)
    {
        if (isFacingRight && dirX > 0 || !isFacingRight && dirX < 0)
        {
            Vector2 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void TakeHit(int damageTaken)
    {
        hitPoints = Mathf.Clamp(hitPoints + damageTaken, 0, 200);
        StartCoroutine(HitCooldown());
    }

    IEnumerator HitCooldown()
    {
        isHit = true;
        anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(1f);
        isHit = false;
    }

    public float ScaleHitForce()
    {
        float knockbackMultiplier = Mathf.Pow(2f, (float)hitPoints / (float)maxHitPoints) - 1f;
        knockbackMultiplier = Mathf.Clamp(knockbackMultiplier, 0f, 2.5f);
        hitForce = baseHitForce * knockbackMultiplier;
        float knockbackPercentage = knockbackMultiplier * 100f;
        return (int)knockbackPercentage;
    }

    bool IsGrounded()
    {
        if (Physics.BoxCast(col.bounds.center, footSize, -transform.up, transform.rotation, footOffsetY, groundLayer))
            return true;
        else
            return false;
    }

    public void Knockout()
    {
        lives = Mathf.Clamp(lives - 1, -1, 3);

        if (lives >= 0)
        {
            transform.position = startPos;
            rb.velocity = Vector2.zero;
            hitPoints = 0;
        }

        Debug.Log("lives: " + lives);
    }

    private void SendInfo()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.ReceiveInfos(playerNum, ScaleHitForce(), lives);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center - transform.up * footOffsetY, footSize);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered something");

        if (other.gameObject.layer.Equals(6))
        {
            Debug.Log("triggered a hurtbox");

            switch (other.gameObject.tag)
            {
                case "Player":
                    Debug.Log("triggered a player");
                    AttackColliders attackEnemy = other.gameObject.GetComponent<AttackColliders>();
                    PlayerBehaviour enemyPlayer = other.gameObject.transform.parent.parent.GetComponent<PlayerBehaviour>();

                    if (enemyPlayer != null)
                    {
                        TakeHit(attackEnemy.damage);
                        ScaleHitForce();
                        rb.AddForce(new Vector2(enemyPlayer.transform.localScale.x, enemyPlayer.transform.localScale.y) * hitForce, ForceMode.Impulse);
                        Debug.Log("hitForce: " + hitForce);
                        Debug.Log("knockBackMultiplier: " + ScaleHitForce() + "%");
                    }
                    break;

                case "OffensiveItem":
                    Debug.Log("triggered an offensive item");
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

        if (other.gameObject.CompareTag("ObtainableItem"))
        {
            if (!hasItem)
            {
                hasItem = true;
                heldItem = other.gameObject.GetComponent<ObtainableItem>().itemID;
            }
        }
    }
}