using System;
using System.IO;
using BepInEx;
using BepInEx.Configuration;
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
		public static Plugin Instance;
		public ConfigEntry<bool> ShowGorillaBody;

		private const string assetsDir = "/BepInEx/plugins/GorillaTagLIV/Assets/";
		private Camera thirdPersonCamera;
        private LIV.SDK.Unity.LIV liv;

		private void Awake()
        {
	        Instance = this;
	        ShowGorillaBody = Config.Bind("Settings", "ShowGorillaBody", false);
        }

        private void OnEnable() {
	        HarmonyPatches.ApplyHarmonyPatches();
			Events.GameInitialized += OnGameInitialized;
			
			var shaderBundle = LoadBundle("liv-shaders");
            SDKShaders.LoadFromAssetBundle(shaderBundle);
            
            LivModComputerInstaller.Install();
            
            Config.SettingChanged += OnSettingChanged;
		}

        private void OnSettingChanged(object sender, SettingChangedEventArgs e)
        {
	        SetUpLivLayerMask();
        }

        private void OnDisable() {
			DestroyExistingLiv();
			HarmonyPatches.RemoveHarmonyPatches();
			Config.SettingChanged -= OnSettingChanged;
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
	        
            Debug.Log($"Setting up LIV with camera {camera.name}");

            DestroyExistingLiv();

            var livObject = new GameObject("LIV");
            livObject.gameObject.SetActive(false);
            livObject.transform.SetParent(parent, false);

            liv = livObject.AddComponent<LIV.SDK.Unity.LIV>();
            liv.stage = parent;
            liv.HMDCamera = camera;
            liv.fixPostEffectsAlpha = true;

            liv.onActivate += SetUpLivLayerMask;

            Debug.Log($"LIV created successfully with stage {parent.name}");
            
            livObject.gameObject.SetActive(true);
        }

        private void SetUpLivLayerMask()
        {
	        if (!liv || !liv.isActive) return;

	        liv.spectatorLayerMask = liv.HMDCamera.cullingMask;
	        if (!ShowGorillaBody.Value)
	        {
		        liv.spectatorLayerMask &= ~(1 << (int) CustomLayers.HideFromLiv);
	        }
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
