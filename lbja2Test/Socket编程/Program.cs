using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Socket编程
{
    class Program
    {

        static void Main(string[] args)
        {
            #region 服务器端多链接
            //    //创建连接对象
            //    Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //    //创建IP对象
            //    IPAddress address = IPAddress.Parse("10.179.54.163");
            //    //创建端口和ip对象
            //    IPEndPoint point = new IPEndPoint(address, 8888);
            //    //绑定
            //    sk.Bind(point);
            //    //开始监听
            //    sk.Listen(10);
            //    Console.WriteLine("开始监听....");
            //    while (true)
            //    {
            //        Socket skAccept = sk.Accept();
            //        Console.WriteLine("客户端{0}连接成功", skAccept.RemoteEndPoint.ToString());
            //        //开启线程接收多个连接对象
            //        Thread t = new Thread(() =>
            //        {
            //            byte[] buffer = new byte[1024 * 1024 * 10];
            //            while (true)
            //            {
            //                int count = skAccept.Receive(buffer);
            //                if (count > 0)
            //                {
            //                    string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //                    Console.WriteLine("客户端{0}发送的数据为{1}", skAccept.RemoteEndPoint.ToString(), msg);
            //                }

            //            }
            //        });
            //        t.IsBackground = true;
            //        t.Start();
            //    }
            #endregion

            #region test2
            ////创建socket对象
            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////获取Ip地址，绑定端口
            //IPEndPoint point = new IPEndPoint(IPAddress.Parse("10.179.54.163"), 8888);
            //sk.Bind(point);
            ////开始监听
            //sk.Listen(10);
            ////多客户端链接
            //while (true)
            //{
            //    Socket skAccept = sk.Accept();
            //    Console.WriteLine("连接成功，对象为{0}", skAccept.RemoteEndPoint);
            //    //开启线程，接受多个连接对象
            //    Thread t = new Thread(() =>
            //    {
            //        byte[] buffer = new byte[1024 * 1024 * 100];
            //        while (true)
            //        {
            //            //把接受的对象赋值给buffer
            //            int count = skAccept.Receive(buffer);
            //            if (count > 0)
            //            {
            //                //把接受的对象转换为字符串
            //                string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //                Console.WriteLine("客户端{0}发送的数据为{1}", skAccept.RemoteEndPoint, msg);
            //            }
            //        }
            //    });
            //} 
            #endregion

            #region test3
            ////获取socket
            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////绑定IP和端口号
            //IPEndPoint point = new IPEndPoint(IPAddress.Parse("10.179.54.163"), 5565);
            //sk.Bind(point);
            //sk.Listen(10);
            //Console.WriteLine("开始监听.....");
            //while (true)
            //{
            //    Socket skAccept = sk.Accept();
            //    Console.WriteLine("客户端{0}连接成功", skAccept.RemoteEndPoint.ToString());
            //    //开启线程接受客户端发送数据
            //    Thread t = new Thread(() =>
            //    {
            //        byte[] buffer = new byte[1024 * 1024 * 10];
            //        int count = skAccept.Receive(buffer);
            //        if (count>0)
            //        {
            //            string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //            Console.WriteLine("客户端{0}发送的数据为{1}",skAccept.RemoteEndPoint,msg);
            //        }
            //    });
            //}
            #endregion

            #region test4
            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////绑定Ip地址和端口号
            //IPEndPoint point = new IPEndPoint(IPAddress.Parse("10.179.54.163"), 8888);
            //sk.Bind(point);
            //sk.Listen(10);
            //Console.WriteLine("开始监听............");
            //while (true)
            //{
            //    Socket skConnection = sk.Accept();
            //    Console.WriteLine("客户端{0}连接成功", skConnection.RemoteEndPoint.ToString());
            //    Thread t = new Thread(() =>
            //    {

            //        while (true)
            //        {
            //            byte[] buffer = new byte[1024 * 1024 * 10];
            //            int count = skConnection.Receive(buffer);
            //            if (count > 0)
            //            {
            //                string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //                Console.WriteLine("客户端{0}发送数据为{1}", skConnection.RemoteEndPoint, msg);
            //            }
            //        }
            //    });
            //    t.IsBackground = true;
            //    t.Start();

            //}
            #endregion

            #region test5
            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////绑定ip地址，端口号
            //IPEndPoint point = new IPEndPoint(IPAddress.Parse("192.168.1.102"), 8888);
            //sk.Bind(point);
            //sk.Listen(10);
            //Console.WriteLine("开始监听。。。");
            //while (true)
            //{
            //    Socket skConnection = sk.Accept();
            //    Console.WriteLine("客户端{0}已连接", skConnection.RemoteEndPoint.ToString());
            //    //开启线程，接受多链接
            //    Thread t = new Thread(() =>
            //    {
            //        while (true)
            //        {
            //            byte[] buffer = new byte[1024 * 1024 * 10];
            //            int count = skConnection.Receive(buffer);
            //            if (count > 0)
            //            {
            //                string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //                Console.WriteLine("客户端{0}发送的消息为{1}", skConnection.RemoteEndPoint.ToString(), msg);
            //            }
            //        }
            //    });
            //    t.IsBackground = true;
            //    t.Start();
            //}
            #endregion

            #region test6
            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            ////绑定ip地址，端口号
            //IPEndPoint point = new IPEndPoint(IPAddress.Parse("192.168.0.112"), 8888);
            //sk.Bind(point);
            //sk.Listen(10);
            //Console.WriteLine("开始监听。。。");
            //ThreadPool.QueueUserWorkItem(new WaitCallback(skCommuObj =>
            //{
            //    Socket skCon = skCommuObj as Socket;

            //    while (true)
            //    {
            //        Socket skConnection = skCon.Accept();
            //        Console.WriteLine("客户端{0}已连接", skConnection.RemoteEndPoint);

            //        //开启另一个线程，循环接受用户传递的信息
            //        ThreadPool.QueueUserWorkItem(new WaitCallback((skWhile) =>
            //        {
            //            while (true)
            //            {
            //                Socket skinter = skWhile as Socket;
            //                byte[] buffer = new byte[1024 * 1024 * 10];
            //                int count = skinter.Receive(buffer);
            //                if (count > 0)
            //                {
            //                    string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //                    Console.WriteLine("客户端{0}发送消息为{1}", skinter.RemoteEndPoint, msg);
            //                }
            //            }

            //        }), skConnection);

            //    }
            //}), sk);
            //Console.ReadKey();


            #endregion

            #region test7

            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //sk.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.112"), 9999));
            //sk.Listen(10);
            //Console.WriteLine("开始监听。。。");
            ////通过循环不断接受用户的连接请求
            //while (true)
            //{
            //    //阻塞线程，等待用户的连接请求
            //    Socket sk2 = sk.Accept();
            //    ThreadPool.QueueUserWorkItem(new WaitCallback(skObj =>
            //    {
            //        Socket skCommu = skObj as Socket;
            //        //获取客户端信息
            //        string userinfo = skCommu.RemoteEndPoint.ToString();

            //        //再启动一个线程来循环接收用户发来的消息
            //            while (true)
            //            {
            //                byte[] buffer = new byte[1024 * 1024 * 2];
            //                int count = skCommu.Receive(buffer);
            //                if (count<=0)
            //                {
            //                    break;
            //                }
            //                string msg = Encoding.UTF8.GetString(buffer, 0, count);
            //                Console.WriteLine("客户端{0}发送的信息为{1}", skCommu.RemoteEndPoint.ToString(), msg);

            //            }

            //    }), sk2);


            //}
            #endregion

            #region test8
            //Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //sk.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.112"), 9999));
            //sk.Listen(10);
            //Console.WriteLine("开始监听。。。。。。。。。");
            //while (true)
            //{
            //    Socket skCommu = sk.Accept();
            //    Console.WriteLine("客户端{0}已连接", skCommu.RemoteEndPoint.ToString());
            //    ThreadPool.QueueUserWorkItem(new WaitCallback((skObj) =>
            //    {
            //        Socket sk2 = skObj as Socket;
            //        while (true)
            //        {
            //            byte[] buffer = new byte[1024 * 1024 * 2];
            //            int count = sk2.Receive(buffer);
            //            if (count <= 0)
            //            {
            //                break;
            //            }
            //            string msg = Encoding.UTF8.GetString(buffer, 0, count);

            //            Console.WriteLine("客户端{0}发送消息：{1}", sk2.RemoteEndPoint.ToString(), msg);
            //        }

            //    }), skCommu);
            //}
            #endregion

            #region test9
            Socket sk = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            sk.Bind(new IPEndPoint(IPAddress.Parse("192.168.0.112"), 999));
            sk.Listen(10);
            Console.WriteLine("开始监听。。。。。");
            while (true)
            {
                ThreadPool.QueueUserWorkItem(new WaitCallback((sk3) =>
                {
                    Socket sk0 = sk3 as Socket;
                    Socket skConn = sk0.Accept();
                    Console.WriteLine("客户端{0}已连接", skConn.RemoteEndPoint.ToString());
                    ThreadPool.QueueUserWorkItem(new WaitCallback((skObj) =>
                    {
                        Socket sk2 = skObj as Socket;
                        while (true)
                        {
                            byte[] buffer = new byte[1024 * 1024 * 2];
                            int r = sk2.Receive(buffer);
                            string msg = Encoding.UTF8.GetString(buffer, 0, r);
                            Console.WriteLine("客户端{0}发送消息为{1}", sk2.RemoteEndPoint.ToString(), msg);
                        }
                    }), skConn);
                }
                    ), sk);

            }
            #endregion
        }
    }
}