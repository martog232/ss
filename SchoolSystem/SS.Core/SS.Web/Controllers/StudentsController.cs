using Microsoft.AspNetCore.Mvc;
using SS.Core.DTOs;
using SS.Core.Services.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace SS.Web.Controllers
{
    [Route("api/[controller]/")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentService _studentService;

        public StudentsController(IStudentService studentService)
        {
            _studentService = studentService;
        }

        [HttpGet]
        public IEnumerable<StudentSeedModel> Get()
        {
            return _studentService
                .GetStudentsByComponentAndEventContext()
                        .Select(s => new StudentSeedModel(s));
        }

        [HttpGet("frequency")]
        public IEnumerable<FrequencyOutputModel> GetFrequency()
        {
            return _studentService.GetFrequency();
        }

        [HttpGet("correlationAnalysis")]
        public double GetCorrelationAnalysis()
        {
            return _studentService.GetCorrelationAnalysis();
        }

        [HttpGet("centralTendention")]
        public CentralTendentionModel GetCentralTendention()
        {
            return _studentService.GetCentralTendention();
        }

        [HttpGet("dispersion")]
        public DispersionOutputModel GetDispersionOutput()
        {
            return _studentService.GetDispersionOutput();
        }
    }
}
