using System;
using System.Threading;
using System.Threading.Tasks;

namespace taskDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            //Do(); //由于这个是异步的，在Main方法里无法加await，所以有警告，这个没关系，不会阻塞下面的代码
            // oldEntityUpdate();
            // oldEntityFindAndNewUpdate();
            newEntityUpdate();
            Console.WriteLine("结束");
            Console.ReadKey();
        }

        static void oldEntityFindAndNewUpdate()
        {
            PayUserRepository payUserRepository = new PayUserRepository(new PayEntities());
            Users user = payUserRepository.Find(i => i.ID == 1);
            payUserRepository.Modify(new Users
            {
                ID = 1,
                PassWord = "bobo3" + DateTime.Now,

            }, i => i.PassWord);
        }

        static void oldEntityUpdate()
        {
            PayUserRepository payUserRepository = new PayUserRepository(new PayEntities());
            Users user = payUserRepository.Find(i => i.ID == 1);
            user.PassWord = "newpassword";
            payUserRepository.Modify(user, i => i.PassWord);
        }
        static void newEntityUpdate()
        {
            PayUserRepository payUserRepository = new PayUserRepository(new PayEntities());
            payUserRepository.Modify(new Users
            {
                ID = 1,
                PassWord = "bobo3" + DateTime.Now,

            }, i => i.PassWord);
        }
        static void testDbUpdate()
        {
            testEntities db = new testEntities();
            userinfo userinfo = new userinfo
            {
                id = 1,
                count = 100
            };
            db.userinfo.Attach(userinfo);
            db.Entry(userinfo).Property(x => x.count).IsModified = true;
            db.SaveChanges();
        }
        static async Task Do()
        {
            await Task.Run(() =>
             {
                 Console.WriteLine("程序1执行3秒钟");
                 Thread.Sleep(3000);//阻塞3秒
             }).ContinueWith((o) =>//当上面的任务完成后，再顺序执行这个任务
             {
                 Console.WriteLine("程序2开始执行5秒种");
                 Thread.Sleep(5000);
             }).ContinueWith((o) =>
             {
                 Console.WriteLine("程序3开始执行");
             });

            //等待这个Task整个完成后
            Console.WriteLine("整个任务结束");
        }
    }
}
