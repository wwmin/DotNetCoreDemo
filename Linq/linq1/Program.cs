using System;
using System.Collections.Generic;
using System.Linq;

namespace linq1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Select1();

            SelectMany();

            Join();

            GroupJoin();

            GroupBy();

            Concat();
        }

        static void Select1()
        {
            Console.WriteLine("==========linq : select==========");
            //使用集合初始化器给集合赋值
            List<Employee> emp = new List<Employee>
            {
               new Employee(){Id=Guid.NewGuid(),Name="张三",Sex=0,CompanyName="xx技术有限公司"},
               new Employee(){Id=Guid.NewGuid(),Name="李四",Sex=0,CompanyName="xx培训"},
               new Employee(){Id=Guid.NewGuid(),Name="王五",Sex=0,CompanyName="xx集团"}
            };

            //查询表达式: 不能省略最后的select
            var query = (from p in emp where p.Name.StartsWith("王") select p).FirstOrDefault();

            //查询方法: 涉及到Lambda表达式,全部返回,可以省略最后的select , 延迟加载
            var query1 = emp.Where(p => p.Name.StartsWith("王")).Select(e => new { e.Name, e.CompanyName });

            //查询方法: 返回匿名类
            var query2 = emp.Where(p => p.Name.StartsWith("王")).Select(p => p);
            foreach (var item in query1)
            {
                Console.WriteLine(item.Name);
            }
            Console.ReadKey();
        }

        static void SelectMany()
        {
            //使用集合初始化器初始化Teacher集合
            List<Teacher> teachers = new List<Teacher> {
               new Teacher("徐老师",
               new List<Student>(){
                 new Student("宋江",80),
                new Student("卢俊义",95),
                new Student("朱武",45)
               }
               ),
                new Teacher("姜老师",
               new List<Student>(){
                 new Student("林冲",90),
                new Student("花荣",85),
                new Student("柴进",58)
               }
               ),
               new Teacher("樊老师",
               new List<Student>(){
                 new Student("关胜",100),
                new Student("阮小七",70),
                new Student("时迁",30)
               }
               )
            };

            //查询score小于60的学生
            //方法1: 循环遍历,会有性能损失
            foreach (Teacher t in teachers)
            {
                foreach (Student s in t.Students)
                {
                    if (s.Score < 60)
                    {
                        Console.WriteLine("姓名:" + s.Name + ",成绩:" + s.Score);
                    }
                }
            }
            //查询表达式
            //方法2：使用SelectMany  延迟加载：在不需要数据的时候，就不执行调用数据，能减轻程序和数据库的交互，可以提供程序的性能，执行循环的时候才去访问数据库取数据            
            //直接返回学生的数据
            var query = from t in teachers
                        from s in t.Students
                        where s.Score < 60
                        select s;
            foreach (var item in query)
            {
                Console.WriteLine("姓名:" + item.Name + ",成绩:" + item.Score);
            }

            //同时返回老师的数据
            var query1 = from t in teachers
                         from s in t.Students
                         where s.Score < 60
                         select new
                         {
                             t,
                             teacherName = t.Name,
                             student = t.Students.Where(p => p.Score < 60).ToList()
                         };
            foreach (var item in query1)
            {
                Console.WriteLine("老师姓名:" + item.teacherName + ",学生姓名:" + item.student.FirstOrDefault().Name + ",成绩:" + item.student.FirstOrDefault().Score);
            }

            // 使用匿名类 返回老师和学生的数据
            var query2 = from t in teachers
                         from s in t.Students
                         where s.Score < 60
                         select new { teacherName = t.Name, studentName = s.Name, studentScore = s.Score };
            foreach (var item in query2)
            {
                Console.WriteLine("老师姓名:" + item.teacherName + ",学生姓名:" + item.studentName + ",成绩:" + item.studentScore);
            }

            //使用查询方法
            var query3 = teachers.SelectMany(p => p.Students.Where(t => t.Score < 60).ToList());
            foreach (var item in query3)
            {
                Console.WriteLine("姓名:" + item.Name + ",成绩:" + item.Score);
            }
            Console.ReadKey();
        }


        static void Join()
        {
            Console.WriteLine("===========Join===============");
            // 初始化数据
            List<Category> listCategory = new List<Category>()
            {
              new Category(){ Id=1,CategoryName="计算机",CreateTime=DateTime.Now.AddYears(-1)},
              new Category(){ Id=2,CategoryName="文学",CreateTime=DateTime.Now.AddYears(-2)},
              new Category(){ Id=3,CategoryName="高校教材",CreateTime=DateTime.Now.AddMonths(-34)},
              new Category(){ Id=4,CategoryName="心理学",CreateTime=DateTime.Now.AddMonths(-34)}
            };
            List<Product> listProduct = new List<Product>()
            {
               new Product(){Id=1,CategoryId=1, Name="C#高级编程第10版", Price=100.67,CreateTime=DateTime.Now},
               new Product(){Id=2,CategoryId=1, Name="Redis开发和运维", Price=69.9,CreateTime=DateTime.Now.AddDays(-19)},
               new Product(){Id=3,CategoryId=2, Name="活着", Price=57,CreateTime=DateTime.Now.AddMonths(-3)},
               new Product(){Id=4,CategoryId=3, Name="高等数学", Price=97,CreateTime=DateTime.Now.AddMonths(-1)},
               new Product(){Id=5,CategoryId=6, Name="国家宝藏", Price=52.8,CreateTime=DateTime.Now.AddMonths(-1)}
            };

            //1. 查询表达式
            var queryExpress = from c in listCategory
                               join p in listProduct on c.Id equals p.CategoryId
                               select new { Id = c.Id, CategoryName = c.CategoryName, ProductName = p.Name, PublishTime = p.CreateTime };
            Console.WriteLine("查询表达式输出:");
            foreach (var item in queryExpress)
            {
                Console.WriteLine($"id:{item.Id},CategoryName:{item.CategoryName},ProductName:{item.ProductName},PublishTime:{item.PublishTime}");
            }
            //方法语法
            Console.WriteLine("方法语法输出:");
            var queryFun = listCategory.Join(listProduct, c => c.Id, p => p.CategoryId, (c, p) =>
            new { Id = c.Id, CategoryName = c.CategoryName, ProductName = p.Name, PublishTime = p.CreateTime });
            foreach (var item in queryFun)
            {
                Console.WriteLine($"id:{item.Id},CategoryName:{item.CategoryName},ProductName:{item.ProductName},PublishTime:{item.PublishTime}");
            }

            Console.ReadKey();
        }

        static void GroupJoin()
        {
            Console.WriteLine("===========GroupJoin===============");
            // 初始化数据
            List<Category> listCategory = new List<Category>()
            {
              new Category(){ Id=1,CategoryName="计算机",CreateTime=DateTime.Now.AddYears(-1)},
              new Category(){ Id=2,CategoryName="文学",CreateTime=DateTime.Now.AddYears(-2)},
              new Category(){ Id=3,CategoryName="高校教材",CreateTime=DateTime.Now.AddMonths(-34)},
              new Category(){ Id=4,CategoryName="心理学",CreateTime=DateTime.Now.AddMonths(-34)}
            };
            List<Product> listProduct = new List<Product>()
            {
               new Product(){Id=1,CategoryId=1, Name="C#高级编程第10版", Price=100.67,CreateTime=DateTime.Now},
               new Product(){Id=2,CategoryId=1, Name="Redis开发和运维", Price=69.9,CreateTime=DateTime.Now.AddDays(-19)},
               new Product(){Id=3,CategoryId=2, Name="活着", Price=57,CreateTime=DateTime.Now.AddMonths(-3)},
               new Product(){Id=4,CategoryId=3, Name="高等数学", Price=97,CreateTime=DateTime.Now.AddMonths(-1)},
               new Product(){Id=5,CategoryId=6, Name="国家宝藏", Price=52.8,CreateTime=DateTime.Now.AddMonths(-1)}
            };
            Console.WriteLine("===========Join Left===============");
            // 1. 使用查询表达式实现左连接
            var listLeft = from c in listCategory
                           join p in listProduct on c.Id equals p.CategoryId
                           into cpList
                           from cp in cpList.DefaultIfEmpty()
                           select new
                           {
                               Id = c.Id,
                               CategoryName = c.CategoryName,
                               ProductName = cp == null ? "无产品名称" : cp.Name,
                               PublishTime = cp == null ? "无创建时间" : cp.CreateTime.ToString()
                           };
            foreach (var item in listLeft)
            {
                Console.WriteLine($"id:{item.Id},CategoryName:{item.CategoryName},ProductName:{item.ProductName},PublishTime:{item.PublishTime}");
            }

            Console.ReadKey();
            Console.WriteLine("===========Join Right===============");
            // 使用查询表达式实现右连接
            var listRight = from p in listProduct
                            join c in listCategory on p.CategoryId equals c.Id
                            into pcList
                            from pc in pcList.DefaultIfEmpty()
                            select new
                            {
                                Id = p.Id,
                                CategoryName = pc == null ? "无分类名称" : pc.CategoryName,
                                ProductName = p.Name,
                                PublishTime = p.CreateTime
                            };
            foreach (var item in listRight)
            {
                Console.WriteLine($"id:{item.Id},CategoryName:{item.CategoryName},ProductName:{item.ProductName},PublishTime:{item.PublishTime}");
            }
            Console.ReadKey();
            Console.WriteLine("===========GroupJoin Left===============");
            var listLeftFun = listCategory.GroupJoin(listProduct, c => c.Id, p => p.CategoryId, (c, listp) => listp.DefaultIfEmpty(new Product()).Select(z => new
            {
                Id = c.Id,
                CategoryName = c.CategoryName,
                ProduceName = z.Name,
                ProductPrice = z.Price
            })).ToList();

            Console.WriteLine("===========GroupJoin Right===============");

            var listRightFun = listProduct.GroupJoin(listCategory, c => c.CategoryId, p => p.Id, (c, plist) => plist.DefaultIfEmpty(new Category()).Select(z => new
            {
                id = c.Id,
                ProductName = c.Name,
                ProductPrice = c.Price,
                CategoreName = z.CategoryName
            }));
            foreach (var item in listRightFun)
            {
                foreach (var p in item)
                {
                    Console.WriteLine($"id:{p.id},ProduceName:{p.ProductName},ProductPrice:{p.ProductPrice},CategoryName:{p.CategoreName}");
                }
            }
            Console.ReadKey();
        }

        static void GroupBy()
        {
            Console.WriteLine("=========GroupBy==============");
            List<Product> listProduct = new List<Product>()
            {
               new Product(){Id=1,CategoryId=1, Name="C#高级编程第10版", Price=100.67,CreateTime=DateTime.Now},
               new Product(){Id=2,CategoryId=1, Name="Redis开发和运维", Price=69.9,CreateTime=DateTime.Now.AddDays(-19)},
               new Product(){Id=3,CategoryId=2, Name="活着", Price=57,CreateTime=DateTime.Now.AddMonths(-3)},
               new Product(){Id=4,CategoryId=3, Name="高等数学", Price=97,CreateTime=DateTime.Now.AddMonths(-1)},
               new Product(){Id=5,CategoryId=6, Name="国家宝藏", Price=52.8,CreateTime=DateTime.Now.AddMonths(-1)}
            };
            // 查询表达式          
            var listExpress = from p in listProduct group p by p.CategoryId;
            Console.WriteLine("输出查询表达式结果");
            foreach (var item in listExpress)
            {
                Console.WriteLine($"CategoryId:{item.Key}");
                foreach (var p in item)
                {
                    Console.WriteLine($"ProduceName:{p.Name},ProductPrice:{p.Price},PublishTime:{p.CreateTime}");
                }
            }
            Console.WriteLine("***************************************");
            // 查询方法
            var listFun = listProduct.GroupBy(p => p.CategoryId);
            Console.WriteLine("输出方法语法结果");
            foreach (var item in listFun)
            {
                Console.WriteLine($"CategoryId:{item.Key}");
                foreach (var p in item)
                {
                    Console.WriteLine($"ProduceName:{p.Name},ProductPrice:{p.Price},PublishTime:{p.CreateTime}");
                }
            }
            Console.ReadKey();

            Console.WriteLine("=========GroupBy 多个分组条件==============");
            // 查询表达式
            var list1 = from p in listProduct group p by new { p.CategoryId, p.Price };
            Console.WriteLine("查询表达式方式1输出：");
            foreach (var item in list1)
            {
                Console.WriteLine("key:" + item.Key);
                foreach (var subItem in item)
                {
                    Console.WriteLine($"ProduceName:{subItem.Name},ProductPrice:{subItem.Price},PublishTime:{subItem.CreateTime}");
                }
            }

            var listExpress1 = from p in listProduct
                               group p by new { p.CategoryId, p.Price } into r  // 使用into把数据填充到局部变量r中，然后select筛选数据
                               select new { key = r.Key, ListGroup = r.ToList() };
            Console.WriteLine("查询表达式方式2输出：");
            foreach (var item in listExpress1)
            {
                Console.WriteLine("key:" + item.key);
                foreach (var subItem in item.ListGroup)
                {
                    Console.WriteLine($"ProduceName:{subItem.Name},ProductPrice:{subItem.Price},PublishTime:{subItem.CreateTime}");
                }
            }
            // 方法语法
            var listFun1 = listProduct.GroupBy(p => new { p.CategoryId, p.Price }).Select(g => new { key = g.Key, ListGroup = g.ToList() });

            Console.WriteLine("方法语法输出：");
            foreach (var item in listFun1)
            {
                Console.WriteLine("key:" + item.key);
                foreach (var subItem in item.ListGroup)
                {
                    Console.WriteLine($"ProduceName:{subItem.Name},ProductPrice:{subItem.Price},PublishTime:{subItem.CreateTime}");
                }
            }
            var ll = listProduct.GroupBy(p => p.CategoryId).Select(p => p.ToList()).ToList();//此时ll的类型为List<List<Product>> 这在返回结果中的结构很有用
            Console.ReadKey();
        }

        
        static void Concat()
        {
            // 初始化数据
            List<Category> listCategory = new List<Category>()
            {
              new Category(){ Id=1,CategoryName="计算机",CreateTime=DateTime.Now.AddYears(-1)},
              new Category(){ Id=2,CategoryName="文学",CreateTime=DateTime.Now.AddYears(-2)},
              new Category(){ Id=3,CategoryName="高校教材",CreateTime=DateTime.Now.AddMonths(-34)},
              new Category(){ Id=4,CategoryName="心理学",CreateTime=DateTime.Now.AddMonths(-34)}
            };
            List<Product> listProduct = new List<Product>()
            {
               new Product(){Id=1,CategoryId=1, Name="C#高级编程第10版", Price=100.67,CreateTime=DateTime.Now},
               new Product(){Id=2,CategoryId=1, Name="Redis开发和运维", Price=69.9,CreateTime=DateTime.Now.AddDays(-19)},
               new Product(){Id=3,CategoryId=2, Name="活着", Price=57,CreateTime=DateTime.Now.AddMonths(-3)},
               new Product(){Id=4,CategoryId=3, Name="高等数学", Price=97,CreateTime=DateTime.Now.AddMonths(-1)},
               new Product(){Id=5,CategoryId=6, Name="国家宝藏", Price=52.8,CreateTime=DateTime.Now.AddMonths(-1)}
            };

            // 查询表达式
            var newList = (from p in listProduct
                           select p.Name).Concat(from c in listCategory select c.CategoryName);
            Console.WriteLine("查询表达式输出:");
            foreach (var item in newList)
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("*************************");

            // 方法语法
            var newListFun = listProduct.Select(p => p.Name).Concat(listCategory.Select(c => c.CategoryName));
            Console.WriteLine("方法语法输出:");
            foreach (var item in newListFun)
            {
                Console.WriteLine(item);
            }

            Console.ReadKey();
        }
    }


    #region Employee
    class Employee
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Sex { get; set; }
        public string CompanyName { get; set; }
    }
    #endregion

    #region Category Product
    class Category
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreateTime { get; set; }
    }

    class Product
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public DateTime CreateTime { get; set; }
    }
    #endregion

    #region Teacher Student
    class Teacher
    {
        public string Name { get; set; }
        public List<Student> Students { get; set; }
        public Teacher(string name, List<Student> students)
        {
            this.Name = name;
            this.Students = students;
        }
    }

    class Student
    {
        public string Name { get; set; }
        public int Score { get; set; }
        public Student(string name, int score)
        {
            this.Name = name;
            this.Score = score;
        }
    }
    #endregion
}
