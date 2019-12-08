using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;

namespace VSRPP1
{
    public partial class Form1 : Form
    {
        private const string Path = "D:\\work\\mini draw\\";
        private const string SysPath = "D:\\work\\mini draw sys\\";
        Graphics g;
        Pen pen;
        Point lastPosition;
        bool moving;
        int lineWidth;
        Color backgroundColor;
        List<Line> lines;

        public Form1()
        {
            InitializeComponent();
            lines = new List<Line>();
            lineWidth = 2;

            backgroundColor = Color.FromArgb(224, 224, 224);

            g = backgroundCover.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            pen = new Pen(Color.Black, lineWidth);
            pen.StartCap = pen.EndCap = System.Drawing.Drawing2D.LineCap.Round;
            openFileDialog1.Filter = "Draw files(*.draw)|*.draw";
            saveFileDialog1.Filter = "Draw files(*.draw)|*.draw";
        }

        private void Undo()
        {
            if (lines.Count != 0)
            {
                lines.RemoveAt(lines.Count - 1);
                g.Clear(backgroundColor);
                DrawLines();
            }
        }

        private void ClearAll()
        {
            lines.Clear();
            g.Clear(backgroundColor);
        }

        private void DrawLines()
        {
            foreach(Line line in lines)
            {
                g.DrawLine(new Pen(ColorTranslator.FromHtml(line.Color), lineWidth), line.Start, line.End);
            }
        }

        private void OpenFileWithDialog()
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = openFileDialog1.FileName;

            OpenFile(filename);
        }

        private void SaveFileWithDialog()
        {

            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }

            string filename = saveFileDialog1.FileName;

            SaveFile(filename);
        }

        private void OpenFile(string filename)
        {
            if (File.Exists(filename))
            {
                lines = new List<Line>();
                BinaryFormatter b = new BinaryFormatter();
                FileStream file = File.OpenRead(filename);
                ClearAll();
                lines = (List<Line>)b.Deserialize(file);
                DrawLines();
                file.Close();
            }
        }

        private void SaveFile(string filename)
        {
            BinaryFormatter b = new BinaryFormatter();
            FileStream file = File.OpenWrite(filename);
            b.Serialize(file, lines);
            file.Close();
            File.WriteAllText(SysPath + "last.txt", filename);
        }

        private void FastSave()
        {
            DateTime time = DateTime.Now;
            string filename = time.ToString().Replace('.', ' ').Replace(':', ' ') + ".draw";
            label1.Text = filename;
            SaveFile(Path + filename);
        }

        private void OpenLast()
        {
            string lastFile = File.ReadAllText(SysPath + "last.txt");
            OpenFile(lastFile);
        }

        private void BackgroundCover_MouseDown(object sender, MouseEventArgs e)
        {
            moving = true;
            lastPosition = e.Location;
        }

        private void BackgroundCover_MouseMove(object sender, MouseEventArgs e)
        {
            if (moving) {
                g.Clear(backgroundColor);
                this.DrawLines();
                g.DrawLine(pen, lastPosition, e.Location);
            }
        }

        private void BackgroundCover_MouseUp(object sender, MouseEventArgs e)
        {
            lines.Add(new Line(pen.Color, lastPosition, e.Location));

            moving = false;
        }

        private void UndoToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Undo();
        }

        private void ClearAllToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.ClearAll();
        }

        private void ColorPanel_Click(object sender, System.EventArgs e)
        {
            label1.Text = "Click";
            Panel obj = (Panel)sender;
            pen.Color = obj.BackColor;
        }

        private void OpenToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.OpenFileWithDialog();
        }

        private void SaveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.SaveFileWithDialog();
        }

        private void QuitToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void FastSaveToolStripMenuItem_Click(object sender, System.EventArgs e)
        {
            this.FastSave();
        }

        private void OpenLastToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.OpenLast();
        }
    }
}
