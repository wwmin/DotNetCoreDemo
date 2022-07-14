namespace ConsoleApp1;

public class Person
{

    /// <summary>
    /// 构造函数
    /// </summary>
    public Person(int age, string name)
    {
        Age = age;
        Name = name;
    }
    public string Name { get; set; }
    public int Age { get; set; }

    public override string ToString()
    {
        return $"Name:{Name}, Age:{Age}";
    }
}
