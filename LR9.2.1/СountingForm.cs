using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR9._2._1
{
    public partial class СountingForm : Form
    {
        private int cntOfCorrect;

        public СountingForm()
        {
            InitializeComponent();
        }

        private void СountingForm_Load(object sender, EventArgs e)
        {
            foreach (Control el in Controls)
            {
                if (el is PictureBox pb)
                {
                    pb.AllowDrop = true;
                }
            }
        }

        private void PictureBoxNumber_MouseDown(object sender, MouseEventArgs e)
        {
            PictureBox currPB = (PictureBox)sender;
            currPB.DoDragDrop(currPB, DragDropEffects.Copy);
        }

        private void AnswerArea_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void AnswerArea_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox receiver = (PictureBox)sender;
            PictureBox source = (PictureBox)e.Data.GetData(typeof(PictureBox));
            receiver.Image = source.Image;

            if(receiver.Tag == source.Tag)
            {
                correctDrag(receiver, false);
                cntOfCorrect++;
            }
            else
            {
                correctDrag(receiver, true);
                if(cntOfCorrect > 0)
                {
                    cntOfCorrect--;
                }
                
                MessageBox.Show("Неверный ответ\nПопробуйте еще раз", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            isEndGame();
        }

        private void correctDrag(PictureBox pb, bool needToDelete)
        {
            if (needToDelete)
            {
                PictureBox toDel = pb.Controls.OfType<PictureBox>().FirstOrDefault();
                pb.Controls.Remove(toDel);
            }
            else
            {
                Image temp = Image.FromFile(@"D:\Users\class\source\repos\C-LR9\correctIMG.jpg");
                Size tempSize = new Size(pb.Size.Width / 5, pb.Size.Height / 5);

                PictureBox pb2 = new PictureBox
                {
                    Size = tempSize,
                    SizeMode = PictureBoxSizeMode.StretchImage,
                    Location = new Point(0, 0),
                    BackColor = Color.Transparent,
                    Image = temp
                };
                pb.Controls.Add(pb2);
            }
        }

        private void isEndGame()
        {
            bool isEnd = cntOfCorrect == 6 ? true : false; 

            if (isEnd)
            {
                if (MessageBox.Show("Вы выйграли!\nНачать заново?", "Конец игры", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Close();
                }
            }
        }

        private void СountingForm_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.F1)
            {
                string message = "Правила игры:\n\n" +
                 "- Вам необходимо сосчитать количество предметов на рисунке.\n" +
                 "- Перетащите нужную цифру в ячейку под рисунком.\n" +
                 "- Если цифра выбрана правильно, она будет помещена в ячейку. В противном случае вы получите сообщение об ошибке.\n" +
                 "- Чтобы узнать правила в любой момент, нажмите кнопку F1.\n" +
                 "- При завершении игры вы получите сообщение с поздравлениями.";

                MessageBox.Show(message, "Правила игры", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
