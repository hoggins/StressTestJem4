using UnityEditor;

[CustomEditor(typeof(CatPlacer))]
public class CatPlacerEditor : Editor
{
  public override void OnInspectorGUI()
  {
    base.OnInspectorGUI();

    var placer = (CatPlacer)target;

    var newLevel = EditorGUILayout.IntField("Set level", placer.Level);
    if (newLevel != placer.Level)
    {
      if (newLevel < placer.Level)
        placer.RemoveLayer(newLevel);
      else
        placer.AddLayer(newLevel);
    }

    //StartCoroutine(Fade());
  }
}