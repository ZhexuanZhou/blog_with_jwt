# Blog Project

## 功能要求

1. 文章以markdown的格式发布，编辑
2. 匿名用户可以评论，单必须输入Email
3. 身份验证（登陆，注册，删除，登出）
4. Markdown 图片上传，保存地址，append到文章的最后

## 数据库

|  Table Name   |            Column Name            |      Column Describtion       |  Type   |
| :-----------: | :-------------------------------: | :---------------------------: | :-----: |
|    Artical    |     ID (PK, Unique, Not null)     |         Auto increase         |   int   |
|               |         Title (Not null)          |         Article title         | varchar |
|               |         Author (Not null)         |                               | varchar |
|               |      Date_Create (Not null)       |                               |  date   |
|               |    Date_Last_Modify (Not null)    |                               |  date   |
|               |        Content (Not null)         |                               | varchar |
|  Aritcal_Cat  |     ID (PK, Unique, Not null)     |         Auto increase         |   int   |
|               | Article_ID (FK, Unique, Not null) |         From Artical          |   int   |
|               |           Catogry_Value           |                               |   int   |
|   Category    |           Category_Name           |     Different Categories      |  char   |
|               |           Catogry_Value           | Value for different categries |   int   |
|    Comment    |     ID (PK, Unique, Not null)     |         Auto increase         |   int   |
|               | Article_ID (FK, Unique, Not null) |         From Artical          |   int   |
|               |         Parent_Comment_ID         |        Null means new         |   int   |
|               |              Author               |     email add. or writer      |  char   |
|               |           Date_Comment            |                               |  date   |
|               |              Content              |                               |         |
|               |            Author_Role            |            0 or 1             |   int   |
| Comment_User  |     ID (PK, Unique, Not null)     |                               |         |
|               |     Author(Unique, Not null)      |                               |         |
|               |      Email(Unique, Not null)      |                               |         |
| Aritcal_Image |     ID (PK, Unique, Not null)     |         Auto increase         |   int   |
|               | Article_ID (FK, Unique, Not null) |         From Artical          |   int   |
|               |             Image_url             |                               |         |
|               |                                   |                               |         |
|               |                                   |                               |         |
|               |                                   |                               |         |
|               |                                   |                               |         |
|               |                                   |                               |         |
|               |                                   |                               |         |
|               |                                   |                               |         |

## 开发流程

