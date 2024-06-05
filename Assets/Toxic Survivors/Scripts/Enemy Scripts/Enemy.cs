using UnityEngine;
using System;

// Classe abstrata Enemy, que serve como base para diferentes tipos de inimigos
public abstract class Enemy : MonoBehaviour
{
    [Header("Components")]
    protected EnemyMovement movement; // Referência ao componente de movimento do inimigo

    [Header("Health")]
    [SerializeField] protected int maxHealth; // Saúde máxima do inimigo
    protected int health; // Saúde atual do inimigo

    [Header("Elements")]
    protected Player player; // Referência ao jogador

    [Header("Spawn Sequence Related")]
    [SerializeField] protected SpriteRenderer render; // Renderizador do inimigo
    [SerializeField] protected SpriteRenderer spawnIndicator; // Indicador de spawn do inimigo
    [SerializeField] protected Collider2D enemyCollider; // Colisor do inimigo
    protected bool hasSpawned; // Indica se o inimigo já foi spawnado

    [Header("Attack")]
    [SerializeField] protected float playerDetectionRadius; // Raio de detecção do jogador

    [Header("Effects")]
    [SerializeField] protected ParticleSystem deathParticles; // Partículas de morte do inimigo

    [Header("Actions")]
    public static Action<int, Vector2, bool> onDamageTaken; // Ação a ser chamada quando o inimigo toma dano
    public static Action<Vector2> onKill; // Ação a ser chamada quando o inimigo é morto

    [Header("DEBUG")]
    [SerializeField] protected bool gizmos; // Ativar/desativar a exibição de gizmos para depuração

    // Start é chamado antes da primeira atualização do frame
    protected virtual void Start()
    {
        health = maxHealth; // Inicializa a saúde do inimigo com a saúde máxima
        movement = GetComponent<EnemyMovement>(); // Obtém o componente EnemyMovement
        player = FindAnyObjectByType<Player>(); // Encontra qualquer objeto do tipo Player

        if (player == null)
        {
            Destroy(gameObject); // Destrói o inimigo se não houver jogador
        } 

        StartSpawnSequence(); // Inicia a sequência de spawn do inimigo
    }

    // Verifica se o inimigo pode atacar
    protected bool CanAttack()
    {
        return render.enabled; // O inimigo pode atacar se o renderizador estiver habilitado
    }

    // Inicia a sequência de spawn do inimigo
    private void StartSpawnSequence()
    {
        SetRenderersVisibility(false); // Define a visibilidade dos renderizadores para falso

        // Escala o indicador de spawn para cima e para baixo
        Vector3 targetScale = spawnIndicator.transform.localScale * 1.2f;
        LeanTween.scale(spawnIndicator.gameObject, targetScale, .3f)
            .setLoopPingPong(4).setOnComplete(SpawnSequenceCompleted);
    }

    // Conclui a sequência de spawn do inimigo
    private void SpawnSequenceCompleted()
    {
        SetRenderersVisibility(true); // Define a visibilidade dos renderizadores para verdadeiro
        hasSpawned = true; // Marca o inimigo como spawnado

        enemyCollider.enabled = true; // Habilita o colisor do inimigo

        movement.StorePlayer(player); // Armazena a referência ao jogador no componente de movimento
    }

    // Define a visibilidade dos renderizadores do inimigo e do indicador de spawn
    private void SetRenderersVisibility(bool visibility)
    {
        render.enabled = visibility; // Define a visibilidade do renderizador do inimigo
        spawnIndicator.enabled = !visibility; // Define a visibilidade do indicador de spawn
    }

    // Método público para aplicar dano ao inimigo
    public void TakeDamage(int damage, bool isCriticalHit)
    {
        int realDamage = Mathf.Min(damage, health); // Calcula o dano real, garantindo que não exceda a saúde atual
        health -= realDamage; // Reduz a saúde do inimigo pelo dano real
        onDamageTaken?.Invoke(damage, transform.position, isCriticalHit); // Invoca a ação onDamageTaken

        if (health <= 0) // Verifica se a saúde do inimigo é zero ou menor
            Kill(); // Chama o método Kill se a saúde for zero ou menor
    }

    // Método privado para lidar com a morte do inimigo
    private void Kill()
    {
        onKill?.Invoke(transform.position); // Invoca a ação onKill

        // Desparenta as partículas de morte e as reproduz
        deathParticles.transform.parent = null;
        deathParticles.Play();
        Destroy(gameObject); // Destrói o inimigo
    }

    // Método para desenhar gizmos para depuração
    private void OnDrawGizmos()
    {
        if (!gizmos)
            return;

        Gizmos.color = Color.red; // Define a cor do gizmo
        Gizmos.DrawWireSphere(transform.position, playerDetectionRadius); // Desenha uma esfera de raio playerDetectionRadius
    }
}
