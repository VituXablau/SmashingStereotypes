using UnityEngine;

// Script do item em sua versão coletável, a qual o jogador precisa colidir para ter acesso à sua versão de combate
public class ObtainableItem : MonoBehaviour
{
    // Variável de identificação do item
    public string itemID;

    void Start()
    {
        // Destruindo o item em 20 segundos após seu surgimento
        Destroy(gameObject, 20);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Verifica se o player colidiu com o item e o destrói em seguida
        if (other.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }
}
