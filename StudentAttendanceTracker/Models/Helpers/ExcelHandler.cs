//C# and Razor Code Written by Zaid Abuisba https://github.com/vgc12 
using Microsoft.IdentityModel.Tokens;
using Microsoft.Office.Interop.Excel;
using StudentAttendanceTracker.Models.ExcelModels;
using StudentAttendanceTracker.Models.ViewModels;

namespace StudentAttendanceTracker.Models.Helpers
{
    public class ExcelHandler
    {

        private static readonly DateTime SemesterStartDate = new(DateTime.Now.Year, 8, 23);
        private static readonly DateTime SemesterEndDate = new(DateTime.Now.Year, 12, 11);

        private readonly string _path = @"./TemporaryReports/";

        
        public Task<string> CreateExcelFileAsync(StudentsInCourse model)
        {

            Application excel = new();
            Workbook wb;
            Worksheet ws;
            string fileName = HelperMethods.GetRandomCharacters(8);


            wb = excel.Workbooks.Add(XlSheetType.xlWorksheet);
            ws = (Worksheet)wb.Worksheets[1];

            GenerateReport(model, ws);
        

            ws.Columns.AutoFit();

            wb.SaveAs(Environment.CurrentDirectory + @"\TemporaryReports\" + fileName, XlFileFormat.xlOpenXMLWorkbook);

            wb.Close();
            excel.Quit();
            return Task.FromResult(fileName);
        }

    
        private static void GenerateReport(StudentsInCourse model, Worksheet ws)
        {
            List<DateTime> datesBetween;
            if (model.StartDate == null)
            {
                model.StartDate = SemesterStartDate;
            }
            if (model.EndDate == null)
            {
                model.EndDate = SemesterEndDate;
            }

            datesBetween = HelperMethods.GetDatesBetween(model.StartDate.Value.Date, model.EndDate.Value.Date, HelperMethods.WeekType.ExcludeWeekends).ToList();

            ws.Cells[1, 1].EntireColumn.Font.Bold = true;
            ws.Cells[1, 1].EntireRow.Font.Bold = true;
            WriteCourseToFile(model, ws, datesBetween);
        }

        private static void WriteCourseToFile(StudentsInCourse model, Worksheet ws, List<DateTime> datesBetween)
        {
            ws.Cells[1, 1] = model.Course.CourseName;
            for (int k = 0; k < model.StudentAttendanceLogs.Count; k++)
            {
                WriteStudentToFile(model, ws, datesBetween, k);
            }
        }

        private static void WriteStudentToFile(StudentsInCourse model, Worksheet ws, List<DateTime> datesBetween, int k)
        {
            ws.Cells[k + 2, 1] = model.StudentAttendanceLogs[k].Student.FirstName.FirstCharToUpper() + " " + model.StudentAttendanceLogs[k].Student.LastName.FirstCharToUpper();

            for (int i = 0; i < datesBetween.Count; i++)
            {
                if (datesBetween[i].Date > DateTime.Now.Date)
                {
                    break;
                }
                if (k < 1)
                    ws.Cells[1, i + 2] = datesBetween[i].ToString("MM/dd/yyyy");


                var log = model.StudentAttendanceLogs[k].AttendanceLogs.FirstOrDefault(x => x.SignInTime.Value.Date == datesBetween[i].Date);

                if (log == null)
                {
                    ws.Cells[k + 2, i + 2] = "Absent";
                    ws.Cells[k + 2, i + 2].Interior.Color = XlRgbColor.rgbRed;
                    continue;
                }
                if (log.Tardy)
                {
                    ws.Cells[k + 2, i + 2] = "Tardy";
                    ws.Cells[k + 2, i + 2].Interior.Color = XlRgbColor.rgbYellow;
                }
                else
                {
                    ws.Cells[k + 2, i + 2] = "Present";
                    ws.Cells[k + 2, i + 2].Interior.Color = XlRgbColor.rgbGreen;
                }


            }
        }

       
           
        
    }

}

