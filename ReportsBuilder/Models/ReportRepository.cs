using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReportsBuilder.Models
{
  public class ReportRepository : IRepository
  {
    private List<ReportBuilder> Context;

    public ReportRepository()
    {
      Context = new List<ReportBuilder>();
    }

    /// <summary>
    /// Получить отчет по Id.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <returns>Отчет.</returns>
    public ReportBuilder Get(int id)
    {
      return Context.FirstOrDefault(t => t.Id == id);
    }

    /// <summary>
    /// Получить все отчеты.
    /// </summary>
    /// <returns>Список отчетов.</returns>
    public IEnumerable<ReportBuilder> Get()
    {
      return Context;
    }
    
    /// <summary>
    /// Добавить отчет.
    /// </summary>
    /// <param name="report">Отчет.</param>
    public void Add(ReportBuilder report)
    {
      Context.Add(report);
    }

    /// <summary>
    /// Получить следующий порядковый номер.
    /// </summary>
    /// <returns>Номер.</returns>
    public int GetNextNumber()
    {
      return Context.Count;
    }
  }
}
