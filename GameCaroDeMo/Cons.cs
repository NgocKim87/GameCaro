using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaroDeMo
{
    //tạo lớp class Contens để lưu sau này sử dụng hằng số
    public class Cons
    {
        //kích thước 1 ô vuông bàn cờ 
        public static int CHESS_WIDTH = 30;
        public static int CHESS_HEIGHT = 30;
        //SAU ĐÓ QUA Form1.cs vẽ bàn cờ
        //đặt tên và thay đổi giá trị ô vuông trên bàn cờ
        public static int CHESS_BOARD_WIDTH = 19;
        public static int CHESS_BOARD_HEIGHT = 15;

        public static int COOL_DOWN_STEP = 100;//Thay đổi tốc độ chạy
        public static int COOL_DOWN_TIME = 50000;//tổng thời gian 10 phút
        public static int COOL_DOWN_INTERVAL = 100;//1/10 sẽ tăng 1 lần trong 10 phút
    }
}
