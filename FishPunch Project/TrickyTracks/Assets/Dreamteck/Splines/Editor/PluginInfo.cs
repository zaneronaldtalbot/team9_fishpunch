using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text.RegularExpressions;

namespace Dreamteck.Splines
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    public class PluginInfo
    {
        public static string version = "1.0.82";
        private static bool open = false;
        static PluginInfo()
        {
            if (open) return;
            bool showInfo = EditorPrefs.GetString("ShowPluginInfo.version", "") != version;
            if (!showInfo) return;
            InitWindow window = EditorWindow.GetWindow<InitWindow>(true);
            window.init();
            EditorPrefs.SetString("ShowPluginInfo.version", version);
            open = true;
        }
    }

    public class InitWindow : EditorWindow
    {
        public class WindowPanel
        {
            public float slideStart = 0f;
            public float slideDuration = 1f;
            public enum SlideDiretion { Left, Right, Up, Down }
            public SlideDiretion slideDirection = SlideDiretion.Left;
            public Vector2 size;
            private Vector2 origin = Vector2.zero;
            public bool open = false;

            public bool isActive
            {
                get
                {
                    return open || Time.realtimeSinceStartup - slideStart <= slideDuration;
                }
            }

            void HandleOrigin()
            {
                float percent = Mathf.Clamp01((Time.realtimeSinceStartup - slideStart) / slideDuration);
                if (open)
                {
                    switch (slideDirection)
                    {
                        case SlideDiretion.Left:
                            origin.x = Mathf.SmoothStep(size.x, 0f, percent);
                            origin.y = 0f;
                            break;

                        case SlideDiretion.Right:
                            origin.x = Mathf.SmoothStep(-size.x, 0f, percent);
                            origin.y = 0f;
                            break;

                        case SlideDiretion.Up:
                            origin.x = 0f;
                            origin.y = Mathf.SmoothStep(size.y, 0f, percent);
                            break;

                        case SlideDiretion.Down:
                            origin.x = 0f;
                            origin.y = Mathf.SmoothStep(-size.y, 0f, percent);
                            break;
                    }
                }
                else
                {
                    switch (slideDirection)
                    {
                        case SlideDiretion.Left:
                            origin.x = Mathf.SmoothStep(0f, -size.x, percent);
                            origin.y = 0f;
                            break;

                        case SlideDiretion.Right:
                            origin.x = Mathf.SmoothStep(0f, size.x, percent);
                            origin.y = 0f;
                            break;

                        case SlideDiretion.Up:
                            origin.x = 0f;
                            origin.y = Mathf.SmoothStep(0f, -size.y, percent);
                            break;

                        case SlideDiretion.Down:
                            origin.x = 0f;
                            origin.y = Mathf.SmoothStep(0f, -size.y, percent);
                            break;
                    }
                }
            }

            public void SetState(bool state, bool useTransition)
            {
                if (open == state) return;
                open = state;
                if (useTransition) slideStart = Time.realtimeSinceStartup;
                else slideStart = Time.realtimeSinceStartup + slideDuration;
            }

            public void Begin()
            {
                HandleOrigin();
                GUILayout.BeginArea(new Rect(origin.x, origin.y, size.x, size.y));
            }

            public void BackButton(WindowPanel backButtonPanel, SlideDiretion dir = SlideDiretion.Left)
            {
                if (GUILayout.Button("◄", GUILayout.Width(60), GUILayout.Height(35)))
                {
                    slideDirection = dir;
                    SetState(false, true);
                    backButtonPanel.slideDirection = dir;
                    backButtonPanel.SetState(true, true);
                }
            }

            public void OpenPanel(WindowPanel backButtonPanel, SlideDiretion dir = SlideDiretion.Left)
            {
                slideDirection = dir;
                SetState(false, true);
                backButtonPanel.slideDirection = dir;
                backButtonPanel.SetState(true, true);
            }

            public void End()
            {
                GUILayout.EndArea();
            }
        }

        private GUIStyle titleText;
        private GUIStyle buttonTitleText;
        private GUIStyle warningText;
        private GUIStyle wrapText;
        private bool guiInitialized = false;
        private bool windowInitialized = false;

        WindowPanel changeLogPanel;
        WindowPanel homePanel;
        WindowPanel supportPanel;
        WindowPanel learnPanel;

        string mailError = "";

        string email = "";
        string subject = "";
        string message = "";
        string senderName = "";
        bool mailSent = false;

        private string changelogText = "changelog.txt wasn't found. No Changelog information available.";

        Texture2D header;

        Texture2D changelogIcon;
        Texture2D supportIcon;
        Texture2D learnIcon;
        Texture2D rateIcon;
        Texture2D videoIcon;
        Texture2D pdfIcon;

        private Vector2 scroll;

        [MenuItem("Window/Dreamteck/Splines/Plugin Info")]
        public static void OpenWindow()
        {
            InitWindow window = EditorWindow.GetWindow<InitWindow>(true);
            window.init(); 
        }

        public void init()
        {
            minSize = maxSize = new Vector2(450, 500);
#if UNITY_5_0
                title = "Dreamteck Splines " + PluginInfo.version;
#else
            titleContent = new GUIContent("Dreamteck Splines " + PluginInfo.version);
#endif
            float x = position.x;
            if (x < 50) x = 50;
            else if (x > Screen.width - 500) x = Screen.width - 500;
            float y = position.y;
            if (y < 50) x = 50;
            else if (y > Screen.height - 550) x = Screen.height - 550;
            position = new Rect(x, y, 500, 550);

            changeLogPanel = new WindowPanel();
            supportPanel = new WindowPanel();
            homePanel = new WindowPanel();
            learnPanel = new WindowPanel();
            changeLogPanel.size = supportPanel.size = homePanel.size = learnPanel.size = new Vector2(maxSize.x, maxSize.y - 82);
            changeLogPanel.slideDuration = supportPanel.slideDuration = homePanel.slideDuration = learnPanel.slideDuration = 0.25f;
            homePanel.SetState(true, false);
            header = ImageDB.GetImage("plugin_header.png", "Splines/Editor/Icons");
            changelogIcon = ImageDB.GetImage("changelog.png", "Splines/Editor/Icons");
            learnIcon = ImageDB.GetImage("get_started.png", "Splines/Editor/Icons");
            supportIcon = ImageDB.GetImage("support.png", "Splines/Editor/Icons");
            rateIcon = ImageDB.GetImage("rate.png", "Splines/Editor/Icons");
            pdfIcon = ImageDB.GetImage("pdf.png", "Splines/Editor/Icons");
            videoIcon = ImageDB.GetImage("video_tutorials.png", "Splines/Editor/Icons");

            string path = ResourceUtility.FindFolder(Application.dataPath, "Dreamteck/Splines/Editor");
            if (Directory.Exists(path))
            { 
                if (File.Exists(path + "/changelog.txt")){
                    string[] lines = File.ReadAllLines(path + "/changelog.txt");
                    changelogText = "";
                    for(int i = 0; i < lines.Length; i++)
                    {
                        changelogText += lines[i] + "\r\n";
                    }
                }
            }
            windowInitialized = true;
        }

        void OnGUI()
        {
            if (!windowInitialized) return;
            if (!guiInitialized)
            {
                guiInitialized = true;
                buttonTitleText = new GUIStyle(GUI.skin.GetStyle("label"));
                buttonTitleText.fontStyle = FontStyle.Bold;
                titleText = new GUIStyle(GUI.skin.GetStyle("label"));
                titleText.fontSize = 25;
                titleText.fontStyle = FontStyle.Bold;
                titleText.alignment = TextAnchor.MiddleLeft;
                titleText.normal.textColor = Color.white;
                warningText = new GUIStyle(GUI.skin.GetStyle("label"));
                warningText.fontSize = 18;
                warningText.fontStyle = FontStyle.Bold;
                warningText.normal.textColor = Color.red;
                warningText.alignment = TextAnchor.MiddleCenter;
                wrapText = new GUIStyle(GUI.skin.GetStyle("label"));
                wrapText.wordWrap = true;
            }

            GUI.DrawTexture(new Rect(0, 0, maxSize.x, 82), header, ScaleMode.StretchToFill);
            GUI.Label(new Rect(126, 15, 115, 50), PluginInfo.version, titleText);
            GUILayout.BeginArea(new Rect(0, 85, maxSize.x, maxSize.y - 85));
            Home();
            ChangeLog();
            Support();
            Learn();
            GUILayout.EndArea();
        }

        void Home()
        {
            if (homePanel == null || !homePanel.isActive) return;
            homePanel.Begin();
            if(MenuButton(new Vector2(25, 25), changelogIcon, "What's new?", "See all new features, importand changes and bugfixes in " + PluginInfo.version))
            {
                homePanel.OpenPanel(changeLogPanel, WindowPanel.SlideDiretion.Left);
            }

            if (MenuButton(new Vector2(25, 85), learnIcon, "Get Started", "Learn how to use Dreamteck Splines in a matter of minutes."))
            {
                homePanel.OpenPanel(learnPanel, WindowPanel.SlideDiretion.Left);
            }

            if (MenuButton(new Vector2(25, 145), supportIcon, "Support", "Got a problem or a feature request? Our support is here to help!"))
            {
                homePanel.OpenPanel(supportPanel, WindowPanel.SlideDiretion.Left);
            }

            if (MenuButton(new Vector2(25, 240), rateIcon, "Rate", "If you like Dreamteck Splines, please consider rating it on the Asset Store"))
            {
                Application.OpenURL("http://u3d.as/sLk");
            }


            GUI.Label(new Rect(25, 320, 400, 80), "This window will not show automatically again. If you need it, you can always open it by going to Window->Dreamteck->Splines->Plugin Info", wrapText);

            if (GUI.Button(new Rect(350, 360, 70, 35), "Close")) Close();
            Repaint();
            homePanel.End();
        }

        void Support()
        {
            if (!supportPanel.isActive) return;
            supportPanel.Begin();
            supportPanel.BackButton(homePanel, WindowPanel.SlideDiretion.Right);
            GUILayout.BeginArea(new Rect(25, 65, 400, 300));
            if (mailSent)
            {
                GUILayout.Label("Message sent. A contact web page should have opened notifying that the message has been sent. If that hasn't happened, please write a message directly to support@dreamteck.io.", wrapText);
                if (GUILayout.Button("New Message")) mailSent = false;
            }
            else if (mailError != "")
            {
                GUILayout.Label(mailError);
                if (GUILayout.Button("OK")) mailError = "";
            }
            else
            {
                email = EditorGUILayout.TextField("Your E-mail:", email);
                senderName = EditorGUILayout.TextField("Your name:", senderName);
                subject = EditorGUILayout.TextField("Subject:", subject);
                GUILayout.Label("Message:");
                message = GUILayout.TextArea(message, GUILayout.MinHeight(60));
                GUILayout.Label("The message is limited to 200 characters. If you want to send a longer message, contact us directly. Left: " + (200-message.Length), wrapText);
                if (message.Length > 200) message = message.Substring(0, 200);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Send", GUILayout.Height(35), GUILayout.Width(50)))
                {
                    SendMail();
                }
                GUILayout.Label("Sending to:");
                GUILayout.TextField("support@dreamteck.io", GUILayout.Height(25));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            if (GUILayout.Button("Dreamteck Website", GUILayout.Height(35))) Application.OpenURL("http://dreamteck.io");
            GUILayout.EndArea();

            Repaint();
            supportPanel.End();
        }

        void SendMail()
        {
            if (subject.Length <= 2) mailError = "ERROR: Subject is too short, please enter a valid subject";
            else if (message.Length <= 10) mailError = "ERROR: Message is too short. Please enter a valid message.";
            else if (Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$"))
            {
                string url = "http://dreamteck.io/team/contact.php?support_message=1&n=" + WWW.EscapeURL(senderName) + "&s=" + WWW.EscapeURL(subject) + "&e=" + WWW.EscapeURL(email) + "&m=" + WWW.EscapeURL(message);
                Application.OpenURL(url);
                mailError = "";
                message = subject = "";
                mailSent = true;
            } else mailError = "ERROR: Invalid e-mail address. Plase provide your e-mail address so we can contact you.";
           
        }

        void Learn()
        {
            if (!learnPanel.isActive) return;
            learnPanel.Begin();
            learnPanel.BackButton(homePanel, WindowPanel.SlideDiretion.Right);

            if (MenuButton(new Vector2(25, 40), videoIcon, "Video Tutorials", "Watch a series of Youtube videos to get started."))
            {
                Application.OpenURL("https://www.youtube.com/playlist?list=PLkZqalQdFIQ4S-UGPWCZTTZXiE5MebrVo");
            }

            if (MenuButton(new Vector2(25, 100), pdfIcon, "User Manual", "Read a thorough documentation of the whole package."))
            {
                Application.OpenURL("http://dreamteck.io/page/dreamteck_splines/user_manual.pdf");
            }

            if (MenuButton(new Vector2(25, 160), pdfIcon, "API Reference", "A documentation of the programmers' part of the package."))
            {
                Application.OpenURL("http://dreamteck.io/page/dreamteck_splines/api_reference.pdf");
            }
            Repaint();
            learnPanel.End();
        }

        void ChangeLog()
        {
            if (changeLogPanel == null || !changeLogPanel.isActive) return;
            changeLogPanel.Begin();
            changeLogPanel.BackButton(homePanel, WindowPanel.SlideDiretion.Right);
            scroll = GUILayout.BeginScrollView(scroll, GUILayout.Width(maxSize.x), GUILayout.MaxHeight(400));
            GUILayout.Label("VERSION " + PluginInfo.version + " WARNING", warningText, GUILayout.Width(maxSize.x - 30), GUILayout.Height(30));
            string text = "The SplineProjector comopnent has been upgraded and inherited from a new base class called the SplineTracer. Due to this, the motion settings of all the spline projector components in your scenes might get reverted. Look up all of your SplineProjector components and in the Transform section, make sure that position and rotation is applied if your SplineProjectors don't work after the update.";
            EditorGUILayout.LabelField(text, wrapText, GUILayout.Width(maxSize.x - 30));
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(changelogText, wrapText, GUILayout.Width(maxSize.x - 30));
            GUILayout.EndScrollView();
            Repaint();
            changeLogPanel.End();
        }

        bool MenuButton(Vector2 position, Texture2D image, string title, string description)
        {
            bool result = false;
            Rect rect = new Rect(position.x, position.y, 400, 50);
            Color buttonColor = Color.clear;
            if (rect.Contains(Event.current.mousePosition)) buttonColor = Color.white;
            GUI.BeginGroup(rect);
            GUI.color = buttonColor;
            if (GUI.Button(new Rect(0, 0, 400, 50), "")) result = true;
            GUI.color = Color.white;
            if(image != null) GUI.DrawTexture(new Rect(0, 0, 50, 50), image, ScaleMode.StretchToFill);
            GUI.Label(new Rect(60, 5, 370 - 65, 16), title, buttonTitleText);
            GUI.Label(new Rect(60, 20, 370 - 65, 40), description, wrapText);
            GUI.EndGroup();
            return result;
        }
    }
#endif
}
