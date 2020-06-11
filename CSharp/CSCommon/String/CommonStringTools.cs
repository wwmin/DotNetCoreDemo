using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CSCommon.String
{
    /// <summary>
    /// 字符串操作知识点汇总
    /// </summary>
    public class CommonStringTools
    {
        public void RunTest()
        {
            //逐字字符串：转义符
            var fileName = @"c:\temp\newfile.txt";
            Console.WriteLine(fileName);//=>c:\temp\newfile.txt

            //逐字字符串：多行文本
            var multiLine = @"This is a
                            multiline paragraph.";
            Console.WriteLine(multiLine);//=>This is a
                                         //multiline paragraph.

            // 非逐字字符串
            var escapedFileName = "c:\temp\newfile.txt";
            Console.WriteLine(escapedFileName);//=>c:      emp

            //逐字字符串中唯一不被原样解释的字符是双引号。由于双引号是定义字符串的关键字，所以在逐字字符串中要表达双引号需要双引号进行转移。
            var str = @"""I don't think so"",he said.";
            Console.WriteLine(str);//=>"I don't think so",he said.

            //在逐字字符串中也可以 $ 符号实现字符串内插值
            Console.WriteLine($@"Testing \n 1 2 {5 - 2}");//=>Testing \n 1 2 3

            //数字格式化方法为：
            //string.Format("{index[:format]}", number);
            //“#”描述：占位符，如果可能，填充位
            Console.WriteLine(string.Format("{0:######}", 1234));//=>1234
            Console.WriteLine(string.Format("{0:000000}", 1234));//=>001234
            Console.WriteLine(string.Format("{0:#0####}", 1234));//=>01234
            Console.WriteLine(string.Format("{0:0#0####}", 1234));//=>0001234

            //“."描述：小数点
            Console.WriteLine(string.Format("{0:000.000}", 1234));//=>1234.000
            Console.WriteLine(string.Format("{0:000.000}", 4321.12543));//=>4321.125

            //“，”描述：千分表示
            Console.WriteLine(string.Format("{0:0,0}", 1234567));//=>1,234,567

            //“%”描述：格式化为百分数
            Console.WriteLine(string.Format("{0:0%}", 1234));//=>123400%;
            Console.WriteLine(string.Format("{0:#%}", 1234.125));//=>123413%
            Console.WriteLine(string.Format("{0:0.00%}", 1234));//=>123400.00%
            Console.WriteLine(string.Format("{0:#.00%}", 1234.125));//=>123412.50%

            //内置快捷字母格式化用法
            //E-科学计数法表示
            Console.WriteLine((25000).ToString("E"));//=>2.500000E+004

            //C-货币表示，带有逗号分隔符，默认小数点后保留两位，四舍五入
            Console.WriteLine((2.5).ToString("C"));//=>￥2.50

            //D[length]-十进制数
            Console.WriteLine((25).ToString("D5"));//=>00025

            // F[precision]-浮点数，保留小数位数(四舍五入)
            Console.WriteLine((25).ToString("F2"));//=>2.52

            // G[digits]-常规，保留指定位数的有效数字，四舍五入
            Console.WriteLine((2.52).ToString("G2"));//=>2.5

            // N- 带有逗号分隔符，默认小数点后保留两位，四舍五入
            Console.WriteLine((2500000).ToString("N"));//=>2,500,000.00

            // X- 十六进制，非整形将产生格式异常
            Console.WriteLine((255).ToString("X"));//=>FF

            //ToString也可以自定义补零格式化
            Console.WriteLine((15).ToString("000"));//=>015
            Console.WriteLine((15).ToString("value is 0"));//=> value is 15
            Console.WriteLine((10.456).ToString("0.00"));//=>10.46
            Console.WriteLine((10.456).ToString("00"));//=>10
            Console.WriteLine((10.456).ToString("value si 0.0"));//=>10.5

            //转化为二进制、八进制、十六进制输出
            int number = 15;
            Console.WriteLine(Convert.ToString(number, 2));//=>1111
            Console.WriteLine(Convert.ToString(number, 8));//=>17
            Console.WriteLine(Convert.ToString(number, 16));//=>f

            //使用自定义格式化器
            var s = string.Format(new CustomFormat(), "-> {0:Reverse} <-", "Hello World");
            Console.WriteLine(s);//=> -> dlroW olleH <-

            //字符串拼接
            //将数组中的字符串拼接成一个字符串
            var parts = new[] { "Foo", "Bar", "Fizz", "Buzz" };
            var joined = string.Join(", ", parts);
            Console.WriteLine(joined);//=>Foo, Bar, Fizz, Buzz

            //以下四种方式都可以达到相同的字符串拼接的目的
            string first = "Hello";
            string second = "World";
            Console.WriteLine(first + " " + second);//=>Hello World
            Console.WriteLine(string.Concat(first, " ", second));//=>Hello World
            Console.WriteLine(string.Format("{0} {1}", first, second));//=>Hello World
            Console.WriteLine($"{first} {second}");//=>Hello World

            //字符串内插法
            var name = "World";
            Console.WriteLine($"Hello,{name}");//Hello,World

            //带有日期格式化
            var date = DateTime.Now;
            Console.WriteLine($"Today is {date:yyyy-MM-dd}");

            //补齐格式化 (padding)
            var num = 42;
            //向左补齐
            Console.WriteLine($"The num is {num,5}.");//=>The num is    42.
                                                      //向右补齐
            Console.WriteLine($"The num is {num,-5}.");//=>The num is 42   .

            //组合内置快捷字母格式化
            var amount = 2.5;
            Console.WriteLine($"It costs {amount:C}");//=>It costs ￥2.50
            Console.WriteLine($"The num is {num,5:f1}.");//=>The num is  42.0.
        }
    }

    //自定义格式化器
    public class CustomFormat : IFormatProvider, ICustomFormatter
    {
        public string Format(string format, object arg, IFormatProvider formatProvider)
        {
            if (!this.Equals(formatProvider))
            {
                return null;
            }
            if (format == "Reverse")
            {
                return string.Join("", arg.ToString().Reverse());
            }
            return arg.ToString();
        }
        public object GetFormat(Type formatType)
        {
            return formatType == typeof(ICustomFormatter) ? this : null;
        }
    }
}
