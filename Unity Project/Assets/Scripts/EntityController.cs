using UnityEngine;
using System.Collections;

public class EntityController : Collidable, AnimationEndHandler {
	public Sprite[] ANIMATION_WALK;
	public float ANIMATION_WALK_TIME;
	public Sprite[] ANIMATION_STAND;
	public float ANIMATION_STAND_TIME;
	public Sprite[] ANIMATION_JUMP;
	public float ANIMATION_JUMP_TIME;
	public Sprite[] ANIMATION_DIE;
	public float ANIMATION_DIE_TIME;
	public Sprite[] ANIMATION_APPEAR;
	public float ANIMATION_APPEAR_TIME;
	public Sprite[] ANIMATION_TAKEOVER;
	public float ANIMATION_TAKEOVER_TIME;
	
	public int blinksNum;
	public int framesForBlink;

	[HideInInspector]
	public bool isJumping;

	private WalkAnimator animator;
	private MovementController moveController;

	private MovementController enemyController;
	private SoundPlayer soundPlayer;
	private EntitiesManager entitiesManager;

	private bool blinking;
	private int blinkFramesCounter;
	private bool isDead;

	// Use this for initialization
	void Start () {
		GameObject world = GameObject.Find ("World");
		this.entitiesManager = world.GetComponent<EntitiesManager> ();
			this.soundPlayer = world.GetComponent<SoundPlayer> ();
		this.animator = this.GetComponent<WalkAnimator> ();
		this.moveController = this.GetComponent<MovementController> ();

		this.SetCollidability(true);

		if (this.isEnemy())
		{
			this.animator.SwitchAnimation(this.ANIMATION_APPEAR, this.ANIMATION_APPEAR_TIME);
			this.animator.setEndHandler(this);
			this.moveController.SetMoving(false);
		}

		this.isJumping = false;
		this.isDead = false;

		this.blinking = false;
		this.blinkFramesCounter = 0;
	}

	override public void Collide(ArrayList collideWith)
	{
		bool isDead = false;
		bool isHouseAttack = false;
		foreach (Object collidingObject in collideWith)
		{
			Collidable other = ((GameObject)collidingObject).GetComponent<Collidable>();
			if ((this.isControlled && other.isEnemy()) || other.entityType == this.entityType ||
			    (!this.isControlled && this.isEnemy() && other.entityType == EntityType.HOUSE))
			{
				if (this.isEnemy() && other.entityType == EntityType.HOUSE){

					isHouseAttack = true;
				}
				isDead = true;
				break;
			}
		}
		if (isDead)
		{
			this.SetCollidability (false);
			if (this.isControlled)
			{
				entitiesManager.RemoveControlledEntity(this.gameObject);
			}
			else if (this.isEnemy() && !isHouseAttack)
			{
				entitiesManager.UpdateScore(100);
			}
			this.animator.SwitchAnimation (this.ANIMATION_DIE, this.ANIMATION_DIE_TIME);
			this.animator.setEndHandler(this);
			this.GetComponent<SpriteRenderer>().color = Color.red;
			this.moveController.SetMoving(false);
			this.isDead = true;
			DestroyObject(this.gameObject, this.ANIMATION_DIE_TIME);
			if (!isHouseAttack)	soundPlayer.playDeathSound();
		}
	}

	public void SetWalking(bool isWalking)
	{
		if (this.isJumping || this.isDead)
		{
			return;
		}
		if (isWalking)
		{
			this.animator.SwitchAnimation(this.ANIMATION_WALK, this.ANIMATION_WALK_TIME);
		}
		else
		{
			this.animator.SwitchAnimation(this.ANIMATION_STAND, this.ANIMATION_STAND_TIME);
		}
	}
	
	public void SetJumping(bool isJumping)
	{
		if (this.isJumping == isJumping || this.isDead)
		{
			return;
		}
		this.isJumping = isJumping;
		if (isJumping)
		{
			// Begin jump
			this.animator.SwitchAnimation(this.ANIMATION_JUMP, this.ANIMATION_JUMP_TIME);
		}
		else
		{
			// End jump
			this.SetWalking(this.moveController.moveDirection != MovementController.MoveDirection.NONE);
		}
	}

	public void BeginTakeOver(MovementController enemyController)
	{
		if (this.isDead)
						return;
		if (this.entityType == EntityType.PLAYER)
		{
			this.enemyController = enemyController;
			this.enemyController.SetMoving(false);
			this.animator.SwitchAnimation(this.ANIMATION_TAKEOVER, this.ANIMATION_TAKEOVER_TIME);
			this.animator.setEndHandler(this);
			this.moveController.SetMoving(false);
			this.entitiesManager.UpdateScore(50);
		} else if (this.isEnemy()) {
			this.moveController = enemyController;
			this.SetWalking(false);
			this.blinking = true;
		}
	}

	public void HandleAnimationEnd()
	{
		if (this.isDead)
		{
			return;
		}
		this.animator.setEndHandler (null);
		this.SetWalking(this.moveController.moveDirection != MovementController.MoveDirection.NONE);
		this.moveController.SetMoving(true);
		if (this.enemyController != null)
		{
			this.enemyController.SetMoving(true);
			this.enemyController = null;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (this.isDead)
		{
			return;
		}

		if (this.blinking) {
			if (blinkFramesCounter % (framesForBlink * 3) == framesForBlink) {
				this.GetComponent<SpriteRenderer>().color = new Color(0.8f, 0.4f, 0.8f, 1f);
			} else  if (blinkFramesCounter % (framesForBlink * 3) == 2*framesForBlink){
				this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			}
			
			++blinkFramesCounter;
			
			if (blinkFramesCounter >= framesForBlink*blinksNum*3){
				this.blinking = false;
				this.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
				blinkFramesCounter = 0;
			}
		}
	}
}
