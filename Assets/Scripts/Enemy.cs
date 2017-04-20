using UnityEngine;

public class Enemy : MonoBehaviour {

	// which checkpoint to pathfind toward
	[SerializeField] private int targetPathNode = 0;

	// the level exit location
	[SerializeField] private Transform exitPoint;

	// store all checkpoints
	[SerializeField] private Transform[] wayPoints;

	// control checks on navigation
	[SerializeField] private float navigationUpdate;

	// unit speed
	public float walkSpeed;

	private Transform enemy;

	private float navigationTime = 0;


	// Use this for initialization
	void Start () {

		
		enemy = GetComponent<Transform>();


	}
	
	// Update is called once per frame
	void Update ()
	{

		if (wayPoints != null) {

			// add time increments
			navigationTime += Time.deltaTime;

			// if we are ready to update
			if (navigationTime > navigationUpdate) {

				// if the target Node number is within the quantity available
				if (targetPathNode < wayPoints.Length) {

					// move the target
					enemy.position = Vector2.MoveTowards (enemy.position, wayPoints [targetPathNode].position, navigationTime);

				} else {
					// only remaining node is the exit
					enemy.position = Vector2.MoveTowards (enemy.position, exitPoint.position, navigationTime);
					
				}

				// reset timer
				navigationTime = 0;

			}

		}


	}


	// to detect navigation changes as waypoints are hit
	void OnTriggerEnter2D (Collider2D collider)
	{

		if (collider.tag == "Checkpoint") {

			// change the target path node to move towards
			targetPathNode += 1;

		} else if (collider.tag == "Finish") {

			Destroy(gameObject);

			GameManager.Instance.RemoveEnemyFromScreen();

		}

	}


}
