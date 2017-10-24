using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(KartController)), CanEditMultipleObjects]
public class KartControllerEditor : Editor
{
	private SerializedProperty topSpeedProp;
	private SerializedProperty accelerationProp;
	private SerializedProperty tractionProp;
	private SerializedProperty decelerationProp;

	private SerializedProperty bodyProp;
	private SerializedProperty wheelFLProp;
	private SerializedProperty wheelFRProp;
	private SerializedProperty wheelBLProp;
	private SerializedProperty wheelBRProp;
	private SerializedProperty wheelRadiusFrontProp;
	private SerializedProperty wheelRadiusBackProp;
	private SerializedProperty maxSteerAngleProp;

	private SerializedProperty steerSpeedProp;
	private SerializedProperty offRoadDragProp;
	private SerializedProperty airDragProp;
	private SerializedProperty visualOversteerProp;

	private bool showSetupControls = false;
	private bool showAdvancedControls = false;

	private Texture2D titleTexture;

	void OnEnable()
	{
		topSpeedProp = serializedObject.FindProperty("topSpeedMPH");
		accelerationProp = serializedObject.FindProperty("accelTime");
		tractionProp = serializedObject.FindProperty("traction");
		decelerationProp = serializedObject.FindProperty("decelerationSpeed");

		bodyProp = serializedObject.FindProperty("body");
		wheelFLProp = serializedObject.FindProperty("wheelFL");
		wheelFRProp = serializedObject.FindProperty("wheelFR");
		wheelBLProp = serializedObject.FindProperty("wheelBL");
		wheelBRProp = serializedObject.FindProperty("wheelBR");
		wheelRadiusFrontProp = serializedObject.FindProperty("wheelRadiusFront");
		wheelRadiusBackProp = serializedObject.FindProperty("wheelRadiusBack");
		maxSteerAngleProp = serializedObject.FindProperty("maxSteerAngle");

		steerSpeedProp = serializedObject.FindProperty("steerSpeed");
		offRoadDragProp = serializedObject.FindProperty("offRoadDrag");
		airDragProp = serializedObject.FindProperty("airDrag");
		visualOversteerProp = serializedObject.FindProperty("visualOversteerAmount");

		titleTexture = (Texture2D)Resources.Load("KRP_EditorTitle", typeof(Texture2D));
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update();

		// draw the title graphic
		Rect rect = GUILayoutUtility.GetRect(0,0);
		rect.width = titleTexture.width;
		rect.height = titleTexture.height;
		GUILayout.Space(rect.height);
		GUI.DrawTexture(rect, titleTexture);

		// basic parameters
		EditorGUILayout.PropertyField(topSpeedProp, new GUIContent("Top Speed (MPH)", "Top speed of the vehicle in miles-per-hour"));
		EditorGUILayout.Slider(accelerationProp, 0.1f, 3.0f, new GUIContent("Acceleration Time", "Time in seconds the vehicle takes to go from stationary to top speed"));
		EditorGUILayout.Slider(tractionProp, 0.0f, 1.0f, new GUIContent("Traction", "Determines the ease of handling for the vehicle"));
		EditorGUILayout.Slider(decelerationProp, 0.0f, 1.0f, new GUIContent("Deceleration Speed", "Determines how quickly the vehicle comes to a rest when thrust is released"));

		// controls for setting up the various models etc
		showSetupControls = EditorGUILayout.Foldout(showSetupControls, new GUIContent("Vehicle Setup", "Controls for setting up the wheels"));
		if(showSetupControls)
		{
			EditorGUILayout.PropertyField(bodyProp, new GUIContent("Kart Body", "Kart body object"));

			EditorGUILayout.PropertyField(wheelFLProp, new GUIContent("Front Left", "Transform to position front-left wheel"));
			EditorGUILayout.PropertyField(wheelFRProp, new GUIContent("Front Right", "Transform to position front-right wheel"));
			EditorGUILayout.PropertyField(wheelBLProp, new GUIContent("Rear Left", "Transform to position rear-left wheel"));
			EditorGUILayout.PropertyField(wheelBRProp, new GUIContent("Rear Right", "Transform to position rear-right wheel"));

			EditorGUILayout.PropertyField(wheelRadiusFrontProp, new GUIContent("Front Radius", "Front wheel radius"));
			EditorGUILayout.PropertyField(wheelRadiusBackProp, new GUIContent("Rear Radius", "Rear wheel radius"));

			EditorGUILayout.Slider(maxSteerAngleProp, 0.0f, 90.0f, new GUIContent("Max Steer Angle", "Maximum angle the wheels will turn"));
		}

		// some controls for advanced tweaking of various things
		showAdvancedControls = EditorGUILayout.Foldout(showAdvancedControls, new GUIContent("Advanced", "Advanced controls"));
		if(showAdvancedControls)
		{
			EditorGUILayout.Slider(steerSpeedProp, 0.0f, 1.0f, new GUIContent("Steering Speed", "Speed at which the vehicle turns"));
			EditorGUILayout.Slider(offRoadDragProp, 0.0f, 3.0f, new GUIContent("Off-Road Drag", "Drag mulitplier to apply when driving off-road"));
			EditorGUILayout.Slider(airDragProp, 0.0f, 1.0f, new GUIContent("Air Drag", "Drag mulitplier to apply when the vehicle is not on the ground"));
			EditorGUILayout.Slider(visualOversteerProp, 0.0f, 1.0f, new GUIContent("Visual Oversteer", "This controls how much the body of the kart is rotated when steering"));
		}

		serializedObject.ApplyModifiedProperties();
	}
}
