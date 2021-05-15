using Ganss.Excel;
using SS.Core.Database;
using SS.Core.DTOs;
using SS.Core.Models;
using SS.Core.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SS.Core.Services
{
    public class ActivityService : IActivityService
    {
        private SSDbContext _context;
        private ExcelMapper _excelMapper;
        private const string filePath = @"C:\Users\Ali\Desktop\pts\InputData\Logs_Course A_StudentsActivities.xlsx";

        public ActivityService(SSDbContext context)
        {
            _context = context;
            _excelMapper = new ExcelMapper(filePath);
        }
        public Activity GetActivity(int id)
        {
            return _context.Activities.Find(id);
        }

        public IEnumerable<Activity> GetActivities()
        {
            return _context.Activities.ToList();
        }

        public async Task SeedActivity()
        {
            var activityData = _excelMapper.Fetch<ActivitySeedModel>().ToArray();

            for (int i = 0; i < activityData.Length; i++)
            {
                var currActivity = new Activity(activityData[i].Time,
                    activityData[i].EventContext,
                    activityData[i].Component,
                    activityData[i].EventName,
                    activityData[i].Description);

                await _context.Activities.AddAsync(currActivity);
                await _context.SaveChangesAsync();
            }
        }
    }
}
