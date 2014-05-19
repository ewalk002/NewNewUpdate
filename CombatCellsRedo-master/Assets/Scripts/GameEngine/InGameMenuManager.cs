using UnityEngine;
using System.Collections;

public class InGameMenuManager : MonoBehaviour {
	
	public int ScoreCount;
	public int AtomCount;
	public int currentWave;
	public int maxWave;
	public int currentPlayerHealth;
	
	private UILabel ScoreCountLabel;
	private UILabel AtomCountLabel;
	private UILabel WaveCountLabel;
	private UILabel HealthCountLabel;
	private GameObject UIroot;
	
	// Use this for initialization
	void Start () {
		
		
		UIroot= GameObject.Find(ConstantsLib.SCORE_NUM_NAME);
		ScoreCountLabel = UIroot.GetComponent<UILabel>();
		ScoreCountLabel.text = ScoreCount.ToString();
		
		UIroot= GameObject.Find(ConstantsLib.ATOM_NUM_NAME );
		AtomCountLabel = UIroot.GetComponent<UILabel>();
		AtomCountLabel.text = AtomCount.ToString ();
		
		UIroot= GameObject.Find(ConstantsLib.WAVE_NUM_NAME );
		WaveCountLabel = UIroot.GetComponent<UILabel>();
		WaveCountLabel.text = currentWave.ToString()+" / "+maxWave.ToString () ;
		
		UIroot= GameObject.Find(ConstantsLib.HEALTH_NUM_NAME);
		HealthCountLabel = UIroot.GetComponent<UILabel>();
		HealthCountLabel.text = currentPlayerHealth.ToString ();
		
	}
	
	// Update is called once per frame
	void Update () {
		ScoreCountLabel.text = ScoreCount.ToString();
		AtomCountLabel.text = AtomCount.ToString();
		WaveCountLabel.text = currentWave.ToString()+" / "+maxWave.ToString () ;
		HealthCountLabel.text = currentPlayerHealth.ToString ();
	}
}
