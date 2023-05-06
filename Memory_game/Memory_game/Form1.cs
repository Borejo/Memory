using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_game
{
    public partial class Form1 : Form
    {
        
        bool firstClick;
        Random random = new Random();
        Stopwatch timing  = new Stopwatch();
        string path = null;
        
        Label firstClicked = null;

        Label secondClicked = null;
        public Form1()
        {
            InitializeComponent();
            AssignIconsToSquares();
        }
        

        private void AssignIconsToSquares()
        {
            firstClick = false;

            List<string> icons = new List<string>()
            {
                "!", "!", "N", "N", ",", ",", "k", "k",
                "b", "b", "v", "v", "w", "w", "z", "z"
            };
            
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;
                if (iconLabel != null)
                {
                    int randomNumber = random.Next(icons.Count);
                    iconLabel.Text = icons[randomNumber];
                    iconLabel.ForeColor = iconLabel.BackColor;
                    icons.RemoveAt(randomNumber);
                }
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {
            if(firstClick == false)
            {
                timing.Restart();
                firstClick = true;
            }
            
            if (timer1.Enabled == true)
                return;

            Label clickedLabel = sender as Label;

            if (clickedLabel != null)
            {
                if (clickedLabel.ForeColor == Color.Black)         
                    return;

                if (firstClicked == null)
                {
                    firstClicked = clickedLabel;
                    firstClicked.ForeColor = Color.Black;
                    return;
                }

                secondClicked = clickedLabel;
                secondClicked.ForeColor = Color.Black;

                CheckForWinner();

                if (firstClicked.Text == secondClicked.Text)
                {
                    firstClicked = null;
                    secondClicked = null;
                    return;
                }
                
                timer1.Start();
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();

            firstClicked.ForeColor = firstClicked.BackColor;
            secondClicked.ForeColor = secondClicked.BackColor;

            firstClicked = null;
            secondClicked = null;

        }

        private void save()
        {
            if (path == null)
            {
                richTextBox1.Text = (DateTime.Now.ToString("dddd, dd MMMM yyyy HH: mm") + " - " + timing.Elapsed.ToString());
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    richTextBox1.SaveFile(saveFileDialog1.FileName);
                    path = saveFileDialog1.FileName.ToString();
                }
                richTextBox1.Dispose();
            }
            else
            {
                try
                {
                        richTextBox1.Text = "";
                        richTextBox1.LoadFile(path);
                        richTextBox1.Text += ("\n" + DateTime.Now.ToString("dddd, dd MMMM yyyy HH: mm") + " - " + timing.Elapsed.ToString());
                        richTextBox1.SaveFile(path);
                        richTextBox1.Dispose() ;
                }
                catch
                {
                    MessageBox.Show("Program will restart...", "⚠️ Do not delete program files while running! ⚠️", MessageBoxButtons.OK);
                    Application.Restart();
                }
            }
        }
        private void CheckForWinner()
        {
            foreach (Control control in tableLayoutPanel1.Controls)
            {
                Label iconLabel = control as Label;

                if (iconLabel != null)
                {
                    if (iconLabel.ForeColor == iconLabel.BackColor)
                        return;
                }
            }

            timing.Stop();
            
            var response = MessageBox.Show("Your time: " + timing.Elapsed + "\n" + "\n" + "Do you want to save your time?", "Congratulations!", MessageBoxButtons.YesNo);
            if (response == DialogResult.Yes)
            {
                save();
            }
            
            var resp = MessageBox.Show("Do you want to start a new game?", "Again?", MessageBoxButtons.YesNo);
            if (resp == DialogResult.Yes)
            {
                AssignIconsToSquares();
            }
            else
            {
                Application.Exit();
            }
        }

        private void opcjeToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssignIconsToSquares();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void showMyResultsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(path);
            }
            
            catch
            {
                MessageBox.Show("You haven't saved any results during this game yet...", "File not found", MessageBoxButtons.OK);
            }
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(" - Memory game \n - Able to save your time \n - File with results (rtf) resets after every restart of the program \n - Enjoy it!", "©️ Matching game (made by Jan Borejko) ©️", MessageBoxButtons.OK);
        }

    }
    
}
