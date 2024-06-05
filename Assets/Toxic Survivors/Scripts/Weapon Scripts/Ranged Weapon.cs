using UnityEngine;
using UnityEngine.Pool;

public class RangedWeapon : Weapon
{
    [Header("Elements")]
    [SerializeField] private Transform shootingPoint; // Ponto de onde as balas serão disparadas

    [Header("Bullet Pooling")]
    private ObjectPool<Bullet> bulletPool; // Pool de balas
    [SerializeField] private Bullet bulletPrefab; // Prefab da bala

    void Start()
    {
        // Inicializa o pool de balas com os métodos de criação e gerenciamento
        bulletPool = new ObjectPool<Bullet>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy, true, 1000);
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        AutoAim(); // Chama o método para realizar o auto-aiming
    }

    // Método para criar uma nova instância de bala
    private Bullet CreateFunction()
    {
        Bullet bulletInstance = Instantiate(bulletPrefab, shootingPoint.position, Quaternion.identity);
        bulletInstance.Configure(this); // Configura a bala com referência a esta arma
        return bulletInstance;
    }

    // Ação a ser executada ao obter uma bala do pool
    private void ActionOnGet(Bullet bullet)
    {
        bullet.Reload(); // Recarrega a bala
        bullet.transform.position = shootingPoint.position; // Define a posição da bala como o ponto de disparo
        bullet.gameObject.SetActive(true); // Ativa o objeto da bala
    }

    // Ação a ser executada ao liberar uma bala de volta para o pool
    private void ActionOnRelease(Bullet bullet)
    {
        bullet.gameObject.SetActive(false); // Desativa o objeto da bala
    }

    // Ação a ser executada ao destruir uma bala
    private void ActionOnDestroy(Bullet bullet)
    {
        Destroy(bullet); // Destrói o objeto da bala
    }

    // Método para liberar uma bala de volta para o pool
    public void ReleaseBullet(Bullet bullet)
    {
        bulletPool.Release(bullet); // Libera a bala de volta para o pool
    }

    // Método para realizar o auto-aiming e gerenciar os disparos
    private void AutoAim()
    {
        Enemy closestEnemy = GetClosestEnemy(); // Obtém o inimigo mais próximo

        Vector2 targetUpVector = Vector3.up; // Vetor para apontar para cima por padrão

        if(closestEnemy != null)
        {
            targetUpVector = (closestEnemy.transform.position - transform.position).normalized; // Define o vetor para apontar em direção ao inimigo
            transform.up = targetUpVector; // Aponta a arma na direção do inimigo

            ManageShooting(); // Gerencia os disparos se houver um inimigo próximo
            return;    
        }

        // Suaviza a rotação da arma em direção para cima se não houver inimigo próximo
        transform.up = Vector3.Lerp(transform.up, targetUpVector, Time.deltaTime * aimLerp);
    }

    // Método para gerenciar os disparos
    private void ManageShooting()
    {
        attackTimer += Time.deltaTime; // Incrementa o temporizador de ataque
        
        if(attackTimer >= attackDelay) // Se o temporizador de ataque atingir o atraso de ataque
        {
            attackTimer = 0;
            Shoot(); // Realiza o disparo
        }
    }

    // Método para realizar o disparo
    private void Shoot()
    {
        int damage = GetDamage(out bool isCriticalHit); // Obtém o dano do disparo e se foi um acerto crítico
        Bullet bulletInstance = bulletPool.Get(); // Obtém uma bala do pool
        bulletInstance.Shoot(damage, transform.up, isCriticalHit); // Realiza o disparo com a bala obtida
    }
}
