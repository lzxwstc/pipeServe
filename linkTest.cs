using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.IO.Pipes;
using System.Net;
using System.Threading.Tasks;

namespace TESTSERVE
{
    public class linkTest
    {
        public event ConnectionMessageEventHandler ReceiveMessage;
        //private readonly AutoResetEvent _writeSignal = new AutoResetEvent(false);
        /// <summary>
        /// To support Multithread, we should use BlockingCollection.为了支持多线程，我们应该使用BlockingCollection
        /// </summary>
        //private readonly BlockingCollection<string> _writeQueue = new BlockingCollection<string>();
        public NamedPipeServerStream serveTable;

        public void linkEntrance()
        {
            //Task.Run(() =>
            //{
            //    NamedPipeServerStream serve = new NamedPipeServerStream("haha", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.WriteThrough, 0, 0);
            //    {
            //        serve.WaitForConnection();
            //        testServe(serve);
            //    } while (false) ;
            //});

            Task.Run(() =>
            {
                serveTable = new NamedPipeServerStream("tabletest", PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.WriteThrough, 0, 0);
                {
                    serveTable.WaitForConnection();
                    testServeTable(serveTable);
                } while (false) ;
            });

        }

        private void testServeTable(NamedPipeServerStream serve)
        {
            Task read = new Task(() =>
            {

                while (serve.CanRead)
                {
                    ArrayList listjson = new ArrayList();
                    int len = ReadLength(serve);
                    byte[] data = new byte[len];
                    serve.Read(data, 0, len);
                    string date = "2019-1";
                    string name = "卢";
                    string address = "浙江";
                    DateTime time = System.DateTime.Now;
                    for (var i = 0; i < 2000; i++)
                    {
                        var obj = new JObject {
                        {"date",date},{"name",name},{"address",address},{"time",time} };
                        listjson.Add(obj);
                    }
                    string json = JsonConvert.SerializeObject(listjson);//将JSON对象转化为字符串
                    byte[] bytes = System.Text.Encoding.UTF8.GetBytes(json);//接受后立马发送返回信息的测试
                    serve.Write(bytes, 0, bytes.Length);
                    serve.Flush();
                }
            });
            read.Start();
            //serve.Disconnect();
        }
        //private void testServe(NamedPipeServerStream serve)
        //{
        //    var i = 1;
        //    Task.Run(() =>
        //    {
        //        while (serve.CanWrite)
        //        {
        //            string fromQueue = _writeQueue.Take();
        //            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(fromQueue);
        //            serve.Write(bytes, 0, bytes.Length);
        //            serve.Flush();
        //        }
        //    });
        //    Task read = new Task(() =>
        //    {

        //        while (serve.CanRead)
        //        {

        //            int len = ReadLength(serve);
        //            byte[] data = new byte[len];
        //            serve.Read(data, 0, len);
        //            var obj = System.Text.Encoding.UTF8.GetString(data);

        //            ReceiveMessage(obj);

        //            byte[] bytes = System.Text.Encoding.UTF8.GetBytes((i++).ToString());//接受后立马发送返回信息的测试
        //            serve.Write(bytes, 0, bytes.Length);
        //            serve.Flush();
        //        }
        //    });
        //    read.Start();
        //    //serve.Disconnect();
        //}

        private int ReadLength(NamedPipeServerStream serve)
        {
            const int lensize = sizeof(int);
            var lenbuf = new byte[lensize];
            var bytesRead = serve.Read(lenbuf, 0, lensize);
            if (bytesRead == 0)
            {
                return 0;
            }
            return IPAddress.NetworkToHostOrder(BitConverter.ToInt32(lenbuf, 0));
        }
        //按下send后触发
        public void PushMessage(string message)
        {
            //string str = message;
            //_writeQueue.Add(message);
            //_writeSignal.Set();
        }


        public delegate void ConnectionMessageEventHandler(string message);
    }
}
