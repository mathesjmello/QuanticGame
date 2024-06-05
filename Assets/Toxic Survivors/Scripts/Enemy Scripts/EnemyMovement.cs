using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Elements")]
    private Player player; // Referência ao jogador

    [Header("Settings")]
    [SerializeField] private float moveSpeed; // Velocidade de movimento do inimigo

    // Método para seguir o jogador
    public void FollowPlayer()
    {
        // Calcula a direção do inimigo em relação ao jogador, normalizada para um vetor unitário
        Vector2 direction = (player.transform.position - transform.position).normalized;
        
        // Calcula a posição alvo movendo-se na direção calculada com base na velocidade de movimento e deltaTime
        Vector2 targetPosition = (Vector2)transform.position + direction * moveSpeed * Time.deltaTime;

        // Atualiza a posição do inimigo para a posição alvo
        transform.position = targetPosition;
    }

    // Armazena a referência ao jogador
    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    // Método Update é chamado uma vez por frame
    void Update()
    {
        // Se o jogador não for nulo, chama o método FollowPlayer para seguir o jogador
        // if(player != null)
        //     FollowPlayer();
    }
}
