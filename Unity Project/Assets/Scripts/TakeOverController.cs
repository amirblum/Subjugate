using UnityEngine;
using System.Collections;

public class TakeOverController : MonoBehaviour {

	public float angleDistance;

	private EntitiesManager entitiesManager;
	private SoundPlayer soundPlayer;
	private MovementController myMovement;
	private EntityController entityController;
	
	private float twopi;

	// Use this for initialization
	void Start () {
		twopi = 2 * Mathf.PI;
		GameObject world =  GameObject.Find ("World");
		entitiesManager = world.GetComponent<EntitiesManager> ();
		soundPlayer = world.GetComponent<SoundPlayer> ();
		myMovement = GetComponent<MovementController> ();
		entityController = GetComponent<EntityController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButton ("Fire1") && ! this.entityController.isJumping) {
			ArrayList entities = entitiesManager.activeEntities;
			ArrayList controlledEntities = entitiesManager.controlledEntities;
//			if (controlledEntities.Count >= 2) return;

			ArrayList enemiesInRange = new ArrayList();
			MovementController currentEnemyMovementController;
			for(int i=0; i < entities.Count ; ++i){
				GameObject currentEnemy = (GameObject)entities[i];
				if (! currentEnemy.GetComponent<Collidable>().isEnemy()) {
					continue;
				}
				currentEnemyMovementController = currentEnemy.GetComponent<MovementController>();
				
				float meRealAngle = realMod(myMovement.currentAngle, twopi);
				float endOfRangeAngle;
				float enemyRealAngle = realMod(currentEnemyMovementController.currentAngle,twopi);
				if (myMovement.facingRight) {
					endOfRangeAngle = realMod(myMovement.currentAngle + angleDistance,twopi);
					if (Mathf.Abs (meRealAngle - endOfRangeAngle) > Mathf.PI) {
						meRealAngle = realMod(meRealAngle + Mathf.PI, twopi);
						endOfRangeAngle = realMod(endOfRangeAngle + Mathf.PI, twopi);
						enemyRealAngle = realMod(enemyRealAngle + Mathf.PI, twopi);
					}
					if (meRealAngle < enemyRealAngle && enemyRealAngle < endOfRangeAngle) {
						enemiesInRange.Add(currentEnemy);
					}
				} else {
					endOfRangeAngle = realMod(myMovement.currentAngle - angleDistance,twopi);
					if (Mathf.Abs(meRealAngle - endOfRangeAngle) > Mathf.PI) {
						meRealAngle = realMod(meRealAngle + Mathf.PI, twopi);
						endOfRangeAngle = realMod(endOfRangeAngle + Mathf.PI, twopi);
						enemyRealAngle = realMod(enemyRealAngle + Mathf.PI, twopi);
					} 
					if (meRealAngle > enemyRealAngle && enemyRealAngle > endOfRangeAngle) {
						enemiesInRange.Add(currentEnemy);
					}
				}


//				if (
//					(myMovement.facingRight && (realMod(myMovement.currentAngle + angleDistance, twopi) > realMod(currentEnemyMovementController.currentAngle, twopi)))
//				    || 
//				    (!myMovement.facingRight && (realMod(myMovement.currentAngle - angleDistance, twopi) < realMod(currentEnemyMovementController.currentAngle,twopi)))) {
//					enemiesInRange.Add(currentEnemy);
//				}
			}

			if (enemiesInRange.Count > 0){
				GameObject closestEnemy = enemiesInRange[0] as GameObject;
				MovementController closestEntityMovementController = closestEnemy.GetComponent<MovementController>();
				foreach (var enemy in enemiesInRange) {
					GameObject enemyObj = enemy as GameObject;
					MovementController enemyController = enemyObj.GetComponent<MovementController>();

					if (computeDifference(myMovement.currentAngle,enemyController.currentAngle, myMovement.facingRight) < 
					    computeDifference(myMovement.currentAngle, closestEntityMovementController.currentAngle, myMovement.facingRight)) {
						closestEnemy = enemyObj;
						closestEntityMovementController = enemyController;
					}
				}

				EntityController enemyEntityController = closestEnemy.GetComponent<EntityController>();

				bool enemyTypeAlreadyControlled = false;
				foreach (object obj in controlledEntities) {
					EntityController controller = ((GameObject)obj).GetComponent<EntityController>();
					if (controller.entityType == enemyEntityController.entityType) {
						enemyTypeAlreadyControlled = true;
					}
				}
				if (!enemyEntityController.isControlled && !enemyTypeAlreadyControlled) {
					PlayerMovementController newPlayerController = closestEnemy.AddComponent(typeof(PlayerMovementController)) as PlayerMovementController;
					newPlayerController.radius = closestEntityMovementController.radius;
					newPlayerController.currentAngle = closestEntityMovementController.currentAngle;
					newPlayerController.movementSpeed = closestEntityMovementController.movementSpeed * 1.5f;
					newPlayerController.gravity = (myMovement as PlayerMovementController).gravity;
					newPlayerController.moveDirection = closestEntityMovementController.moveDirection;
					newPlayerController.facingRight = closestEntityMovementController.facingRight;
					newPlayerController.vVelocity = (myMovement as PlayerMovementController).vVelocity;

					DestroyObject(closestEntityMovementController);
					entityController.BeginTakeOver(newPlayerController);
					soundPlayer.playControlSound();
					enemyEntityController.BeginTakeOver(newPlayerController);
					enemyEntityController.isControlled = true;
					entitiesManager.AddControlledEntity(closestEnemy);
				}
			}
		}
	}

	private float computeDifference(float playerPosition, float enemyPosition, bool facingRight) {
		playerPosition = realMod(playerPosition, twopi);
		enemyPosition = realMod(enemyPosition, twopi);
		float diffAngle = Mathf.Abs(playerPosition - enemyPosition) ;
		if (diffAngle < Mathf.PI)  {
			return diffAngle;
		}
		else {
			if (facingRight) {
				enemyPosition += twopi;
				return enemyPosition - playerPosition;
			}
			else {
				enemyPosition -= twopi;
				return playerPosition - enemyPosition;
			}
		}
	}

	private float realMod(float f, float mod) {
		return ((f % mod) + mod) % mod;
	}
}
