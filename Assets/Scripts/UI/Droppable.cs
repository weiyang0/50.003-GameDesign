using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Droppable : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image containerImage;
    private Color normalColor;
    public Color highlightColor = Color.yellow;
    public GameObject[] fruits;
    public Dictionary<string, GameObject> items = new Dictionary<string, GameObject>();

    public void Start()
    {

        foreach (GameObject fruit in fruits)
        {
            items.Add(fruit.name, fruit);
        }
        
    }

    public void OnEnable()
    {
        if (containerImage != null)
            normalColor = containerImage.color;
    }

    public void OnDrop(PointerEventData data)
    {
        containerImage.color = normalColor;

        Sprite dropSprite = GetDropSprite(data);

        if (dropSprite != null)
        {
            foreach (Transform child in gameObject.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            GameObject instance = Instantiate(items[data.pointerDrag.name], new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            instance.transform.SetParent(gameObject.transform);
            instance.transform.parent = gameObject.transform;
        }
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (containerImage == null)
            return;

        Sprite dropSprite = GetDropSprite(data);
        if (dropSprite != null)
            containerImage.color = highlightColor;
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (containerImage == null)
            return;

        containerImage.color = normalColor;
    }

    private Sprite GetDropSprite(PointerEventData data)
    {
        var originalObj = data.pointerDrag;
        if (originalObj == null)
            return null;

        var dragMe = originalObj.GetComponent<Draggable>();
        if (dragMe == null)
            return null;

        var srcImage = originalObj.GetComponent<Image>();
        if (srcImage == null)
            return null;

        return srcImage.sprite;
    }
}
