using System.Drawing;

namespace SHAPE 
{
    using System;
    using System.Collections;

    [Serializable]
    public abstract class Shape
    {
        protected int pen_size_White; 
        public Point start;
        public int my_Color;
        public Point Start { get { return start; } set { start = value; } }
        public int Pen_size_White { get { return pen_size_White; } set { pen_size_White = value; } }
        public int My_Color { get { return my_Color; } set { my_Color = value; } }
        public abstract void Draw(Graphics g);
        public abstract bool isInside(int xP, int yP);
    }

    [Serializable]
    public class Line : Shape
    {
        public Point end;
        public Point End { get { return end; } set { end = value; } }
        public Line(int pen_size_pen_size_White, Point start, Point end, int my_color)    
        { this.pen_size_White = pen_size_pen_size_White; this.start = start; this.end = end; my_Color = my_color; }
        public override void Draw(Graphics g)
        {
            Pen pen = new Pen(Color.FromArgb(my_Color), this.pen_size_White);
            g.DrawLine(pen, start, end);
        }
        public override bool isInside(int x_c, int y_c)
        {
            int Min, Max;
            if (start.X < end.X) { Min = start.X; }
            else { Min = end.X; }

            if (start.X > end.X) { Max = start.X; }
            else { Max = end.X;}

            if (x_c < Min || x_c > Max) { return false; }

            if (end.Y == start.Y || end.X == start.X)
                return false;
                
            if (Math.Abs(end.Y - start.Y) <= 20 && Math.Abs(y_c - end.Y) <= 20) { return true; }
               
            if (Math.Abs((end.X - start.X)) <= 20 && Math.Abs(x_c - end.X) <= 20) { return true; }
                
            return Math.Abs((y_c - end.Y) + -1 * ((int)((((double)(end.Y - start.Y) / ((double)(end.X - start.X))) * ((double)(x_c - end.X)))))) <= 15;
        }
        ~Line() { }
    }
    
    [Serializable]
    public abstract class ElipsBlockedByRectangle : Shape
    {
        public int color_in;
        protected float my_width;
        protected float my_height;
        public int Color_in { get { return color_in; } set { color_in = value; } }
        public float My_width { get { return my_width; } set { my_width = value; } }
        public float My_height { get { return my_height; } set { my_height = value; } }
    }
    [Serializable]
    public class MyElips : ElipsBlockedByRectangle
    {
        public MyElips()    
        { }  

        public MyElips(int pen_size_pen_size_White, Point start, int my_color, int color_in, float width, float height)                
        { this.pen_size_White = pen_size_pen_size_White; this.start = start; this.my_Color = my_color; this.color_in = color_in; my_width = width; my_height = height; }

