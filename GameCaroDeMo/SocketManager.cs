using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameCaroDeMo
{
    public class SocketManager
    {
        #region Client
        //tạo 1 client
        Socket client;
        //hàm kết nối với Server

        public bool ConnectServer()
        {
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), PORT);//thêm thư viện IPEndPoint
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //thực hiện kết nối server
            try
            {
                client.Connect(iep);// tao IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), PORT);//thêm thư viện IPEndPoint
                return true;//mac dinh tra ve true
            }
            catch
            {
                return false;
            }


        }

        #endregion

        #region server
        Socket server;
        public void CreateServer()
        {
            //tạo server
            //return false;//neu dung bool, vì phải khởi tạo sever nên ko dùng bool
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), PORT);//thêm thư viện IPEndPoint
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            //ngồi lắng nghe bind()

            server.Bind(iep);//lang nghe
            server.Listen(10);//time đợi kết nối của client 10s
            //thêm thư viện Thread =>accept tạo 1 luồng riêng ko luồng hàm chính dùng method
            Thread acceptClient = new Thread(() =>
                {

                    client = server.Accept();
                });
            acceptClient.IsBackground = true;//ctr tự tắc

            acceptClient.Start();//
        }

        #endregion

        #region Both
        public string IP = "127.0.0.1";
        public int PORT = 9999;

        public const int BUFFER = 1024;
        //xác định sever
        public bool isServer = true;
        //tạo hàm dùng chung
        public bool Send(object data)
        {
            //phân tích gói data[]
            byte[] sendData = SerializeData(data);
            /* if(isServer)
             {
                 return SendData(server, sendData);
             }
             else*/
            {
                return SendData(client, sendData);
            }

        }
        //Receive()
        public object Receive()
        {
            byte[] receiveData = new byte[BUFFER];//dc tạo [BUFFER] tren
            bool isOK = ReceiveData(client, receiveData);
            return DeserializeData(receiveData);//tra ve (o)
        }

        private bool ReceiveData()
        {
            throw new NotImplementedException();
        }

        //gửi và nhận tin 2 thằng
        private bool SendData(Socket target, byte[] data)
        {
            return target.Send(data) == 1 ? true : false;
        }

        private bool ReceiveData(Socket target, byte[] data)
        {
            return target.Receive(data) == 1 ? true : false;
        }


        /// <summary>
        ///  //client gửi qua server nén đối tượng thành 1 mảng byte[] //copy
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>

        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();//tạo MemoryStream() lưu trữ sau đo phân tích
            BinaryFormatter bf1 = new BinaryFormatter();// bf1 BinaryFormatter format kiểu byte[]
            bf1.Serialize(ms, o);//  từ bf1 BinaryFormatter phân tách theo dạng MemoryStream()theo dữ liệu của mình
            return ms.ToArray();//trả ra
        }
        /// <summary>
        /// //server trả về giải nén  mảng byte[] thành 1 đối tượng (objcet)
        /// </summary>
        /// <param name="theByteArray"></param>
        /// <returns></returns>
        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);//từ theByteArray) đưa vào MemoryStream
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;//bat dau vtri 0
            return bf1.Deserialize(ms);//tra ve o
        }





        /// <summary>
        /// copy1 public  string GetLocalIPv4 lấy ra IPv4 của card mạng đang dùng
        /// </summary>
        /// <param name="?"></param>
        /// <returns></returns>
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            return output;
        }



        #endregion
    }
}


