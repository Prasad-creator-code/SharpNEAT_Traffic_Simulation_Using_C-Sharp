using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace NEATDrive_WPF.DrivingScripts
{
    class SaveManager
    {
        public static SaveManager? instance;

        #region Excel Sheet
        public void SaveToExcelSheet()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            string fileName = "NEAT_OutputExcelSheet.xlsx"; // or "OutputFile.csv"
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);
            DataGrid dataGrid = ApplicationManager.instance.configWindow.Metrics_DataGrid;

            try
            {

                List<List<string>> data = new List<List<string>>();
                foreach (DataGridColumn column in dataGrid.Columns)
                {
                    List<string> columnData = new List<string>();
                    columnData.Add(column.Header.ToString()); // Add column headers to the column data
                    data.Add(columnData);
                }

                foreach (var item in dataGrid.Items)
                {
                    for (int i = 0; i < dataGrid.Columns.Count; i++)
                    {
                        DataGridColumn column = dataGrid.Columns[i];
                        var cellContent = column.GetCellContent(item);
                        string cellValue = "";

                        if (cellContent is TextBlock textBlock)
                        {
                            cellValue = textBlock.Text;
                        }
                        else if (cellContent is TextBox textBox)
                        {
                            cellValue = textBox.Text;
                        }


                        data[i].Add(cellValue);
                    }
                }


                using (ExcelPackage package = new ExcelPackage())
                {

                    ExcelWorksheet worksheet = package.Workbook.Worksheets.Add("Sheet1");


                    for (int i = 0; i < data.Count; i++)
                    {
                        for (int j = 0; j < data[i].Count; j++)
                        {
                            worksheet.Cells[j + 1, i + 1].Value = data[i][j];
                        }
                    }


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
        #endregion

        #region CSV Sheet

        public void SaveToCSVSheet()
        {
            string fileName = "NEAT_OutputCSVSheet.csv";
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string filePath = Path.Combine(desktopPath, fileName);
            DataGrid dataGrid = ApplicationManager.instance.configWindow.Metrics_DataGrid;

            try
            {
                // Transpose the data
                List<List<string>> transposedData = TransposeDataGrid(dataGrid);

                // Write the transposed data to a CSV file
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    foreach (List<string> rowData in transposedData)
                    {
                        string csvLine = string.Join(",", rowData.Select(FormatCsvValue));
                        writer.WriteLine(csvLine);
                    }
                }

                MessageBox.Show("CSV file generated successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }

        }
        private List<List<string>> TransposeDataGrid(DataGrid dataGrid)
        {
            List<List<string>> transposedData = new List<List<string>>();

            // Add the headers as the first row in the transposed data
            List<string> headerRow = new List<string>();
            foreach (var column in dataGrid.Columns)
            {
                headerRow.Add(column.Header.ToString());
            }
            transposedData.Add(headerRow);

            // Add the data rows transposed
            for (int i = 0; i < dataGrid.Items.Count; i++)
            {
                var dataGridRow = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(i);
                List<string> rowData = new List<string>();
                for (int j = 0; j < dataGrid.Columns.Count; j++)
                {
                    var cellContent = dataGrid.Columns[j].GetCellContent(dataGridRow);
                    if (cellContent is TextBlock textBlock)
                    {
                        rowData.Add(textBlock.Text);
                    }
                    else if (cellContent is TextBox textBox)
                    {
                        rowData.Add(textBox.Text);
                    }
                    // Add other data grid cell types as needed
                }
                transposedData.Add(rowData);
            }

            return transposedData;
        }
        private string FormatCsvValue(string value)
        {
            // If the value contains a comma or double-quote, enclose it in double-quotes and escape any existing double-quotes
            if (value.Contains(",") || value.Contains("\""))
            {
                return "\"" + value.Replace("\"", "\"\"") + "\"";
            }
            return value;
        }

        #endregion
    }


}

