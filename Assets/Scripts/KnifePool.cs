using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifePool : MonoBehaviour
{
    public static KnifePool Instance;

    public GameObject KnifePrefab;
    public List<GameObject> knivesList;

    private void Awake()
    {
        if (Instance != null)
            Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public GameObject SpawnKnife()
    {
        Vector3 knifeOriginPoint = GameManager.Instance.knifeOriginPoint.position;

        GameObject spawnedKnife;


        if (knivesList.Count > 0)
        {
            spawnedKnife = knivesList[0];
            knivesList.RemoveAt(0);
        }
        else
        {
            spawnedKnife = Instantiate(KnifePrefab);
        }
        spawnedKnife.transform.position = knifeOriginPoint;
        spawnedKnife.transform.parent = null;
        spawnedKnife.GetComponent<Knife>().Activate();
        return spawnedKnife;
    }

    public void ReturnKnife(GameObject _knife)
    {
        _knife.transform.parent = transform;
        knivesList.Add(_knife);
    }
}
