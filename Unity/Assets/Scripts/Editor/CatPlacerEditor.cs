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
      {
        for (int i = placer.Level-1; i >= newLevel; i--)
        {
          placer.RemoveLayer(i);
        }
      }
      else
        for (int i = placer.Level; i < newLevel; i++)
        {
          placer.AddLayer(i);
        }
    }

    //StartCoroutine(Fade());
  }
}