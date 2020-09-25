using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaroDeMo
{
   public class ChessBoardManager
    {
        //muốn dùng pnlChessBoard vì bêm form nên tạo hàm dựng truyền từ form xuống rồi dùng=> tạo properties cho nó
        #region Properties
        private Panel chessBoard;
        public Panel ChessBoard
        {
          get { return chessBoard;} 
          set { chessBoard = value;}

        }
       //tạo 1 list player
        private List<Player> player;

        public List<Player> Player
        {
            get { return player; }
            set { player = value; }
        }
       //tạo ra 1 biến để biết người nào đang đánh
        private int currentPlayer;

        public int CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }
       //tạo list lòng trong list
        private List<List<Button>> matrix;

        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }
       //khoi tạo textbox ten ng choi, ảnh kèm theo
        private TextBox playerName;

        public TextBox PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }
        private PictureBox playerMark;

        public PictureBox PlayerMark
        {
            get { return playerMark; }
            set { playerMark = value; }
        }
       //tạo event khi co nguoi choi 
       // private event EventHandler playerMarked;
       // public event EventHandler PlayerMarked
        private event EventHandler<ButtonClickEvent> playerMarked;
        public event EventHandler<ButtonClickEvent> PlayerMarked
        {
            add
            {
                playerMarked += value;
            }
            remove
            {
                playerMarked -= value;

            }
        }

       //tao event EndGame
        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;

            }
        }
       //lưu lại bước đi người chơi
       //từ hàm dựng đổi Point =PlayInfo
        private Stack<PlayInfo> playTimeline;

        public Stack<PlayInfo> PlayTimeline
        {
            get { return playTimeline; }
            set { playTimeline = value; }
        }



        #endregion

        #region Initialize
        //hàm dựng
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark)//value =tu dat gtri
        {
            this.ChessBoard = chessBoard;//ChessBoard dc tạo public panel
            //truyen gia tri textbox ten và anh ng choi vao
            this.PlayerName = playerName;
            this.PlayerMark = mark;//ưu tien giatri gần


            //khoi tao player
            this.Player = new List<Player>() 
            {
                new Player("NgocKim",Image.FromFile(Application.StartupPath + "\\Resources\\k1c.jpg")),
                new Player("ThanhSon", Image.FromFile(Application.StartupPath + "\\Resources\\k2c.jpg"))

            };
            //dat gia tri hien tai mac dinh, khi bấm newgame ko thay đổi gtri mac dịnh vào hàm public void DrawChessBroad()
           //CurrentPlayer = 0;
          // ChangePlayer();
           
        }
      
      
        #endregion
        #region Methods
       public void DrawChessBroad()
       {
           //ket thuc game
           ChessBoard.Enabled = true;
           //khi click newgame nên xóa dữ liệu củ
           ChessBoard.Controls.Clear();
           //khoi tạo Point lưu ng chơi
           PlayTimeline = new Stack<PlayInfo>();
           //dat gia tri hien tai mac dinh
          CurrentPlayer = 0;
          ChangePlayer();
           //khoi tao trong khi tao newgame
           Matrix = new List<List<Button>>();

           // MessageBox.Show("Bàn cờ đã vẽ");

           //dùng code vẽ

           /*Button btn = new Button();
           btn.Text = "1";

           Button btn2 = new Button() { Text = "2", Location = new Point(btn.Location.X + btn.Width, btn.Location.Y) };
           pnlChessBoard.Controls.Add(btn);
           pnlChessBoard.Controls.Add(btn2);*/
           //dùng vòng lặp for
           //lưu lại button trước đó để sử dụng VÀ SÉT GIÁ TRỊ VỊ TRÍ MẶC ĐỊNH BẰNG O
           Button oldButton = new Button() { Width = 0, Location = new Point(0, 0) };
           for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
           {
               //tạo 1 mảng mới lưu lạiMatrix
               Matrix.Add(new List<Button>());

               for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
               {
                   Button btn = new Button()
                   {
                       Width = Cons.CHESS_WIDTH,
                       Height = Cons.CHESS_HEIGHT,
                       Location = new Point(oldButton.Location.X + oldButton.Width, oldButton.Location.Y),//LẤY BUTTON Cũ
                       //thaydoi kich thuc playout ảnh hiện khi chơi
                       BackgroundImageLayout = ImageLayout.Stretch,
                       //de biet button nằm o hàng nào
                       Tag= i.ToString()

                   };
                   btn.Click += btn_Click;//su kien click sinh ra private void btn_Click(object sender, EventArgs e)..


                   ChessBoard.Controls.Add(btn);//dc goi o  Initialize
                   //dây la matrix luu lai dòng thu mấy
                   Matrix[i].Add(btn);
                   //tao ra button moi them vao
                   oldButton = btn;
               }
               //sửa lại giá trị mặc định khi hết 1 dòng sửa tọa độ (0,0)
               oldButton.Location = new Point(0, oldButton.Location.Y + Cons.CHESS_HEIGHT);//kích thước của 1 dòng, còn x=0, y>0
               //sau đó về giá trị button se trả về tọa độ (0,0)
               oldButton.Width = 0;
               oldButton.Height = 0;
           }
       }

       private void btn_Click(object sender, EventArgs e)
       {
           //button click
           Button btn = sender as Button;
           //đổi hình button         
           //  btn.BackgroundImage = Image.FromFile(Application.StartupPath + "\\Resources\\k1c.jpg");//copy Resources vào debug đường đẫn ko đúng vào 
          //thay doi button theo luot choi thay doi
           //câu lệnh if để khi chọn giá trị ko dc bấm tiếp
           if (btn.BackgroundImage != null)
               return;
          /* //lệnh khi choi thay đổi nguoi choi
           btn.BackgroundImage = Player[CurrentPlayer].Mark;
           CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
           PlayerName.Text = Player[CurrentPlayer].Name;
           PlayerMark.Image = Player[CurrentPlayer].Mark;*/
           Mark(btn);
           //lưu ng đang đánh
           PlayTimeline.Push(new PlayInfo(GetChessPoint(btn),CurrentPlayer));

           CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

           ChangePlayer();
           //khi còn thời gian nhưng đã thắng nên kết thúc ngay,
           if (playerMarked != null)
               playerMarked(this, new ButtonClickEvent(GetChessPoint(btn)));
           //thực hiện kiểm tra hàm ket thuc game khi hết thời gian
           if (isEndGame(btn))
           {
               //tạo hàm EndGame rồi cho vào
               EndGame();
           }

       }
       //poit phải có //ng đang đánh
       public void OtherPlayerMark(Point point)
       {
           //ng đang đánh
           Button btn = Matrix[point.Y][point.X];
           if (btn.BackgroundImage != null)
               return;
           
           Mark(btn);
           PlayTimeline.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

           CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

           ChangePlayer();
          /* if (playerMarked != null)
               playerMarked(this, new EventArgs());*///ko can event ánh xạ từ bên kia qua nên bỏ
           //thực hiện kiểm tra hàm ket thuc game khi hết thời gian
           if (isEndGame(btn))
           {
               //tạo hàm EndGame rồi cho vào
               EndGame();
           }

       }
       //tạo hàm undo mỗi khi thay đổi
       public bool Undo()
       {
           if (PlayTimeline.Count <= 0)
               return false;

           bool isUndo1 = UndoAStep();
           bool isUndo2 = UndoAStep();

           PlayInfo oldPoint = PlayTimeline.Peek();
           CurrentPlayer = oldPoint.CurrentPlayer == 1 ? 0 : 1;
           return  isUndo1 && isUndo2 ;
           /*
           //gom về  private bool UndoAStep()
           //cài đặt giá trị hết số lần undo(0)
           if (PlayTimeline.Count <= 0)
               return false;
           PlayInfo oldPoint = PlayTimeline.Pop();//lay ra nguoi choi cuoi pop() neu bang 0 thi ko lay dc cần xét thêm 
           Button btn = Matrix[oldPoint.Point.Y][oldPoint.Point.X];

           btn.BackgroundImage = null;

           //hien thi ng choi cuoi
           // CurrentPlayer = PlayTimeline.Peek().CurrentPlayer == 1?0:1;//thay doi hinh khi đánh
           

           if (PlayTimeline.Count <= 0)//nếu nhỏ hơn bằng 0 
           {
               CurrentPlayer = 0;
           }
           else
           {
               oldPoint = PlayTimeline.Peek();
               CurrentPlayer = oldPoint.CurrentPlayer == 1 ? 0 : 1;
           }

           ChangePlayer();//thay doi avatra ng choi

           return true;*/
       }
       //undo 1 lần là  2 nuoc do 2 app
       private bool UndoAStep()
       {
          
           if (PlayTimeline.Count <= 0)
               return false;
           PlayInfo oldPoint = PlayTimeline.Pop();//lay ra nguoi choi cuoi pop() neu bang 0 thi ko lay dc cần xét thêm 
           Button btn = Matrix[oldPoint.Point.Y][oldPoint.Point.X];

           btn.BackgroundImage = null;

           //hien thi ng choi cuoi
           // CurrentPlayer = PlayTimeline.Peek().CurrentPlayer == 1?0:1;//thay doi hinh khi đánh


           if (PlayTimeline.Count <= 0)//nếu nhỏ hơn bằng 0 
           {
               CurrentPlayer = 0;
           }
           else
           {
               oldPoint = PlayTimeline.Peek();
              
           }

           ChangePlayer();//thay doi avatra ng choi

           return true;
       }
       //tạo hàm EndGame
       public void EndGame()
       {
           //MessageBox.Show("Tớ đã thắng");
           if (endedGame != null)
               endedGame(this, new EventArgs());
       }
       //hàm ktra ket qua thang thua
       private bool isEndGame(Button btn)
       {
           return isEndHorizontal(btn)||isEndVertical(btn)||isEndPrimary(btn)||isEndSub(btn);
       }
       //xét 4 hàm định nghĩa
       //hàm lấy tọa độ ra
       private Point GetChessPoint(Button btn)
       {
           //Point point = new Point();
           //xu lý lay ponit lay toa do x, y
           int vertical = Convert.ToInt32(btn.Tag);
           int horizontal = Matrix[vertical].IndexOf(btn);
           //truyen gtri, x,y
           Point point = new Point(horizontal,vertical);

           return point; //lay point nay ra
       }

       //xuly hang ngang
       private bool isEndHorizontal(Button btn)
       {
           Point point = GetChessPoint(btn);// đã dc tao tọa độ 

           int countLeft = 0;//xu ly kq ben trai
           for (int i = point.X; i >= 0 ; i--)//gtri bat dau tu 0 
           {
               if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
               {
                   countLeft++;
               }
               else
                   break;
           }



           int countRight = 0;//xu ly kq ben phai
           for (int i = point.X + 1; i < Cons.CHESS_BOARD_WIDTH; i++)//point.X+1 phải cộng thêm 1 ko trùng gtri5 trên
           {
               if (Matrix[point.Y][i].BackgroundImage == btn.BackgroundImage)
               {
                   countRight++;
               }
               else
                   break;
           }
           //cong cho  =5
           return countLeft + countRight == 5;

       }
          
       private bool isEndVertical(Button btn)
       {
           Point point = GetChessPoint(btn);// đã dc tao tọa độ 

           int countTop = 0;//xu ly kq tren
           for (int i = point.Y; i >= 0; i--)//gtri bat dau tu 0 
           {
               if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
               {
                   countTop++;
               }
               else
                   break;
           }



           int countBottom = 0;//xu ly kq dưới
           for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)//point.X+1 phải cộng thêm 1 ko trùng gtri5 trên
           {
               if (Matrix[i][point.X].BackgroundImage == btn.BackgroundImage)
               {
                   countBottom++;
               }
               else
                   break;
           }
           //cong cho  =5
           return countTop + countBottom == 5;
       }
       private bool isEndPrimary(Button btn)//đường chéo chính cùng tăng hoặc cùng giảm(LẤY GIÁ TRỊ THEO Y RỒI X)
       {
           Point point = GetChessPoint(btn);// đã dc tao tọa độ 

           int countTop = 0;//xu ly kq tren
           for (int i = 0; i <=point.X; i++)//khai bao i bắt đầu từ 0, 
           {
               //ktra ra ngoai bảng thì dừng(x, y) vì i<0 ra khoi bảng 
               if (point.Y - i < 0 || point.X - i < 0)
                   break;

               if (Matrix[point.Y-i][point.X-i].BackgroundImage == btn.BackgroundImage)
               {
                   countTop++;
               }
               else
                   break;
           }



           int countBottom = 0;//xu ly kq dưới
           for (int i =  1; i < Cons.CHESS_BOARD_WIDTH-point.X; i++)//point.X+1 phải cộng thêm 1 ko trùng gtri5 trên
           {
               //kiểm tra ra khỏi bàn cờ WIDTH(x, y) bằng or lớn hơn bàn cờ sẽ ra khỏi
               if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT|| point.X + i >= Cons.CHESS_BOARD_WIDTH)//Ktra truong hop gócben duoi
                   break;
               if (Matrix[point.Y+ i][point.X+ i].BackgroundImage == btn.BackgroundImage)
               {
                   countBottom++;
               }
               else
                   break;
           }
           //cong cho  =5
           return countTop + countBottom == 5;
       }
       private bool isEndSub(Button btn)
           //duong cheo phu : đi lên x tang y giam, di xuong x giảm y tăng
       {
           Point point = GetChessPoint(btn);// đã dc tao tọa độ 

           int countTop = 0;//xu ly kq đi lên
           for (int i = 0; i <= point.X; i++)//khai bao i bắt đầu từ 0, 
           {
               //ktra ra ngoai bảng thì dừng(x, y) vì i<0 ra khoi bảng 
               if (point.Y - i < 0 || point.X + i > Cons.CHESS_BOARD_WIDTH)
                   break;

               if (Matrix[point.Y - i][point.X + i].BackgroundImage == btn.BackgroundImage)
               {
                   countTop++;
               }
               else
                   break;
           }



           int countBottom = 0;//xu ly kq xuống
           for (int i = 1; i < Cons.CHESS_BOARD_WIDTH - point.X; i++)//point.X+1 phải cộng thêm 1 ko trùng gtri5 trên
           {
               //kiểm tra ra khỏi bàn cờ WIDTH(x, y) bằng or lớn hơn bàn cờ sẽ ra khỏi
               if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X - i <0)//Ktra x ko nho hơn 0 sẽ ko qua truc oxy nằm bên y ko thỏa ra khỏi bảng
                   break;
               if (Matrix[point.Y + i][point.X - i].BackgroundImage == btn.BackgroundImage)
               {
                   countBottom++;
               }
               else
                   break;
           }
           //cong cho  =5
           return countTop + countBottom == 5;
       }

       private void Mark(Button btn)
       {
           btn.BackgroundImage = Player[CurrentPlayer].Mark;
 
       }
       private void ChangePlayer()
       {
           PlayerName.Text = Player[CurrentPlayer].Name;
           PlayerMark.Image = Player[CurrentPlayer].Mark;
       }
        #endregion

        //đưa code từ Form1.cs sang ChessBoardManager( từ void }})
       //thêm vào thư viện button, poin click vào ô vuông dưới chữ rồi using...
       
    }
    public class ButtonClickEvent : EventArgs
    {
        private Point clickedPoint;

        public Point ClickedPoint
        {
            get { return clickedPoint; }
            set { clickedPoint = value; }
        }
        //hàm dựng
        public ButtonClickEvent(Point point)
        {
            this.ClickedPoint = point;
        }

    }
}
