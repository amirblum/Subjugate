using UnityEngine;
using System.Collections;

public class House : Collidable {

	public Sprite[] HOUSE_SPRITES;

	private int damage;
	private SpriteRenderer spriteRenderer;
	private EntitiesManager game;

	// Use this for initialization
	void Start () {
		this.damage = 0;
		this.SetCollidability (true);
		this.spriteRenderer = this.renderer as SpriteRenderer;
		this.game = GameObject.Find("World").GetComponent<EntitiesManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	override public void Collide(ArrayList collideWith) {
		foreach (Object collidingObject in collideWith)
		{
			var other = ((GameObject)collidingObject).GetComponent<Collidable>();
			if (other.isEnemy() && !other.isControlled && this.HOUSE_SPRITES.Length > 0)
			{
				if (++this.damage >= this.HOUSE_SPRITES.Length)
				{
					this.game.GameOver();
					return;
				}
				this.spriteRenderer.sprite = this.HOUSE_SPRITES[this.damage];
				this.GetComponent<AudioSource>().Play();
			}
		}
	}
}
