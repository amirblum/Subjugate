using UnityEngine;
using System.Collections;

public interface AnimationEndHandler
{
	void HandleAnimationEnd ();
}

public class WalkAnimator : MonoBehaviour {	
	public Sprite[] currentAnimation;
	public float currentAnimationTime;

	private SpriteRenderer spriteRenderer;
	private float timeToNextStep;
	private float stepTime;
	private AnimationEndHandler endHandler;
	private int currentSpriteIndex;

	// Use this for initialization
	void Start () {
		this.spriteRenderer = this.renderer as SpriteRenderer;
		this.SwitchAnimation (this.currentAnimation, this.currentAnimationTime);
	}

	public void setEndHandler(AnimationEndHandler handler)
	{
		this.endHandler = handler;
	}

	// Switch animation
	public void SwitchAnimation(Sprite[] newAnimation, float newTime)
	{
		this.currentAnimation = newAnimation;
		this.stepTime = newTime / newAnimation.Length;
		this.currentSpriteIndex = 0;
		this.timeToNextStep = this.stepTime;
	}
	
	// Update is called once per frame
	void Update () {
		if (this.currentAnimation == null || this.currentAnimation.Length == 0)
		{
			return;
		}

		this.timeToNextStep -= Time.deltaTime;
		if (this.timeToNextStep < 0)
		{
			this.currentSpriteIndex = (this.currentSpriteIndex + 1) % this.currentAnimation.Length;
			if (this.endHandler != null && this.currentSpriteIndex == 0)
			{
				--this.currentSpriteIndex;
				this.endHandler.HandleAnimationEnd();
				return;
			}
			this.spriteRenderer.sprite = this.currentAnimation[this.currentSpriteIndex];
			this.timeToNextStep += this.stepTime;
		}
	}
}
