using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
/* John G. Toland 4/10/17 Audio Mixer Handler Script
 * Gives control of the audio main mix mixer levels to the user
 * This control is done throught the sliders in the Audio Menu found in NavMenu via Pause button
 * */
public class MixerLevels : MonoBehaviour {

	public AudioMixer mainMix;
	public Slider Explostions;
	public Slider Blasters;
	public Slider Music;
	public Slider Collectables;
	public Slider MasterVolume;
	private PauseNavGUI pNG;
	private bool WaitingForMute = true;
	public GameObject muteAudio;

	void Start(){
		GameObject pauseNavGUI = GameObject.FindGameObjectWithTag ("PauseBtn");
		if(pauseNavGUI != null){
			pNG = pauseNavGUI.GetComponent<PauseNavGUI> ();
		}
	}

	void Update(){
		if (AudioListener.volume == 0 && WaitingForMute) {
			WaitingForMute = false;
			SliderValuesToMute ();
		}
		if (AudioListener.volume == 1 && !WaitingForMute) {
			WaitingForMute = true;
			ClearAll ();
		}
	}

	public void SliderValuesToMute(){
		Explostions.value = -80;
		Collectables.value = -80;
		Blasters.value = -80;
		Music.value = -80;
		MasterVolume.value = -80;
	}

	public void SetMasterLvl(float MasterLvl){
		mainMix.SetFloat ("Master", MasterLvl);
		if (MasterLvl < -10) {
			Explostions.value = MasterLvl;
		}
		if (MasterLvl < -20) {
			Collectables.value = MasterLvl;
		}
		if (MasterLvl < -16) {
			Blasters.value = MasterLvl;
		}
		if (MasterLvl < -4) {
			Music.value = MasterLvl;
		}
	}

	public void SetMusicLvl(float MusicLvl){
		mainMix.SetFloat ("Music", MusicLvl);
	}

	public void SetLazerShotLvl(float LazerShotLvl){
		mainMix.SetFloat ("EnemyShots", LazerShotLvl);
		mainMix.SetFloat ("PlayerShots", LazerShotLvl);
	}

	public void SetPickUpsLvl(float PickUpsLvl){
		mainMix.SetFloat ("PickUps", PickUpsLvl);
	}

	public void SetExplosionsLvl(float ExplosionsLvl){
		mainMix.SetFloat ("Explosions", ExplosionsLvl);
	}

	public void ClearAll(){
		AudioListener.volume = 1;
		pNG.AUDIO_MUTED = false;
		muteAudio.GetComponentInChildren<Text>().text = "Mute Audio";
		mainMix.ClearFloat ("Explosions");
		Explostions.value = -10;
		mainMix.ClearFloat ("PickUps");
		Collectables.value = -20;
		mainMix.ClearFloat ("EnemyShots");
		mainMix.ClearFloat ("PlayerShots");
		Blasters.value = -16;
		mainMix.ClearFloat ("Music");
		Music.value = -4;
		mainMix.ClearFloat ("Master");
		MasterVolume.value = 0;
	}
}
//finito