using SHAPE;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Invalidate();
            this.Width = 1130;
            this.Height = 900;
            bm = new Bitmap(canvas.Width, canvas.Height);
            g = Graphics.FromImage(bm);
            g.Clear(Color.White);
            canvas.Image = bm;
        }
        Bitmap bm; 
        Graphics g;
        bool paint = false;
        Point px, py; 
        Pen my_pen = new Pen(Color.Black, 1);
        Pen erase = new Pen(Color.White, 25);
        int my_index;
        int x, y, sX, sY, cX, cY; 
        ColorDialog c = new ColorDialog(); 
        Color new_color;
        float R;
        int S_Original_x = 0, S_Original_y = 0, LS_Original_x = 0, LS_Original_y = 0;
        ShapeList CollectionOfShapes = new ShapeList(); 
        int CurrentIndex;
        Shape S;
        bool isPress = false;
        bool isfill = false;
        bool darkM = false;
        ShapeList myrendo = new ShapeList();
        private void canvas_MouseDown(object sender, MouseEventArgs e)
        {
            paint = true;
            py = e.Location;
            cX = e.X;
            cY = e.Y;
            px = e.Location;
            
            if (my_index == 9)
            {
                canvas.Refresh();
                for (int i = 0; i < CollectionOfShapes.NextIndex; i++)
                {
                        if (CollectionOfShapes[i].isInside(e.X, e.Y)) 
                        {                                          
                            isPress = true;
                            text_user_click.Text = " Shape in move...";
                            CurrentIndex = i; 
                            S = CollectionOfShapes[i];
                            S_Original_x = S.Start.X;
                            S_Original_y = S.Start.Y;
                
                            if (S.GetType() == typeof(Line)) 
                            {
                                LS_Original_x = ((Line)S).end.X;
                                LS_Original_y = ((Line)S).end.Y;
                            }
                            CollectionOfShapes.Shape_Remove(i);
                             canvas.Refresh();
                         }
                }
                
            }

            if (my_index == 7)
            {
                if (isfill == true)
                {
                    for (int i = 0; (i < CollectionOfShapes.NextIndex && isfill); i++)
                    {
                     canvas.Refresh();
                      if (CollectionOfShapes[i].isInside(e.X, e.Y))
                        {
                            CurrentIndex = i;      

                            if (CollectionOfShapes[i].GetType() == typeof(MyElips))
                            {
                                ((MyElips)CollectionOfShapes[i]).Color_in = new_color.ToArgb();

                                ((MyElips)CollectionOfShapes[i]).Draw(g);
                                text_user_click.Text = "Color in Saved(Elips)...";

                            }
                            else if (CollectionOfShapes[i].GetType() == typeof(MyRectangle))
                            {
                                ((MyRectangle)CollectionOfShapes[i]).Color_in = new_color.ToArgb();

                                ((MyRectangle)CollectionOfShapes[i]).Draw(g);
                                text_user_click.Text = "Color in Saved(Rectangle)...";
                            }
                            else if (CollectionOfShapes[i].GetType() == typeof(MyCircle))
                            {
                                ((MyCircle)CollectionOfShapes[i]).Color_in = new_color.ToArgb();

                                ((MyCircle)CollectionOfShapes[i]).Draw(g);
                                text_user_click.Text = "Color in Saved(Circle)...";
                            }

                            else if (CollectionOfShapes[i].GetType() == typeof(MySquare))
                            {
                                ((MySquare)CollectionOfShapes[i]).my_Color = new_color.ToArgb();

                                ((MySquare)CollectionOfShapes[i]).Draw(g);
                                text_user_click.Text = "Color in Saved(Square)...";

                            }
                            canvas.Refresh();


                            if (isfill)
                                isfill = false;
                        }

                    }
                }
                isfill = true;
                canvas.Invalidate();
                    canvas.Image = bm;
                if (darkM == true)
                {

                    g.Clear(Color.Black);
                }
                else
                    g.Clear(Color.White);

                    CollectionOfShapes.Shape_DrawAll(g);
            }
 
        }
        private void canvas_MouseUp(object sender, MouseEventArgs e)
        {
            paint = false;
            sX = x - cX; 
            sY = y - cY;
            py = e.Location;

            CurrentIndex = -1;
            if (CurrentIndex < 0)
            {
                switch (my_index)  
                {
                    case 3://Elips

                        CollectionOfShapes[CollectionOfShapes.NextIndex] = new MyElips(((int)my_pen.Width), px, my_pen.Color.ToArgb(), Color.Transparent.ToArgb(), sX, sY);
                        CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);
                        canvas.Invalidate();
                        break;

                    case 4://Line

                        CollectionOfShapes[CollectionOfShapes.NextIndex] = new Line(((int)my_pen.Width), new Point(cX, cY), new Point(x, y), my_pen.Color.ToArgb());
                        CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);
                        break;

                    case 5://Rectangle 

                        CollectionOfShapes[CollectionOfShapes.NextIndex] = new MyRectangle(((int)my_pen.Width), new Point(cX, cY), my_pen.Color.ToArgb(), Color.Transparent.ToArgb(), sX, sY);
                        CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);
                        break;

                    case 8://Circle
                        try
                        {
                            R = (sY / 2) + ((sX * sX) / (8 * sY)); //calculate radius
                            CollectionOfShapes[CollectionOfShapes.NextIndex] = new MyCircle(((int)my_pen.Width), px, my_pen.Color.ToArgb(), Color.Transparent.ToArgb(), sX, sY, R);
                            CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);

                        }
                        catch (DivideByZeroException)
                        {
                            textBoxUserError.Text = "you tryng to Draw invald Circle...";
                            break;

                        }
                        break;

                    case 10://Square
                        CollectionOfShapes[CollectionOfShapes.NextIndex] = new MyRectangle(((int)my_pen.Width), new Point(cX, cY), my_pen.Color.ToArgb(), Color.Transparent.ToArgb(), sY, sY);
                        CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);
                        break;


                }
                CurrentIndex = CollectionOfShapes.NextIndex;

            }

            if (my_index == 9)
            {
                if (isPress)
                {
                    CollectionOfShapes[CurrentIndex] = S; 
                    
                    g.Clear(Color.White);
                    canvas.Image = bm;

                    S = null;
                    isPress = false;
                   
                    CurrentIndex = -1;
                    CollectionOfShapes.Shape_DrawAll(g);
                }

            }
            textBox2.Text = CollectionOfShapes.NextIndex.ToString();
        }

        private void canvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (my_index == 9)
            {
                if (isPress)
                {
                    if (darkM == true)
                    {

                        g.Clear(Color.Black);
                    }
                    else
                    g.Clear(Color.White);
                    S.start.X = sX + S_Original_x;
                    S.start.Y = sY + S_Original_y;

                    if (S.GetType() == typeof(Line))
                    {
                        ((Line)S).end.X = sX + LS_Original_x;   
                        ((Line)S).end.Y = sY + LS_Original_y;

                    }
    
                    g.Clear(Color.White);
                    S.Draw(g);
                    canvas.Invalidate();
                    canvas.Image = bm;
                    CollectionOfShapes.Shape_DrawAll(g);
                    py = px;
                }

            }

            if (paint)
            {
                CurrentIndex = -1;
                if (CurrentIndex < 0)
                {
                    switch (my_index)
                    {
                        case 1:
                            px = e.Location;
                            CollectionOfShapes[CollectionOfShapes.NextIndex] = new Line(((int)my_pen.Width), px, new Point(x, y), my_pen.Color.ToArgb());
                            CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);
                            py = px;
                            break;

                        case 2://eraser 
                            px = e.Location;
                            CollectionOfShapes[CollectionOfShapes.NextIndex] = new Line(((int)my_pen.Width) + 30, px, new Point(x, y), erase.Color.ToArgb());
                            CollectionOfShapes[CollectionOfShapes.NextIndex - 1].Draw(g);
                            py = px;
                            break;
                    }

                    CurrentIndex = CollectionOfShapes.NextIndex - 1;

                    canvas.Invalidate();

                }
            }

            canvas.Refresh();
            x = e.X;
            y = e.Y;
            sX = e.X - cX;//width 
            sY = e.Y - cY;//height
            
        }
        private void canvs_paint(object sender, PaintEventArgs e)
        {
            sX = (x - cX);
            sY = (y - cY);
            Graphics g = e.Graphics;

            //Calculate the rectangle coordinates
            int sx_abs = Math.Abs(x - cX); int sy_abs = Math.Abs(y - cY);
            int minX = x > cX ? cX : x;
            int minY = y > cY ? cY : y;
            int maxX = x < cX ? cX : x;
            int maxY = y < cY ? cY : y;
            float R_changed = R;

            if (paint)
            {
                if (my_index == 3) { g.DrawEllipse(my_pen, cX, cY, sX, sY); }
                if (my_index == 4) { g.DrawLine(my_pen, cX, cY, x, y); }
                if (my_index == 5) { g.DrawRectangle(my_pen, minX, minY, sx_abs, sy_abs); }
                if (my_index == 10) { g.DrawRectangle(my_pen, minX, minY, sy_abs, sy_abs); }

                if (my_index == 8)
                {
                    try { R_changed = (sY / 2) + ((sX * sX) / (8 * sY)); }
                    catch (DivideByZeroException) { textBoxUserError.Text = "you tryng to Draw invald Circle..."; }
                    g.DrawEllipse(my_pen, cX - R_changed, cY - R_changed, 2 * R_changed, 2 * R_changed);
                }


                CollectionOfShapes.Shape_DrawAll(g);

            }
        }
        private void button16_Click(object sender, EventArgs e)
        {
            my_index = 15;
            c.ShowDialog();
            new_color = c.Color;
            canvas.BackColor = c.Color;
            pictureBox1.BackColor = c.Color;
            my_pen.Color = c.Color;
            text_user_click.Text = "change Color...";
        }
        private void button4_Click(object sender, EventArgs e) //pencil btn
        {
            my_index = 1;
            text_user_click.Text = "Drawing Free...(Slow Undo!)";
        }
        private void button9_Click(object sender, EventArgs e) //Rectangle 
        {
            my_index = 5;
            text_user_click.Text = "Drawing Rectangle...";
        }
        private void setPenWidth()
        {
            my_pen.Width = (float)numericUpDown4.Value;
        }

        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            float f = (float)(trackBar4.Value) / 10;
            numericUpDown4.Value = new decimal(f);
            setPenWidth();
            text_user_click.Text = "change brush Size...";
        }

        private void button7_Click(object sender, EventArgs e)
        {
            my_index = 3;
            text_user_click.Text = "Drawing ellipse...";
        }

        private void button8_Click(object sender, EventArgs e) 
        {
            my_index = 4;
            text_user_click.Text = "Drawing Line...";
        }

        private void button1_Click_1(object sender, EventArgs e) 
        {
            my_index = 2;
            text_user_click.Text = "erase";
        }

        private void FillShap(object sender, EventArgs e)
        {
            my_index = 7;
            isfill = true;
            text_user_click.Text = "fill Shape...";
        }

        private void button12_Click(object sender, EventArgs e)//Exit btn
        {

            string message = "Do you want to close this window?";
            string title = "Close Window";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                Close();
            }
            else
            {
                MessageBox.Show("Good,Continue to draw...");
            }
        }
        private void btn_save_Click(object sender, EventArgs e) 
        {

            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.InitialDirectory = Directory.GetCurrentDirectory();
            saveFile.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            saveFile.FilterIndex = 1;
            saveFile.RestoreDirectory = true;
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                IFormatter formatter = new BinaryFormatter();
                using (Stream stream = new FileStream(saveFile.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                {

                    formatter.Serialize(stream, CollectionOfShapes);
                }
            }

            text_user_click.Text = "Save...";
        }

        private void button15_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "model files (*.mdl)|*.mdl|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Stream stream = File.Open(openFileDialog1.FileName, FileMode.Open);
                var binaryFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                CollectionOfShapes = (ShapeList)binaryFormatter.Deserialize(stream);
                canvas.Invalidate();
                if (my_index == 20)
                {
                    canvas.BackColor = Color.Black;
                    g.Clear(Color.Black);
                    
                }
                CollectionOfShapes.Shape_DrawAll(g);
            }

        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)//whitemode
        {
            my_index = 35;
            darkM =false;
            string message = "Do you want to change mode,if yes it will delate your draw?";
            string title = "Change mode color to white mode";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                CollectionOfShapes.removeAll();
                g.Clear(Color.White);
                canvas.BackColor = Color.White;
                canvas.Invalidate();
                my_pen = new Pen(Color.Black, 1);
                erase = new Pen(Color.White, 25);
            }
            else
            {
                MessageBox.Show("Good,Continue to draw...");
            }

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            my_index = 21;
           
            radioButton3.Checked = true;
            string message = "Do you want to change mode,if yes it will delate your draw?";
            string title = "Change mode color to dark mode";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                 darkM = true;
                CollectionOfShapes.removeAll();
                canvas.Refresh();
                canvas.Invalidate();
                canvas.BackColor = Color.Black;
                g.Clear(Color.Black);
                my_pen = new Pen(Color.White, 1);
                erase = new Pen(Color.Black, 25);
                text_user_click.Text = "dark mode, just move(whitout fill)/fill...";
            }
            else
            {
                MessageBox.Show("Good,Continue to draw...");
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            text_user_click.Text = "Clear all...";
            textBox2.Text = "0";
            g.Clear(Color.White);
            canvas.Image = bm;
            my_index = 0;
            CollectionOfShapes.removeAll();
            radioButton3.Checked = false;
            radioButton1.Checked = false;
            darkM = false;

        }

        private void button5_Click(object sender, EventArgs e)//Circle btn
        {
            my_index = 8;
            text_user_click.Text = "Circle Drawing...";
        }


        private void button1_Click(object sender, EventArgs e)//Move Shape btn
        {
            my_index = 9;
            text_user_click.Text = "Move Shape...";


        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            my_index = 10;
            text_user_click.Text = "Square Drawing...";
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            this.process1.StartInfo.FileName = "C:\\Program Files (x86)\\Google\\Chrome\\Application\\chrome.exe";
            this.process1.Start();

        }

        private void color_picker_Click(object sender, EventArgs e)
        {
            my_index = 7;
            isfill = true;
            text_user_click.Text = "Color selected...";

        }

        private void canvas_Click(object sender, EventArgs e)
        {

        }

        private void btn_polygon_Click(object sender, EventArgs e)
        {
            Polygon poly = new Polygon(5, new Point(canvas.Width / 2, canvas.Height / 2), new Point(canvas.Width / 2 + 50, canvas.Height / 2 + 50), my_pen.Color.ToArgb(), Int32.Parse(box_points.Text), Int32.Parse(box_angel.Text), Int32.Parse(box_length.Text), Int32.Parse(books_increm.Text));
            CollectionOfShapes.ADD(poly);
            text_user_click.Text = "Drawing Polygon...";
            poly.Draw(g);
        }

        private void box_points_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_SizeChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            my_index = 20;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            my_index = 20;
            
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void button5_Click_2(object sender, EventArgs e)
        {

                my_index = 30;

                if (myrendo.NextIndex != 0)
                {
                    CollectionOfShapes.ADD(myrendo[myrendo.NextIndex - 1]);
                    myrendo.RemoveOn();
                    g.Clear(Color.White);
                    canvas.Invalidate();
                    CollectionOfShapes.Shape_DrawAll(g);
                    text_user_click.Text = "rendo...";
                }
                else
                    text_user_click.Text = "No rendo...";
            

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)//undo
        {
            my_index = 13;

            if (CollectionOfShapes.NextIndex != 0)
            {
                myrendo.ADD(CollectionOfShapes[CollectionOfShapes.NextIndex - 1]); 
                CollectionOfShapes.RemoveOn();
                text_user_click.Text = "undo...";
                g.Clear(Color.White);
                canvas.Invalidate();
                CollectionOfShapes.Shape_DrawAll(g);

            }
            else
                text_user_click.Text = "Empty drawing ,No undo...";
            textBox2.Text = CollectionOfShapes.NextIndex.ToString();
        }



        private void label2_Click(object sender, EventArgs e)
        {
            my_index = 11;
            text_user_click.Text = "Pull the margins to the right";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            try
            {
                // open file dialog   
                OpenFileDialog open = new OpenFileDialog();
                // image filters  
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box
                    pictureBox2.Image = new Bitmap(open.FileName); 
                    //image file path
                    textBox1.Text = open.FileName.ToString();
                }
            }
            catch 
            {
                MessageBox.Show("Imageinvalid...");
            }

        }

        private void validate(Bitmap bm, Stack<Point> sp, int x, int y, Color old_color, Color new_color)
        {
            Color cx = bm.GetPixel(x, y);
            if (cx == old_color)
            {
                sp.Push(new Point(x, y));
                bm.SetPixel(x, y, new_color);
            }
        }

        public void Fill(Bitmap bm, int x, int y, Color new_clr)
        {

            Color old_color = bm.GetPixel(x, y);
            Stack<Point> pixel = new Stack<Point>();
            pixel.Push(new Point(x, y));
            bm.SetPixel(x, y, new_clr);
            if (old_color == new_clr) return;

            while (pixel.Count > 0)              
            {
                Point pt = (Point)pixel.Pop();
                if (pt.X > 0 && pt.Y > 0 && pt.X < bm.Width - 1 && pt.Y < bm.Height - 1)
                {
                    validate(bm, pixel, pt.X - 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y - 1, old_color, new_clr);
                    validate(bm, pixel, pt.X + 1, pt.Y, old_color, new_clr);
                    validate(bm, pixel, pt.X, pt.Y + 1, old_color, new_clr);
                }
            }
        }


        private void canvas_MouseClick(object sender, MouseEventArgs e)
        {
            if (my_index == 7)
            {
                Point point = set_point(canvas, e.Location);
                Fill(bm, point.X, point.Y, new_color);
            }
        }
        static Point set_point(PictureBox pb, Point pt)
        {
            float px = 1f * pb.Image.Width / pb.Width;
            float py = 1f * pb.Image.Height / pb.Height;
            return new Point((int)(pt.X * px), (int)(pt.Y * py));
        }
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                // open file dialog   
                OpenFileDialog open = new OpenFileDialog();
                // image filters  
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    // display image in picture box
                    pictureBox2.Image = new Bitmap(open.FileName); 
                    //image file path
                    textBox1.Text = open.FileName.ToString();
                }

            }
            catch 
            {
                MessageBox.Show("Imageinvalid...");
            }
        }
        private void pictureBox2_MouseClick(object sender, MouseEventArgs e)
        {
            Point point = set_point(color_picker, e.Location);//
            pictureBox1.BackColor = ((Bitmap)color_picker.Image).GetPixel(point.X, point.Y);
            new_color = pictureBox1.BackColor;
            my_pen.Color = pictureBox1.BackColor;
        }
    }
}


