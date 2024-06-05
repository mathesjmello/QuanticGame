using UnityEngine;

// Garante que o componente EnemyMovement esteja presente no GameObject
[RequireComponent(typeof(EnemyMovement))]
public class MeleeEnemy : Enemy
{   
    [Header("Attack")]
    [SerializeField] private int damage; // Dano causado pelo ataque do inimigo
    [SerializeField] private float attackRate; // Taxa de ataque (número de ataques por segundo)
    private float attackDelay; // Tempo de atraso entre ataques
    private float attackTimer; // Temporizador para controlar o tempo entre ataques

    // Start é chamado antes da primeira atualização do frame
    protected override void Start()
    {   
        base.Start(); // Chama o método Start da classe base Enemy
        attackDelay = 1f / attackRate; // Calcula o atraso baseado na taxa de ataque
        attackTimer = attackDelay; // Inicializa o temporizador de ataque para permitir ataque imediato
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Ajusta a escala do inimigo para que ele "olhe" para o jogador
        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);

        // Verifica se o inimigo pode atacar
        if (!CanAttack())
            return;

        // Verifica se o temporizador de ataque excedeu o atraso e tenta atacar
        if (attackTimer >= attackDelay)
            TryAttack();
        else
            Wait(); // Incrementa o temporizador de ataque

        // Faz o inimigo seguir o jogador
        movement.FollowPlayer();
    }

    // Incrementa o temporizador de ataque
    private void Wait()
    {
        attackTimer += Time.deltaTime;
    }

    // Executa o ataque
    private void Attack()
    {
        attackTimer = 0; // Reseta o temporizador de ataque
        if (player != null) // Verifica se o jogador não é nulo
            player.TakeDamage(damage); // Aplica dano ao jogador
    }

    // Tenta executar o ataque se o jogador estiver dentro do raio de detecção
    private void TryAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position); // Calcula a distância até o jogador

        if (distanceToPlayer <= playerDetectionRadius) // Verifica se o jogador está dentro do raio de detecção
            Attack(); // Executa o ataque
    }
}
