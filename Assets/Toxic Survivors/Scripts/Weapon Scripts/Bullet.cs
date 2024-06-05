using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class Bullet : MonoBehaviour
{
    [Header("Elements")]
    private Rigidbody2D rig; // Rigidbody da bala
    private Collider2D coll; // Collider da bala
    private RangedWeapon rangedWeapon; // Referência à arma que disparou a bala

    [Header("Settings")]
    [SerializeField] private int moveSpeed; // Velocidade de movimento da bala
    [SerializeField] private LayerMask enemyMask; // Máscara de camada para detecção de inimigos
    private int damage; // Dano causado pela bala
    private Enemy target; // Alvo da bala (inimigo atingido)
    private bool isCriticalHit; // Flag indicando se o ataque foi um acerto crítico

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>(); // Obtém o Rigidbody2D da bala
        coll = GetComponent<Collider2D>(); // Obtém o Collider2D da bala
    }

    public void Configure(RangedWeapon rangedWeapon)
    {
        this.rangedWeapon = rangedWeapon; // Configura a referência à arma que disparou a bala
    }

    public void Reload()
    {
        target = null; // Reseta o alvo da bala para nulo

        rig.velocity = Vector2.zero; // Reseta a velocidade da bala
        coll.enabled = true; // Ativa o Collider da bala
    }

    private void Release()
    {
        if (gameObject.activeSelf)
            rangedWeapon.ReleaseBullet(this); // Libera a bala de volta para a arma
    }

    public void Shoot(int damage, Vector2 direction, bool isCriticalHit)
    {
        this.damage = damage; // Define o dano da bala
        this.isCriticalHit = isCriticalHit; // Define se o ataque é um acerto crítico
        transform.right = direction; // Define a direção de movimento da bala
        rig.velocity = direction * moveSpeed; // Aplica a velocidade de movimento à bala

        // Agenda a liberação da bala após um atraso (por exemplo, 5 segundos)
        LeanTween.delayedCall(gameObject, 5, () => Release());
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (target != null) // Se a bala já atingiu um alvo, sai da função
            return;

        if (IsInLayerMask(coll.gameObject.layer, enemyMask)) // Se o colisor estiver em uma camada de inimigos
        {
            target = coll.GetComponent<Enemy>(); // Obtém o componente Enemy do objeto atingido

            if (target != null) // Se o objeto atingido for um inimigo
            {
                LeanTween.cancel(gameObject); // Cancela qualquer movimento programado da bala
                Attack(target); // Chama o método para atacar o inimigo
                Release(); // Libera a bala após atingir o alvo
            }
        }
    }

    private void Attack(Enemy enemy)
    {
        enemy.TakeDamage(damage, isCriticalHit); // Chama o método de dano do inimigo
    }

    private bool IsInLayerMask(int layer, LayerMask layerMask)
    {
        return (layerMask.value & (1 << layer)) != 0; // Verifica se a camada está presente na máscara de camadas
    }
}
