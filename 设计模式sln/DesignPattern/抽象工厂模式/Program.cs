using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 抽象工厂模式
{
    class Program
    {
        static void Main(string[] args)
        {
            IFacotory factory = (IFacotory)AppConfigHelper.GetFactoryInstance();
            User u=new User();
            u.Name="燕七";
            u.id=2;
            Department d=new Department();
            d.depName="英雄山庄";
            d.id=1;
            if (factory==null)
            {
                Console.WriteLine("读取内容失败");
            }
            IUser user = factory.GetUser();
            user.Insert(u);
            user.GetUser(2);
            IDepartment dep = factory.GetDepartment();
            dep.Insert(d);
            dep.GetDepartment(1);
            ICompany company = factory.GetCompany();
            company.Insert();
            company.GetCompany();
            IUser user2 = DataAccess.GetUser();
            user2.Insert(u);
            user2.GetUser(2);
            IDepartment dep2 = DataAccess.GetDepartment();
            dep2.Insert(d);
            dep2.GetDepartment(1);
            Console.ReadKey();

        }
    }
    class User
    {
        public int id { get; set; }
        public string Name { get; set; }
    }
    class Department 
    {
        public int id { get; set; }
        public string depName { get; set; }
    }
    interface IUser
    {
        void Insert(User user);
        User GetUser(int id);
    }
    interface IDepartment
    {
        void Insert(Department dep);
        Department GetDepartment(int id);
    }
    interface ICompany
    {
        void Insert();
        void GetCompany();
    }

    #region sqlserver
    class SqlServerUser : IUser
    {

        public void Insert(User user)
        {
            Console.WriteLine("在sqlserver中向user表中插入一条记录");
        }

        public User GetUser(int id)
        {
            Console.WriteLine("在sqlserver中根据ID得到User表中一条记录");
            return null;
        }

    }
    class SqlServerDepartment : IDepartment
    {
        public void Insert(Department dep)
        {
            Console.WriteLine("在sqlserver中向department表中插入一条记录");
        }

        public Department GetDepartment(int id)
        {
            Console.WriteLine("在sqlserver中根据id获得Department表中一条记录");
            return null;
        }
    }

    class SqlServerCompany : ICompany
    {
        public void Insert()
        {
            Console.WriteLine("在sqlserver中向Company表中插入数据");
        }

        public void GetCompany()
        {
            Console.WriteLine("在sqlserver中获取company");
        }
    }

    #endregion

    #region Access
    class AccessUser : IUser
    {
        public void Insert(User user)
        {
            Console.WriteLine("在Access中向User表中插入一条记录");
        }

        public User GetUser(int id)
        {
            Console.WriteLine("在Access中根据Id查询一条记录");
            return null;
        }
    }
    class AccessDepartment : IDepartment
    {
        public void Insert(Department dep)
        {
            Console.WriteLine("在Access中向Department表中插入一条记录");
        }

        public Department GetDepartment(int id)
        {
            Console.WriteLine("在Access中根据Id查询一条记录");
            return null;
        }
    }

    class AccessCompany : ICompany
    {
        public void Insert()
        {
            Console.WriteLine("在Access中向Company表中插入数据");
        }

        public void GetCompany()
        {
            Console.WriteLine("在Access中获取company");
        }
    }

    #endregion

    #region Orical
    class OricalUser : IUser
    {

        public void Insert(User user)
        {
            Console.WriteLine("在Orical中向User表中插入一条数据");
        }

        public User GetUser(int id)
        {
            Console.WriteLine("在Orical中根据id获取user表数据");
            return null;
        }
    }

    class OricalDepartment : IDepartment
    {
        public void Insert(Department dep)
        {
            Console.WriteLine("在Orical中向Department表中插入一条数据");
        }

        public Department GetDepartment(int id)
        {
            Console.WriteLine("在Orical中根据id获取department表中数据");
            return null;
        }
    }

    class OricalCompany : ICompany
    {
        public void Insert()
        {
            Console.WriteLine("在Orical中向Company表中插入数据");
        }

        public void GetCompany()
        {
            Console.WriteLine("在Orical中获取company");
        }
    }
    #endregion

    #region mysql
    class MySqlUser : IUser
    {
        public void Insert(User user)
        {
            Console.WriteLine("在mysql中向User表中插入一条记录");
        }

        public User GetUser(int id)
        {
            Console.WriteLine("在mysql中根据Id查询一条记录");
            return null;
        }
    }
    class MySqlDepartment : IDepartment
    {
        public void Insert(Department dep)
        {
            Console.WriteLine("在mysql中向Department表中插入一条记录");
        }

        public Department GetDepartment(int id)
        {
            Console.WriteLine("在mysql中根据Id查询一条记录");
            return null;
        }
    }

    class MySqlCompany : ICompany
    {
        public void Insert()
        {
            Console.WriteLine("在mysql中向Company表中插入数据");
        }

        public void GetCompany()
        {
            Console.WriteLine("在mysql中获取company");
        }
    }



    #endregion

    interface IFacotory
    {
        IUser GetUser();
        IDepartment GetDepartment();
        ICompany GetCompany();
    }
    class SqlserverFactory : IFacotory
    {
        public IUser GetUser()
        {
            return new SqlServerUser();
        }

        public IDepartment GetDepartment()
        {
            return new SqlServerDepartment();
        }


        public ICompany GetCompany()
        {
            return new SqlServerCompany();
        }
    }
    class AccessFactory : IFacotory
    {
        public IUser GetUser()
        {
            return new AccessUser();
        }

        public IDepartment GetDepartment()
        {
            return new AccessDepartment();
        }


        public ICompany GetCompany()
        {
            return new AccessCompany();
        }
    }
    class OricalFactory : IFacotory
    {
        public IUser GetUser()
        {
            return new OricalUser();
        }

        public IDepartment GetDepartment()
        {
            return new OricalDepartment();
        }


        public ICompany GetCompany()
        {
            return new OricalCompany();
        }
    }

    class MySqlFactory : IFacotory
    {
        public IUser GetUser()
        {
            return new MySqlUser();
        }

        public IDepartment GetDepartment()
        {
            return new MySqlDepartment();
        }

        public ICompany GetCompany()
        {
            return new  MySqlCompany();
        }
    }


    class DataAccess
    {
        private static readonly string AssemblyName = "抽象工厂模式";
        private static readonly string db = ConfigurationManager.AppSettings["DB"];
        public static IUser GetUser()
        {
            string className = AssemblyName +"."+ db+"User";
            Type type = Type.GetType(className);
            var instance = Activator.CreateInstance(type);
            return (IUser)instance;
        }
        public static IDepartment GetDepartment()
        {
            string className = AssemblyName + "." + db + "Department";
            Type type = Type.GetType(className);
            var instance = Activator.CreateInstance(type);
            return (IDepartment)instance;
        }
        public static ICompany GetCompany()
        {
            string className = AssemblyName + "." + db + "Company";
            Type type = Type.GetType(className);
            var instance = Activator.CreateInstance(type);
            return (ICompany)instance;
        }
    }







}
