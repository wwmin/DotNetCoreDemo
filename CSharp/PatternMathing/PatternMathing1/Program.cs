using System;

namespace PatternMathing1
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("参考:https://www.cnblogs.com/cgzl/p/11673661.html");

            //var o = new Person("wwmin");
            //if (o is Person p)
            //{
            //    Console.WriteLine(o.Name);
            //}


            //Console.WriteLine(ShowShapeInfo(new Rectangle(1, 1)));
            Console.WriteLine(GetMixedColor(Color.Red, Color.Red));
            Console.ReadKey();
        }
        #region switch和when的正常写法
        /// <summary>
        /// switch 和 when的正常写法
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public bool Print(Person person)
        {
            switch (person)
            {
                case Student s when s.Name == "wwmin":
                    return true;
                case Teacher t when t.Name == "bob":
                    return true;
                default:
                    return false;
            }
        }
        #endregion
        #region 位置匹配模式
        public bool IsFifthGradeMathWithPosition(Student student)
        {
            //return student is Student(_, 5, Teacher(_, "Math"));
            return student is (_, 5, Teacher(_, "Math"));
        }
        #endregion
        #region 属性匹配模式
        public bool IsFifthGradeMathWithAttribute(Student student)
        {
            return student is { Grade: 5, Teacher: { Subject: "Math" } };
        }

        public bool IsFifthGradeMathWithAttribute(Object obj)
        {
            return obj is Student s && s is { Grade: 5, Teacher: { Subject: "Math" } };
        }
        #endregion
        #region Switch表达式
        public static string ShowShapeInfo(object shape) => shape switch
        {
            //Rectangle r => $"矩形:长={r.Length},宽={r.Width}",
            Rectangle r => r switch
            {
                _ when r.Length == r.Width => $"正方形,边长为{r.Length}",
                _ => $"矩形:长={r.Length},宽={r.Width}"
            },
            Circle { Radius: 1 } => "圆形: 半径为1",
            Circle c => $"圆形:半径:{c.Radius}",
            Triangle t => $"三角形:A边={t.SideA},B边={t.SideB},C边={t.SideC}",
            _ => $"未知形状"
        };
        #endregion
        #region 元组匹配模式
        public static Color GetMixedColor(Color c1, Color c2)
        {
            return (c1, c2) switch
            {
                (Color.Red, Color.Blue) => Color.Purple,
                (Color.Blue, Color.Red) => Color.Purple,

                (Color.Yellow, Color.Red) => Color.Orange,
                (Color.Red, Color.Yellow) => Color.Orange,

                (Color.Red, Color.Green) => Color.Brown,
                (Color.Green, Color.Red) => Color.Brown,

                (_, _) when c1 == c2 => c1,
                _ => Color.Unknown
            };
        }
        #endregion
    }

    #region Person/Student/Teacher
    class Person
    {
        public Person(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }

    class Student : Person
    {
        public Student(string name, int grade, Teacher teacher) : base(name)
        {
            Grade = grade;
            Teacher = teacher;
        }

        public int Grade { get; set; }
        public Teacher Teacher { get; set; }

        public void Deconstruct(out string name, out int grade, out Teacher teacher)
        {
            name = Name;
            grade = Grade;
            teacher = Teacher;
        }
    }

    class Teacher : Person
    {
        public Teacher(string name, string subject) : base(name)
        {
            Subject = subject;
        }

        public string Subject { get; set; }

        public void Deconstruct(out string name, out string subject)
        {
            name = Name;
            subject = Subject;
        }
    }
    #endregion
    #region Circle/Rectangle/Triangle
    public class Circle
    {
        public Circle(int radius) => Radius = radius;
        public int Radius { get; set; }
    }

    public class Rectangle
    {
        public Rectangle(int length, int width) => (Length, Width) = (length, width);
        public int Length { get; set; }
        public int Width { get; set; }
    }

    public class Triangle
    {
        public Triangle(int sideA, int sideB, int sideC) => (SideA, SideB, SideC) = (sideA, sideB, sideC);
        public int SideA { get; set; }
        public int SideB { get; set; }
        public int SideC { get; set; }
    }
    #endregion
    #region Enum color
    public enum Color
    {
        Unknown,
        Red,
        Green,
        Blue,
        Purple,
        Orange,
        Brown,
        Yellow
    }
    #endregion
}
