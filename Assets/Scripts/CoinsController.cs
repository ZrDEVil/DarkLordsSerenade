using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinsController : MonoBehaviour
{

    [SerializeField] private GameObject coinsObj;
    private GameObject[] coinsArray;
    private int childrenCount;

    [SerializeField] public static GameObject instanciaCoinsController;
    private static CoinsController _instanciaCoinsController;
    public static CoinsController InstanciaCoinsController {
        get {
            if(_instanciaCoinsController == null) {
                _instanciaCoinsController = instanciaCoinsController.GetComponent<CoinsController>();
            }
            return _instanciaCoinsController;
        }
    }

    private void Awake() {
        instanciaCoinsController = FindObjectOfType<CoinsController>().gameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        GetCoins();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GetCoins() {
        childrenCount = coinsObj.transform.childCount;
        coinsArray = new GameObject[childrenCount];

        for(int i=0; i < childrenCount;i++) {
            coinsArray[i] = coinsObj.transform.GetChild(i).gameObject;
        }
    }

    public void RestartCoins() {
        for(int i = 0;i < childrenCount;i++) {
            coinsArray[i].SetActive(true);
        }
    }
}
