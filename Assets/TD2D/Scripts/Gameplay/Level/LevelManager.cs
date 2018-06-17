using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
	public string levelUiSceneName;
	public int goldAmount = 20;
	public int defeatAttempts = 1;
	public List<GameObject> allowedEnemies = new List<GameObject>();
	public List<GameObject> allowedTowers = new List<GameObject>();
	public List<GameObject> allowedSpells = new List<GameObject>();

    private UiManager uiManager;
    private int spawnNumbers;
	private int beforeLooseCounter;
	private bool triggered = false;

    void Awake()
    {
		SceneManager.LoadScene(levelUiSceneName, LoadSceneMode.Additive);
    }

    void Start()
	{
		uiManager = FindObjectOfType<UiManager>();
		SpawnPoint[] spawnPoints = FindObjectsOfType<SpawnPoint>();
		spawnNumbers = spawnPoints.Length;
		if (spawnNumbers <= 0)
		{
			Debug.LogError("Have no spawners");
		}
		foreach (SpawnPoint spawnPoint in spawnPoints)
		{
			spawnPoint.randomEnemiesList = allowedEnemies;
		}
		Debug.Assert(uiManager, "Wrong initial parameters");
		uiManager.SetGold(goldAmount);

		beforeLooseCounter = defeatAttempts;
		uiManager.SetDefeatAttempts(beforeLooseCounter);
	}

    void OnEnable()
    {
        EventManager.StartListening("Captured", Captured);
        EventManager.StartListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    void OnDisable()
    {
        EventManager.StopListening("Captured", Captured);
        EventManager.StopListening("AllEnemiesAreDead", AllEnemiesAreDead);
    }

    private void Captured(GameObject obj, string param)
    {
		if (beforeLooseCounter > 0)
		{
			beforeLooseCounter--;
			uiManager.SetDefeatAttempts(beforeLooseCounter);
			if (beforeLooseCounter <= 0)
			{
				triggered = true;
				// Defeat
				EventManager.TriggerEvent("Defeat", null, null);
			}
		}
    }

    private void AllEnemiesAreDead(GameObject obj, string param)
    {
        spawnNumbers--;

        if (spawnNumbers <= 0)
        {
			if (triggered == false)
			{
				EventManager.TriggerEvent("Victory", null, null);
			}
        }
    }
}
