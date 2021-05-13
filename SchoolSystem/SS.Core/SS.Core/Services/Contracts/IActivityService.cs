using SS.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS.Core.Services.Contracts
{
    interface IActivityService
    {
        public Activity GetActivity(int id);
        public IEnumerable<Activity> GetActivities();
        public Task SeedActivity();
    }
}
