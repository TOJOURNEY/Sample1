using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

/// <summary>
/// UI窗口的基类
/// </summary>
public class WindowRoot : MonoBehaviour
{
    /// <summary> 
    /// Root节点
    /// </summary>
    protected GameRoot root;
    /// <summary>
    /// 网络管理服务
    /// </summary>
    protected NetSvc netSvc;
    /// <summary>
    /// 资源管理服务
    /// </summary>
    protected ResSvc resSvc;
    /// <summary>
    /// 音频管理服务
    /// </summary>
    protected AudioSvc audioSvc;
    /// <summary>
    /// 激活界面
    /// </summary>
    /// <param name="isActive"></param>
    public virtual void SetWndState(bool isActive=true)
    {
        if(gameObject.activeSelf!=isActive)
        {
            gameObject.SetActive(isActive);
        }
        if(isActive)
        {
            InitWnd();
        }
        else
        {
            UninitWnd();
        }
    }
    /// <summary>
    /// 初始化
    /// </summary>
    protected virtual void InitWnd()
    {
        root = GameRoot.Instance;
        netSvc = NetSvc.Instance;
        resSvc = ResSvc.Instance;
        audioSvc = AudioSvc.Instance; 
    }
    /// <summary>
    /// 反初始化
    /// </summary>
    protected virtual void UninitWnd()
    {
        root = null;
        netSvc = null;
        resSvc = null;
        audioSvc = null;
    }
    protected void SetActive(GameObject go,bool state=true)
    {
        go.SetActive(state);
    }
    protected void SetActive(Transform trans, bool state = true)
    {
        trans.gameObject.SetActive(state);
    }
    protected void SetActive(RectTransform rectTrans, bool state = true)
    {
        rectTrans.gameObject.SetActive(state);
    }
    protected void SetActive(Image img, bool state = true)
    {
        img.gameObject.SetActive(state);
    }
    protected void SetActive(Text txt, bool state = true)
    {
        txt.gameObject.SetActive(state);
    }
    protected void SetActive(InputField ipt, bool state = true)
    {
        ipt.gameObject.SetActive(state);
    }


    protected void SetText(Transform trans, int num = 0)
    {
        SetText(trans.GetComponent<Text>(), num.ToString());
    }
    protected void SetText(Transform trans, string context = "")
    {
        SetText(trans.GetComponent<Text>(),context);
    }
    protected void SetText(Text txt, int num = 0)
    {
        SetText(txt, num.ToString());
    }
    protected void SetText(Text txt,string context="")
    {
        txt.text = context;
    }
    protected void SetSprite(Image image,string path)
    {
        Sprite sp = ResSvc.Instance.LoadSprite(path, true);
        image.sprite = sp;
    }



    protected Transform GetTrans(Transform trans,string name)
    {
        if(trans!=null)
        {
            return trans.Find(name);
        }
        else
        {
            return transform.Find(name);
        }
    }
    protected Image GetImage(Transform trans,string path)
    {
        if(trans!=null)
        {
            return trans.Find(path).GetComponent<Image>() ;
        }
        else
        {
            return transform.Find(path).GetComponent<Image>();
        }
    }
    protected Image GetImage(Transform trans)
    {
        if (trans != null)
        {
            return trans.GetComponent<Image>();
        }
        else
        {
            return transform.GetComponent<Image>();
        }
    }

    protected Text GetText(Transform trans, string path)
    {
        if (trans != null)
        {
            return trans.Find(path).GetComponent<Text>();
        }
        else
        {
            return transform.Find(path).GetComponent<Text>();
        }
    }
    protected Text GetText(Transform trans)
    {
        if (trans != null)
        {
            return trans.GetComponent<Text>();
        }
        else
        {
            return transform.GetComponent<Text>();
        }
    }

    private T GetOrAddComponent<T>(GameObject go) where T:Component
    {
        T t = go.GetComponent<T>();
        if(t==null)
        {
            t = go.AddComponent<T>();
        }
        return t;
    }
    protected void OnClick(GameObject go, Action<PointerEventData, object[]> clickCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClick = clickCB;
        if (args != null)
        {
            listener.args = args;
        }
    }
    protected void OnClickDown(GameObject go, Action<PointerEventData, object[]> clickDownCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickDown = clickDownCB;
        if (args != null)
        {
            listener.args = args;
        }
    }
    protected void OnClickUp(GameObject go, Action<PointerEventData, object[]> clickUpCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onClickUp = clickUpCB;
        if (args != null)
        {
            listener.args = args;
        }
    }
    protected void OnDrag(GameObject go, Action<PointerEventData, object[]> dragCB, params object[] args)
    {
        PEListener listener = GetOrAddComponent<PEListener>(go);
        listener.onDrag = dragCB;
        if (args != null)
        {
            listener.args = args;
        }
    }

    protected MonoTimer CreateMonoTimer(
        Action<int> cbAction,
        float intervelTime,
        int loopCount = 1,
        Action<bool, float, float> prgAction = null,
        Action endAction = null,
        float delayTime = 0
        ){
        MonoTimer timer = new MonoTimer(
        cbAction,
        intervelTime,
        loopCount,
        prgAction,
        endAction,
        delayTime);
        root.AddMonoTimer(timer);
        return timer;
    }
}
