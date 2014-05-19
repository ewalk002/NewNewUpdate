using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class tower_wbc : MonoBehaviour {

	public GameObject projectilePrefab = null;
	public GameObject muzzleEffect = null;
	public float reloadTime = 0;
	public float turnSpeed = 0;
	public int cost = 0;

	public Transform target = null;
	public List<Transform> multiTarget = null;

	public Transform muzzlePosition = null;
	public List<Transform> multiMuzzlePosition = null;

	public Transform tower = null;

	private GameObject projectile = null;
	private float nextFireTime;
	private float nextMoveTime;
	private Quaternion desiredRotation;
	private List<GameObject> enemyList;

	private float lockOnTime;
	// Use this for initialization
	void Start () {
		enemyList = new List<GameObject> ();
		lockOnTime = 0;

		if( gameObject.tag == ConstantsLib.WBC_TAG )
		{
			//projectilePrefab = null;
			reloadTime = ConstantsLib.WBC_RELOAD_TIME;
			turnSpeed = ConstantsLib.WBC_TURN_SPEED;
			cost = ConstantsLib.WBC_COST;
			gameObject.GetComponent<SphereCollider>().radius = 
				ConstantsLib.WBC_PROJ_RANGE / gameObject.transform.localScale.magnitude;
		}
		else if( gameObject.tag == ConstantsLib.MUCUS_TAG )
		{
			//projectilePrefab = null;
			reloadTime = ConstantsLib.MUCUS_RELOAD_TIME;
			turnSpeed = ConstantsLib.MUCUS_TURN_SPEED;
			cost = ConstantsLib.MUCUS_COST;
			gameObject.GetComponent<SphereCollider>().radius = 
				ConstantsLib.MUCUS_PROJ_RANGE / gameObject.transform.localScale.magnitude;
		}
		else if( gameObject.tag == ConstantsLib.MORTAR_TAG )
		{
			//projectilePrefab = null;
			reloadTime = ConstantsLib.MORTAR_RELOAD_TIME;
			turnSpeed = ConstantsLib.MORTAR_TURN_SPEED;
			cost = ConstantsLib.MORTAR_COST;	
			gameObject.GetComponent<SphereCollider>().radius = 
				ConstantsLib.MORTAR_PROJ_RANGE / gameObject.transform.localScale.magnitude;
		}
		else if( gameObject.tag == ConstantsLib.SYRINGE_TAG )
		{
			//projectilePrefab = null;
			reloadTime = ConstantsLib.SYRINGE_RELOAD_TIME;
			turnSpeed = ConstantsLib.SYRINGE_TURN_SPEED;
			cost = ConstantsLib.SYRINGE_COST;			
			gameObject.GetComponent<SphereCollider>().radius = 
				ConstantsLib.SYRINGE_PROJ_RANGE / gameObject.transform.localScale.magnitude;
			disableParticleSystem();
		}
		else if( gameObject.tag == ConstantsLib.TESLA_TAG )
		{
			//projectilePrefab = null;
			reloadTime = ConstantsLib.TESLA_RELOAD_TIME;
			turnSpeed = ConstantsLib.TESLA_TURN_SPEED;
			cost = ConstantsLib.TESLA_COST;		
			gameObject.GetComponent<SphereCollider>().radius = 
				ConstantsLib.TESLA_PROJ_RANGE / gameObject.transform.localScale.magnitude;

		}

	}
	
	// Update is called once per frame
	void Update () {

		if( enemyList.Count > 0 )
		{
			if( target == null )
			{
				lockOnTime = 0;
				int minIndex = enemyList.Count;

				for( int i = 0 ; i < enemyList.Count ; ++i )
				{
					if( enemyList[i] == null ) // remove all destroyed enemies
					{
						enemyList.RemoveAt( i );
						--i;
					}
					else // else look for most recent collision
					{
						if( i < minIndex )
						{
							minIndex = i;
						}
					}
				}

				if( enemyList.Count > 0 ) // choose enemy
				{
					if( gameObject.tag != ConstantsLib.TESLA_TAG )
					{
						if(  minIndex < enemyList.Count && enemyList[minIndex] != null )
						{
							target = enemyList[minIndex].transform;
							enableParticleSystem();
						}
						else
						{
							disableParticleSystem();
							target = null;
						}
					}
					else
					{
						for( int i = 0 ; i < multiTarget.Count && i < enemyList.Count; ++i )
						{
							//Debug.Log( multiTarget.Count );
							if( multiTarget[i] == null )
							{
								if( i == 0 )
								{
									multiTarget[i] = enemyList[minIndex].transform;
								}
								else
								{
									int random = Random.Range( 0, enemyList.Count );
									Debug.Log( random );
									while( multiTarget[i] == enemyList[random].transform )
									{
										random = Random.Range( 0, enemyList.Count );
									}
									multiTarget[i] = enemyList[random].transform;
								}
							}
						}
					}
				}
			}
			else 
			{
				if( gameObject.tag == ConstantsLib.MUCUS_TAG ) // mucus check
				{
					int minIndex = enemyList.Count - 1;

					for( int i = 0 ; i < enemyList.Count ; ++i )
					{
						if( enemyList[i] == null ) // remove all destroyed enemies
						{
							enemyList.RemoveAt( i );
							--i;
						}
						else // else look for most recent collision
						{
							if( i < minIndex )
							{
								if( gameObject.tag == ConstantsLib.MUCUS_TAG ) // for mucus only
								{
									if( enemyList[i].GetComponent<enemy>().slow == false )
									{
										minIndex = i;
									}
								}
							}
						}

						if( enemyList.Count > 0 )
						{
							if( minIndex < enemyList.Count &&  enemyList[minIndex] != null )
							{
								target = enemyList[minIndex].transform;
							}
							else
							{
								target = null;
							}
						}
					}
				}
			}
		}
		else
		{
			target = null;
			lockOnTime = 0;
		}


		if( target != null )
		{
			lockOnTime += Time.deltaTime;
			CalculateAimPosition( target.position );
			tower.rotation = Quaternion.Lerp( tower.rotation, 
			                                      desiredRotation, 
			                                      Time.deltaTime * turnSpeed );

			if( gameObject.tag != ConstantsLib.SYRINGE_TAG )
			{
				if( Time.time >= nextFireTime )
				{
					FireProjectile();
				}
			}
			else // continuous shooting
			{
				FireProjectile();
			}
		}


		if( gameObject.tag == ConstantsLib.TESLA_TAG )
		{
			if( multiTarget[0] != null )
			{
				CalculateAimPosition( multiTarget[0].position );
				tower.rotation = Quaternion.Lerp( tower.rotation, 
				                                 desiredRotation, 
				                                 Time.deltaTime * turnSpeed );
				
				if( Time.time >= nextFireTime )
				{
					FireProjectile();
				}
			}
		}
		
		
	}

	void OnTriggerEnter( Collider other )
	{
		if (other.gameObject.tag == ConstantsLib.ENEMY_TAG )
		{
			enemyList.Add( other.gameObject );
		}
	}

	void OnTriggerExit( Collider other )
	{
		if( other.gameObject.tag == ConstantsLib.ENEMY_TAG )
		{
			if( gameObject.tag != ConstantsLib.TESLA_TAG )
			{
				if( target != null )
				{
					if( target == other.gameObject.transform )
					{
						target = null;
					}
				}
			}
			else
			{
				for( int i = 0 ; i < multiTarget.Count ; ++i )
				{
					if( multiTarget[i] != null )
					{
						if( multiTarget[i] == other.gameObject.transform )
						{
							multiTarget[i] = null;
						}
					}
				}
			}

			enemyList.Remove( other.gameObject );
		}
	}

	void disableParticleSystem()
	{
		if( gameObject.tag == ConstantsLib.SYRINGE_TAG )
		{
			projectilePrefab.particleEmitter.emit = false;
		}
	}

	void enableParticleSystem()
	{
		if( gameObject.tag == ConstantsLib.SYRINGE_TAG )
		{
			projectilePrefab.particleEmitter.emit = true;
		}
	}

	void CalculateAimPosition( Vector3 targetPos )
	{
		Vector3 aimPoint = new Vector3 (targetPos.x, targetPos.y, targetPos.z);
		desiredRotation = Quaternion.LookRotation (aimPoint - transform.position);
	}

	void FireProjectile()
	{
		audio.Play ();
		nextFireTime = Time.time + reloadTime;

		if( muzzleEffect != null )
		{
			GameObject muzzleE = ( GameObject) Instantiate( muzzleEffect,
			                                               muzzlePosition.position,
			                                               muzzlePosition.rotation );
		}

		if( gameObject.tag != ConstantsLib.TESLA_TAG && gameObject.tag != ConstantsLib.SYRINGE_TAG )
		{
			projectile = null;
			projectile = (GameObject)Instantiate ( projectilePrefab, 
			                                                 muzzlePosition.position,
			                                                 muzzlePosition.rotation);
			projectile.gameObject.GetComponent<projectile_wbc> ().target = target;
		}
		else if( gameObject.tag == ConstantsLib.TESLA_TAG )// we are a tesla tower
		{
			projectile = null;
			for( int i = 0 ; i < multiMuzzlePosition.Count ; ++i )
			{
				if( multiTarget[i] != null )
				{
					Vector3 midpoint = new Vector3( 
					    ( multiMuzzlePosition[i].transform.position.x + multiTarget[i].transform.position.x )/2,
					    ( multiMuzzlePosition[i].transform.position.y + multiTarget[i].transform.position.y )/2,
					    ( multiMuzzlePosition[i].transform.position.z + multiTarget[i].transform.position.z )/2 );

					projectile = (GameObject)Instantiate (projectilePrefab, 
					                                      midpoint,
					                                      Quaternion.Euler( 270, 90, 0 ) );

					Vector3 origMinBounds = Camera.main.WorldToScreenPoint( projectile.renderer.bounds.min );
					Vector3 origMaxBounds = Camera.main.WorldToScreenPoint( projectile.renderer.bounds.max );

					projectile.transform.rotation = Camera.main.transform.rotation;
					projectile.transform.Rotate( new Vector3( 0, 180, 0 ) );

					Vector3 enemyVec = Camera.main.WorldToScreenPoint( multiTarget[i].transform.position );
					Vector3 muzzleVec = Camera.main.WorldToScreenPoint( multiMuzzlePosition[i].transform.position );

					projectile.transform.Rotate( 
					    0, 0, Mathf.Atan2( ( enemyVec.x - muzzleVec.x ), ( enemyVec.y - muzzleVec.y ) ) * Mathf.Rad2Deg + 270 );

					float worldDistance = Mathf.Sqrt( ( origMinBounds.x - origMaxBounds.x )*( origMinBounds.x - origMaxBounds.x ) +
					                                 ( origMinBounds.y - origMaxBounds.y )*( origMinBounds.y - origMaxBounds.y ) );

					float screenDistance = Mathf.Sqrt( ( enemyVec.x - muzzleVec.x )*( enemyVec.x - muzzleVec.x ) +
					                                  ( enemyVec.y - muzzleVec.y )*( enemyVec.y - muzzleVec.y ) );

					float scaleFactor = screenDistance / worldDistance;

					projectile.transform.localScale *= scaleFactor;

					projectile.transform.Translate( 0, 0, Camera.main.transform.position.y / 2 );

					multiTarget[i].renderer.material.color = Color.cyan;
					multiTarget[i].GetComponent<enemy>().health -= ConstantsLib.TESLA_PROJ_DAMAGE;

				}
			}
		}
		else if( gameObject.tag == ConstantsLib.SYRINGE_TAG )
		{
			if( projectile == null )
			{
				Debug.Log( "insta" );
				projectile = (GameObject) Instantiate( projectilePrefab, muzzlePosition.transform.position, muzzlePosition.rotation );
			}

			//Debug.Log( projectilePrefab );
			//Debug.Log( projectile.particleEmitter.emit );
			target.GetComponent<enemy>().health -= ConstantsLib.SYRINGE_PROJ_DAMAGE * lockOnTime;
			target.renderer.material.color = Color.yellow;
			projectile.transform.LookAt( target.transform.position );
			projectile.transform.Rotate( 0, 270, 0 );

			if( target.GetComponent<enemy>().health < 0 )
			{
				Destroy( projectile );
			}


		}


	}
}
