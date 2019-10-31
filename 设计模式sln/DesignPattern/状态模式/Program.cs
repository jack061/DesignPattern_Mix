using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 状态模式
{
    class Program
    {
        static void Main(string[] args)
        {
            #region old
            //Work emergencyProjects = new Work();
            //emergencyProjects.hour = 16;
            //emergencyProjects.WriteProgram();
            //emergencyProjects.hour =5;
            //emergencyProjects.WriteProgram();
            //emergencyProjects.hour = 15;
            //emergencyProjects.WriteProgram(); 
            #endregion

            #region old1

            //Work w = new Work();
            //w.hour = 9;
            //w.WirteProgram();
            //w.hour = 12;
            //w.WirteProgram();
            //w.hour = 15;
            //w.WirteProgram(); 
            #endregion

            Work w = new Work();
            //w.hour = 4;
            //w.WriteProgram();
            //w.hour = 9;
            //w.WriteProgram();
            //w.hour = 14;
            //w.WriteProgram();
            w.hour = 20;
            w.WriteProgram();
            Console.ReadKey();
        }
    }

    public class Work
    {
        public int hour;
        public State current;
        public Work()
        {
            current = new MorningState();
        }
        public void SetState(State state)
        {
            this.current = state;
        }
        public void WriteProgram()
        {
            current.WriteProgram(this);
        }
    }
    public abstract class State
    {
        public abstract void WriteProgram(Work w);
    }
    public class MorningState : State
    {
        public override void WriteProgram(Work w)
        {
            if (w.hour<9)
            {
                Console.WriteLine("现在时间:{0},精神百倍",w.hour);
            }
            else
            {
                w.SetState(new MoonState());
                w.WriteProgram();
            }
        }
    }
    public class MoonState : State
    {
        public override void WriteProgram(Work w)
        {
            if (w.hour<14)
            {
                Console.WriteLine("现在时间{0},开始犯困",w.hour);
            }
            else
            {
                w.SetState(new NightState());
                w.WriteProgram();
            }

        }
    }
    public class NightState : State
    {
        public override void WriteProgram(Work w)
        {
            if (w.hour<21)
            {
                Console.WriteLine("现在时间{0},开始睡觉",w.hour);
            }
        }
    }





    #region define
    //public class Work
    //{
    //    private State state { get; set; }
    //    public int hour { get; set; }
    //    public void SetState(State state)
    //    {
    //        this.state = state;
    //    }
    //    public void Run()
    //    {
    //        state.WriteProgram(this);
    //    }
    //}
    //public abstract class State
    //{
    //    public abstract void WriteProgram(Work w);
    //}
    //public class MorningState : State
    //{

    //    public override void WriteProgram(Work w)
    //    {
    //        if (w.hour < 9)
    //        {

    //            Console.WriteLine("这是早上，精神百倍");
    //        }
    //        else
    //        {
    //            w.SetState(new MoonState());
    //            w.Run();
    //        }
    //    }
    //}
    //public class MoonState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        if (w.hour < 14)
    //        {
    //            Console.WriteLine("这是中午，犯困");
    //        }
    //        else
    //        {
    //            w.SetState(new NightState());
    //            w.Run();
    //        }
    //    }
    //}
    //public class NightState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        Console.WriteLine("晚上该休息了");
    //    }
    //}

    #endregion




    #region old1
    //public abstract class State
    //{
    //    public abstract void WriteProgram(Work w);
    //}
    //public class ForenoonState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        if (w.hour < 10)
    //        {
    //            Console.WriteLine("当前时间{0} 精神百倍 工作", w.hour);
    //        }
    //        else
    //        {
    //            w.SetState(new NoonState());
    //            w.WriteProgram();
    //        }
    //    }
    //}
    //public class NoonState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        if (w.hour < 13)
    //        {
    //            Console.WriteLine("当前时间{0} 午休时间，犯困", w.hour);
    //        }
    //        else
    //        {
    //            w.SetState(new SleepingState());
    //            w.WriteProgram();
    //        }

    //    }
    //}
    //public class SleepingState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        Console.WriteLine("当前时间:{0} 点 不行了，睡着了。", w.hour);
    //    }
    //}




    //public class Work
    //{
    //    public State current;
    //    public double hour { get; set; }
    //    public Work()
    //    {
    //        this.current = new ForenoonState();
    //    }
    //    public void SetState(State s)
    //    {
    //        this.current = s;
    //    }
    //    public void WriteProgram()
    //    {
    //        current.WriteProgram(this);
    //    }
    //}

    #endregion

    #region old
    //抽象状态
    //public abstract class State
    //{
    //    public abstract void WriteProgram(Work w);
    //}
    ////上午工作状态
    //public class ForenoonState : State
    //{

    //    public override void WriteProgram(Work w)
    //    {
    //        if (w.hour < 12)
    //        {
    //            Console.WriteLine("当前时间:{0}点上午工作，精神百倍", w.hour);
    //        }
    //        else
    //        {
    //            w.SetState(new NoonState());
    //            w.WriteProgram();
    //        }
    //    }
    //}
    ////中午工作状状态
    //public class NoonState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        if (w.hour < 13)
    //        {
    //            Console.WriteLine("当前时间:{0}点，午饭；犯困，午休", w.hour);
    //        }
    //        else
    //        {
    //            w.SetState(new SleepingState());
    //            w.WriteProgram();
    //        }
    //    }
    //}
    //public class SleepingState : State
    //{
    //    public override void WriteProgram(Work w)
    //    {
    //        Console.WriteLine("当前时间:{0} 点 不行了，睡着了。", w.hour);
    //    }
    //}


    ////工作
    //public class Work
    //{
    //    private State current;
    //    public Work()
    //    {
    //        current = new ForenoonState();
    //    }
    //    public double hour { get; set; }
    //    public bool taskFinished { get; set; }
    //    public void SetState(State s)
    //    {
    //        this.current = s;
    //    }
    //    public void WriteProgram()
    //    {
    //        current.WriteProgram(this);
    //    }
    //} 
    #endregion
}
