using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager> {

	[SerializeField] private AudioClip sfxArrow;
	[SerializeField] private AudioClip sfxFireball;
	[SerializeField] private AudioClip sfxRock;
	[SerializeField] private AudioClip sfxHit;
	[SerializeField] private AudioClip sfxDeath;
	[SerializeField] private AudioClip sfxTowerBuilt;
	[SerializeField] private AudioClip sfxNewGame;
	[SerializeField] private AudioClip sfxGameOver;

	[SerializeField] private AudioClip musicLevel;



	public AudioClip SFXArrow {
		get {
			return sfxArrow;
		}
	}

	public AudioClip SFXFireball {
		get {
			return sfxFireball;
		}
	}

	public AudioClip SFXRock{
		get {
			return sfxRock;
		}
	}

	public AudioClip SFXHit {
		get {
			return sfxHit;
		}
	}

	public AudioClip SFXDeath {
		get {
			return sfxDeath;
		}
	}

	public AudioClip SFXTowerBuilt {
		get {
			return sfxTowerBuilt;
		}
	}

	public AudioClip SFXNewGame {
		get {
			return sfxNewGame;
		}
	}

	public AudioClip SFXGameOver {
		get {
			return sfxGameOver;
		}
	}

	public AudioClip MusicLevel {
		get {
			return musicLevel;
		}
	}




}
