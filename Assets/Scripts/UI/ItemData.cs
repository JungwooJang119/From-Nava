using UnityEngine;

public interface ItemData { }
public class ScriptableItem : ScriptableObject, ItemData { }

public class CollectibleItem : ScriptableItem {
	[Tooltip("Sprite associated with the item.")]
	public Sprite sprite;

	[TextArea]
	[Tooltip("Text displayed beneath the item.")]
	public string text;
}