// Terms of the use :
// * You can only use this script to offer platers for other or your games
// * You cannot offer anythings else like sexual contents or other things (just content for children)

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;
using System.Linq;
public class RecommendedGames : MonoBehaviour {
    [Space(3)]
	// Button sprites
	public Image[] targetSprite;

	// Games icon urls
	public string[] ImagesURL;
	// Internal
	Texture2D[] textures;

	string[] LinksURL;

	// Game links file with https, splited with lines     
	public string gameLinks;

	// Activated when player is online    (Ad border    )
	public GameObject Loading;

	void Start()
	{
		
		textures = new Texture2D[targetSprite.Length];

		LinksURL = new string[targetSprite.Length];

		for(int a = 0;a<textures.Length;a++)
			textures[a] =  new Texture2D(4, 4, TextureFormat.DXT1, false);

		Reload ();

	}

	public void Reload()
	{
		StopCoroutine (ReadLinks());
		StopCoroutine (ReadImages ());
		Loading.SetActive (true);

		StartCoroutine (ReadLinks ());
	}
	public void LoadAd(int id)
	{
		if(LinksURL [id].Contains("https"))
			Application.OpenURL (LinksURL [id]);
		else
			GameObject.FindObjectOfType<MenuTools>().OpenGooglePlay(LinksURL [id]);
		
	}

	int loaded;

    public WWW Www { get; set; }
    public WWW Www1 { get => Www; set => Www = value; }

    IEnumerator ReadImages()
	{
		for(int b = 0;b<ImagesURL.Length;b++)
		{
			if (b >= loaded) {
				Www = new WWW (ImagesURL [b]);

				yield return Www;
				Www.LoadImageIntoTexture (textures [b]);
				targetSprite [b].sprite = Sprite.Create (textures [b], new Rect (0, 0, textures [b].width, textures [b].height), new Vector2 (0, 0), 100.0f);
				targetSprite [b].gameObject.SetActive (true);
				Www.Dispose ();
				Www = null;
				loaded++;
			}

		}
		Loading.SetActive (false);
	}
	IEnumerator ReadLinks()
	{
		// Read Link URLs
		Www = new WWW (gameLinks);

		yield return Www;

		string	longStringFromFile = Www.text;
		List<string> lines = new List<string>(
			longStringFromFile
			.Split(new string[] { "\r","\n" },
				StringSplitOptions.RemoveEmptyEntries) );
		// remove comment lines...
		lines = lines
			.Where(line => !(line.StartsWith("//")
				|| line.StartsWith("#")))
			.ToList();

		for(int c = 0;c<lines.Count;c++)
			LinksURL [c] = lines [c];

		Www.Dispose ();
		Www = null;

		StartCoroutine(ReadImages ());
	}
}
	   