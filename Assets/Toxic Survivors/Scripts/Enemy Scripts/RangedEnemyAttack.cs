using System;
using UnityEngine;
using UnityEngine.Pool;

public class RangedEnemyAttack : MonoBehaviour
{
    [Header("Elements")]
    private Player player; // Referência ao jogador
    [SerializeField] private Transform shootingPoint; // Ponto de onde os projéteis serão disparados
    [SerializeField] private EnemyBullet bulletPrefab; // Prefab do projétil

    [Header("Settings")]
    [SerializeField] private int damage; // Dano causado por cada projétil
    [SerializeField] private float attackRate; // Taxa de ataque (número de ataques por segundo)
    private float attackDelay; // Tempo de atraso entre ataques
    private float attackTimer; // Temporizador para controlar o tempo entre ataques

    [Header("Bullet Pooling")]
    private ObjectPool<EnemyBullet> bulletPool; // Pool de objetos para os projéteis

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        attackDelay = 1f / attackRate; // Calcula o atraso baseado na taxa de ataque
        attackTimer = attackDelay; // Inicializa o temporizador de ataque para permitir ataque imediato

        // Inicializa a pool de projéteis com as funções de criação, obtenção, liberação e destruição
        bulletPool = new ObjectPool<EnemyBullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Função para criar uma nova instância de projétil
    private EnemyBullet CreateFunction()
    {
        EnemyBullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity); // Instancia o projétil
        bulletInstance.Configure(this); // Configura o projétil com a referência ao script RangedEnemyAttack
        return bulletInstance;
    }

    // Ação realizada ao obter um projétil da pool
    private void ActionOnGet(EnemyBullet bullet)
    {
        bullet.Reload(); // Reseta o estado do projétil
        bullet.transform.position = shootingPoint.position; // Define a posição inicial do projétil
        bullet.gameObject.SetActive(true); // Ativa o projétil
    }

    // Ação realizada ao liberar um projétil de volta para a pool
    private void ActionOnRelease(EnemyBullet bullet)
    {
        bullet.gameObject.SetActive(false); // Desativa o projétil
    }

    // Ação realizada ao destruir um projétil
    private void ActionOnDestroy(EnemyBullet bullet)
    {
        Destroy(bullet.gameObject); // Destroi o projétil
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Implementar a lógica de atualização, se necessário
    }

    // Libera o projétil de volta para a pool
    public void ReleaseBullet(EnemyBullet bullet)
    {
        bulletPool.Release(bullet);
    }

    // Armazena a referência ao jogador
    public void StorePlayer(Player player)
    {
        this.player = player;
    }

    // Função para realizar o ataque com auto-mira
    public void AutoAim()
    {
        ManageShooting();
    }

    // Gerencia o temporizador de disparo e realiza o disparo quando necessário
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime;
        
        if (attackTimer >= attackDelay)
        {
            attackTimer = 0;
            Shoot();
        }
    }

    // Função para disparar um projétil na direção do jogador
    private void Shoot()
    {
        Vector2 direction = (player.GetCenter() - (Vector2)shootingPoint.position).normalized; // Calcula a direção do disparo
        EnemyBullet bulletInstance = bulletPool.Get(); // Obtém um projétil da pool
        bulletInstance.Shoot(damage, direction); // Dispara o projétil
    }
}
