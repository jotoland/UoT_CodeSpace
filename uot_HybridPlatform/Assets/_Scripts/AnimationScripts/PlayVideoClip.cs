using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* John G. Toland 4/10/17
 * This scrip only works if the current platform is not a mobile platform
 * Mobile platforms like IOS and Android do not support movie textures yet.
 * 
 * This script starts the movie texture that houses the movie clip
 * */
public class PlayVideoClip : MonoBehaviour {
	#if UNITY_STANDALONE
	private Renderer r;
	private MovieTexture movie;
	// Use this for initialization
	void Start () {
		r = GetComponent<Renderer>();
		movie = (MovieTexture)r.material.mainTexture;
		movie.Play ();
		movie.loop = true;

	}
	#endif

}
