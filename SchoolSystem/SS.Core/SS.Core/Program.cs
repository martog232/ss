using SS.Core.Database;
using SS.Core.Services;
using System.Threading.Tasks;

namespace SS.Core
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var context = new SSDbContext();

            var studentService = new StudentService(context);
            await studentService.SeedStudents();

            var activityService = new ActivityService(context);
            await activityService.SeedActivity();
        }
    }
}
