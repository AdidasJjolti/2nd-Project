using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabsManager : MonoBehaviour
{
    private static PrefabsManager instance;

    public static PrefabsManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindObjectOfType<PrefabsManager>();
            }
            return instance;
        }
    }

    [SerializeField] private GameObject[] cookedPrefabs;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            return;
        }
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public GameObject GetCookedPrefab(int index)
    {
        if(index <0 || index >= cookedPrefabs.Length)
        {
            return null;
        }
        return cookedPrefabs[index];
    }
}
