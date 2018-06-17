using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic class for attacks types.
/// </summary>
public class Attack : MonoBehaviour
{
	public int damage = 1;
	public float cooldown = 1f;
	public float fireDelay = 0f;

	public AudioClip sfx;

	public virtual void TryAttack(Transform target)
	{

	}

	public virtual void Fire(Transform target)
	{

	}
}
