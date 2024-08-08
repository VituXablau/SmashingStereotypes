using System.Collections;
using UnityEngine;

// Script de mecânicas dos Itens de Combate, como a direção em que se movimentam, seu tempo de ativação e desativação
public class CombatItem : MonoBehaviour
{
    // Variável que identifica o item
    [SerializeField] private string itemID;

    // Componente do Game Object
    private Rigidbody rb;

    // Variável que determina o dano causado pelo item, em segida, variáveis que controlam o seu tempo de ativação
    public int damage;
    [SerializeField] private bool freeze;
    [SerializeField] private float timeToHarm;

    // Variável que recebe a direção em que o item deve ser atirado
    private Vector2 direction;

    // Variáveis que recebem o colisor de dano e o efeito visual gerado pelo item
    [SerializeField] private GameObject damageCollider, vfx;


    void Start()
    {
        // Atribuindo o Rigidbody do Game Object à variável
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Verificando se o item não está "congelado" e inicializa seu comportamento
        if (!freeze)
            StartCoroutine(InitializeBehaviour(timeToHarm));

        Debug.Log(itemID + ": " + damage);
    }

    IEnumerator InitializeBehaviour(float waitTime)
    {
        // Determina o comportamento de cada item
        switch (itemID)
        {
            // Comportamento da bola de futebol
            case "BRA":
                // A bola é atirada na direção em que o jogador está virado
                rb.velocity = new Vector2(direction.x * 20, rb.velocity.y);
                Destroy(gameObject, 10);
                break;

            // Comportamento do balão chinês
            case "CHN":
                // O balão é atirado para cima e vai flutuando até explodir
                rb.velocity = Vector2.up * 4;
                yield return new WaitForSeconds(waitTime);
                rb.velocity = Vector2.zero;
                freeze = true;
                damageCollider.SetActive(true);
                Instantiate(vfx, damageCollider.transform.position, Quaternion.identity);
                Destroy(gameObject, 0.2f);
                break;

            // Comportamento da vaca
            case "IND":
                // A mecânica da vaca é apenas o seu rigidbody com gravidade. Ela é destruída em 8 segundos após seu surgimento
                Destroy(gameObject, 8);
                break;

            // Comportamento do taco
            case "MEX":
                // O taco é atirado para a direção em que o jogador está olhando
                rb.velocity = new Vector2(direction.x * 40, rb.velocity.y);
                Destroy(gameObject, 6);
                break;
        }

        if (!freeze)
        {
            yield return new WaitForSeconds(waitTime);
            damageCollider.SetActive(true);
        }
    }

    // Método chamado no personagem, que recebe a direção em que ele está virado
    public void SetDirection(Vector2 dir)
    {
        // Normalizando o Vector2 que recebe a direção
        direction = dir.normalized;
    }
}
