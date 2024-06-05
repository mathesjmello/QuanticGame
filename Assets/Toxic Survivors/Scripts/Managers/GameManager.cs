using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start é chamado antes da primeira atualização do frame
    void Start()
    {
        Application.targetFrameRate = 60; // Define a taxa de quadros alvo como 60 FPS
    }

    // Update é chamado uma vez por frame
    void Update()
    {
        // Este método está vazio, pois não temos lógica de atualização específica para o GameManager neste exemplo
    }
}
