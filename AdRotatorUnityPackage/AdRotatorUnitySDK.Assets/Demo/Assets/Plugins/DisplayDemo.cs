// Version 1.0.0.0
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using System.Collections;
using AdRotatorUnitySDK.Assets.Plugins;

namespace AdRotatorUnitySDK.Demo.Assets.Plugins
{
	public class DisplayDemo : MonoBehaviour
	{
		void OnGUI()
		{
			if (GUI.Button(new Rect(300, 100, 200, 30), "TopCenter, 728x90 (sliding)"))
			{	
				UpdateAd(true, AdPosition.TopCenter, AdSize.Leaderboard728x90, SlidingAdDirection.Top, 3, 3);
			}
			else if (GUI.Button(new Rect(300, 140, 200, 30), "TopLeftCorner, 292x60"))
			{
				UpdateAd(true, AdPosition.TopLeftCorner, AdSize.SnapViewBanner292x60, SlidingAdDirection.None, 0, 0);
			}
			else if (GUI.Button(new Rect(300, 180, 200, 30), "TopRightCorner, 250x250"))
			{
				UpdateAd(true, AdPosition.TopRightCorner, AdSize.SquarePopUp250x250, SlidingAdDirection.None, 0, 0);
			}
			else if (GUI.Button(new Rect(300, 220, 200, 30), "LeftCenter, 160x600 (sliding)"))
			{
				UpdateAd(true, AdPosition.LeftCenter, AdSize.WideSkyscraper160x600, SlidingAdDirection.Left, 5, 2);
			}
			else if (GUI.Button(new Rect(300, 260, 200, 30), "RightCenter, 250x125"))
			{
				UpdateAd(true, AdPosition.RightCenter, AdSize.HalfTile250x125, SlidingAdDirection.None, 0, 0);
			}
			else if (GUI.Button(new Rect(300, 300, 200, 30), "BottomCenter, 300x259"))
			{
				UpdateAd(true, AdPosition.BottomCenter, AdSize.MediumRectangle300x250, SlidingAdDirection.None, 0, 0);
			}
			else if (GUI.Button(new Rect(300, 340, 200, 30), "Center, 250x250"))
			{
				UpdateAd(true, AdPosition.Center, AdSize.SquarePopUp250x250, SlidingAdDirection.None, 0, 0);
			}
			else if (GUI.Button(new Rect(300, 380, 200, 30), "Disable Ad"))
			{
				UpdateAd(false);
			}
			else if (GUI.Button(new Rect(300, 420, 200, 30), "Switch Scene"))
			{
				if (Application.loadedLevelName == "red")
				{
					Application.LoadLevel("green");
				}
				else
				{
					Application.LoadLevel("red");
				}
			}
		}

		void UpdateAd(bool isEnabled, AdPosition position, AdSize size, SlidingAdDirection slidingDirection, int slidingAdDisplaySeconds, int slidingAdHiddenSeconds)
		{
			var ads = GameObject.Find("AdRotatorManagement");
			if (ads != null)
			{
				var adMgt = ads.GetComponent<AdRotatorManagement>();

				adMgt.AdSettings.IsEnabled = false;
				adMgt.AdSettings.Position = AdPosition.TopCenter;
				adMgt.AdSettings.IsEnabled = isEnabled;
				adMgt.AdSettings.Position = position;
				adMgt.AdSettings.Size = size;
				adMgt.AdSettings.SlidingAdDirection = slidingDirection;
				adMgt.AdSettings.SlidingAdDisplaySeconds = slidingAdDisplaySeconds;
				adMgt.AdSettings.SlidingAdHiddenSeconds = slidingAdHiddenSeconds;
				adMgt.UpdateAd();
			}
			else
			{
				Debug.LogError("AdRotatorManagement game object cannot be found.");
			}
		}

		void UpdateAd(bool isEnabled)
		{
			var ads = GameObject.Find("AdRotatorManagement");
			if (ads != null)
			{
				var adMgt = ads.GetComponent<AdRotatorManagement>();
				adMgt.AdSettings.IsEnabled = isEnabled;
				adMgt.UpdateAd();
			}
		}
	}
}
