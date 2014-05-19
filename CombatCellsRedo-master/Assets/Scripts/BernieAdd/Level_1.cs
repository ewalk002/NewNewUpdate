using UnityEngine;
using System.Collections;

public class Level_1 : MonoBehaviour {
	
	//Public Variables
	public int currentWave = 1;
	public float InterWaveWait = 30;
	public float ShowBtnWait = 10;
	public Transform destination = null;
	public Transform []spawnPoints;	
	
	
	//Wave and WaitTimes
	private int maxWaves = 5;
	private GameObject[][] Waves;
	private float[][] Wait_Times;
	
	//Current index of enemy
	private int enemyIndex = 0;
	
	//Sey up next enemy 
	private bool readyForNextEnemy = true;
	private float nextEnemySpawnTime = 0;
	
	//Timers for InterWave Waiting and showing start wave bt
	private float InterWaveTimer = 0;
	private float ShowBtnTimer = 0;
	
	//If the current wave is compelete
	private bool waveDone = false;
	
	//Reference to the GameEngine
	private GameObject GameEngine;
	
	//Refernce to Enemies
	private GameObject Cube_Enemy;
	
	//To select a random location for spawn points
	private int rand_loc;
	
	
	void Start () {
		//EnemySetup
		Cube_Enemy = (GameObject)Resources.Load ( ConstantsLib.ENEMY_CUBE_PATH, typeof(GameObject));
		
		
		//WaveSetup
		Waves = new GameObject[maxWaves][];
		Wait_Times = new float[maxWaves][];
		//Wave0
		Waves [0] = new GameObject[4];
		Wait_Times[0] = new float[3];
		Waves [0][0] = Cube_Enemy;
		Wait_Times [0] [0] = .2f;
		Waves [0][1] = Cube_Enemy;
		Wait_Times [0] [1] = 1.2f;
		Waves [0][2] = Cube_Enemy;
		Wait_Times [0] [2] = .2f;
		Waves [0][3] = Cube_Enemy;
		
		
		//Wave1
		Waves [1] = new GameObject[5];
		Wait_Times[1] = new float[4];
		Waves [1][0] = Cube_Enemy;
		Wait_Times [1] [0] = .2f;
		Waves [1][1] = Cube_Enemy;
		Wait_Times [1] [1] = .2f;
		Waves [1][2] = Cube_Enemy;
		Wait_Times [1] [2] = .2f;
		Waves [1][3] = Cube_Enemy;
		Wait_Times [1] [3] = .2f;
		Waves [1][4] = Cube_Enemy;
		
		//Wave2
		Waves [2] = new GameObject[3];
		Wait_Times[2] = new float[2];
		Waves [2][0] = Cube_Enemy;
		Wait_Times [2] [0] = 1.2f;
		Waves [2][1] = Cube_Enemy;
		Wait_Times [2] [1] = 1.2f;
		Waves [2][2] = Cube_Enemy;
		
		//Wave3
		Waves [3] = new GameObject[3];
		Wait_Times[3] = new float[2];
		Waves [3][0] = Cube_Enemy;
		Wait_Times [3] [0] = 1.2f;
		Waves [3][1] = Cube_Enemy;
		Wait_Times [3] [1] = 1.2f;
		Waves [3][2] = Cube_Enemy;
		
		//Wave4
		Waves [4] = new GameObject[3];
		Wait_Times[4] = new float[2];
		Waves [4][0] = Cube_Enemy;
		Wait_Times [4] [0] = 1.2f;
		Waves [4][1] = Cube_Enemy;
		Wait_Times [4] [1] = 1.2f;
		Waves [4][2] = Cube_Enemy;
		
		
		//GameEngine for getting Components
		GameEngine = GameObject.Find(ConstantsLib.GAME_ENGINE_NAME);
		GameEngine.GetComponent<InGameMenuManager> ().maxWave= maxWaves;
	}
	// Update is called once per frame
	void Update () {
		
		
		bool okToShowWaveBtn = GameEngine.GetComponent<StartWaveManager> ().showNextWaveBtn;
		if(!okToShowWaveBtn)
		{
			if(!waveDone)
			{
				//Spawns An Enemy if its Ready and the enemyIndex is not out of range
				if( (currentWave <= maxWaves) && readyForNextEnemy && (enemyIndex < Waves[currentWave-1].Length ) )
				{
					GameEngine.GetComponent<InGameMenuManager> ().currentWave = currentWave;
					rand_loc =Random.Range(0,spawnPoints.Length);
					GameObject enemy = (GameObject)Instantiate (Waves [currentWave-1][enemyIndex], spawnPoints[rand_loc].position, Quaternion.identity);
					enemy.animation.Play ("CubeWalkingAnimation");
					NavMeshAgent n = enemy.GetComponent<NavMeshAgent>();
					n.destination = destination.position;
					
					//Set up for next enemy to spawn
					readyForNextEnemy = false;
					if(enemyIndex < Wait_Times[currentWave-1].Length )
					{
						nextEnemySpawnTime = Time.time +Wait_Times[currentWave-1][enemyIndex];
					}
					enemyIndex++;
					if(enemyIndex == Waves[currentWave-1].Length )
					{
						//Ready For Next Wave Timer to Start
						waveDone=true;
						currentWave++;
						enemyIndex = 0;
						ShowBtnTimer = Time.time + ShowBtnWait;
					}
				}
				else 
				{
					if(Time.time >= nextEnemySpawnTime)
					{
						readyForNextEnemy = true;
					}
				}
				
			} 
			if(currentWave > maxWaves)
			{
				GameObject[] gos;
				gos = GameObject.FindGameObjectsWithTag("Enemy");
				if(gos.Length == 0)
				{
					AutoFade.LoadLevel( ConstantsLib.MAIN_MENU, ConstantsLib.FADE_OUT_DUR,
					                   ConstantsLib.FADE_OUT_DUR, Color.green );
					
				}
				
			}
			if(waveDone)
			{
				
				if(Time.time >= ShowBtnTimer)
				{
					InterWaveTimer = Time.time + InterWaveWait;
					if(currentWave <= maxWaves)
					{
						GameEngine.GetComponentInChildren<StartWaveManager>().showNextWaveBtn =true;
					}
					waveDone = false;
				}
				
			}
		}//End If showNextWaveBtn is false
		else 
		{
			if(currentWave != 1)
			{
				//Start the next Wave until timerOver and NextWaveBtn Not pressed 
				
				if((Time.time >= InterWaveTimer) && GameEngine.GetComponentInChildren<StartWaveManager>().showNextWaveBtn)
				{
					GameEngine.GetComponentInChildren<StartWaveManager>().showNextWaveBtn = false;
					
				}
			}
		}
	}//End Update
}