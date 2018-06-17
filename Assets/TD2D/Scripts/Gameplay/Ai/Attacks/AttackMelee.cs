using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack with melee weapon
/// </summary>
public class AttackMelee : Attack
{
	private Animator anim;
	private float cooldownCounter;

	void Awake()
	{
		anim = GetComponentInParent<Animator>();
		cooldownCounter = cooldown;
	}

    void FixedUpdate()
	{
		if (cooldownCounter < cooldown)
		{
			cooldownCounter += Time.fixedDeltaTime;
		}
	}

	public override void TryAttack(Transform target)
	{
		if (cooldownCounter >= cooldown)
		{
			cooldownCounter = 0f;
			Fire(target);
		}
	}

	private IEnumerator FireCoroutine(Transform target)
	{
		if (target != null)
		{
			if (anim != null && anim.runtimeAnimatorController != null)
			{
				foreach (AnimationClip clip in anim.runtimeAnimatorController.animationClips)
				{
					if (clip.name == "Attack")
					{
						anim.SetTrigger("attack");
						break;
					}
				}
			}

			yield return new WaitForSeconds(fireDelay);
            
			if (target != null)
			{
				DamageTaker damageTaker = target.GetComponent<DamageTaker>();
				if (damageTaker != null)
				{
					damageTaker.TakeDamage(damage);
				}
				
				if (sfx != null && AudioManager.instance != null)
				{
					AudioManager.instance.PlayAttack(sfx);
				}
			}
		}
	}

	public override void Fire(Transform target)
	{
		StartCoroutine(FireCoroutine(target));
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
