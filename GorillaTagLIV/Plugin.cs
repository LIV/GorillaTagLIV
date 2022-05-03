using System;
using System.IO;
using BepInEx;
using Cinemachine;
using UnityEngine;
using Utilla;
using LIV.SDK.Unity;

namespace GorillaTagLIV
{
	[ModdedGamemode]
	[BepInDependency("org.legoandmars.gorillatag.utilla", "1.5.0")]
	[BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
	public class Plugin : BaseUnityPlugin
	{
		private const string assetsDir = "/BepInEx/plugins/GorillaTagLIV/Assets/";
		private Camera thirdPersonCamera;

        private LIV.SDK.Unity.LIV liv;

        private void OnEnable() {
	        HarmonyPatches.ApplyHarmonyPatches();
			Events.GameInitialized += OnGameInitialized;
			
			var shaderBundle = LoadBundle("liv-shaders");
            SDKShaders.LoadFromAssetBundle(shaderBundle);
		}

        private void OnDisable() {
			DestroyExistingLiv();
			HarmonyPatches.RemoveHarmonyPatches();
		}

        private void OnGameInitialized(object sender, EventArgs e)
		{
			var player = GorillaLocomotion.Player.Instance;
			var tagger = player.GetComponent<GorillaTagger>();

			SetUpLiv(tagger.mainCamera.GetComponent<Camera>(), tagger.mainCamera.transform.parent);
			SetUpThridPersonCamera();
		}
		
		private static AssetBundle LoadBundle(string assetName)
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
	        if (!enabled) return;

	        
            Debug.Log(string.Format("Setting up LIV with camera {0}", camera.name));

            DestroyExistingLiv();

            var livObject = new GameObject("LIV");
            livObject.gameObject.SetActive(false);
            livObject.transform.SetParent(parent, false);

            liv = livObject.AddComponent<LIV.SDK.Unity.LIV>();
            liv.stage = parent;
            liv.HMDCamera = camera;
            liv.fixPostEffectsAlpha = true;
            liv.spectatorLayerMask = camera.cullingMask;
            
            liv.spectatorLayerMask &= ~(1 << (int) CustomLayers.HideFromLiv);

            Debug.Log(string.Format("LIV created successfully with stage {0}", parent.name));
            
            livObject.gameObject.SetActive(true);
        }

        private void SetUpThridPersonCamera()
        {
	        var cinemachineBrain = FindObjectOfType<CinemachineBrain>();
	        if (cinemachineBrain)
	        {
				thirdPersonCamera = cinemachineBrain.GetComponent<Camera>();
	        }
        }

        private void DestroyExistingLiv()
        {
	        if (!liv) return;
	        Debug.Log("LIV instance already exists. Destroying it.");
	        thirdPersonCamera = null;
	        Destroy(liv.gameObject);
        }

        private void Update()
        {
	        if (!liv || !thirdPersonCamera || !liv.isActive) return;

	        var cameraTransform = thirdPersonCamera.transform;
	        liv.render.SetPose(cameraTransform.position, cameraTransform.rotation, thirdPersonCamera.fieldOfView);
        }
	}
}
