新建工程
dotnet new sln -o mongoDbDemo
cd mongoDbDemo
dotnet new console -o mongoConsole
dotnet sln add mongoConsole/mongoConsole.csproj
cd mongoConsole
dotnet add package mongodb.driver
tree /f > ./tree.txt

文件夹 PATH 列表
卷序列号为 6CCD-E61C
D:.
│  mongoDbDemo.sln
│  tree.txt
│  
└─mongoConsole
    │  mongoConsole.csproj
    │  Program.cs
    │  
    └─obj
            mongoConsole.csproj.nuget.dgspec.json
            mongoConsole.csproj.nuget.g.props
            mongoConsole.csproj.nuget.g.targets
            project.assets.json
            project.nuget.cache
            
