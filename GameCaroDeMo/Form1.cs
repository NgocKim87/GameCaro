using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaroDeMo
{
    public partial class Form1 : Form
    {
        //partial trong public partial  lass
        #region Properties
        ChessBoardManager ChessBoard;

       
        SocketManager socket;
        #endregion

        public Form1()
        {
            InitializeComponent();
            //ChessBoard = new ChessBoardManager(pnlChessBoard)
            Control.CheckForIllegalCrossThreadCalls = false;
           // DrawChessBroad(); đổi thành
            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName,pctbMark);//truyền tên vào khi gọi DrawChessBroad();truyền thêm txb ten , pctb
            //de chay ng chay COOLDOWN
            ChessBoard.EndedGame +=ChessBoard_EndedGame;//+= click TAB chọn góc chân add se sinh ra code
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;//+= click TAB chọn góc chân add se sinh ra code

            prcbCoolDown.Step = Cons.COOL_DOWN_STEP;//lay theo ten bang gia tri cua bang = Cons tạo lớp trước
            prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;//đồng hồ 
            // KHỞI TẠO new SocketManager();
            socket = new SocketManager();
           // tmCoolDown.Start();//tự động chạy khoi động cho đồng hồ và dừng lại

            // ChessBoard.DrawChessBroad();tái sd hàmvào  NewGame();
            NewGame();

        }
        #region Methods
        //tạo tên code hàm NewGame, undo,quit
        void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            //de mo chuc nag Undo() lai
            undoToolStripMenuItem.Enabled = true;

            ChessBoard.DrawChessBroad();
            //reset lại gtri
           // prcbCoolDown.Value = 0;
           // tmCoolDown.Stop();//new game vtri nay van con hien thi prcbCoolDown nên di chuyển lên trước khi vẽ thanh sẽ mất
        }
        void Quit()
        {
            Application.Exit();
        }
        void Undo()
        {
            //hàm Undo() dc khoi tạo bên method
            ChessBoard.Undo();
            prcbCoolDown.Value = 0;
        }
        //khởi tạo dòng chữ, pháo hoa
        //truyền vào private void ChessBoard_EndedGame(object sender, EventArgs e)
        //khi prcbCoolDowm ơ vi tri Maximum
        void EndGame()
        {
            //de cau thong bao dừng lại
            tmCoolDown.Stop();
            //han che ng khac đánh tiếp khi kết thúc
            pnlChessBoard.Enabled = false;
            //ko Undo khi ket thuc
            undoToolStripMenuItem.Enabled = false;

           // MessageBox.Show("Kết thúc");
        }
        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)//EventArgs e==ButtonClickEvent e
        {
            tmCoolDown.Start();//khi debug đồng hồ tắt ko ảnh hướng 
            pnlChessBoard.Enabled = false;
            prcbCoolDown.Value = 0;

            //gtri ng đánh đầu tiên bất kỳ ở vtri nào cũng là(0,0)
            socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));
            //chỉ cho phép 1 ng dc Undo khi chưa đánh
            undoToolStripMenuItem.Enabled = false;

            //2 bên lang nghe
            Listen();
        }

       private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            //Thông báo ket thuc ktra 5 con thắng
            EndGame();
            socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();//thực hiện step theo 1/10 giay 1 lần
            //THONG BAO KET THUC
            if (prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                EndGame();
                socket.Send(new SocketData((int)SocketCommand.TIME_OUT, "", new Point()));
            }
        }
        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            socket.Send(new SocketData((int)SocketCommand.NEW_GAME, "", new Point()));
            pnlChessBoard.Enabled = true;//bên ng choi gui newgame() thì bên đó đi trước vẫn giữ lượt đi mặc định chỉ là giữa 2 app
        }


        private void pnlChessBoard_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

   

   

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
        }

        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

       
        //khi đóng màn hình dau x se hien thi thong báo
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            //Application.Exit();
            //cac
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                socket.Send(new SocketData((int)SocketCommand.QUIT, "", new Point()));
                }
                catch//nếu bên kia còn thì gửi ko thì kết thúc
                {

                }
            }
        }
        #endregion

        private void LAN_Click(object sender, EventArgs e)
        {
            //click 2 lần LAN DESIGN hiện code
           socket.IP = txbIP.Text;
            if(!socket.ConnectServer())
            {
                socket.isServer = true;
                pnlChessBoard.Enabled = true;
                socket.CreateServer();

               /* //them thu vien Thread
                //TẠO VÒNG LẶP SERVER
                Thread listenThread = new Thread(() =>
                {
                    while (true)
                    {
                        Thread.Sleep(500);
                        try
                        {
                            Listen();
                            break;//RA KHOI 
     
                        }
                        catch 
                           {

                           }
                    }
                   
                });
                listenThread.IsBackground = true;
                listenThread.Start();*/
                //vòng lặp chờ từ phía client thừa có thể bỏ

            }
            else
            {
                socket.isServer = false;
                pnlChessBoard.Enabled = false;
                Listen();// cat xuong void Listen() nghe server thui
                //thong bao client
                // socket.Send(""Thông tin từ Client);


                //tạo nhieu thông báo khác nhau

               // socket.Send(new SocketData((int)SocketCommand.NOTIFY,"Client đã kết nối" ));// bo di do bỏ vòng lặp thừa chờ client
            }

          
        }
        //hàm lắng nghe
        void Listen()
        {
            //xu ly thoat dot ngot try..catch
            
            Thread listenThread = new Thread(() =>
            {
                    try
                    { 
                    SocketData data =(SocketData)socket.Receive();
                    ProcessData(data);
                   }

                   catch(Exception e)
                  {
                      //var a = e;
                  }
                
            });
            listenThread.IsBackground = true;
            listenThread.Start();
            //MessageBox.Show(data);
       
        }
        private void ProcessData(SocketData data)
        {
            
            switch (data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    MessageBox.Show(data.Message);
                    break;
                case (int)SocketCommand.NEW_GAME://goi ham NewGame();
                    this.Invoke((MethodInvoker)(() =>
                    {
                        NewGame();
                        pnlChessBoard.Enabled = false;
                    }));                    
                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker) (() =>
                    {

                        prcbCoolDown.Value = 0;
                        pnlChessBoard.Enabled = true;
                        tmCoolDown.Start();
                        ChessBoard.OtherPlayerMark(data.Point);
                        undoToolStripMenuItem.Enabled = true;
                    }));
              
                    break;
                case (int)SocketCommand.UNDO:
                          Undo();
                    //resetprcbCoolDown ve bang 0
                          prcbCoolDown.Value = 0;                   
                    break;
                case (int)SocketCommand.END_GAME:
                    MessageBox.Show("Đã 5 con trên 1 hàng ");
                    break;
                case (int)SocketCommand.TIME_OUT:
                    MessageBox.Show("Hết giờ đi tiếp");
                    break;
                case (int)SocketCommand.QUIT://THOAT GAME
                    tmCoolDown.Stop();
                    MessageBox.Show("Người chơi đã thoát");
                    break;

                default:
                    break;
            }
            Listen();//giúp lắng nghe tin hiện lên 2 app
        }
       
       
        //su kien Shown
        private void Form1_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);
            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
           
        }
        
    }
}
