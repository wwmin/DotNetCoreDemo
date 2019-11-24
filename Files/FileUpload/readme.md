#### NetCore文件上传两种方式
　　NetCore官方给出的两种文件上传方式分别为“缓冲”、“流式”。我简单的说说两种的区别，

　　1.缓冲：通过模型绑定先把整个文件保存到内存，然后我们通过IFormFile得到stream，优点是效率高，缺点对内存要求大。文件不宜过大。

　　2.流式处理：直接读取请求体装载后的Section 对应的stream 直接操作strem即可。无需把整个请求体读入内存，

以下为官方微软说法

 - 缓冲

　　
整个文件读入 IFormFile，它是文件的 C# 表示形式，用于处理或保存文件。文件上传所用的资源（磁盘、内存）取决于并发文件上传的数量和大小。如果应用尝试缓冲过多上传，站点就会在内存或磁盘空间不足时崩溃。如果文件上传的大小或频率会消耗应用资源，请使用流式传输。

- 流式处理 　　

　　从多部分请求收到文件，然后应用直接处理或保存它。流式传输无法显著提高性能。流式传输可降低上传文件时对内存或磁盘空间的需求。

- 文件大小限制

　　说起大小限制，我们得从两方面入手，



1应用服务器Kestrel 

2.应用程序（我们的netcore程序），

1.应用服务器Kestre设置
　　应用服务器Kestrel对我们的限制主要是对整个请求体大小的限制通过如下配置可以进行设置(Program -> CreateHostBuilder)，超出设置范围会报 BadHttpRequestException: Request body too large 异常信息

```c#

public static IHostBuilder CreateHostBuilder(string[] args) =>
           Host.CreateDefaultBuilder(args)
               .ConfigureWebHostDefaults(webBuilder =>
               {
                   webBuilder.ConfigureKestrel((context, options) =>
                   {
                       //设置应用服务器Kestrel请求体最大为50MB
                       options.Limits.MaxRequestBodySize = 52428800;
                   });
                   webBuilder.UseStartup<Startup>();
});

```

2.应用程序设置

　　应用程序设置 (Startup->  ConfigureServices) 超出设置范围会报InvalidDataException 异常信息

```c#

services.Configure<FormOptions>(options =>
 {
             options.MultipartBodyLengthLimit = long.MaxValue;
 });
 ```

 通过设置即重置文件上传的大小限制。

 源码分析
　　这里我主要说一下 MultipartBodyLengthLimit  这个参数他主要限制我们使用“缓冲”形式上传文件时每个的长度。为什么说是缓冲形式中，是因为我们缓冲形式在读取上传文件用的帮助类为 MultipartReaderStream 类下的 Read 方法，此方法在每读取一次后会更新下读入的总byte数量，当超过此数量时会抛出  throw new InvalidDataException($"Multipart body length limit {LengthLimit.GetValueOrDefault()} exceeded.");  主要体现在 UpdatePosition 方法对 _observedLength  的判断

以下为 MultipartReaderStream 类两个方法的源代码，为方便阅读，我已精简掉部分代码
Read
```
public override int Read(byte[] buffer, int offset, int count)
 {
          
          var bufferedData = _innerStream.BufferedData;
　　　　　　int read;
　　　　  read = _innerStream.Read(buffer, offset, Math.Min(count, bufferedData.Count));
          return UpdatePosition(read);
}
```
UpdatePosition
```

private int UpdatePosition(int read)
        {
            _position += read;
            if (_observedLength < _position)
            {
                _observedLength = _position;
                if (LengthLimit.HasValue && _observedLength > LengthLimit.GetValueOrDefault())
                {
                    throw new InvalidDataException($"Multipart body length limit {LengthLimit.GetValueOrDefault()} exceeded.");
                }
            }
            return read;
}
```
通过代码我们可以看到 当你做了 MultipartBodyLengthLimit 的限制后，在每次读取后会累计读取的总量，当读取总量超出

 MultipartBodyLengthLimit  设定值会抛出 InvalidDataException 异常，



如果你部署 在iis上或者Nginx 等其他应用服务器 也是需要注意的事情，因为他们本身也有对请求体的限制，还有值得注意的就是我们在创建文件流对象时 缓冲区的大小尽量不要超过netcore大对象的限制。
这样在并发高的时候很容易触发二代GC的回收.