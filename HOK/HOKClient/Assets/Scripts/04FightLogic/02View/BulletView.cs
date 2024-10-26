using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// 子弹显示
/// </summary>
public class BulletView : ViewUnit
{
    public override void Init(LogicUnit logicUnit)
    {
        base.Init(logicUnit);
    }

    public void DestroyBullet()
    {
        Destroy(gameObject, 0.1f);
    }
}
