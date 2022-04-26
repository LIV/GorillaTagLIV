using System;
using System.IO;
using BepInEx;
using UnityEngine;
using Utilla;
using LIV.SDK.Unity;
using UnityEngine.SpatialTracking;

namespace GorillaTagLIV
{
	/// <summary>
	/// This is your mod's main class.
	/// </summary>

	/* This attribute tells Utilla to look for [ModdedGameJoin] and [ModdedGameLeave] */
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		private const string assetsDir = "/BepInEx/plugins/GorillaTagLIV/Assets/";

        private LIV.SDK.Unity.LIV liv;
		bool inRoom;

		void OnEnable() {
			/* Set up your mod here */
			/* Code here runs at the start and whenever your mod is enabled*/

			HarmonyPatches.ApplyHarmonyPatches();
			Utilla.Events.GameInitialized += OnGameInitialized;
			
			var shaderBundle = LoadBundle("liv-shaders");
            SDKShaders.LoadFromAssetBundle(shaderBundle);
		}

		void OnDisable() {
			/* Undo mod setup here */
			/* This provides support for toggling mods with ComputerInterface, please implement it :) */
			/* Code here runs whenever your mod is disabled (including if it disabled on startup)*/

			HarmonyPatches.RemoveHarmonyPatches();
			Utilla.Events.GameInitialized -= OnGameInitialized;
		}

		void OnGameInitialized(object sender, EventArgs e)
		{
			var player = GorillaLocomotion.Player.Instance;
			var tagger = player.GetComponent<GorillaTagger>();

			SetUpLiv(tagger.mainCamera.GetComponent<Camera>(), tagger.mainCamera.transform.parent);
		}

		void Update()
		{
			/* Code here runs every frame when the mod is enabled */
		}

		/* This attribute tells Utilla to call this method when a modded room is joined */
		[ModdedGamemodeJoin]
		public void OnJoin(string gamemode)
		{
			/* Activate your mod here */
			/* This code will run regardless of if the mod is enabled*/

			inRoom = true;
		}

		/* This attribute tells Utilla to call this method when a modded room is left */
		[ModdedGamemodeLeave]
		public void OnLeave(string gamemode)
		{
			/* Deactivate your mod here */
			/* This code will run regardless of if the mod is enabled*/

			inRoom = false;
		}
		
		private AssetBundle LoadBundle(string assetName)
        {
            var bundle = AssetBundle.LoadFromFile(string.Format("{0}{1}{2}", Directory.GetCurrentDirectory(), assetsDir,
	            assetName));

            if (bundle == null)
            {
	            throw new Exception("Failed to load asset bundle" + assetName);
            }

            return bundle;
        }
		
        private void SetUpLiv(Camera camera, Transform parent)
        {
            Debug.Log(string.Format("Setting up LIV with camera {0}", camera.name));
            
            if (liv)
            {
                Debug.Log("LIV instance already exists. Destroying it.");
                Destroy(liv.gameObject);
            }

            var livObject = new GameObject("LIV");
            livObject.gameObject.SetActive(false);
            livObject.transform.SetParent(parent, false);

            liv = livObject.AddComponent<LIV.SDK.Unity.LIV>();
            liv.stage = parent;
            liv.HMDCamera = camera;
            liv.fixPostEffectsAlpha = true;
            liv.spectatorLayerMask = camera.cullingMask;
            
			// TODO: make a custom layer and add the player meshes to it.
            liv.spectatorLayerMask &= ~(1 << (int) CustomLayers.HideFromLiv);

            Debug.Log(string.Format("LIV created successfully with stage {0}", parent.name));
            
            livObject.gameObject.SetActive(true);
        }
	}
}
