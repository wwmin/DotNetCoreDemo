
using FileWatcher;

using System.Diagnostics;
//var basePath = Directory.GetDirectoryRoot(Directory.GetCurrentDirectory());
//var basePath = Environment.CurrentDirectory;
//var basePath = AppDomain.CurrentDomain.BaseDirectory;
//var basePath = Process.GetCurrentProcess()?.MainModule?.FileName;
var basePath = $@"D:/";
Console.WriteLine(basePath);
FileUtil.WatchFiles(basePath, "*.txt");
Console.Read();