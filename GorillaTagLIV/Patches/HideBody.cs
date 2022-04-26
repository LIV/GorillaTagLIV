using System;
using HarmonyLib;
using UnityEngine;

namespace GorillaTagLIV.Patches {
	[HarmonyPatch]
	internal class HideBody
	{
		[HarmonyPostfix]
		[HarmonyPatch(typeof(VRRig), "Start")]
		private static void HidePlayerBody(VRRig __instance)
		{
			if (__instance.photonView)
			{
				if (!__instance.photonView.IsMine) return;
			}
			else
			{
				if (!__instance.isOfflineVRRig) return;
			}
			
			var renderers = __instance.GetComponentsInChildren<Renderer>();
			foreach (var renderer in renderers)
			{
				renderer.gameObject.layer = (int) CustomLayers.HideFromLiv;
			}
		}
		
		[HarmonyPostfix]
		[HarmonyPatch(typeof(GorillaTagger), "Start")]
		private static void ChangeCameraCullingMasks(GorillaTagger __instance)
		{
			ChangeCameraCullingMask(__instance.mainCamera);
			ChangeCameraCullingMask(__instance.thirdPersonCamera);
			ChangeCameraCullingMask(__instance.mirrorCamera);
		}

		private static void ChangeCameraCullingMask(GameObject cameraObject)
		{
			var camera = cameraObject.GetComponentInChildren<Camera>();
			if (!camera)
			{
				throw new Exception("Failed to find camera in object " + cameraObject.name);
			}
			ChangeCameraCullingMask(camera);
		}

		private static void ChangeCameraCullingMask(Camera camera)
		{
			camera.cullingMask |= 1 << (int) CustomLayers.HideFromLiv;
		}
	}
}
