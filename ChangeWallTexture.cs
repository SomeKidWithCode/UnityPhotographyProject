using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System;

public class ChangeWallTexture : MonoBehaviour
{
	Renderer renderTarget;

	readonly Dictionary<string, Texture> backdropTextures = new();

	void Start()
	{
		// First we fetch the renderer target that we want to change
		renderTarget = GetComponent<Renderer>();

		// Load all of the possible textures
		string infinityWallTexturesPath = Path.Combine(Application.dataPath, "Resources", "TextureWallTextures");
		foreach (string backdropPath in Directory.GetFiles(infinityWallTexturesPath, "*.png"))
		{
			backdropTextures.Add(
				Path.GetFileNameWithoutExtension(backdropPath),
				Resources.Load<Texture>(Path.Combine(
					"TextureWallTextures",
					Path.GetFileNameWithoutExtension(backdropPath))
				)
			);
			Debug.Log($"Loaded <{Path.GetFileNameWithoutExtension(backdropPath)}> backdrop texture");
		}
    }

	bool toggleTex = false;
	bool isDog = false;

	private void LateUpdate()
	{
		toggleTex |= Input.GetKeyDown(KeyCode.F);
		if (toggleTex)
		{
			if (isDog) {
				changeTexture("dog");
			}
			else {
				changeTexture("fish");
			}
			
			isDog = !isDog;
			toggleTex = false;
		}
	}

	void changeTexture(string textureString)
	{
		if (!backdropTextures.ContainsKey(textureString))
			throw new ArgumentException($"{textureString} is not an available texture");
		renderTarget.material.mainTexture = backdropTextures[textureString];
		Debug.Log($"Changed tex to <{backdropTextures[textureString]}>");
	}
}
