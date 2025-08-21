using UnityEngine;
using UnityEngine.UI;

public class DieUI : MonoBehaviour
{
    public Text valueText;
    public Image background;
    public Button button;

    public int Value { get; private set; }
    public bool Held { get; private set; }

    Color baseColor;
    public Color heldColor = new Color(0.85f, 0.9f, 1f);

    void Awake()
    {
        baseColor = background ? background.color : Color.white;
        if (button) button.onClick.AddListener(ToggleHold);
        SetValue(0);
        SetHeld(false);
    }

    public void SetHeld(bool h)
    {
        Held = h;
        if (background) background.color = Held ? heldColor : baseColor;
    }

    public void ToggleHold()
    {
        SetHeld(!Held);
    }

    public void Roll()
    {
        if (Held) return;
        SetValue(Random.Range(1, 7));
    }

    public void ResetDie()
    {
        SetHeld(false);
        SetValue(0);
    }

    void SetValue(int v)
    {
        Value = v;
        if (valueText) valueText.text = v == 0 ? "-" : v.ToString();
    }
}
