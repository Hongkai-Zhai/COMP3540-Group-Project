using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class StartButton : MonoBehaviour
{
    // Start is called before the first frame update
    private Button button;
    private SpawnManager spawnManager;
    public int diffculty;
    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(Begin);
        spawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Begin()
    {
        spawnManager.StartGame();
    }
}
