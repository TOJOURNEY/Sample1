using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/// <summary>
/// buff基础表现控制(技能特效)
/// </summary>
public class BuffView:ViewUnit
{
    Buff buff;
    public override void Init(LogicUnit buff)
    {
        base.Init(buff);
        this.buff = buff as Buff;

        if (this.buff.cfg.staticPosType != StaticPosTypeEnum.None)
        {
            //固定位置buff
            transform.position = buff.LogicPos.ConvertViewVector3();
            //transform.rotation = CalcRotation(buff.LogicDir.ConvertViewVector3());
            transform.forward = buff.LogicDir.ConvertViewVector3();
        
        }
    }

    //用一个空函数覆盖位置与方向的更新
    protected override void Update() { }

    public void DestroyBuff()
    {
        Destroy(gameObject, 0.1f);
    }

    public override void PlayAni(string aniName)
    {
        throw new NotImplementedException();
    }
}
