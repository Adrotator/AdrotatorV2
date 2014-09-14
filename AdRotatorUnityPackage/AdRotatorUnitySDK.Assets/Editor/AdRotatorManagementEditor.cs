// Version 2.0.0.0
using AdRotatorUnitySDK.Assets.Plugins;
using UnityEditor;
using UnityEngine;

namespace AdRotatorUnitySDK.Assets.Editor
{

    /// <summary>
    /// Ad Rotator settings editor.
    /// Store in "Assets\Editor".
    /// </summary>
    [CustomEditor(typeof(AdRotatorManagement))]
    public class AdRotatorManagementEditor : UnityEditor.Editor
    {
        Vector3 adRotatorPosition = Vector3.zero;
        Vector2 adRotatorPositionOffset = new Vector2(0, 1);

        private float ScreenHeight;
        private float ScreenWidth;


        /// <summary>
        /// Ad Rotator inspector GUI layout and behavior.
        /// </summary>
        public override void OnInspectorGUI()
        {
            var customTarget = (AdRotatorManagement)base.target;

            GUILayout.BeginVertical();
            customTarget.AdSettings.IsEnabled = EditorGUILayout.Toggle(new GUIContent("Enabled?", "Check to enable Ad Rotator. Ad will not be displayed if disabled."), customTarget.AdSettings.IsEnabled);
            if (!customTarget.AdSettings.IsEnabled)
            {
                GUI.contentColor = Color.gray;
            }

            GUILayout.Label("Layout", EditorStyles.boldLabel);
            customTarget.AdSettings.Position = (AdPosition)EditorGUILayout.EnumPopup(new GUIContent("  Ad Position", "Screen position of the Ad banner."), customTarget.AdSettings.Position);
            customTarget.AdSettings.Size = (AdSize)EditorGUILayout.EnumPopup(new GUIContent("  Ad Size", "Size of the Ad Banner. Only supported sizes are listed."), customTarget.AdSettings.Size);
            GUILayout.Label("");
            GUILayout.Label("Sliding Ads", EditorStyles.boldLabel);
            customTarget.AdSettings.SlidingAdDirection = (SlidingAdDirection)EditorGUILayout.EnumPopup(new GUIContent("  Direction", "Set SlidingAdDirection to Left, Right, Bottom or Top to have the ad slide in, stay for SlidingAdDisplaySeconds, slide out and stay hidden for SlidingAdHiddenSeconds. If SlidingAdDirection is set to None (this is the default), this behaviour does not take place, the ad remains static."), customTarget.AdSettings.SlidingAdDirection);

            if (customTarget.AdSettings.SlidingAdDirection == SlidingAdDirection.None && customTarget.AdSettings.IsEnabled)
            {
                GUI.contentColor = Color.gray;
            }

            customTarget.AdSettings.SlidingAdDisplaySeconds = EditorGUILayout.IntSlider(new GUIContent("  Ad Display in seconds", "When Sliding Ad is enabled, ad stays for SlidingAdDisplaySeconds, slide out and stay hidden for SlidingAdHiddenSeconds."), customTarget.AdSettings.SlidingAdDisplaySeconds, 0, 1800);
            customTarget.AdSettings.SlidingAdHiddenSeconds = EditorGUILayout.IntSlider(new GUIContent("  Ad Hidden in seconds", "When Sliding Ad is enabled, ad stays for SlidingAdDisplaySeconds, slide out and stay hidden for SlidingAdHiddenSeconds."), customTarget.AdSettings.SlidingAdHiddenSeconds, 0, 1800);

            GUI.contentColor = Color.white;
            GUILayout.Label("");
            GUILayout.Label("Ad Providers Settings", EditorStyles.boldLabel);
            customTarget.AppSettings.SettingsUrl = EditorGUILayout.TextField(new GUIContent("  Ad Settings URL", "URL to the remote XML file that controls the probability of ad providers shown. Strongly advised to set this property. Example: http://mydomain.com/myAdSettings.xml"), customTarget.AppSettings.SettingsUrl);
            GUILayout.Label("");
            customTarget.AppSettings.AdMode = (AdMode)EditorGUILayout.EnumPopup(new GUIContent("  Ad Retrieval Mode", "Mode in which Ads will be selected."), customTarget.AppSettings.AdMode);

            GUILayout.EndVertical();

            if (GUI.changed)
            {
                EditorUtility.SetDirty(customTarget);
            }
        }

        void OnSceneGUI()
        {
            GetAdRotatorPosition();
            Handles.RectangleCap(0, adRotatorPosition, Quaternion.identity, 1);
            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
            }
        }

        private void GetAdRotatorPosition()
        {
            AdRotatorManagement managementScript = (AdRotatorManagement)target;
            ScreenHeight = Camera.main.orthographicSize * 2;
            ScreenWidth = ScreenHeight * Screen.width / Screen.height;
            switch (managementScript.AdSettings.Position)
            {
                case AdPosition.TopLeftCorner:
                    adRotatorPosition = new Vector3(-(ScreenWidth / 2) - adRotatorPositionOffset.x, (ScreenHeight / 2) - adRotatorPositionOffset.y, 0);
                    break;
                case AdPosition.TopRightCorner:
                    adRotatorPosition = new Vector3((ScreenWidth / 2) - adRotatorPositionOffset.x, (ScreenHeight / 2) - adRotatorPositionOffset.y, 0);
                    break;
                case AdPosition.BottomLeftCorner:
                    adRotatorPosition = new Vector3(-(ScreenWidth / 2) + adRotatorPositionOffset.x, -(ScreenHeight / 2) + adRotatorPositionOffset.y, 0);
                    break;
                case AdPosition.BottomRightCorner:
                    adRotatorPosition = new Vector3((ScreenWidth / 2) - adRotatorPositionOffset.x, -(ScreenHeight / 2) + adRotatorPositionOffset.y, 0);
                    break;
                case AdPosition.TopCenter:
                    adRotatorPosition = new Vector3(0, (ScreenHeight / 2) - adRotatorPositionOffset.y, 0);
                    break;
                case AdPosition.BottomCenter:
                    adRotatorPosition = new Vector3(0, -(ScreenHeight / 2) + adRotatorPositionOffset.y, 0);
                    break;
                case AdPosition.LeftCenter:
                    adRotatorPosition = new Vector3(-(ScreenWidth / 2) + adRotatorPositionOffset.x, 0, 0);
                    break;
                case AdPosition.RightCenter:
                    adRotatorPosition = new Vector3((ScreenWidth / 2) - adRotatorPositionOffset.x, 0, 0);
                    break;
                case AdPosition.Center:
                    adRotatorPosition = Vector3.zero;
                    break;
            }
        }
    }
}