using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 组合模式
{
    class Program
    {
        static void Main(string[] args)
        {
            #region old
            //ConcreteCompany root = new ConcreteCompany("北京总公司");
            //root.Add(new HRDepartment("总公司人力资源部"));
            //root.Add(new FinanceDepartment("总公司财务部"));
            //ConcreteCompany comp = new ConcreteCompany("上海华东分公司");
            //comp.Add(new HRDepartment("华东分公司人力资源部"));
            //comp.Add(new FinanceDepartment("华东分公司财务部"));
            //root.Add(comp);
            //ConcreteCompany comp1 = new ConcreteCompany("南京办事处");
            //comp1.Add(new HRDepartment("南京办事处人力资源部"));
            //comp1.Add(new FinanceDepartment("南京办事处财务部"));
            //comp.Add(comp1);
            //ConcreteCompany comp2 = new ConcreteCompany("苏州办事处");
            //comp2.Add(new HRDepartment("苏州办事处人力资源部"));
            //comp2.Add(new FinanceDepartment("苏州办事处财务部"));
            //comp.Add(comp2);
            //ConcreteCompany comp3 = new ConcreteCompany("镇江办事处");
            //comp3.Add(new HRDepartment("镇江人力资源部"));
            //comp3.Add(new FinanceDepartment("镇江财务部"));
            //comp.Add(comp3);
            //Console.WriteLine("\n结构图:");
            //root.Display(1);
            //Console.WriteLine("\n职责:");
            //root.LineOfDuty();
            #endregion

            #region old1
            //ConcreteCompany root = new ConcreteCompany("北京总公司");
            //root.Add(new HRDepartment("北京总公司人力资源部"));
            //root.Add(new FinanceDepartment("北京总公司财务部门"));
            //ConcreteCompany comp1 = new ConcreteCompany("山东分公司");
            //comp1.Add(new HRDepartment("山东分公司人力资源部"));
            //comp1.Add(new FinanceDepartment("山东分公司财务部门"));
            //ConcreteCompany comp2 = new ConcreteCompany("青岛分公司");
            //comp2.Add(new HRDepartment("青岛分公司人力资源部"));
            //comp2.Add(new FinanceDepartment("青岛分公司财务部门"));
            //comp1.Add(comp2);
            //root.Add(comp1);
            //root.Display(2); 
            #endregion

            Company root = new ConcreteCompany("北京总公司");
            root.Add(new FinancalDepartment("北京总公司财务部"));
            root.Add(new HRDepartment("北京总公司人力资源部"));
            Company root1 = new ConcreteCompany("山东分公司");
            root1.Add(new FinancalDepartment("山东分公司财务部"));
            root1.Add(new HRDepartment("山东分公司人力资源部"));
            root.Add(root1);
            root.Show(2);
            Console.ReadKey();
        }
    }
    public abstract class Company
    {
        public string name;
        public Company(string name)
        {
            this.name = name;
        }
        public List<Company> list = new List<Company>();
        public abstract void Add(Company c);
        public abstract void Remove(Company c);
        public abstract void Show(int depth);
    }
    public class ConcreteCompany : Company
    {
        public ConcreteCompany(string name):base(name)
        {

        }
        public override void Add(Company c)
        {
            list.Add(c);
        }

        public override void Remove(Company c)
        {
            list.Remove(c);
        }

        public override void Show(int depth)
        {
            Console.WriteLine(new string('-',depth)+name);
            foreach (var component in list)
            {
                component.Show(depth+2);
            }
        }
    }
    public class FinancalDepartment : Company
    {
        public FinancalDepartment(string name):base(name)
        {

        }
        public override void Add(Company c)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Company c)
        {
            throw new NotImplementedException();
        }

        public override void Show(int depth)
        {
            Console.WriteLine(new string('-',depth)+name);
        }
    }
    public class HRDepartment : Company
    {
        public HRDepartment(string name):base(name)
        {

        }
        public override void Add(Company c)
        {
            throw new NotImplementedException();
        }

        public override void Remove(Company c)
        {
            throw new NotImplementedException();
        }

        public override void Show(int depth)
        {
            Console.WriteLine(new string('-',depth)+name);
        }
    }






    #region old1
    //public abstract class Company
    //{
    //    public string name;
    //    public Company(string name)
    //    {
    //        this.name = name;
    //    }
    //    public abstract void Add(Company c);
    //    public abstract void Remove(Company c);
    //    public abstract void Display(int depth);
    //}
    //public class ConcreteCompany : Company
    //{
    //    public ConcreteCompany(string name)
    //        : base(name)
    //    {

    //    }
    //    public List<Company> children = new List<Company>();
    //    public override void Add(Company c)
    //    {
    //        children.Add(c);
    //    }

    //    public override void Remove(Company c)
    //    {
    //        children.Remove(c);
    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new string('-', depth) + name);
    //        foreach (Company component in children)
    //        {
    //            component.Display(depth + 2);
    //        }
    //    }
    //}

    //public class HRDepartment : Company
    //{
    //    public HRDepartment(string name)
    //        : base(name)
    //    {

    //    }
    //    public override void Add(Company c)
    //    {

    //    }

    //    public override void Remove(Company c)
    //    {

    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new string('-', depth) + name);
    //    }
    //}
    //public class FinanceDepartment : Company
    //{
    //    public FinanceDepartment(string name)
    //        : base(name)
    //    {

    //    }
    //    public override void Add(Company c)
    //    {

    //    }

    //    public override void Remove(Company c)
    //    {

    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new string('-', depth) + name);
    //    }
    //} 
    #endregion

    #region old
    //abstract class Company
    //{
    //    public string name;
    //    public Company(string name)
    //    {
    //        this.name = name;
    //    }
    //    public abstract void Add(Company c);
    //    public abstract void Remove(Company c);
    //    public abstract void Display(int depth);
    //    public abstract void LineOfDuty();
    //}
    //class ConcreteCompany:Company
    //{
    //    private List<Company> children = new List<Company>();
    //    public ConcreteCompany(string name):base(name)
    //    {

    //    }
    //    public override void Add(Company c)
    //    {
    //        children.Add(c);
    //    }
    //    public override void Remove(Company c)
    //    {
    //        children.Remove(c);
    //    }
    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new String('-', depth) + name);

    //        foreach (Company component in children)
    //        {
    //            component.Display(depth + 2);
    //        }
    //    }
    //    //履行职责
    //    public override void LineOfDuty()
    //    {
    //        foreach (Company component in children)
    //        {
    //            component.LineOfDuty();
    //        }
    //    }
    //}
    ////人力资源部
    //class HRDepartment:Company
    //{
    //    public HRDepartment(string name):base(name)
    //    {

    //    }
    //    public override void Add(Company c)
    //    {

    //    }
    //    public override void Remove(Company c)
    //    {

    //    }
    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new string('-',depth)+name);
    //    }
    //    public override void LineOfDuty()
    //    {
    //        Console.WriteLine("{0} 员工招聘培训管理",name);
    //    }
    //}
    ////财务部
    //class FinanceDepartment:Company
    //{
    //    public FinanceDepartment(string name):base(name)
    //    {

    //    }
    //    public override void Add(Company c)
    //    {

    //    }

    //    public override void Remove(Company c)
    //    {

    //    }

    //    public override void Display(int depth)
    //    {
    //        Console.WriteLine(new string('-', depth) + name);
    //    }

    //    public override void LineOfDuty()
    //    {
    //        Console.WriteLine("{0} 公司财务收支管理", name);
    //    }
    //} 
    #endregion
}
