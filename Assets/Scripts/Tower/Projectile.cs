using UnityEngine;

// create enumerator for projectile type
public enum ProjectileTypeEnum {
	rock, arrow, fireball
};

public class Projectile : MonoBehaviour {

	// damage of the projectile
	[SerializeField] private int attackStrength;


	[SerializeField] private ProjectileTypeEnum projectileType;

	// Getter for Attack Strength
	public int AttackStrength {

		get 
		{
			return attackStrength;
		}

	}

	// Getter for Projectile Type
	public ProjectileTypeEnum ProjectileType {

		get 
		{
			return projectileType;
		}

	}



}
