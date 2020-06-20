using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PaintApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        Color selectedColor = Color.Black;
        int chose = 0;
        int lineWidth = 1;
        int window = 0;
        int startx, starty;
        bool mousedowncontol = false;
        bool save = false;
        int selectedPic = 0;
        TextBox metin;
        bool texbox = false;
        public void MouseDownEvent(object sender, EventArgs e)
        {
            var pos = e as MouseEventArgs;
            mousedowncontol = true;
            startx = pos.X;
            starty = pos.Y;
            PictureBox current = sender as PictureBox;
            lineWidth = Convert.ToInt16(comboBox1.SelectedItem);
            if (chose == 6)
            {
                splinePoints.Add(new Point(pos.X, pos.Y));
            }
            if (chose == 8)
            {
                if (!texbox)
                {
                    metin = new TextBox();
                    metin.Location = new Point(pos.X, pos.Y);
                    metin.BringToFront();
                    metin.BackColor = current.BackColor;
                    metin.BorderStyle = BorderStyle.FixedSingle;

                    current.Controls.Add(metin);
                    texbox = true;
                }
                else
                {
                    Bitmap bmp = current.Image as Bitmap;
                    RectangleF rectf = new RectangleF(metin.Location.X, metin.Location.Y, 200, 200);

                    Graphics g = Graphics.FromImage(bmp);

                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    FontDialog f = new FontDialog();
                    if (f.ShowDialog() == DialogResult.OK)
                    {
                        g.DrawString(metin.Text, f.Font, new SolidBrush(selectedColor), rectf);
                        g.Flush();
                        current.Image = bmp;
                        metin.Visible = false;
                        texbox = false;
                    }
                
                }
            }


        }
        List<Point> splinePoints = new List<Point>();
        public void MouseUpEvent(object sender, EventArgs e)
        {
            mousedowncontol = false;
            PictureBox current = sender as PictureBox;
            var pos = e as MouseEventArgs;


            if (chose == 2)
            {
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Bitmap redim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(redim);
                g.DrawRectangle(new Pen(selectedColor, lineWidth), rect);
                current.Image = redim;
                lastImage = redim;
            }
            if (chose == 3)
            {
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Bitmap redim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(redim);
                g.FillRectangle(new SolidBrush(selectedColor), rect);
                current.Image = redim;
                lastImage = redim;
            }
            if (chose == 4)
            {
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Bitmap redim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(redim);
                g.DrawEllipse(new Pen(selectedColor, lineWidth), rect);
                current.Image = redim;
                lastImage = redim;
            }
            if (chose == 5)
            {
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Bitmap redim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(redim);
                g.FillEllipse(new SolidBrush(selectedColor), rect);
                current.Image = redim;
                lastImage = redim;
            }
            if (chose == 1)
            {
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Bitmap redim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(redim);
                g.DrawLine(new Pen(selectedColor, lineWidth), new Point(startx, starty), new Point(pos.X, pos.Y));
                current.Image = redim;
                lastImage = redim;
            }
            if (chose == 6)
            {
                Bitmap redim = new Bitmap(current.Width, current.Height);
                Graphics g = Graphics.FromImage(redim);
                foreach (Point point in splinePoints)
                    g.FillEllipse(Brushes.Red,
                        point.X - 3, point.Y - 3, lineWidth + 5, lineWidth + 5);

                if (splinePoints.Count < 2) return;

                g.DrawCurve(new Pen(selectedColor, lineWidth), splinePoints.ToArray());
                Bitmap image = current.Image as Bitmap;
                if (lastImage == null)
                {
                    g.DrawImage(new Bitmap(current.Width, current.Height), 0, 0);
                    lastImage = redim;
                }
                else
                {
                    g.DrawImage(lastImage, 0, 0);
                }
                current.Image = redim;
            }
            if (save == true)
            {
                Titles[selectedPic].Text = Titles[selectedPic].Text + "*";
                save = false;
            }
        }
        Bitmap lastImage;
        int eraserSize;
        public void MouseMoveEvent(object sender, EventArgs e)
        {
            var pos = e as MouseEventArgs;

            PictureBox current = sender as PictureBox;
            if (chose == 0 && mousedowncontol)
            {
                Bitmap resim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(resim);
                g.DrawLine(new Pen(selectedColor, lineWidth), startx, starty, pos.X, pos.Y);
                startx = pos.X;
                starty = pos.Y;
                current.Image = resim;
                lastImage = resim;

            }
            if (chose == 2 && mousedowncontol)
            {
                Refresh();
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Graphics g = current.CreateGraphics();
                g.DrawRectangle(new Pen(selectedColor, lineWidth), rect);

            }
            if (chose == 3 && mousedowncontol)
            {
                Refresh();
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Graphics g = current.CreateGraphics();
                g.FillRectangle(new SolidBrush(selectedColor), rect);

            }
            if (chose == 4 && mousedowncontol)
            {
                Refresh();
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Graphics g = current.CreateGraphics();
                g.DrawEllipse(new Pen(selectedColor, lineWidth), rect);

            }
            if (chose == 5 && mousedowncontol)
            {
                Refresh();
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Graphics g = current.CreateGraphics();
                g.FillEllipse(new SolidBrush(selectedColor), rect);

            }
            if (chose == 1 && mousedowncontol)
            {
                Refresh();
                Rectangle rect = new Rectangle(startx, starty, pos.X - startx, pos.Y - starty);
                Graphics g = current.CreateGraphics();
                g.DrawLine(new Pen(selectedColor, lineWidth), new Point(startx, starty), new Point(pos.X, pos.Y));

            }
            if (chose == 7 && mousedowncontol)
            {
                if (lineWidth == 1)
                {
                    eraserSize = 10;
                }
                else
                {
                    eraserSize = lineWidth + 5;
                }
                Bitmap resim = current.Image as Bitmap;
                Graphics g = Graphics.FromImage(resim);
                g.FillEllipse(new SolidBrush(current.BackColor), pos.X, pos.Y, eraserSize, eraserSize);
                current.Image = resim;
            }

        }
        List<Label> Titles = new List<Label>();
        List<PictureBox> windows = new List<System.Windows.Forms.PictureBox>();
        public void labelClickEvent(object sender, EventArgs e)
        {
            Label current = sender as Label;
            splinePoints.Clear();
            for (int i = 0; i < Titles.Count; i++)
            {
                Titles[i].Font = new Font(current.Font, FontStyle.Regular);
            }
            current.Font = new Font(current.Font, FontStyle.Bold);
            foreach (PictureBox i in windows)
            {
                i.Visible = false;
            }
            windows[Convert.ToInt16(current.Name)].Visible = true;
            selectedPic = Convert.ToInt16(current.Name);


        }
        Label label = new Label();
        int left = 0;
        int i = 0;
        private void button21_Click(object sender, EventArgs e)
        {
            OpenFileDialog aç = new OpenFileDialog();
            aç.Filter = "Tümü|*.*";
            aç.RestoreDirectory = true;
            if (aç.ShowDialog() == DialogResult.OK)
            {
                Image resim = Image.FromFile(aç.FileName);
                Titles.Add(new Label());

                Titles[Titles.Count - 1].Name = i.ToString();
                FileInfo fi = new FileInfo(aç.FileName);
                Titles[Titles.Count - 1].Text = fi.Name;
                if (Titles.Count == 1)
                {
                    Titles[Titles.Count - 1].Left = 0;
                }
                else
                {
                    Titles[Titles.Count - 1].Left = Titles[Titles.Count - 2].Right + 10;
                }
                Titles[Titles.Count - 1].AutoSize = true;

                Titles[Titles.Count - 1].Click += new System.EventHandler(this.labelClickEvent);

                panel1.BorderStyle = BorderStyle.FixedSingle;

                panel1.Left = panel2.Left;
                panel1.Width = panel2.Width;
                panel1.Controls.Add(Titles[Titles.Count - 1]);
                panel1.BackColor = Color.Wheat;
                left += 90;

                //ADD Picturbox

                windows.Add(new PictureBox());
                windows[windows.Count - 1].Name = "untitle" + i.ToString();
                windows[windows.Count - 1].Left = panel1.Left;
                windows[windows.Count - 1].Top = panel2.Bottom + 5;
                windows[windows.Count - 1].Width = panel2.Width;
                windows[windows.Count - 1].Height = this.Height - 100;
                windows[windows.Count - 1].BorderStyle = BorderStyle.FixedSingle;
                windows[windows.Count - 1].BackColor = Color.White;

                windows[windows.Count - 1].MouseDown += new MouseEventHandler(this.MouseDownEvent);
                windows[windows.Count - 1].MouseUp += new MouseEventHandler(this.MouseUpEvent); ;
                windows[windows.Count - 1].MouseMove += new MouseEventHandler(this.MouseMoveEvent); ;
                windows[windows.Count - 1].Image = resim;
                window = Titles.Count;
                i++;
                Controls.Add(windows[windows.Count - 1]);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            YeniEkle();
        }

        private void button29_Click(object sender, EventArgs e)
        {
            lineWidth = 1;
            comboBox1.SelectedIndex = 0;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 2;
            lineWidth = 3;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 4;
            lineWidth = 5;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            splinePoints.Clear();
            chose = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            chose = 1;
            splinePoints.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chose = 2;
            splinePoints.Clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            chose = 4;
            splinePoints.Clear();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            chose = 3;
            splinePoints.Clear();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            chose = 5;
            splinePoints.Clear();
        }

        private void button24_Click(object sender, EventArgs e)
        {
            chose = 6;
            splinePoints.Clear();
        }

        private void button25_Click(object sender, EventArgs e)
        {
            chose = 7;
            splinePoints.Clear();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            selectedColor = button7.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            selectedColor = button8.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            selectedColor = button9.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            selectedColor = button10.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            selectedColor = button11.BackColor;
            button18.BackColor = selectedColor;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            selectedColor = button12.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            selectedColor = button13.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            selectedColor = button14.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            selectedColor = button15.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            selectedColor = button16.BackColor;
            button18.BackColor = selectedColor;
            splinePoints.Clear();
        }

        private void button17_Click(object sender, EventArgs e)
        {
            ColorDialog color = new ColorDialog();
            if (color.ShowDialog() == DialogResult.OK)
            {
                selectedColor = color.Color;
                button18.BackColor = selectedColor;
                splinePoints.Clear();
            }
        }

        private void button20_Click(object sender, EventArgs e)
        {
            save = true;
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AutoUpgradeEnabled = true;
            sfd.Filter = "Images|*.png";
            ImageFormat format = ImageFormat.Png;
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string ext = System.IO.Path.GetExtension(sfd.FileName);
                switch (ext)
                {
                    case ".png":
                        format = ImageFormat.Png;
                        break;
                }
                windows[selectedPic].Image.Save(sfd.FileName, format);
                FileInfo fi = new FileInfo(sfd.FileName);
                string text = fi.Name;
                Titles[selectedPic].Text = text;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            YeniEkle();
            button18.BackColor = selectedColor;

        }

        private void button26_Click(object sender, EventArgs e)
        {
            chose = 8;
        }

        public void YeniEkle()
        {
            Titles.Add(new Label());

            Titles[Titles.Count - 1].Name = i.ToString();
            Titles[Titles.Count - 1].Text = "untitle" + i.ToString();
            if (Titles.Count == 1)
            {
                Titles[Titles.Count - 1].Left = 0;
            }
            else
            {
                Titles[Titles.Count - 1].Left = Titles[Titles.Count - 2].Right + 10;
            }
            Titles[Titles.Count - 1].AutoSize = true;

            Titles[Titles.Count - 1].Click += new System.EventHandler(this.labelClickEvent);

            panel1.BorderStyle = BorderStyle.FixedSingle;

            panel1.Left = panel2.Left;
            panel1.Width = panel2.Width;
            panel1.Controls.Add(Titles[Titles.Count - 1]);
            panel1.BackColor = Color.Wheat;
            left += 90;

            //ADD Picturbox

            windows.Add(new PictureBox());
            windows[windows.Count - 1].Name = "Adsız" + i.ToString();
            windows[windows.Count - 1].Left = panel1.Left;
            windows[windows.Count - 1].Top = panel2.Bottom + 5;
            windows[windows.Count - 1].Width = panel2.Width;
            windows[windows.Count - 1].Height = this.Height - 100;
            windows[windows.Count - 1].BorderStyle = BorderStyle.FixedSingle;
            windows[windows.Count - 1].BackColor = Color.White;
            windows[windows.Count - 1].Anchor = (AnchorStyles.Left|AnchorStyles.Right);
            

            windows[windows.Count - 1].MouseDown += new MouseEventHandler(this.MouseDownEvent);
            windows[windows.Count - 1].MouseUp += new MouseEventHandler(this.MouseUpEvent); ;
            windows[windows.Count - 1].MouseMove += new MouseEventHandler(this.MouseMoveEvent); ;
            Bitmap resim = new Bitmap(windows[windows.Count - 1].Width, windows[windows.Count - 1].Height);
            windows[windows.Count - 1].Image = resim;
            window = Titles.Count;
            i++;
            Controls.Add(windows[windows.Count - 1]);
        }
    }
}
