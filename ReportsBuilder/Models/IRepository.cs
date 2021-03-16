using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportsBuilder.Models
{
  public interface IRepository
  {
    ReportBuilder Get(int id);
    IEnumerable<ReportBuilder> Get();
    void Add(ReportBuilder report);
    int GetNextNumber();
  }
}
