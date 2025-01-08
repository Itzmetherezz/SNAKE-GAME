using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        int cols = 50;
        int rows = 25;
        int length = 1; // Initial length of the snake
        int dx = 0;
        int dy = 0;
        int front = 0;
        int back = 0;

        Class1[] snake = new Class1[1250];
        bool[,] visit;
        List<int> available = new List<int>();

        Random rand = new Random();
        Timer timer = new Timer();

        public Form1()
        {
            InitializeComponent();
            this.KeyDown += new KeyEventHandler(OnKeyDown); // Hook up the KeyDown event
            timer.Interval = 100; // Set the timer interval
            timer.Tick += new EventHandler(OnTimerTick); // Hook up the TimerTick event
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            visit = new bool[rows, cols];
            Class1 head = new Class1((rand.Next() % rows) * 10, (rand.Next() % cols) * 10)
            {
                BackColor = Color.Green,
                Size = new Size(10, 10),
                Location = new Point((rand.Next() % rows) * 10, (rand.Next() % cols) * 10)
            };
            lblFood.Location = new Point((rand.Next() % rows) * 10, (rand.Next() % cols) * 10);

            for (int i = 0; i < rows; i++)
                for (int j = 0; j < cols; j++)
                {
                    visit[i, j] = false;
                    available.Add(i * cols + j);
                }

            visit[head.Location.Y / 10, head.Location.X / 10] = true;
            int posY = head.Location.Y / 10;
            int posX = head.Location.X / 10;

            if (posY >= 0 && posY < rows && posX >= 0 && posX < cols)
            {
                int indexToRemove = posY * cols + posX;
                if (indexToRemove >= 0 && indexToRemove < available.Count)
                {
                    available.Remove(indexToRemove);
                }
                else
                {
                    MessageBox.Show("GAME OVER");
                    return;
                }
            }
            else
            {
                MessageBox.Show("GAME OVER");
                return;
            }

            snake[front++] = head;
            Controls.Add(head);
            timer.Start(); // Start the timer
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (dy == 0) { dx = 0; dy = -10; }
                    break;
                case Keys.Down:
                    if (dy == 0) { dx = 0; dy = 10; }
                    break;
                case Keys.Left:
                    if (dx == 0) { dx = -10; dy = 0; }
                    break;
                case Keys.Right:
                    if (dx == 0) { dx = 10; dy = 0; }
                    break;
            }
        }

        private void OnTimerTick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void MoveSnake()
        {
            int newX = snake[front - 1].Location.X + dx;
            int newY = snake[front - 1].Location.Y + dy;

            if (newX < 0 || newX >= cols * 10 || newY < 0 || newY >= rows * 10 || visit[newY / 10, newX / 10])
            {
                timer.Stop();
                MessageBox.Show("Game Over!");
                return;
            }

            Class1 newHead = new Class1(newX, newY)
            {
                BackColor = Color.Green,
                Size = new Size(10, 10),
                Location = new Point(newX, newY)
            };
            visit[newY / 10, newX / 10] = true;
            available.Remove(newY / 10 * cols + newX / 10);
            snake[front++] = newHead;
            Controls.Add(newHead);

            if (length == front - back)
            {
                Class1 tail = snake[back++];
                visit[tail.Location.Y / 10, tail.Location.X / 10] = false;
                available.Add(tail.Location.Y / 10 * cols + tail.Location.X / 10);
                Controls.Remove(tail);
            }
        }
    }
}
