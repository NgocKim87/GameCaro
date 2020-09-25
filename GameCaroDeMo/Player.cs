using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaroDeMo
{
   public class Player
    {
       //tạo nguoi choi
        private string name;//Ctrl+R+E sau do ok --apply sinh code public string Name

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
       //tao ảnh
        private Image mark;//Image neu chưa co thu vien add vao

        public Image Mark
        {
            get { return mark; }
            set { mark = value; }
        }
       //hàm dựng tạo ng choi
       public Player(string name, Image mark)
        {
            this.Name = name;
            this.Mark = mark;
        }
       
        
       
    }
}
