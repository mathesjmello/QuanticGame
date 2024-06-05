using UnityEngine;

// Garante que os componentes EnemyMovement e RangedEnemyAttack estejam presentes no GameObject
[RequireComponent(typeof(EnemyMovement), typeof(RangedEnemyAttack))]
public class RangedEnemy : Enemy
{
    private RangedEnemyAttack attack; // Referência ao componente RangedEnemyAttack

    // Start é chamado antes da primeira atualização do frame
    protected override void Start()
    {
        base.Start(); // Chama o método Start da classe base Enemy
        attack = GetComponent<RangedEnemyAttack>(); // Obtém o componente RangedEnemyAttack
        attack.StorePlayer(player); // Armazena a referência do jogador no componente de ataque à distância
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Verifica se o inimigo pode atacar
        if (!CanAttack())
            return;

        ManageAttack(); // Gerencia a lógica de ataque

        // Ajusta a escala do inimigo para que ele "olhe" para o jogador
        transform.localScale = player.transform.position.x > transform.position.x ? Vector3.one : Vector3.one.With(x: -1);
    }

    // Tenta realizar o ataque
    private void TryAttack()
    {
        attack.AutoAim(); // Realiza o ataque com auto-mira
    }

    // Gerencia o ataque do inimigo
    private void ManageAttack()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position); // Calcula a distância até o jogador

        if (distanceToPlayer > playerDetectionRadius) // Se a distância for maior que o raio de detecção
            movement.FollowPlayer(); // Faz o inimigo seguir o jogador
        else
            TryAttack(); // Tenta realizar o ataque
    }
}
