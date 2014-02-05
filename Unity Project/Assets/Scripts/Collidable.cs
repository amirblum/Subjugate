using UnityEngine;
using System.Collections;

//parent class for objects that can collide with other objects
abstract public class Collidable : MonoBehaviour	{
	public enum EntityType {PLAYER,ENEMY_1,ENEMY_2,ENEMY_3,HOUSE};
	public EntityType entityType;
	[HideInInspector]
	public bool isControlled;

	public Vector3 ScaleToCollision;
	private bool EnableCollision;

	void start(){
		EnableCollision = false;
	}

	public bool GetCollidability (){
		return EnableCollision;
	}

	public bool isEnemy()
	{
		return this.entityType == EntityType.ENEMY_1 ||
			this.entityType == EntityType.ENEMY_2 || this.entityType == EntityType.ENEMY_3 ;
	}



	//set collidability option - true (enabled) or false (disabled)
	public void SetCollidability(bool collidability){
		if (EnableCollision == collidability) {
			return;
		}
		//if the programme has reached here - EnableCollision is changing
		EnableCollision = collidability;
		EntitiesManager detect = GameObject.Find ("World").GetComponent <EntitiesManager> ();
		if (collidability) {
			detect.AddEntity (this.gameObject);
		} else {
			//if a functionality of eliminating collidability is required - uncomment: 
			detect.RemoveEntity(this.gameObject);
		}
	}

	//write a specific functionality upon collision - if required
	abstract public void Collide(ArrayList collideWith);
}