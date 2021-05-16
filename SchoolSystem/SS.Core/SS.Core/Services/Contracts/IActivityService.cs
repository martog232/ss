using SS.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SS.Core.Services.Contracts
{
    public interface IActivityService
    {
        public Activity GetActivity(int id);
        public IEnumerable<Activity> GetActivities();
        public Task SeedActivity();
    }
}
