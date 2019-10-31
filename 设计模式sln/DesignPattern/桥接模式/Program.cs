using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 桥接模式
{
    class Program
    {
        static void Main(string[] args)
        {

            #region old
            //HandsetBrand ab;
            //ab = new HandsetBrandM();
            //ab.SetHandsetSoft(new HandsetGame());
            //ab.Run();
            //ab.SetHandsetSoft(new HandsetAddressList());
            //ab.Run();
            //ab = new HandsetBrandN();
            //ab.SetHandsetSoft(new HandsetGame());
            //ab.Run(); 
            #endregion

            HandsetBrand brand;
            brand = new HandsetBrandNokia();
            brand.SetHandsetSoft(new HandsetSoftWechat());
            brand.Run();
            Console.ReadKey();
        }
    }
//手机品牌抽象类
    abstract class HandsetBrand
    {
        public HandsetSoft soft;
        public void SetHandsetSoft(HandsetSoft soft)
        {
            this.soft = soft;
        }
        public abstract void Run();
    }
    //手机品牌Nokia
    class HandsetBrandNokia:HandsetBrand
    {
        public override void Run()
        {
            soft.Run();
        }
    }
    //手机品牌HTC
    class HandsetBrandHTC:HandsetBrand
    {
        public override void Run()
        {
            soft.Run();
        }
    }
    //手机软件抽象类
    abstract class HandsetSoft
    {
       public abstract void Run();
    }
    //手机软件wechat
    class HandsetSoftWechat:HandsetSoft
    {
        public override void Run()
        {
            Console.WriteLine("启动微信");
        }
    }
    class HandsetSoftQQ:HandsetSoft
    {
        public override void Run()
        {
            Console.WriteLine("启动QQ");
        }
    }


    #region old
    ////手机品牌
    //abstract class HandsetBrand
    //{
    //    protected HandsetSoft soft;
    //    //设置手机软件
    //    public void SetHandsetSoft(HandsetSoft soft)
    //    {
    //        this.soft = soft;
    //    }
    //    public abstract void Run();
    //}
    ////手机品牌M
    //class HandsetBrandM : HandsetBrand
    //{
    //    public override void Run()
    //    {
    //        soft.Run();
    //    }
    //}
    ////手机品牌N
    //class HandsetBrandN : HandsetBrand
    //{
    //    public override void Run()
    //    {
    //        soft.Run();
    //    }
    //}
    ////手机软件
    //abstract class HandsetSoft
    //{
    //    public abstract void Run();
    //}
    ////手机游戏
    //class HandsetGame : HandsetSoft
    //{
    //    public override void Run()
    //    {
    //        Console.WriteLine("运行手机游戏");
    //    }
    //}
    ////手机通讯录
    //class HandsetAddressList : HandsetSoft
    //{
    //    public override void Run()
    //    {
    //        Console.WriteLine("运行手机通讯录");
    //    }
    //} 
    #endregion
}

