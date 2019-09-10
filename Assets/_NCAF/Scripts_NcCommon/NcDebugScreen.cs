using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NcAF;

namespace NcCommon
{
    public class NcDebugScreen : MonoBehaviour
    {
        public bool m_isEnabled =false;
        [Header("Parameters to display debug info")]
        public static int labelCount = 40;
        public static float labelWidthRatio = 0.9f;
        public static float topMarginRatio = 0.075f;
        public static float leftMarginRatio = 0.05f;

        public static Color defaultTextColor = Color.black;
        public static Color32 textBackgroundColor;

        public static int m_msgCount { get; private set; }
        public static GUIStyle m_GUIStyle = null;

        //private member. These will update per frame
        int labelWidth = 0;
        int labelHeight = 0;
        int TopMargin = 10;
        int LeftMargin = 10;

        private ScreenOrientation PREV_ORIENTATION;

        // On Overray Debug Printed
        //public delegate void OverrayMsgHandler(string msg, Color? color = null, GUIStyle style = null);
        //public static event OverrayMsgHandler OnOverrayDebugPrinted;


        #region Singleton Stuff
        public static NcDebugScreen Instance { get; private set; }

        private void Awake()
        {
            if (NcDebugScreen.Instance == null) NcDebugScreen.Instance = this;
            if (NcDebugScreen.Instance != this) Destroy(this);
        }
        #endregion

        // Start is called before the first frame update
        private void Start()
        {
            // check if the scene controller is specified. 
            PREV_ORIENTATION = Screen.orientation;
            m_msgCount = 0;
            initiateGUIStyle();
            UpdateDebugScreenParams();
        }

        private void initiateGUIStyle()
        {
            m_GUIStyle = new GUIStyle();
            var texture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            texture.SetPixel(0, 0, new Color(1f, 1f, 1f, 0.5f));
            texture.Apply();

            m_GUIStyle.normal.background = texture;
            m_GUIStyle.normal.textColor = Color.red;
            m_GUIStyle.fontSize = (int)((Screen.height / labelCount) * 0.75f);
        }
        // Update is called once per frame
        void Update()
        {

        }



        public void ResetDebugScreen()
        {
            m_msgCount = 0;
            ChangeTextColor(defaultTextColor);
        }

        public void ChangeTextColor(Color color)
        {
            m_GUIStyle.normal.textColor = color;
        }

        void UpdateDebugScreenParams()
        {
            LeftMargin = (int)(Screen.width * leftMarginRatio);
            TopMargin = (int)(Screen.height * topMarginRatio);

            labelWidth = (int)(Screen.width * labelWidthRatio);
            labelHeight = (int)(Screen.height / labelCount);
            m_GUIStyle.fontSize = (int)((Screen.height / labelCount) * 0.75f);
        }
        public void ShowDebugMsg(string msg, GUIStyle style = null, Color? color = null)
        {
            if (style == null) { style = m_GUIStyle; }
            if (color != null) { style.normal.textColor = (Color)color; }
            GUI.Label(new Rect(LeftMargin, TopMargin + ((m_msgCount) * labelHeight), labelWidth, labelHeight), msg, m_GUIStyle);
            m_msgCount += 1;
        }

        public void OnGUI()
        {
            UpdateDebugScreenParams();

            if (NcafMainController.Instance == null || !m_isEnabled) return;

            NcDebugScreen.Instance.ChangeTextColor(Color.green);
            if (NcafMainController.Instance.WorldTrackingAlignProcess == null)
            {
                ShowDebugMsg("no alignment interpolation process");
            }
            else ShowDebugMsg(NcafMainController.Instance.WorldTrackingAlignProcess.ToString());
            ResetDebugScreen();
        }
        public void _ShowDebugMsg(string msg, Color? color = null, GUIStyle style = null)
        {
            if (msg != "") NcDebugScreen.Instance.ShowDebugMsg(msg, style, color);
        }

        public void ToggleOnOff()
        {
            m_isEnabled = !m_isEnabled;
        }
    }
}