using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 事件EventHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            MainManger m = new MainManger();
            Fax x = new Fax(m);
            m.SimulateNewMail("lbj", "messi", "bas");
            Console.ReadKey();
        }
    }
    class NewMailEventArgs : EventArgs
    {
        private readonly string m_from, m_to, m_subject;
        public NewMailEventArgs(string from,string to,string subject)
        {
            this.m_from = from;
            this.m_to = to;
            this.m_subject = subject;
        }
        public string From { get { return m_from; } }
        public string To { get { return m_to; } }
        public string Subject { get { return m_subject; } }
    }
    class MainManger
    {
        public event EventHandler<NewMailEventArgs> NewMail;
        public void SimulateNewMail(string from, string to, string subject)
        {
            NewMailEventArgs e = new NewMailEventArgs(from, to, subject);
            NewMail(this, e);
        }
    }

    sealed class Fax
    {
        public Fax(MainManger mm)
        {
            mm.NewMail += mm_NewMail;
        }

        void mm_NewMail(object sender, NewMailEventArgs e)
        {
            Console.WriteLine("Faxing mail message:From:{0},To:{1},Subject:{2}",e.From,e.To,e.Subject);
        }
    }
}
