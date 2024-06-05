using UnityEngine;

// Garante que o componente Rigidbody2D e Collider2D estejam presentes no GameObject
[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class EnemyBullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig; // Referência ao componente Rigidbody2D
    private Collider2D coll; // Referência ao componente Collider2D
    private RangedEnemyAttack rangedEnemyAttack; // Referência ao script RangedEnemyAttack

    [Header("Settings")]
    private int damage; // Dano que a bala causa ao jogador
    [SerializeField] private int moveSpeed; // Velocidade de movimento da bala

    // Método Awake é chamado quando o script é inicializado
    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>(); // Obtém o componente Rigidbody2D
        coll = GetComponent<Collider2D>(); // Obtém o componente Collider2D

        // Configura um delay para liberar a bala após 5 segundos
        LeanTween.delayedCall(gameObject, 5, () => rangedEnemyAttack.ReleaseBullet(this));
    }

    // Método chamado quando a bala colide com outro Collider2D
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Verifica se o objeto colidido é um jogador
        if (collider.TryGetComponent(out Player player))
        {
            LeanTween.cancel(gameObject); // Cancela qualquer animação LeanTween associada ao objeto
            player.TakeDamage(damage); // Aplica dano ao jogador
            coll.enabled = false; // Desabilita o collider da bala
            rangedEnemyAttack.ReleaseBullet(this); // Libera a bala para reutilização
        }
    }

    // Método para resetar a bala após ser liberada
    public void Reload()
    {
        rig.velocity = Vector2.zero; // Reseta a velocidade da bala
        coll.enabled = true; // Reabilita o collider da bala
    }

    // Método para configurar a referência ao script RangedEnemyAttack
    public void Configure(RangedEnemyAttack rangedEnemyAttack)
    {
        this.rangedEnemyAttack = rangedEnemyAttack;
    }

    // Método para disparar a bala
    public void Shoot(int damage, Vector2 direction)
    {
        this.damage = damage; // Define o dano da bala
        transform.right = direction; // Define a direção da bala
        rig.velocity = direction * moveSpeed; // Define a velocidade da bala
    }
}