        public override void Draw(Graphics g)
        {
            SolidBrush b = new SolidBrush(Color.FromArgb(color_in));//fill Elips
            g.FillEllipse(b, start.X, start.Y, my_width, my_height);

            Pen pen = new Pen(Color.FromArgb(my_Color), this.pen_size_White);
            g.DrawEllipse(pen, start.X, start.Y, my_width, my_height);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - start.X) <= my_width / 2 && Math.Abs(yP - start.Y) <= my_height / 2;

        }
        ~MyElips() { }
    }

    [Serializable]
    public class MyCircle : MyElips
    {
        private float myRadius;
        public MyCircle(int pen_size_pen_size_White, Point start, int my_color, int color_in, float width, float height, float radius)               
        { this.pen_size_White = pen_size_pen_size_White; this.start = start; this.my_Color = my_color; this.color_in = color_in; my_width = width; my_height = height; myRadius = radius; }
        public float MyRadius { get { return myRadius; } set { myRadius = value; } }

        //Methods
        public override void Draw(Graphics g)
        {
            SolidBrush b = new SolidBrush(Color.FromArgb(color_in));
            Pen pen = new Pen(Color.FromArgb(my_Color), this.pen_size_White);
            g.FillEllipse(b, start.X - myRadius, start.Y - myRadius, 2 * myRadius, 2 * myRadius);
            g.DrawEllipse(pen, start.X - myRadius, start.Y - myRadius, 2 * myRadius, 2 * myRadius);
        }

        public override bool isInside(int xP, int yP)
        {
            return Math.Sqrt((xP - start.X) * (xP - start.X) + (yP - start.Y) * (yP - start.Y)) < myRadius;
        }
        ~MyCircle() { }
    }

    [Serializable]
    public class MyRectangle : ElipsBlockedByRectangle
    {

        public MyRectangle()   
        { }
      

        public MyRectangle(int pen_size_pen_size_White, Point start, int my_color, int color_in, float width, float height)                 //constractor;
        { this.pen_size_White = pen_size_pen_size_White; this.start = start; this.my_Color = my_color; this.color_in = color_in; my_width = width; my_height = height; }
        public override void Draw(Graphics g)
        {
            SolidBrush b = new SolidBrush(Color.FromArgb(color_in));
            g.FillRectangle(b, start.X, start.Y, my_width, my_height);
            Pen pen = new Pen(Color.FromArgb(my_Color), this.pen_size_White);
            g.DrawRectangle(pen, start.X, start.Y, my_width, my_height);

        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - start.X) <= my_width / 2 && Math.Abs(yP - start.Y) <= my_height / 2;
        }
        ~MyRectangle() { }
    }

    [Serializable]
    public class MySquare : MyRectangle 
    {
        public MySquare(int pen_size_pen_size_White, Point start, int my_color, int color_in, float width, float height)                
        { this.pen_size_White = pen_size_pen_size_White; this.start = start; this.my_Color = my_color; this.color_in = color_in; my_width = width; my_height = height; }

        public override void Draw(Graphics g)
        {
            SolidBrush brush = new SolidBrush(Color.Transparent);
            Pen mypen = new Pen(Color.Cyan, 2);
            g.FillRectangle(brush, start.X - my_width / 2, start.Y - my_height / 2, my_width, my_height);
            g.DrawRectangle(mypen, start.X - my_width / 2, start.Y - my_height / 2, my_width, my_height);
        }
        public override bool isInside(int xP, int yP)
        {
            return Math.Abs(xP - start.X) <= my_width / 2 && Math.Abs(yP - start.Y) <= my_height / 2;
        }
        ~MySquare() { }
    }

    [Serializable]

     class ShapeList 
    {
        private SortedList Shape;
        public ShapeList()
        {
            Shape = new SortedList();
        }
        public int NextIndex
        {
            get
            {
                return Shape.Count;
            }
        }
        public Shape this[int index]
        {
            get
            {
                if (index >= Shape.Count)
                    return (Shape)null;
                return (Shape)Shape.GetByIndex(index);
            }
            set
            {
                if (index <= Shape.Count)
                    Shape[index] = value;
            }
        }

        public void removeAll()
        {
            Shape.Clear();

        }

        public void Shape_Remove(int element)
        {
            if (element >= 0 && element < Shape.Count)
            {
                for (int i = element; i < Shape.Count - 1; i++)
                    Shape[i] = Shape[i + 1];
                Shape.RemoveAt(Shape.Count - 1);
            }
        }

        public void RemoveOn()
        {
            
            if (Shape.Count != 0)
             {
               
                Shape.RemoveAt(Shape.Count-1);
                
             }

        }
        
        public void ADD(Shape e)
        {
            Shape.Add(Shape.Count, e); 
        }
       
        public void Shape_DrawAll(Graphics g)
        {
            for (int i = 0; i <Shape.Count; i++)
            {
                ((Shape)Shape[i]).Draw(g);
                
            }                                   
         } 

    }

    [Serializable]
     public class Polygon : Shape
    {
        protected int length;
        protected Point end;
        protected int increment;
        protected int size;
        protected int angle;
        public Polygon(int size_pen, Point start, Point end, int color, int size, int angle, int length, int increment)
        {
            this.size = size;
            this.angle = angle;
            this.length = length;
            this.increment = increment;
            my_Color = color;
            pen_size_White = size_pen;
            this.start = start;
            this.end = end; 
        }

        public override void Draw( Graphics g)
        {
            int angel_original = angle; 
            int lenth_original = this.length;
            int incroment_original = this.increment;
            Point start_or = this.start;
            Point end_or = this.end;

            for (int i = 0; i < size; i++)
            {
                this.end.X = (int)(this.start.X + Math.Cos(angle * 0.017453292519) * length);
                this.end.Y = (int)(this.start.Y + Math.Sin(angle * 0.017453292519) * length);
                Point[] temp = { new Point(this.Start.X, this.Start.Y), new Point(this.end.X, this.end.Y) };
                g.DrawLines(new Pen(Color.FromArgb(my_Color), pen_size_White), temp); 
                this.start.X = this.end.X;
                this.start.Y = this.end.Y;
                angle += angel_original; 
                length += increment;
            }
            this.angle = angel_original;
            this.length = lenth_original;
            this.increment = incroment_original;
            this.end = end_or;
            this.start = start_or;
        }
        public override bool isInside(int xP, int yP) 
        {
            return false;
        }
    }

}




