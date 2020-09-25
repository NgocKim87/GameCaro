using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaroDeMo
{
    [Serializable]
   public class SocketData
    {
       
       //dat kieu int command
        private int command;

        public int Command
        {
            get { return command; }
            set { command = value; }
        }
       //toa do
        private Point point;//add Point vào thư viện

        public Point Point
        {
            get { return point; }
            set { point = value; }
        }
        // message;
        private string message;

        public string Message
        {
            get { return message; }
            set { message = value; }
        }
       //tạo hàm dựng
       public SocketData(int command, string message, Point point)//cho dấu chấm hỏi thì gtri co the null
        {
            this.Command = command;
            this.Point = point;
            this.Message = message;
        }

       public static int NOTIFY { get; set; }
    }
   //tạo danh sach su ly command enum là kieu liet ke de su dung
    public enum SocketCommand
    {
        SEND_POINT,
        NOTIFY,
        NEW_GAME,
        UNDO,
        END_GAME,
        TIME_OUT,
        QUIT
    }
}
