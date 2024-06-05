using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float range; // Alcance da arma
    [SerializeField] protected LayerMask enemyMask; // Máscara das camadas dos inimigos

    [Header("Attack")]
    [SerializeField] protected int damage; // Dano da arma
    [SerializeField] protected float attackDelay; // Atraso entre ataques
    protected float attackTimer; // Temporizador para controlar o tempo entre ataques

    [Header("Animations")]
    [SerializeField] protected float aimLerp; // Suavização da rotação da arma
    [SerializeField] protected Animator animator; // Referência ao componente de animação

    // Método para obter o inimigo mais próximo dentro do alcance da arma
    protected Enemy GetClosestEnemy()
    {
        Enemy closestEnemy = null;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, range, enemyMask); // Obtém todos os colisores dentro do alcance e da camada especificada

        if (enemies.Length <= 0)
            return null; // Retorna null se não houver inimigos dentro do alcance

        float minDistance = range; // Inicializa a distância mínima como o alcance máximo da arma

        for (int i = 0; i < enemies.Length; i++)
        {
            Enemy enemyChecked = enemies[i].GetComponent<Enemy>(); // Obtém o componente Enemy do inimigo verificado

            float distanceToEnemy = Vector2.Distance(transform.position, enemyChecked.transform.position); // Calcula a distância até o inimigo verificado

            if (distanceToEnemy < minDistance)
            {
                closestEnemy = enemyChecked; // Atualiza o inimigo mais próximo se a distância for menor que a distância mínima
                minDistance = distanceToEnemy; // Atualiza a distância mínima
            }
        }
        return closestEnemy; // Retorna o inimigo mais próximo
    }

    // Método para calcular o dano do ataque e se foi um acerto crítico
    protected int GetDamage(out bool isCriticalHit)
    {
        isCriticalHit = false; // Define inicialmente que não houve acerto crítico

        if (Random.Range(0, 101) <= 50) // Gera um número aleatório entre 0 e 100 e verifica se está abaixo de 50%
        {
            isCriticalHit = true; // Define que houve acerto crítico
            return damage * 2; // Retorna o dano dobrado
        }

        return damage; // Retorna o dano padrão
    }

    // Método para desenhar gizmos no editor para visualizar o alcance da arma
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.magenta; // Define a cor do gizmo como magenta
        Gizmos.DrawWireSphere(transform.position, range); // Desenha uma esfera de alcance ao redor da posição da arma
    }
}
