using UnityEngine;

public class Cameracontroller : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Transform target; // O objeto que a câmera seguirá

    [Header("Settings")]
    [SerializeField] private Vector2 minMaxXY; // Limites de movimento da câmera

    private void LateUpdate()
    {   
        // Verifica se o objeto de destino não é nulo
        if(target == null)
        {
            Debug.LogWarning("No target to follow."); // Emite um aviso no console
            return; // Sai do método para evitar erros
        }
        
        // Define a posição alvo da câmera
        Vector3 targetPosition = target.position;
        targetPosition.z = -10; // Mantém a câmera sempre à frente do restante da cena

        // Limita a posição alvo da câmera dentro dos limites especificados
        targetPosition.x = Mathf.Clamp(targetPosition.x, -minMaxXY.x, minMaxXY.x);
        targetPosition.y = Mathf.Clamp(targetPosition.y, -minMaxXY.y, minMaxXY.y);

        // Define a posição da câmera para a posição alvo
        transform.position = targetPosition;
    }    
}
