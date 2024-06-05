
using UnityEngine;

public class DropManager : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private Money moneyPrefab;


    private void Awake()
    {
        Enemy.onKill += EnemyOnKillCallBack;
    }


    private void OnDestroy()
    {
        Enemy.onKill += EnemyOnKillCallBack;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

        private void EnemyOnKillCallBack(Vector2 enemyPosition)
    {
        Instantiate(moneyPrefab, enemyPosition, Quaternion.identity, transform);
    }
}
