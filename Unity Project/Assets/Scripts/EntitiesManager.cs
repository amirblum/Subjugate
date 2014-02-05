using UnityEngine;
using System.Collections;

public class EntitiesManager : MonoBehaviour {

	public float scoreUpdateInterval;
	[HideInInspector]
	public ArrayList activeEntities;
	public ArrayList controlledEntities;

	private float lastScoreUpdate;
	private int score;
	private GUIText scoreText;

	// Use this for initialization
	void Awake () {
		activeEntities = new ArrayList();
		controlledEntities = new ArrayList ();
	}

	void Start() {
		this.score = 0;
		this.scoreText = GameObject.Find ("Score").GetComponent<GUIText> ();
		this.UpdateScore (0);
	}

	public void UpdateScore(int scoreChange)
	{
		this.score += scoreChange;
		this.scoreText.text = "Score: " + this.score.ToString();
		this.lastScoreUpdate = Time.time;
	}

	public void GameOver()
	{
		Application.LoadLevel (2);
	}
	
	// Update is called once per frame
	void Update () {
	}

	//adds a new enemy to the list of active entities to check collisions
	public void AddEntity(GameObject newEnetity){
		activeEntities.Add (newEnetity);
	}

	//reomoves an entity from the collidable entities list
	public void RemoveEntity(GameObject entity){
		activeEntities.Remove (entity);
	}

	//adds a new enemy to the list of controlled entities
	public void AddControlledEntity(GameObject newEnetity){
		controlledEntities.Add (newEnetity);
	}
	
	//reomoves an entity from the controlled entities list
	public void RemoveControlledEntity(GameObject entity){
		controlledEntities.Remove (entity);
	}
}
