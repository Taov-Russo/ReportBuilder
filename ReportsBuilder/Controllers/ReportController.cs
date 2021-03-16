using Microsoft.AspNetCore.Mvc;
using ReportsBuilder.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ReportsBuilder.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class ReportController : ControllerBase
  {
    IRepository Repository;
    public ReportController(IRepository repository)
    {
      Repository = repository;
    }

    [HttpGet]
    [Route("Build")]
    public async Task<IActionResult> Build()
    {
      var report = new ReportBuilder(Repository.GetNextNumber());
      Repository.Add(report);
      var data = report.Build();
      // Какие-то действия с data.
      return new ObjectResult(new JSONBody(report.Id));
    }

    [HttpPost]
    [Route("Stop")]
    public void Stop([FromBody] JSONBody json)
    {
      if (json.Id > 0 && json.Id < Repository.GetNextNumber())
      {
        var report = Repository.Get(json.Id);
        report.Stop();
      }
    }
  }
}
