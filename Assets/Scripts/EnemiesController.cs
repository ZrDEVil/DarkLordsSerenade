using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesController : MonoBehaviour
{
    [SerializeField] private GameObject enemiesPatrolObj;
    private EnemyPatrol[] enemyPatrolArray;
    private int countEnemiesPatrol;

    [SerializeField] public static GameObject instanciaEnemiesController;
    private static EnemiesController _instanciaEnemiesController;
    public static EnemiesController InstanciaEnemiesController {
        get {
            if(_instanciaEnemiesController == null) {
                _instanciaEnemiesController = instanciaEnemiesController.GetComponent<EnemiesController>();
            }
            return _instanciaEnemiesController;
        }
    }

    private void Awake() {
        instanciaEnemiesController = FindObjectOfType<EnemiesController>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetEnemies() {
        countEnemiesPatrol = enemiesPatrolObj.transform.childCount;
        enemyPatrolArray = new EnemyPatrol[countEnemiesPatrol];

        for(int i = 0;i < countEnemiesPatrol;i++) {
            enemyPatrolArray[i] = enemiesPatrolObj.transform.GetChild(i).gameObject.GetComponent<EnemyPatrol>();
        }
    }

    public void RestartEnemies() {
        for(int i = 0;i < countEnemiesPatrol;i++) {
            enemyPatrolArray[i].gameObject.SetActive(true);
            enemyPatrolArray[i].Restart();
        }
    }
}
