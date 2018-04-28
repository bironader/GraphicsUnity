using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {
	NavMeshAgent nav; 
	Transform player;
	Animator controller;
	GameManagement game;
	Animator anim;     
	float health;
	bool isDead = false;
	int damage = 20;
	int pointValue = 10;
	bool behindWall = true;

	public AudioClip[] attackClips;
	public AudioClip[] deathClips;

	public float timeToAttack;
	float attackTimer;
	public AudioClip zombieAttackClip;

	CapsuleCollider capsuleCollider;

	// Use this for initialization
	void Awake () {
		nav = GetComponent <NavMeshAgent> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		controller = GetComponentInParent<Animator> ();
		game = FindObjectOfType<GameManagement> ();
		health = 20 + (10 * game.round);
		capsuleCollider = GetComponent <CapsuleCollider> ();
		anim = GetComponent <Animator> ();



	}
	
	// Update is called once per frame
	void Update () {
		if(!isDead)
		{
			nav.SetDestination (player.position);
			controller.SetFloat ("speed", Mathf.Abs(nav.velocity.x) + Mathf.Abs (nav.velocity.z));
			float distance = Vector3.Distance(transform.position, player.position);
			if(distance < 3  || behindWall)
			{
				attackTimer += Time.deltaTime;
			}
			else if (attackTimer > 0)
			{
				anim.SetBool ("Attack", false);
				attackTimer -= Time.deltaTime*2;
			}
			else
			{
				attackTimer = 0;
				anim.SetBool ("Attack", false);
			}

		}
	}

	bool Attack()
	{
		if(!anim.GetBool ("Attack"))
			anim.SetFloat ("AttackType",Random.Range (0,2));

		anim.SetBool ("Attack", true);

		if(!GetComponent<AudioSource>().isPlaying)
		{
			//audio.PlayOneShot(attackClips[Random.Range (0,attackClips.Length-1)]);
			GetComponent<AudioSource>().clip = attackClips[Random.Range (0,attackClips.Length-1)];
			GetComponent<AudioSource>().Play ();
		}
		if(attackTimer > timeToAttack)
		{
			attackTimer = 0;
			return true;
		}
		return false;
	}


	void Death ()
	{
		// The enemy is dead.
		isDead = true;
		nav.Stop ();
		// Turn the collider into a trigger so shots can pass through it.
		capsuleCollider.isTrigger = true;
		capsuleCollider.enabled = false;
		

		anim.SetTrigger ("Dead");

		GetComponent<AudioSource>().clip = deathClips [Random.Range (0, deathClips.Length-1)];
		GetComponent<AudioSource>().Play ();

		GameManagement.zombiesLeftInRound -= 1;
		Destroy (gameObject, 4f);
	}


	void ApplyDamage(float damage)
	{

		health -= damage;

		if (!isDead)
		{
			GameManagement.AddPoints(pointValue);
			if (health <= 0)
			{
				Death ();
			
			}
		}


	}
	void OnCollisionStay(Collision collisionInfo)
	{
		if(collisionInfo.gameObject.tag == "Player")
		{
			if(Attack ())
				collisionInfo.collider.SendMessageUpwards("PlayerDamage", damage, SendMessageOptions.RequireReceiver);
		}


	}

	void OnTriggerStay(Collider collide)
	{
		//print ("collision");
		if(collide.gameObject.tag == "SpawnWall")
		{
			if(collide.gameObject.GetComponent<Renderer>().enabled)
			{
				behindWall = true;
				nav.Stop ();
				if(Attack ())
				{
					collide.GetComponent<Collider>().SendMessageUpwards("RemoveBoard", SendMessageOptions.RequireReceiver);
				}
				
			}
			else if(behindWall)
			{
				nav.Resume();
				behindWall = false;
				anim.SetTrigger("JumpThrough");
			}
		}
	}

}










