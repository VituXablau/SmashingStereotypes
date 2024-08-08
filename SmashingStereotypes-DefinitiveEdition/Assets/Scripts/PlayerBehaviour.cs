using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

// Script de comportamento e mecânicas de personagem, como movimentação, pulo, dash, golpes, verificações de estado e colisões
public class PlayerBehaviour : MonoBehaviour
{
    // Variável de identificação do personagem
    [Header("Character Identification")]
    public string characterID;

    // Variáveis que recebem os componentes do Game Object
    [Header("Base Components")]
    public Rigidbody rb;
    public Collider col;
    public Animator anim;
    private PhotonView view;

    //Variáveis utilizadas nas mecânicas do personagem
    private Vector2 direction;
    private float moveSpeed, jumpForce, dashForce;
    [HideInInspector] public float hitForce, baseHitForce;
    private int jumpCount, dashCount, hitPoints, maxHitPoints;
    private bool isFacingRight, isDashing, isAttacking, isHit;
    [HideInInspector] public bool hasItem;
    private string heldItem;
    [HideInInspector] public int lives;

    private int playerNum;

    // Variáveis que recebem as ações do Input Map
    [Header("Player Actions")]
    public InputActionReference move, lightAttack, heavyAttack, jump, dash, useItem;

    // Variáveis para outros fins
    [HideInInspector] public Vector2 startPos;
    [Header("Others")]
    [SerializeField] private Vector3 footSize;
    [SerializeField] private float footOffsetY;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private GameObject[] combatItems;

    void Start()
    {
        // Recebendo o componente do PhotonView e verificando se este jogador é o que está controlando
        view = GetComponent<PhotonView>();
        if (view.IsMine)
            playerNum = PhotonNetwork.LocalPlayer.ActorNumber;

        // Adicionando o personagem à lista de alvos da camera
        Camera.main.GetComponent<CameraController>().targets.Add(this.transform);

        // Inicializando valores das variáveis de mecânicas
        moveSpeed = 20f;
        jumpForce = 20f;
        dashForce = 27f;
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

        // Pegando a posição inicial do jogador
        startPos = transform.position;
        Debug.Log(gameObject.name + ": " + startPos);

        // Flipando o personagem caso inicie no lado direito do mapa
        if (startPos.x > 0)
        {
            Flip(1);
        }
    }

    private void OnEnable()
    {
        // Atribuindo os métodos de ações aos eventos de Input quando o Game Object é inicializado
        jump.action.performed += OnJumpPerformed;
        dash.action.performed += OnDashPerformed;
        lightAttack.action.performed += OnLightAttackPerformed;
        heavyAttack.action.performed += OnHeavyAttackPerformed;
        useItem.action.performed += OnUseItemPerformed;
    }

    private void OnDisable()
    {
        // Desatribuindo os métodos de ações aos eventos de Input quando o Game Object é desativado
        jump.action.performed -= OnJumpPerformed;
        dash.action.performed -= OnDashPerformed;
        lightAttack.action.performed -= OnLightAttackPerformed;
        heavyAttack.action.performed -= OnHeavyAttackPerformed;
        useItem.action.performed -= OnUseItemPerformed;
    }

