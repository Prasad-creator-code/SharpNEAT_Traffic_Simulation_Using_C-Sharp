using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;

namespace NEATDrive_WPF.DrivingScripts
{
    class SaveManager
    {
        public static SaveManager? instance;


        public void SaveToExcelSheet()
        {
            string fileName = "OutputFile.xlsx"; // or "OutputFile.csv"
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);
            List<string> data = new List<string> { "John", "Doe", "Jane", "Smith" };

            try
            {
                // Create a new Excel package
                using (ExcelPackage package = new ExcelPackage())
                {
                    // Create the worksheet
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");

                    // Populate the data
                    for (int i = 0; i < data.Count; i++)
                    {
                        worksheet.Cells[i + 1, 1].Value = data[i];
                    }

                    // Save the Excel package to a file
                    FileInfo file = new FileInfo(filePath);
                    package.SaveAs(file);
                }

                MessageBox.Show($"Excel file generated successfully and saved on the desktop as {fileName}!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }

        public void SaveToCSVSheet()
        {
            string fileName = "OutputFile.csv";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);
            List<string> data = new List<string> { "John", "Doe", "Jane", "Smith" };

            try
            {
                // Create a new StreamWriter and specify the CSV file path
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write the data to the CSV file
                    foreach (string item in data)
                    {
                        writer.WriteLine(item);
                    }
                }

                MessageBox.Show($"CSV file generated successfully and saved on the desktop as {fileName}!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }


    }
}
