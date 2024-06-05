using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    enum State
    {
        Idle, // Estado de espera
        Attack // Estado de ataque
    }

    [Header("Elements")]
    [SerializeField] private Transform hitDetectionTransform; // Transform do ponto de detecção de acerto
    [SerializeField] private BoxCollider2D hitCollider; // Collider da área de acerto

    private State state; // Estado atual da arma
    private List<Enemy> damagedEnemies = new List<Enemy>(); // Lista de inimigos já danificados pela arma

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        state = State.Idle; // Define o estado inicial como Idle (espera)
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        switch(state)
        {
            case State.Idle:
                AutoAim(); // Método para determinar a direção do ataque
                break;
            case State.Attack:
                Attacking(); // Método para realizar o ataque
                break;
        } 
    }

    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy(); // Obtém o inimigo mais próximo

        Vector2 targetUpVector = Vector3.up;
        if(closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized;
            transform.up = targetUpVector;
            ManageAttack(); // Gerencia o ataque se houver um inimigo próximo           
        }

        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp); // Suaviza a rotação da arma em direção ao inimigo

        IncrementAttackTimer(); // Incrementa o temporizador de ataque
    }

    private void ManageAttack()
    {        
        if(attackTimer >= attackDelay) // Se o temporizador de ataque atingir o atraso de ataque
        {
            attackTimer = 0;
            StartAttack(); // Inicia o ataque
        }
    }

    private void IncrementAttackTimer()
    {
        attackTimer += Time.deltaTime; // Incrementa o temporizador de ataque
    }

    private void StartAttack()
    {
        animator.Play("Attack"); // Inicia a animação de ataque
        state = State.Attack; // Define o estado como Attack (ataque)
        
        damagedEnemies.Clear(); // Limpa a lista de inimigos danificados

        animator.speed = 1f / attackDelay; // Define a velocidade da animação de acordo com o atraso de ataque
    }

    private void Attacking()
    {
        Attack(); // Realiza o ataque
    }

    private void Attack()
    {
        // Detecta colisões com inimigos na área de acerto
        Collider2D[] enemies = Physics2D.OverlapBoxAll
            (
            hitDetectionTransform.position, 
            hitCollider.bounds.size, 
            hitDetectionTransform.localEulerAngles.z,
            enemyMask
            );

        // Para cada inimigo detectado na área de acerto
        for(int i = 0; i < enemies.Length; i++)
        {   
            Enemy enemy = enemies[i].GetComponent<Enemy>();

            // Verifica se o inimigo ainda não foi danificado por esta arma
            if(!damagedEnemies.Contains(enemy))
            {
                int damage = GetDamage(out bool isCriticalHit); // Obtém o dano do ataque e se foi um acerto crítico
                
                enemy.TakeDamage(damage,isCriticalHit); // Aplica o dano ao inimigo
                damagedEnemies.Add(enemy); // Adiciona o inimigo à lista de danificados
            }  
        }
    }
}
