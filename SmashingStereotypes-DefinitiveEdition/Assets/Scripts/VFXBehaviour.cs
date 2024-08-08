using UnityEngine;

// Script de controle dos efeitos visuais como explosão
public class VFXBehaviour : MonoBehaviour
{
    // Variável que determina o tempo em que o efeito permanece em tela
    [SerializeField] private float screenTime;

    void Start()
    {
        // Destruindo o efeito após seu tempo de tela expirar
        Destroy(gameObject, screenTime);
    }
}
