using UnityEngine;
using UnityEngine.Pool;

public class DamageTextManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private DamageText damageTextPrefab; // Prefab do texto de dano

    [Header("Pooling")]
    private ObjectPool<DamageText> damageTextPool; // Pool de objetos para o texto de dano

    private void Awake()
    {
        // Inscreve o método EnemyHitCallBack como um callback para o evento onDamageTaken da classe Enemy
        Enemy.onDamageTaken += EnemyHitCallBack;
    }

    private void OnDestroy()
    {
        // Remove o método EnemyHitCallBack como um callback para o evento onDamageTaken da classe Enemy
        Enemy.onDamageTaken -= EnemyHitCallBack;
    }

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        // Inicializa a pool de objetos para o texto de dano com as funções de criação, obtenção, liberação e destruição
        damageTextPool = new ObjectPool<DamageText>(CreateFunction, ActionOnGet, ActionOnRelease, ActionOnDestroy);
    }

    // Função para criar uma nova instância do texto de dano
    private DamageText CreateFunction()
    {
        return Instantiate(damageTextPrefab, transform); // Instancia o texto de dano
    }

    // Ação realizada ao obter um texto de dano da pool
    private void ActionOnGet(DamageText damageText)
    {
        damageText.gameObject.SetActive(true); // Ativa o texto de dano
    }

    // Ação realizada ao liberar um texto de dano de volta para a pool
    private void ActionOnRelease(DamageText damageText)
    {
        damageText.gameObject.SetActive(false); // Desativa o texto de dano
    }

    // Ação realizada ao destruir um texto de dano
    private void ActionOnDestroy(DamageText damageText)
    {
        Destroy(damageText.gameObject); // Destroi o texto de dano
    }

    // Callback para quando um inimigo recebe dano
    private void EnemyHitCallBack(int damage, Vector2 enemyPos, bool isCriticalHit)
    {
        DamageText damageTextInstance = damageTextPool.Get(); // Obtém um texto de dano da pool

        Vector3 spawnPosition = enemyPos + Vector2.up * 1.5f; // Calcula a posição de spawn do texto de dano
        damageTextInstance.transform.position = spawnPosition; // Define a posição do texto de dano

        damageTextInstance.Animate(damage, isCriticalHit); // Anima o texto de dano com o dano e a indicação de acerto crítico

        // Libera o texto de dano de volta para a pool após um atraso
        LeanTween.delayedCall(1, () => damageTextPool.Release(damageTextInstance));
    }
}
