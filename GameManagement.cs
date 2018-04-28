using UnityEngine;
using System.Collections;

public class GameManagement : MonoBehaviour {
	public int round = 1;
	int zombiesInRound = 10;
	public static int zombiesLeftInRound = 10;
	int zombiesSpawnedInRound = 0;
	float zombieSpawnTimer = 0;
	public Transform[] zombieSpawnPoints;
	public GameObject zombieEnemy;
	public static bool player1HasJug = false;



	float countdown= 0;

	public static int playerScore = 0;
	public static int playerCash = 0;


	public GUISkin mySkin;

	// Use this for initialization
	void Awake () {
		Screen.lockCursor = true;
		
	}


	
	// Update is called once per frame
	void Update () {
		if(zombiesSpawnedInRound < zombiesInRound  && countdown == 0)
		{
			if(zombieSpawnTimer > 2)
			{
				SpawnZombie ();
				zombieSpawnTimer = 0;
			}
			else
			{
				zombieSpawnTimer+=Time.deltaTime;
			}
		}
		else if (zombiesLeftInRound == 0)
		{
			StartNextRound();
		}


		if(countdown > 0)
			countdown -= Time.deltaTime;
		else 
			countdown = 0;
	}

	public static void AddPoints(int pointValue)
	{
		playerScore += pointValue;
		playerCash += pointValue;
	}


	void SpawnZombie()
	{
		Vector3 randomSpawnPoint = zombieSpawnPoints[Random.Range (0,zombieSpawnPoints.Length)].position;
		Instantiate(zombieEnemy,randomSpawnPoint,Quaternion.identity);
		zombiesSpawnedInRound ++;
	}

	void StartNextRound()
	{
		zombiesInRound = zombiesLeftInRound = round * 10;
		zombiesSpawnedInRound = 0;
		countdown = 15;
		round++;
	}

	void OnGUI()
	{
		GUI.skin = mySkin;
		GUIStyle style1 = mySkin.customStyles[0];

		GUI.Label(new Rect(40, Screen.height - 80, 100, 60), " SCORE :");
		GUI.Label(new Rect(100, Screen.height - 80, 160, 60), "" + playerScore, style1);
		
		GUI.Label(new Rect(40, Screen.height - 110, 100, 60), " $ :");
		GUI.Label(new Rect(100, Screen.height - 110, 160, 60), "" + playerCash, style1);

		GUI.Label(new Rect(540, Screen.height - 80, 100, 60), " Z :");
		GUI.Label(new Rect(600, Screen.height - 80, 160, 60), "" + zombiesLeftInRound, style1);

		if(countdown != 0)
		{
			GUI.Label(new Rect(Screen.width/2 - 50, Screen.height/2 - 80, 100, 60), " NextRound :");
			GUI.Label(new Rect(Screen.width/2 + 50, Screen.height/2 - 80, 160, 60), "" + Mathf.RoundToInt(countdown), style1);
		}

		GUI.Label(new Rect(740, Screen.height - 80, 100, 60), " Spawned :");
		GUI.Label(new Rect(800, Screen.height - 80, 160, 60), "" + zombiesSpawnedInRound, style1);
		
	}

}
