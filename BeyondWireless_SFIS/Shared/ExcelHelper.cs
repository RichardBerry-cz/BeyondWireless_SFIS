using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

namespace BeyondWireless_SFIS.Shared
{
    public static class ExcelHelper
    {
        //****************** NPOI nuget wrapper class **********************************
        // Ok to update outdated NPOI packages, although there are some that do not update due to dependencies
        // Some issues with double reference to HashSet<int> -- Leave using System.Collections.Generic; and delete the other Collections using statement

        /// <summary>
        /// Excel file format options.
        /// </summary>
        public enum ExcelFormat
        {
            Xlsx, // Excel 2007+ (.xlsx)
            Xls   // Excel 2003 (.xls)
        }

        // --------------------------
        // Core Formatting Utilities
        // --------------------------

        /// <summary>
        /// Applies text formatting to a column (forces Excel to treat values as text).
        /// </summary>
        private static void ApplyTextFormat(ISheet sheet, int columnIndex)
        {
            IDataFormat dataFormat = sheet.Workbook.CreateDataFormat();
            ICellStyle textStyle = sheet.Workbook.CreateCellStyle();
            textStyle.DataFormat = dataFormat.GetFormat("@"); // "@" = Text format

            foreach (IRow row in sheet)
            {
                ICell cell = row.GetCell(columnIndex);
                if (cell != null) cell.CellStyle = textStyle;
            }
        }

        /// <summary>
        /// Makes header row bold.
        /// </summary>
        private static void ApplyHeaderStyle(IWorkbook workbook, ISheet sheet)
        {
            ICellStyle headerStyle = workbook.CreateCellStyle();

            // 1. Font (Bold + Custom Color)
            IFont font = workbook.CreateFont();
            font.IsBold = true;
            //font.FontName = "Arial";
            //font.FontHeightInPoints = 12;
            font.Color = IndexedColors.White.Index; // White text
            headerStyle.SetFont(font);

            // 2. Background Color (Dark Blue)
            headerStyle.FillForegroundColor = IndexedColors.SkyBlue.Index;
            headerStyle.FillPattern = FillPattern.SolidForeground;

            // 3. Borders (Thin Black)
            //headerStyle.BorderTop = BorderStyle.Thin;
            //headerStyle.BorderBottom = BorderStyle.Thin;
            //headerStyle.BorderLeft = BorderStyle.Thin;
            //headerStyle.BorderRight = BorderStyle.Thin;
            //headerStyle.TopBorderColor = IndexedColors.Black.Index;
            //headerStyle.BottomBorderColor = IndexedColors.Black.Index;
            //headerStyle.LeftBorderColor = IndexedColors.Black.Index;
            //headerStyle.RightBorderColor = IndexedColors.Black.Index;

            //// 4. Center Alignment
            //headerStyle.Alignment = HorizontalAlignment.Center;
            //headerStyle.VerticalAlignment = VerticalAlignment.Center;

            // Apply to all cells in the header row
            IRow headerRow = sheet.GetRow(0);
            if (headerRow != null)
            {
                for (int i = 0; i < headerRow.LastCellNum; i++)
                {
                    ICell cell = headerRow.GetCell(i);
                    if (cell != null)
                    {
                        cell.CellStyle = headerStyle;
                        sheet.AutoSizeColumn(i);
                    }
                }
            }
        }

        // --------------------------
        // Write Methods (with formatting)
        // --------------------------

        /// <summary>
        /// Writes a list to an Excel file with text formatting and bold headers.
        /// </summary>
        public static void WriteListToExcel<T>(List<T> data, string filePath, string sheetName = "Sheet1")
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data list is empty or null.");

            IWorkbook workbook = Path.GetExtension(filePath).ToLower() == ".xls"
                ? (IWorkbook)new HSSFWorkbook()
                : new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet(sheetName);
            var properties = typeof(T).GetProperties();
            HashSet<int> textColumns = new HashSet<int>();

            // Freeze the top row
            sheet.CreateFreezePane(0, 1);

            // Write headers
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < properties.Length; i++)
            {
                ICell cell = headerRow.CreateCell(i);  // Keep reference
                cell.SetCellValue(properties[i].Name);
                if (properties[i].PropertyType == typeof(string))
                    textColumns.Add(i);
            }

            // Write data
            for (int rowIdx = 0; rowIdx < data.Count; rowIdx++)
            {
                IRow row = sheet.CreateRow(rowIdx + 1);
                T item = data[rowIdx];
                for (int colIdx = 0; colIdx < properties.Length; colIdx++)
                {
                    object value = properties[colIdx].GetValue(item, null);
                    string cellValue = value?.ToString() ?? "";
                    row.CreateCell(colIdx).SetCellValue(CleanStringForExcel(cellValue));
                }
            }

            // Apply formatting

