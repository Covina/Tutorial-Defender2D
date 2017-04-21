using UnityEngine;


// Make a generic class type, specify that all passed types inherit from Mono
public class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

	private static T instance;

	public void Create ()
	{




	}


	public static T Instance
	{
		get 
			{ 
				if (instance == null) {
					instance = FindObjectOfType<T>();
				} else if (instance != FindObjectOfType<T>() ){
					Destroy(FindObjectOfType<T>() );
				}

				DontDestroyOnLoad( FindObjectOfType<T>() );		

				return instance; 

			}
	}



}
