using UnityEngine;
public enum PanelLayer{BasicUI,OverlayUI,TopUI};
//BasicUI为基础UI，如果显示基础UI需要清空栈
//OverlayUI为非全屏UI，通常由显示在画面中的一部分以及剩余部分的遮罩构成
//TopUI显示时需要覆盖前面所有UI，但是不会清空栈
public abstract class BasePanel : MonoBehaviour
{
    public PanelLayer panelLayer;
    public abstract void OnEnter();
    public abstract void OnPause();
    public abstract void OnResume();
    public abstract void OnExit(bool cache = false);
}
