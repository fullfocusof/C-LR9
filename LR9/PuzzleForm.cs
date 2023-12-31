﻿using LR9.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR9
{
    public partial class PuzzleForm : Form
    {
        List<int> correctIMGs;
        List<int> IMGsID;

        public PuzzleForm()
        {
            InitializeComponent();           
        }

        private void PuzzleForm_Load(object sender, EventArgs e)
        {
            correctIMGs = new List<int>();
            IMGsID = new List<int>();
            for (int i = 0; i < 16; i++)
            {
                correctIMGs.Add(i);
                IMGsID.Add(i);
            }

            foreach (Control el in Field.Controls)
            {
                if(el is PictureBox pb)
                {
                    pb.AllowDrop = true;
                }
            }

            shuffle();
        }

        private void picture1_MouseDown(object sender, MouseEventArgs e)
        {
            DoDragDrop((PictureBox)sender, DragDropEffects.Move);
        }

        private void picture1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void picture1_DragDrop(object sender, DragEventArgs e)
        {
            PictureBox receiver = (PictureBox)sender;
            PictureBox source = (PictureBox)e.Data.GetData(typeof(PictureBox));
            Image Temp = receiver.Image;
            receiver.Image = source.Image;
            source.Image = Temp;

            int receiverID = Field.Controls.IndexOf(receiver);
            int sourceID = Field.Controls.IndexOf(source);

            int tempID = IMGsID[receiverID];
            IMGsID[receiverID] = IMGsID[sourceID];
            IMGsID[sourceID] = tempID;

            if (IMGsID[receiverID] == correctIMGs[receiverID])
            {
                correctDrag(receiver, false);
            }
            else
            {
                correctDrag(receiver, true);
            }

            if (IMGsID[sourceID] == correctIMGs[sourceID])
            {
                correctDrag(source, false);
            }
            else
            {
                correctDrag(source, true);
            }

            isEndGame();
        }

        private void shuffle()
        {
            List<Image> IMGs = new List<Image>();

            Random random = new Random();

            foreach (Control el in Field.Controls)
            {
                if (el is PictureBox pb)
                {
                    IMGs.Add(pb.Image);
                }
            }

            int n = IMGs.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                Image temp = IMGs[k];
                IMGs[k] = IMGs[n];
                IMGs[n] = temp;

                int tempID = IMGsID[k];
                IMGsID[k] = IMGsID[n];
                IMGsID[n] = tempID;
            }

            int i = 0;
            foreach (Control el in Field.Controls)
            {
                if (el is PictureBox pb)
                {
                    pb.Image = IMGs[i];
                    i++;
                }
            }
        }

        private void isEndGame()
        {
            bool isEnd = Enumerable.SequenceEqual(correctIMGs, IMGsID);

            if (isEnd)
            {
                if(MessageBox.Show("Вы выйграли!\nНачать заново?", "Конец игры", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Application.Restart();
                }
                else
                {
                    Close();
                }
            }
        }

        private void correctDrag(PictureBox pb, bool needToDelete)
        {
            if(needToDelete)
            {
                PictureBox toDel = pb.Controls.OfType<PictureBox>().FirstOrDefault();
                pb.Controls.Remove(toDel);
            }
            else
            {
                Image temp = Image.FromFile(@"E:\Projects\C#\LR9\LR9\correctIMG.png");
                Size tempSize = new Size(pb.Size.Width / 10, pb.Size.Height / 10);

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

    }
}