1. 配置项目：

   1. 项目结构：Sln（apore，infrastructure）：

      1. infrastructure引用core
      2. api引用core和infrasturcture

   2. 配置Serilog：

      1. 添加四个nuget packages 在api里，Aspnetcore，serilog，serilog.sinks.file，serilog.sinks.console
      2. 在program的main中配置serilog，并在IWebHostBuilder中UseSerilog。

   3. 添加Entity ([Entity Framework tutorial](http://www.entityframeworktutorial.net/))：

      1. 添加entity 

         1. 用户类继承IdentityUser，其使用的DbContext即成IdentityDbContext

      2. 添加身份认证和jwt授权：

         1. 在startup的ConfigureServices中添加

            ```c#
            var builder = services.AddIdentityCore<User>(o =>
            {
                // configure identity options
                o.Password.RequireDigit = false;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 6;
            });
            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddEntityFrameworkStores<RepositoryDbContext>().AddDefaultTokenProviders();
            
            //添加开启Authentication，否则出现500错误
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = Configuration["Jwt:Issuer"],
                        ValidAudience = Configuration["Jwt:Issuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                    };
                });
            ```

         2. 在startup的Configure中添加

            ```c#
             app.UseAuthentication(); // before mvc
            ```

         3. 在使用权限时，通过`UserManager<User> _userManager`管理用户，包括新建，验证等

         4. 验证用户权限后，创建Claims$\rightarrow$ SymmetricSecurityKey$\rightarrow$SigningCredentials$\rightarrow$JwtSecurityToken，代码如下

            ```c#
            private string BuildToken(User user)
            {
                var claims = new[] {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                   };
            
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            
                var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                  _configuration["Jwt:Issuer"],
                  claims,
                  expires: DateTime.Now.AddMinutes(30),
                  signingCredentials: creds);
            
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            ```

      3. 添加entity configurations：使用fluent API

      4. 添加种子数据

      5. 重点：一对一，一对多，多对多关系在fluent API中的设置

         ```C#
         public class Student
         {
             public int Id { get; set; }
             public string Name { get; set; }
         
             public int CurrentGradeId { get; set; }
             public Grade Grade { get; set; }
         }
         
         public class Grade
         {
             public int GradeId { get; set; }
             public string GradeName { get; set; }
             public string Section { get; set; }
         
             public ICollection<Student> Students { get; set; }
         }
         
         public class StudentAddress
         {
             public int StudentAddressId { get; set; }
             public string Address { get; set; }
             public string City { get; set; }
             public string State { get; set; }
             public string Country { get; set; }
         
             public int AddressOfStudentId { get; set; }
             public Student Student { get; set; }
         }
         
         public class Course
         {
             public int CourseId { get; set; }
             public string CourseName { get; set; }
             public string Description { get; set; }
         }
         ```

         A Grade entity includes many Student entities: One-to-Many:

         ```C#
         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             modelBuilder.Entity<Student>()
                 .HasOne<Grade>(s => s.Grade)
                 .WithMany(g => g.Students)
                 .HasForeignKey(s => s.CurrentGradeId);
         }
         
         // if in the configuration class:
         public void Configure(EntityTypeBuilder<Comment> builder)
         {
             builder.HasOne(s => s.Grade)
                 .WithMany(g => g.Students)
                 .HasForeignKey(s => s.CurrentGradeId);
         
         }
         ```

         ![One-to-Many](http://www.entityframeworktutorial.net/images/efcore/configure-onetomany-fluentapi.png)

         One-to-One: Student and StudentAddress:

         ```C#
         protected override void OnModelCreating(ModelBuilder modelBuilder)
         {
             modelBuilder.Entity<Student>()
                 .HasOne<StudentAddress>(s => s.Address)
                 .WithOne(ad => ad.Student)
                 .HasForeignKey<StudentAddress>(ad => ad.AddressOfStudentId);
         }
         ```

         ![One-to-One](http://www.entityframeworktutorial.net/images/efcore/onetoone-fluent.png)

         Many-to-Many: student and course

         ```C#
         // add a thrid table
         public class StudentCourse
         {
             public int StudentId { get; set; }
             public Student Student { get; set; }
         
             public int CourseId { get; set; }
             public Course Course { get; set; }
         }
         // modify student and course
         public class Student
         {
             public int StudentId { get; set; }
             public string Name { get; set; }
         	
             // new 
             public IList<StudentCourse> StudentCourses { get; set; }
         }
         
         public class Course
         {
             public int CourseId { get; set; }
             public string CourseName { get; set; }
             public string Description { get; set; }
         
             // new
             public IList<StudentCourse> StudentCourses { get; set; }
         }
         
         protected override void OnModelCreating(ModelBuilder modelBuilder)
             {
                 modelBuilder.Entity<StudentCourse>().HasKey(sc => new { sc.StudentId, sc.CourseId });
             
             	// Suppose that the foreign key property names do not follow the convention (e.g. SID instead of StudentId and CID instead of CourseId), then you can configure it using Fluent API, as shown below.
             	modelBuilder.Entity<StudentCourse>()
                     .HasOne<Student>(sc => sc.Student)
                     .WithMany(s => s.StudentCourses)
                     .HasForeignKey(sc => sc.SId);
         
         
                 modelBuilder.Entity<StudentCourse>()
                     .HasOne<Course>(sc => sc.Course)
                     .WithMany(s => s.StudentCourses)
                     .HasForeignKey(sc => sc.CId);
             }
         ```

   4. 添加Repository操作entities，降低对持久化技术（EF core）的依赖，使用UnitiOfWork操作Repository的Save操作。

   5. 添加Resource，并使用AutoMap对Resource和Entity进行Mapping。

   6. 添加Fluent Validation

   7. 翻页

      1. 添加paginatedlist和queryparameter， pagequeryparameter（翻页，翻页原数据）

      2. 生成前后页的Url，使用IUrlHelper

         1. 创建PaginationResourceUriType 枚举类

         2. 注册UrlHelper

            ```C#
            //注册Urihelper
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
            	var actionContext = factory
                    .GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
            ```

         3. 创建CreatePostUri方法为post添加url

   8. 过滤

      1. 在其Parameter中添加变量用于查询

      2. 在其Repository中添加以下代码

         ```C#
         var query = _repositoryDbContext.Posts.AsQueryable();
         if (!string.IsNullOrEmpty(postParameters.Title))
         {
             // 在PostParameters中添加title，可根据需求更改
             var title = postParameters.Title.ToLowerInvariant();
             // query = query.Where(x=>x.Title.ToLowerInvariant().Contains(title));
             query = query.Where(x => x.Title.ToLowerInvariant() == title);
         
         }
         ```

      3. 搜索与过滤类似

   9. 排序（不是很懂）

      1. PropertyMappingContainer

         1. PropertyMapping(PostPropertyMapping): resource到entity的属性映射
            1. MappedProperty

      2. 在startup中注册

         ```C#
         //注册排序的propertyMapping
         var propertyMappingContainer = new PropertyMappingContainer();
         propertyMappingContainer.Register<PostPropertyMapping>();
         services.AddSingleton<IPropertyMappingContainer>(propertyMappingContainer);
         ```

      3. 添加QueryableExtensions

      4. To smiply usage:

         ```C#
         // Too easy without any complication:
         
         using System.Linq.Dynamic.Core; 
         vehicles = vehicles.AsQueryable().OrderBy("Make ASC, Year DESC").ToList();
         ```


## 常用dotnet 命令

1. dotnet add package
2. dotnet ef migrations add XX --project=../Infrastructure
3. dotnet ef database update --project=../Infrastructure --verbose
4. dotnet ef migrations remove
5. dotnet new sln
6. dotnet sln add Api/Api.csproj 
7. dotnet add reference ../Core/Core.csproj

## 开发问题

1. 项目结构：

   Api：用于存放Api等主要的Controllers

   Core：Model等数据结构

   Infrasturcture：DbContext等

2. 使用Efcore时，要添加Design才能使用命令行。必须在有startup.cs的的项目中才能使用dotnet ef add的命令。同时，--project=可以指定输出的目标项目（dotnet ef migrations add Initial --project=../Infrastructure）

3. dotnet ef database update --project=../Infrastructure --verbose： 更新数据库，verbose用于显示执行内容

4. The name 'MySqlValueGenerationStrategy' does not exist in the current context： 这个错误说明，在执行update的那个项目中没有引用mysql的包

5. dotnet ef migrations script --project=../LearnEf.Data/LearnEf.Data.csproj： 用于生成sql脚本

6. dotnet ef migrations remove --project=../Infrastructure： 删除最后添加的migrations

7. AutoMapper.Extensions.Microsoft.DependencyInjection和AutoMapper， 用到的项目都要添加nuget包

8. 添加HATEOAS

9. FluentValidation.AspnetCore 验证model

10. Http方法总结：

  1. Get：成功返回200和单个（集合）资源，失败404。GET: Api/controller/, Api/controller/{id}

  2. Delete：成功204，失败404。DELETE: Api/controller/{id}

  3. Post：成功201和单个（集合）资源，失败404；POST: Api/controller（单个）, Api/controllerCollection（集合）, Api/controller/{id}（一定失败，返回404(不存在)或409（冲突））

  4. Put：整体更新，PUT：Api/controller/{id}， 成功204， 200，失败404

  5. Patch：局部更新，Patch：Api/controller/{id}，成功204， 200，失败404，注意更新格式

     ```json
     // Patch格式的例子，Content-Type = application/json-patch+json
     [
         {
             "op": "replace",
             "path":"/title",
             "value": "Updated by patch"
         },
         {
             "op": "replace",
             "path":"/remark",
             "value": "Updated by patch"
         }
     ]
     ```

11. REST跟文档，API版本，主从资源，缓存并发， Swashbuckle等未添加

12. OAuth2: 授权， OpenIdConnect：身份认证

13. Implicit flow适合浏览器内应用，hybrid flow要求安全客户端（如MVC）

14. 添加 dotnet new -i identityserver4.templates 模版

15. 删除： dotnet new --debug:reinit

16. Error: The name 'HttpStatusCode' does not exist in the current context.  Add **using Microsoft.AspNetCore.Http;**

17. EFCore一对一，一对多和多对多关系

18. Add an implementation of IDesignTimeDbContextFactory： 添加Pomelo.EntityFrameworkCore.MySql

19. [Guid is all 0's (zeros)?](https://stackoverflow.com/questions/7972150/guid-is-all-0s-zeros)

20. [EF Core Many To Many. Error returning entities](https://stackoverflow.com/questions/49432504/ef-core-many-to-many-error-returning-entities)

21. [Why use ICollection and not IEnumerable or List on many-many/one-many relationships?](https://stackoverflow.com/questions/10113244/why-use-icollection-and-not-ienumerable-or-listt-on-many-many-one-many-relatio).  Shortly, `ICollection<T>` is used because the `IEnumerable<T>` interface provides no way of adding items, removing items, or otherwise modifying the collection.

22. 在使用automapper的时候，如果需要对resource进行嵌套，只要对被嵌套的resource进行mapping，再对整体进行mapping就行了。嵌套的类型中的要使用resource的类型，而不是entity的类型。Automapper对ICollection类型进行map的时候，只需要对里面的类型进行CreateMap，然后在进行map的时候加上ICollection等就行了。

23. [Automapper expression must resolve to top-level member](https://stackoverflow.com/questions/11633021/automapper-expression-must-resolve-to-top-level-member)

24. [Save array of string EntityFramework Core](https://kimsereyblog.blogspot.com/2017/12/save-array-of-string-entityframework.html)

25. AutoMapper many to many mapping: https://stackoverflow.com/questions/28407392/automapper-many-to-many-mapping and revese it back https://stackoverflow.com/questions/49207534/automapper-one-to-many-many-to-many

## 语法

1. [What are the true benefits of ExpandoObject?](https://stackoverflow.com/questions/1653046/what-are-the-true-benefits-of-expandoobject) Understanding the **ExpandoObject**.
2. 静态类的拓展方法：https://stackoverflow.com/questions/866921/static-extension-methods