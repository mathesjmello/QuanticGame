using TMPro; // Importa a biblioteca TextMeshPro para usar componentes de texto
using UnityEngine;
using UnityEngine.UI; // Importa a biblioteca UI para usar componentes de interface de usuário
using UnityEngine.SceneManagement; // Importa a biblioteca SceneManagement para gerenciar cenas

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")] // Seção de configuração de saúde do jogador
    [SerializeField] private int maxHealth; // Saúde máxima do jogador
    private int health; // Saúde atual do jogador

    [Header("Elements")] // Seção de elementos da interface de usuário
    [SerializeField] private Slider healthSlider; // Referência ao slider da barra de saúde
    [SerializeField] private TextMeshProUGUI healthtext; // Referência ao texto que exibe a saúde
    [SerializeField] private Animator animator; // Referência ao componente Animator

    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth; // Inicializa a saúde atual com a saúde máxima
        UpdateUI(); // Atualiza a interface de usuário com os valores iniciais
    }

    // Método público para aplicar dano ao jogador
    public void TakeDamage(int damage)
    {
        int realDamage = Mathf.Min(damage, health); // Calcula o dano real, garantindo que não exceda a saúde atual
        health -= realDamage; // Reduz a saúde do jogador pelo dano real

        animator.SetTrigger("TakeDamage");
        UpdateUI(); // Atualiza a interface de usuário após tomar dano

        if (health <= 0) // Verifica se a saúde do jogador é zero ou menor
        {
            Kill(); // Chama o método Kill se a saúde for zero ou menor
        }
    }

    // Método privado para lidar com a morte do jogador
    private void Kill()
    {
        SceneManager.LoadScene(0); // Recarrega a cena atual (índice 0)
    }

    // Método privado para atualizar a interface de usuário
    private void UpdateUI()
    {
        float healthBarValue = (float)health / maxHealth; // Calcula o valor da barra de saúde como uma fração da saúde máxima
        healthSlider.value = healthBarValue; // Atualiza o slider da barra de saúde

        healthtext.text = health + "/" + maxHealth; // Atualiza o texto da saúde
    }

    // Update é chamado uma vez por frame
    private void Update()
    {
        
    }
}
