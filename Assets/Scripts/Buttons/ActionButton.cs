using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{

    public IUseable MyUseable { get; set; }

    public Button MyButton { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        MyButton = GetComponent<Button>();

        MyButton.onClick.AddListener(OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick()
    {
        if (MyUseable != null)
        {
            MyUseable.Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }
}
