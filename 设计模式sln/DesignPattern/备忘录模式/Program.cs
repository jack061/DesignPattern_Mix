using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 备忘录模式
{
    class Program
    {
        static void Main(string[] args)
        {
            #region old
            //GameRole yanqi = new GameRole();
            //yanqi.GetInitState();
            //yanqi.StateDisplay();
            ////保存进度
            //RolesStateCaretaker stateAdmin = new RolesStateCaretaker();
            //stateAdmin.Memento = yanqi.SaveState();
            //yanqi.Fight();
            //yanqi.StateDisplay();
            ////恢复之前状态
            //yanqi.RecoveryState(stateAdmin.Memento);
            //yanqi.StateDisplay(); 
            #endregion

            GameRole lbj = new GameRole();
            RolesStateCareTaker taker = new RolesStateCareTaker();
            lbj.GetInitState();
            //保存角色状态
            taker.Memento = lbj.SaveState();
            lbj.Fight();
            lbj.RecoveryState(taker.Memento);
            Console.ReadKey();
        }
    }
    class GameRole
    {
        public int Activatity;
        public int Defence;
        //设置初始状态
        public void GetInitState()
        {
            this.Activatity = 100;
            this.Defence = 100;
        }
        //保存角色状态
        public RolesStateMemento SaveState()
        {
            return new RolesStateMemento(Activatity, Defence);
        }
        //恢复角色状态
        public void RecoveryState(RolesStateMemento memento)
        {
            this.Activatity = memento.Activatity;
            this.Defence = memento.Defence;
        }
        //战斗
        public void Fight()
        {
            this.Activatity = 0;
            this.Defence = 0;
        }


    }
    //角色状态存储箱
    class RolesStateMemento
    {
        public int Activatity;
        public int Defence;
        public RolesStateMemento(int Activatity, int Defence)
        {
            this.Activatity = Activatity;
            this.Defence = Defence;
        }


    }
    //角色状态管理者
    class RolesStateCareTaker
    {
        public RolesStateMemento Memento { get; set; }
    }

    #region old
    //class GameRole
    //{
    //    private int vit;
    //    private int atk;
    //    private int def;
    //    public int Vit
    //    {
    //        get { return vit; }
    //        set { vit = value; }
    //    }
    //    public int Atk
    //    {
    //        get { return atk; }
    //        set { atk = value; }
    //    }
    //    public int Def
    //    {
    //        get { return def; }
    //        set { def = value; }
    //    }
    //    //状态显示
    //    public void StateDisplay()
    //    {
    //        Console.WriteLine("角色当前状态:");
    //        Console.WriteLine("体力:{0}",this.vit);
    //        Console.WriteLine("攻击力:{0}", this.vit);
    //        Console.WriteLine("防御力:{0}", this.vit);

    //    }
    //    //保存角色状态
    //    public RoleStateMemento SaveState()
    //    {
    //        return new RoleStateMemento(vit, atk, def);
    //    }
    //    //恢复角色状态
    //    public void RecoveryState(RoleStateMemento memento)
    //    {
    //        this.vit = memento.Vit;
    //        this.atk = memento.Atk;
    //        this.def = memento.Def;
    //    }
    //    //获得初始状态
    //    public void GetInitState()
    //    {
    //        this.vit = 100;
    //        this.atk = 100;
    //        this.def = 100;
    //    }
    //    //战斗
    //    public void Fight()
    //    {
    //        this.vit = 0;
    //        this.atk = 0;
    //        this.def = 0;
    //    }

    //}
    ////角色状态管理者
    //class RolesStateCaretaker
    //{
    //    private RoleStateMemento memento;
    //    public RoleStateMemento Memento
    //    {
    //        get { return memento; }
    //        set { memento = value; }
    //    }
    //}
    ////角色状态存储箱
    //class RoleStateMemento
    //{
    //    private int vit;
    //    private int atk;
    //    private int def;
    //    public RoleStateMemento(int vit, int atk, int def)
    //    {
    //        this.vit = vit;
    //        this.atk = atk;
    //        this.def = def;
    //    }
    //    public int Vit
    //    {
    //        get { return vit; }
    //        set { vit = value; }
    //    }
    //    public int Atk
    //    {
    //        get { return atk; }
    //        set { atk = value; }
    //    }
    //    public int Def
    //    {
    //        get { return def; }
    //        set { def = value; }
    //    }
    //} 
    #endregion
}
