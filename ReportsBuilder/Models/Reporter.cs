using NLog;
using System;
using System.IO;
using System.Text;

namespace ReportsBuilder.Models
{
  public class Reporter
  {
    /// <summary>
    /// Путь к папке.
    /// </summary>
    private string DirectoryPath;

    /// <summary>
    /// Текст ошибки.
    /// </summary>
    private const string ErrorText = "Report error";

    /// <summary>
    /// Главная сущность.
    /// </summary>
    private static Reporter reporter;

    /// <summary>
    /// Логер.
    /// </summary>
    private static Logger Logger { get; set; }

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    private Reporter()
    {
      DirectoryPath = Directory.GetCurrentDirectory();
      Directory.CreateDirectory($"{DirectoryPath}//Reports");
    }

    /// <summary>
    /// Получение главной сущности.
    /// </summary>
    /// <returns>Главная сущность.</returns>
    public static Reporter GetReporter()
    {
      if (reporter == null)
        reporter = new Reporter();

      return reporter;
    }

    /// <summary>
    /// Записать успешный отчет.
    /// </summary>
    /// <param name="Data">Тело отчета.</param>
    /// <param name="Id">ИД задачи.</param>
    public void ReportSuccess(byte[] Data, int Id)
    {
      try
      {
        File.WriteAllBytes($"{DirectoryPath}//Reports//Report_{Id}.txt", Data);
      }
      catch (Exception ex)
      {
        Logger.Debug($"Отчет (Id = {Id}). Ошибка записи отчета в файл.");
      }
    }

    /// <summary>
    /// Записать отчет с ошибкой.
    /// </summary>
    /// <param name="Id">ИД задачи.</param>
    public void ReportError(int Id)
    {
      var data = Encoding.UTF8.GetBytes(ErrorText);
      try
      {
        File.WriteAllBytes($"{DirectoryPath}//Reports//Report_{Id}.txt", data);
      }
      catch (Exception ex)
      {
        Logger.Debug($"Отчет (Id = {Id}). Ошибка записи ошибки построения отчета в файл.");
      }
    }

    /// <summary>
    /// Записать отчет с ошибкой таймаута.
    /// </summary>
    /// <param name="Id">ИД задачи.</param>
    public void ReportTimeout(int Id)
    {
      var data = Encoding.UTF8.GetBytes(ErrorText);
      try
      {
        File.WriteAllBytes($"{DirectoryPath}//Reports//Timeout_{Id}.txt", data);
      }
      catch (Exception ex)
      {
        Logger.Debug($"Отчет (Id = {Id}). Ошибка записи таймаута построения отчета в файл.");
      }
    }
  }
}