            foreach (int colIdx in textColumns) ApplyTextFormat(sheet, colIdx);
            ApplyHeaderStyle(workbook, sheet);
            // Save

            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    workbook.Write(fs);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static byte[] WriteListToExcelBytes<T>(List<T> data, ExcelFormat format = ExcelFormat.Xlsx, string sheetName = "Sheet1")
        {
            if (data == null || data.Count == 0)
                throw new ArgumentException("Data list is empty or null.");

            using (IWorkbook workbook = format == ExcelFormat.Xlsx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet(sheetName);
                var properties = typeof(T).GetProperties();
                HashSet<int> textColumns = new HashSet<int>();

                // Freeze the top row
                sheet.CreateFreezePane(0, 1);

                // Write headers
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < properties.Length; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(properties[i].Name);
                    if (properties[i].PropertyType == typeof(string))
                        textColumns.Add(i);
                }

                // Write data
                for (int rowIdx = 0; rowIdx < data.Count; rowIdx++)
                {
                    IRow row = sheet.CreateRow(rowIdx + 1);
                    T item = data[rowIdx];
                    for (int colIdx = 0; colIdx < properties.Length; colIdx++)
                    {
                        object value = properties[colIdx].GetValue(item, null);
                        string cellValue = value?.ToString() ?? "";
                        row.CreateCell(colIdx).SetCellValue(CleanStringForExcel(cellValue));
                    }
                }

                // Apply formatting
                foreach (int colIdx in textColumns) ApplyTextFormat(sheet, colIdx);
                ApplyHeaderStyle(workbook, sheet);

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }
        }

        public static byte[] WriteDataTableToExcelBytes(DataTable data, ExcelFormat format = ExcelFormat.Xlsx, string sheetName = "Sheet1")
        {
            if (data == null || data.Rows.Count == 0)
                throw new ArgumentException("DataTable is empty or null.");

            using (IWorkbook workbook = format == ExcelFormat.Xlsx ? (IWorkbook)new XSSFWorkbook() : new HSSFWorkbook())
            {
                ISheet sheet = workbook.CreateSheet(sheetName);
                HashSet<int> textColumns = new HashSet<int>();

                // Freeze the top row
                sheet.CreateFreezePane(0, 1);

                // Write headers
                IRow headerRow = sheet.CreateRow(0);
                for (int i = 0; i < data.Columns.Count; i++)
                {
                    ICell cell = headerRow.CreateCell(i);
                    cell.SetCellValue(data.Columns[i].ColumnName);
                    if (data.Columns[i].DataType == typeof(string))
                        textColumns.Add(i);
                }

                // Write data
                for (int rowIdx = 0; rowIdx < data.Rows.Count; rowIdx++)
                {
                    IRow row = sheet.CreateRow(rowIdx + 1);
                    DataRow dataRow = data.Rows[rowIdx];
                    for (int colIdx = 0; colIdx < data.Columns.Count; colIdx++)
                    {
                        object value = dataRow[colIdx];
                        string cellValue = value?.ToString() ?? "";
                        row.CreateCell(colIdx).SetCellValue(CleanStringForExcel(cellValue));
                    }
                }

                // Apply formatting
                foreach (int colIdx in textColumns) ApplyTextFormat(sheet, colIdx);
                ApplyHeaderStyle(workbook, sheet);

                using (MemoryStream stream = new MemoryStream())
                {
                    workbook.Write(stream);
                    return stream.ToArray();
                }
            }
        }


        public static List<T> ReadExcelToList<T>(string filePath, int sheetIndex = 0) where T : new()
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Excel file not found.", filePath);

            IWorkbook workbook;
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                workbook = Path.GetExtension(filePath).ToLower() == ".xls" ? (IWorkbook)new HSSFWorkbook(fs) : new XSSFWorkbook(fs);
            }

            ISheet sheet = workbook.GetSheetAt(sheetIndex);
            IRow headerRow = sheet.GetRow(0);
            if (headerRow == null)
                throw new InvalidDataException("Excel file has no header row.");

            // Map headers to properties
            Dictionary<int, PropertyInfo> columnMappings = new Dictionary<int, PropertyInfo>();
            PropertyInfo[] properties = typeof(T).GetProperties();

            for (int colIdx = 0; colIdx < headerRow.LastCellNum; colIdx++)
            {
                ICell cell = headerRow.GetCell(colIdx);
                if (cell == null) continue;

                string headerText = cell.StringCellValue?.Trim() ?? "";
                PropertyInfo property = Array.Find(properties, p =>
                    p.Name.Equals(headerText, StringComparison.OrdinalIgnoreCase) && p.CanWrite);

                if (property != null && property.CanWrite)
                    columnMappings[colIdx] = property;
                else
                    Console.WriteLine($"Warning: Column '{headerText}' has no matching property in {typeof(T).Name}.");
            }

            // Read data rows
            List<T> result = new List<T>();
            for (int rowIdx = 1; rowIdx <= sheet.LastRowNum; rowIdx++)
            {
                IRow row = sheet.GetRow(rowIdx);
                if (row == null) continue;

                T item = new T();
                bool hasErrors = false;

                foreach (var mapping in columnMappings)
                {
                    ICell cell = row.GetCell(mapping.Key);
                    if (cell == null) continue;

                    try
                    {
                        object cellValue = GetCellValue(cell, mapping.Value.PropertyType);
                        mapping.Value.SetValue(item, cellValue);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error in Row {rowIdx + 1}, Column '{mapping.Value.Name}': {ex.Message}");
                        hasErrors = true;
                    }
                }

                if (!hasErrors)
                    result.Add(item);
                else
                    Console.WriteLine($"Warning: Row {rowIdx + 1} was skipped due to errors.");
            }

            return result;
        }

        /// <summary>
        /// Converts cell value to target type with error handling.
        /// </summary>
        private static object GetCellValue(ICell cell, Type targetType)
        {
            try
            {
                if (cell == null || targetType == null)
                    return null;

                switch (cell.CellType)
                {
                    case CellType.String:
                        return ConvertType(cell.StringCellValue, targetType);
                    case CellType.Numeric:
                        if (DateUtil.IsCellDateFormatted(cell))
                            return ConvertType(cell.DateCellValue, targetType);
                        else
                            return ConvertType(cell.NumericCellValue, targetType);
                    case CellType.Boolean:
                        return ConvertType(cell.BooleanCellValue, targetType);
                    case CellType.Formula:
                        return GetCellValue(cell, targetType); // Recurse to handle formula result
                    default:
                        return null;
                }
            }
            catch (Exception ex)
            {
                throw new InvalidCastException($"Failed to convert cell value to {targetType.Name}.", ex);
            }
        }

        /// <summary>
        /// Safely converts a value to the target type.
        /// </summary>
        private static object ConvertType(object value, Type targetType)
        {
            if (value == null || value is DBNull)
                return null;

            if (targetType == typeof(string))
                return value.ToString();

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch
            {
                // Handle common edge cases (e.g., parsing "30.5" to int)
                if (targetType == typeof(int) && value is double dblValue)
                    return Convert.ToInt32(dblValue);

                throw;
            }
        }

        public static void WriteDataTableToExcel(DataTable data, string filePath, string sheetName = "Sheet1")
        {
            if (data == null || data.Rows.Count == 0)
                throw new ArgumentException("DataTable is empty or null.");

            IWorkbook workbook = Path.GetExtension(filePath).ToLower() == ".xls"
                ? (IWorkbook)new HSSFWorkbook()
                : new XSSFWorkbook();

            ISheet sheet = workbook.CreateSheet(sheetName);
            HashSet<int> textColumns = new HashSet<int>();

            // Freeze the top row
            sheet.CreateFreezePane(0, 1);

            // Write headers
            IRow headerRow = sheet.CreateRow(0);
            for (int i = 0; i < data.Columns.Count; i++)
            {
                ICell cell = headerRow.CreateCell(i);
                cell.SetCellValue(data.Columns[i].ColumnName);
                if (data.Columns[i].DataType == typeof(string))
                    textColumns.Add(i);
            }

            // Write data
            for (int rowIdx = 0; rowIdx < data.Rows.Count; rowIdx++)
            {
                IRow row = sheet.CreateRow(rowIdx + 1);
                DataRow dataRow = data.Rows[rowIdx];
                for (int colIdx = 0; colIdx < data.Columns.Count; colIdx++)
                {
                    object value = dataRow[colIdx];
                    string cellValue = value?.ToString() ?? "";
                    row.CreateCell(colIdx).SetCellValue(CleanStringForExcel(cellValue));
                }
            }

            // Apply formatting
            foreach (int colIdx in textColumns) ApplyTextFormat(sheet, colIdx);
            ApplyHeaderStyle(workbook, sheet);

            // Save
            using (FileStream fs = new FileStream(filePath, FileMode.Create))
            {
                workbook.Write(fs);
            }
        }

        public static string CleanStringForExcel(string input, int maxLength = 32766, bool removeMultipleSpaces = true)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string cleaned = input;

            // Remove control characters (CR, LF, TAB, etc.)
            cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"[\x00-\x1F]", " ");

                       // Remove multiple consecutive spaces if requested
            if (removeMultipleSpaces)
            {
                cleaned = System.Text.RegularExpressions.Regex.Replace(cleaned, @"\s+", " ");
            }

            // Trim and limit length
            cleaned = cleaned.Trim();

            if (cleaned.Length > maxLength)
            {
                cleaned = cleaned.Substring(0, maxLength);
            }

            return cleaned;
        }

    }
}
