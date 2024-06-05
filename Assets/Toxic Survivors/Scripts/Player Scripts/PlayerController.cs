using UnityEngine;

// Requer que o componente Rigidbody2D esteja presente no mesmo GameObject
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    // Referências configuráveis no inspector
    [SerializeField] private MobileJoystick Playerjoystick; // Referência ao joystick móvel para controle do jogador
    [SerializeField] private float moveSpeed; // Velocidade de movimento do jogador
    private Rigidbody2D rig; // Referência ao Rigidbody2D do jogador

    // Variáveis internas
    private Animator anim; // Referência ao componente Animator
    private int facingDirection = 1; // Direção que o jogador está enfrentando: 1 = direita, -1 = esquerda
    private float moveHorizontal, moveVertical; // Variáveis para armazenar entrada horizontal e vertical
    private Vector2 movement; // Vetor de movimento para armazenar a direção de movimento

    // Método Start é chamado antes da primeira atualização do frame
    void Start()
    {
        // Obtém e armazena a referência ao componente Animator
        anim = GetComponent<Animator>();

        // Obtém e armazena a referência ao componente Rigidbody2D
        rig = GetComponent<Rigidbody2D>();

        // Inicializa a velocidade do Rigidbody2D para a direita
        rig.velocity = Vector2.right;
    }

    // Método Update é chamado uma vez por frame
    private void Update()
    {
        // Obtém a entrada do joystick e normaliza o vetor de movimento
        movement = Playerjoystick.GetMoveVector().normalized;

        // Define o parâmetro de velocidade no Animator
        float speed = movement.magnitude;
        anim.SetFloat("moveSpeed", speed);

        // Log de depuração para rastrear a velocidade de movimento (desativado)
        // Debug.Log($"moveSpeed: {speed}");

        // Determina a direção que o jogador está enfrentando com base na entrada horizontal
        if (movement.x != 0)
            facingDirection = movement.x > 0 ? 1 : -1;

        // Aplica a direção de enfrentamento ao transform do jogador
        transform.localScale = new Vector2(facingDirection, 1);
    }

    // Método FixedUpdate é chamado em intervalos fixos de tempo
    private void FixedUpdate()
    {
        // Define a velocidade do Rigidbody2D com base na entrada do joystick, velocidade de movimento e deltaTime
        rig.velocity = Playerjoystick.GetMoveVector() * moveSpeed * Time.deltaTime;
    }
}
