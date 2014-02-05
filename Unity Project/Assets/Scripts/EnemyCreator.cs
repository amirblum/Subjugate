using UnityEngine;
using System.Collections;

public class EnemyCreator : MonoBehaviour {
	public int nextEnemySpawnTime;
	public int enemySpawnInterval;
	public GameObject[] enemies;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (enemies.Length == 0)
		{
			return;
		}
		if (--this.nextEnemySpawnTime <= 0)
		{
			// Choose random enemy
			int index = (int)Random.Range(0, enemies.Length);
			MovementController.MoveDirection direction = MovementController.MoveDirection.LEFT;

			GameObject newEnemy = (GameObject)Instantiate(enemies[index]);
			if (Random.value > 0.5)
			{
				newEnemy.GetComponent<MovementController>().Flip();
				direction = MovementController.MoveDirection.RIGHT;
			}
			MovementController controller = newEnemy.GetComponent<MovementController>();
			controller.currentAngle = 3.14f;
			controller.moveDirection = direction;
			newEnemy.transform.position = new Vector3(0f,-2.55f,0f);
			newEnemy.transform.rotation = new Quaternion(0,0,180,0);
			newEnemy.SetActive(true);
			this.nextEnemySpawnTime = enemySpawnInterval;
		}
	}
}
