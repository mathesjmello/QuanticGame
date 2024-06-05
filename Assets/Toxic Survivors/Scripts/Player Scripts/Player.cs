using UnityEngine;

// Requer que o componente PlayerHealth esteja presente no mesmo GameObject
[RequireComponent(typeof(PlayerHealth))]
public class Player : MonoBehaviour
{
    // Seção de componentes, acessível no inspector do Unity
    [Header("Components")]
    private PlayerHealth playerHealth; // Referência ao componente PlayerHealth
    [SerializeField] private CircleCollider2D coll; // Referência ao componente CircleCollider2D, configurável pelo inspector

    // Método chamado ao inicializar o script, antes do Start
    private void Awake()
    {
        // Obtém e armazena a referência ao componente PlayerHealth presente no mesmo GameObject
        playerHealth = GetComponent<PlayerHealth>();
    }

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        // No momento, este método está vazio e pode ser usado para inicialização adicional
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // No momento, este método está vazio e pode ser usado para atualizações a cada frame
    }

    // Método público para aplicar dano ao jogador
    public void TakeDamage(int damage)
    {
        // Chama o método TakeDamage do componente PlayerHealth, passando o valor do dano
        playerHealth.TakeDamage(damage);
    }

    // Método público para obter o centro do colisor do jogador
    public Vector2 GetCenter()
    {
        // Retorna a posição do transform do jogador somada ao offset do colisor circular
        return (Vector2)transform.position + coll.offset;
    }
}
