using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIManager : MonoBehaviour
{
    public UIDocument main;

    private int using_tool_index;
    private List<Button> buttons;
    // Start is called before the first frame update
    void Start()
    {
        buttons = new List<Button>();
        main.rootVisualElement.Query("ToolboxList").Children<Button>().ForEach(buttons, (button)=>button);
        using_tool_index = 0;
        buttons[using_tool_index].AddToClassList("Using-Tool");
    }

    public void change_tool(int index)
    {
        buttons[using_tool_index].RemoveFromClassList("Using-Tool");
        buttons[index].AddToClassList("Using-Tool");
        using_tool_index = index;
    }
    public void change_health(int now_value, int max_value)
    {
        VisualElement HP_fore = main.rootVisualElement.Query("HP_fore");
        Label HP_label = (Label)main.rootVisualElement.Query("HP_label");
        HP_fore.style.width = Length.Percent(100f * now_value / max_value);
        HP_label.text = now_value.ToString() + '/' + max_value.ToString();
    }
}
