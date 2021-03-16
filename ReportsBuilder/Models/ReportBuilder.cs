using Newtonsoft.Json;
using NLog;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ReportsBuilder.Models
{
  public class ReportBuilder
  {
    /// <summary>
    /// Логер.
    /// </summary>
    private static Logger Logger { get; set; }

    /// <summary>
    /// ИД.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Признак необходимости остановить построение отчета.
    /// </summary>
    private bool NeedStop { get; set; }

    /// <summary>
    /// Репортер.
    /// </summary>
    private Reporter Reporter;

    /// <summary>
    /// Конструктор.
    /// </summary>
    /// <param name="id">ИД.</param>
    public ReportBuilder(int id)
    {
      this.Id = id;

      Logger = LogManager.GetCurrentClassLogger();
      Reporter = Reporter.GetReporter();
    }

    /// <summary>
    /// Ассинхронно построить отчет.
    /// </summary>
    /// <returns>Сформированный отчет.</returns>
    public async Task<byte[]> Build()
    {
      Logger.Debug($"Отчет(Id = {Id}). Старт построение отчета.");
      byte[] data = null;
      try
      {
        data = await Task.Run(() => GenerateReport(new CancellationTokenSource()));
        if (data != null)
        {
          Reporter.ReportSuccess(data, this.Id);
          Logger.Debug($"Отчет(Id = {Id}). Отчет успешно построен.");
        }
        else
          Logger.Debug($"Отчет(Id = {Id}). Построение отчета было отменено.");
      }
      catch (OperationCanceledException ex)
      {
        Reporter.ReportTimeout(this.Id);
        Logger.Debug($"Отчет(Id = {Id}). Вышло время построения отчета. {ex.Message} StackTrace: {ex.StackTrace}");
      }
      catch (Exception ex)
      {
        Reporter.ReportError(this.Id);
        Logger.Debug($"Отчет(Id = {Id}). Во время построение отчета возникла ошибка. {ex.Message} StackTrace: {ex.StackTrace}");
      }
      finally
      {
        Logger.Debug($"Отчет(Id = {Id}). Завершение.");
      }
      return data;
    }

    /// <summary>
    /// Ассинхронно сгенерировать отчет.
    /// </summary>
    /// <param name="cancellationToken">Токен.</param>
    /// <returns>Сформированный отчет.</returns>
    public async Task<byte[]> GenerateReport(CancellationTokenSource cancellationToken)
    {
      NeedStop = false;
      var second = 1000;
      var random = new Random();
      // Время построение отчета.
      int workTime = random.Next(5, 45);
      // Текущее время.
      int currentTime = 0;
      // Добавление таймаута задачи.
      cancellationToken.CancelAfter(30 * second);

      while (!NeedStop)
      {
        if (currentTime >= workTime)
        {
          var errorProbability = random.Next(0, 99);

          if (errorProbability >= 20)
            return Encoding.UTF8.GetBytes($"Report ready at {currentTime} s.");
          else
            throw new Exception("Report failed");
        }

        // Усыпление потока на секунду.
        await Task.Delay(second, cancellationToken.Token);
        currentTime += 1;
      }
      return null;
    }

    /// <summary>
    /// Остановить построение отчета.
    /// </summary>
    public void Stop()
    {
      NeedStop = true;
    }
  }
}