    void OnJumpPerformed(InputAction.CallbackContext context)
    {
        // Verificando se este jogador é o que está controlando e chamando o método de Pulo se outra ação não estiver sendo executada
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                Jump();
            }
    }

    void OnDashPerformed(InputAction.CallbackContext context)
    {
        // Verificando se este jogador é o que está controlando e chamando a coroutine de Dash se outra ação não estiver sendo executada
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                StartCoroutine(Dash());
            }
    }

    void OnLightAttackPerformed(InputAction.CallbackContext context)
    {
        // Verificando se este jogador é o que está controlando e chamando a coroutine de Ataque Leve se outra ação não estiver sendo executada
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                // Verifica se o personagem está no chão e determina o ataque certo se estiver no chão ou no ar
                if (IsGrounded())
                    StartCoroutine(Hit("LightGround", 0.3f));
                else
                    StartCoroutine(Hit("LightAir", 0.3f));
            }
    }

    void OnHeavyAttackPerformed(InputAction.CallbackContext context)
    {
        // Verificando se este jogador é o que está controlando e chamando a coroutine de Ataque Pesado se outra ação não estiver sendo executada
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                // Verifica se o personagem está no chão e determina o ataque certo se estiver no chão ou no ar
                if (IsGrounded())
                    StartCoroutine(Hit("HeavyGround", 0.6f));
                else
                    StartCoroutine(Hit("HeavyAir", 0.6f));
            }
    }

    void OnUseItemPerformed(InputAction.CallbackContext context)
    {
        // Verificando se este jogador é o que está controlando e chamando o método de Usar Item se outra ação não estiver sendo executada
        if (view.IsMine)
            if (!isDashing && !isAttacking && !isHit)
            {
                UseItem();
            }
    }

    void Update()
    {
        // Atribui os valores do Input de Movimentação
        direction = move.action.ReadValue<Vector2>();

        // Verifica se o personagem está no chão, reseta o pulo e o dash e muda o parâmetro referente do animator
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

        // Verifica se o personagem passou do limite do mapa e chama o método para "matá-lo"
        if (transform.position.x >= 120 || transform.position.x <= -120 || transform.position.y >= 80 || transform.position.y <= -80)
            Knockout();

        // Enviando status do personagem para o Game Manager
        SendInfo();
    }

    void FixedUpdate()
    {
        // Verifica se é o jogador que está controlando e executa o método de movimentação caso nenhuma das ações esteja sendo realizada
        if (view.IsMine)
        {
            if (isDashing || isAttacking || isHit)
                return;

            Movement(direction.x, direction.y);
        }
    }

    void Movement(float dirX, float dirY)
    {
        // Veritifca se o personagem está no ar e o permite descer rapidamente se o jogador mover o analógico para baixo
        if (!IsGrounded() && dirY < 0)
        {
            rb.AddForce(new Vector2(0, dirY * (moveSpeed * 10f)), ForceMode.Force);
        }

        // Verifica se o personagem está no chão ou no ar e muda o modo de movimentação apropriadamente
        if (IsGrounded())
            rb.velocity = new Vector2(dirX * moveSpeed, rb.velocity.y);
        else
        {
            rb.AddForce(new Vector2(dirX * (moveSpeed * 10f), 0), ForceMode.Force);
            float velocityX = Mathf.Clamp(rb.velocity.x, -moveSpeed, moveSpeed);
            rb.velocity = new Vector2(velocityX, rb.velocity.y);
        }

        // Determinando valores ao parâmetro do animator que controla a animação de andar
        if (dirX != 0)
            anim.SetBool("Moving", true);
        else
            anim.SetBool("Moving", false);

        // Flipando o Game Object
        Flip(dirX);
    }

    void Jump()
    {
        // Verificando se o personagem está no chão ou ainda possui 2 cargas de pulo e executa a lógica do pulo
        if (IsGrounded() || jumpCount < 2)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode.Impulse);
            anim.SetTrigger("Jump");
            jumpCount += 1;
        }
    }

    IEnumerator Dash()
    {
        // Verificando se o personagem ainda possui uma carga de dash
        if (dashCount < 1)
        {
            // Verifica se está no ar, somando o uso da carga
            if (!IsGrounded())
                dashCount += 1;

            // Executando a lógica do dash
            isDashing = true;
            gameObject.layer = 8;
            rb.useGravity = false;
            rb.velocity = Vector3.zero;
            rb.AddForce(new Vector2(transform.localScale.x * dashForce, 0f), ForceMode.Impulse);
            anim.SetTrigger("Dash");

            yield return new WaitForSeconds(0.4f);
            // Retornando aos valores padrões
            rb.useGravity = true;
            gameObject.layer = 7;
            isDashing = false;
        }
    }

    IEnumerator Hit(string attackID, float cooldownTime)
    {
        // Atribuindo o estado de "Atacando" e chamando o parâmetro referente ao identificador do ataque para a animação correta
        isAttacking = true;
        anim.SetTrigger(attackID);

        // Verificando o identificador do ataque e executando a lógica correta de cada um
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
        // Verifica o personagem já possui um item
        if (hasItem)
        {
            // Pega os seus valores de direção e limpando o item armazenado
            Vector2 direction = transform.localScale;
            GameObject deployedItem = null;

            // Verifica qual item está sendo carregado e executa o instanciamento correto de cada um
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
                            deployedItem = PhotonNetwork.Instantiate(combatItems[2].name, new Vector2(player.transform.position.x, player.transform.position.y + 20), transform.rotation);
                            break;
                        }
                    }
                    break;

                case "MEX":
                    deployedItem = PhotonNetwork.Instantiate(combatItems[3].name, transform.position, transform.rotation);
                    break;
            }

            // Verifica se há um item armazenado na variável, pega o seu script e determina a sua direção (no código do item, ele é atirado na direção determinada aqui)
            if (deployedItem != null)
            {
                CombatItem combatItem = deployedItem.GetComponent<CombatItem>();
                combatItem.SetDirection(direction);
            }

            // Perdendo a condição de estar carregando um item
            hasItem = false;
            Debug.Log(direction);
        }
    }

    void Flip(float dirX)
    {
        // Verificando para que lado o personagem está olhando e para qual direção o jogador está se movimentando
        if (isFacingRight && dirX > 0 || !isFacingRight && dirX < 0)
        {
            // Flipando o Game Object através de sua escala
            Vector2 localScale = transform.localScale;
            isFacingRight = !isFacingRight;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void TakeHit(int damageTaken)
    {
        // Recebendo dano (limitado entre 0 e 200) e executando a coroutine que atordoa o personagem
        hitPoints = Mathf.Clamp(hitPoints + damageTaken, 0, 200);
        StartCoroutine(Stagger());
    }

    IEnumerator Stagger()
    {
        // Determinando o estado de "acertado" e chamando o parâmetro trigger do animator que toca a animação de "apanhar"
        isHit = true;
        anim.SetTrigger("Hurt");
        yield return new WaitForSeconds(1f);
        isHit = false;
    }

    public float ScaleHitForce()
    {
        // Executando a lógica de escalonamento da força que projeta o personagem para longe e retornando o valor multiplicador da força
        float knockbackMultiplier = Mathf.Pow(2f, (float)hitPoints / (float)maxHitPoints) - 1f;
        knockbackMultiplier = Mathf.Clamp(knockbackMultiplier, 0f, 2.5f);
        hitForce = baseHitForce * knockbackMultiplier;
        float knockbackPercentage = knockbackMultiplier * 100f;
        return (int)knockbackPercentage;
    }

    bool IsGrounded()
    {
        // Criando uma caixa nos pés do personagem para verificar se ele está no chão e retornando a informação
        if (Physics.BoxCast(col.bounds.center, footSize, -transform.up, transform.rotation, footOffsetY, groundLayer))
            return true;
        else
            return false;
    }

    public void Knockout()
    {
        // Limitando a perda de vidas entre -1 e 3
        lives = Mathf.Clamp(lives - 1, -1, 3);

        // Verificando se ainda há 0 ou mais vidas antes de resetar o personagem para sua posição inicial
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
        // Enviando informações ao Game Manager (essas informações são utilizadas lá para controle dos elementos da HUD)
        if (GameManager.Instance != null)
            GameManager.Instance.ReceiveInfos(playerNum, ScaleHitForce(), lives);
    }

    void OnDrawGizmos()
    {
        // Desenhando a caixa de verificação nos pés do personagem (existe apenas no editor da Unity)
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(col.bounds.center - transform.up * footOffsetY, footSize);
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("triggered something");

        // Verifica se colidiu com um objeto de layer "Hurtbox"
        if (other.gameObject.layer.Equals(6))
        {
            Debug.Log("triggered a hurtbox");

            // Verifica a tag do objeto colidido
            switch (other.gameObject.tag)
            {
                // Executa a lógica de colisão com ataques vindos de um player
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

                // Executa a lógica de colisão com ataques vindos de Itens de Combate
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

                // Executa a lógica de colisão com Obstáculos o cenário
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

        // Verifica se colidiu com um item coletável 
        if (other.gameObject.CompareTag("ObtainableItem"))
        {
            // Verifica se não está carregando um item antes de coletar o item colidido
            if (!hasItem)
            {
                hasItem = true;
                heldItem = other.gameObject.GetComponent<ObtainableItem>().itemID;
            }
        }
    }
}