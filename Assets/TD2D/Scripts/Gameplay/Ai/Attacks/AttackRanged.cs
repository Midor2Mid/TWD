using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attack with ranged weapon
/// </summary>
public class AttackRanged : Attack
{
	public GameObject arrowPrefab;
	public Transform firePoint;

	private Animator anim;
	private float cooldownCounter;

	void Awake()
	{
		anim = GetComponentInParent<Animator>();
		cooldownCounter = cooldown;
		Debug.Assert(arrowPrefab && firePoint, "Wrong initial parameters");
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

	private IEnumerator FireCoroutine(Transform target, GameObject bulletPrefab)
	{
		if (target != null && bulletPrefab != null)
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

            // Chờ hết thời gian chờ thì đánh tiếp
			yield return new WaitForSeconds(fireDelay);
			if (target != null)
			{
				GameObject arrow = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
				IBullet bullet = arrow.GetComponent<IBullet>();
				bullet.SetDamage(damage);
				bullet.Fire(target);

				if (sfx != null && AudioManager.instance != null)
				{
					AudioManager.instance.PlayAttack(sfx);
				}
			}
		}
	}


	public override void Fire(Transform target)
	{
		StartCoroutine(FireCoroutine(target, arrowPrefab));
	}

	public void Fire(Transform target, GameObject bulletPrefab)
	{
		StartCoroutine(FireCoroutine(target, bulletPrefab));
	}

	void OnDestroy()
	{
		StopAllCoroutines();
	}
}
