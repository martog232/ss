using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;

namespace SS.Core.Utilities
{
    public class ExcelHelpersNpoi
    {
        public static Dictionary<int, string> BuildHeadersMap(ISheet sheet, int headerRowNum, HashSet<string> requiredHeaders, Regex cleanRegex, bool checkForAdditionalHeadersInFile, bool checkForMissingHeadersInFile, IReadOnlyDictionary<int, string> headerSubstitutionMap = null)
        {
            var headerRow = GetRow(sheet, headerRowNum, cleanRegex, false);
            if (IsEmptyRow(headerRow))
            {
                throw new Exception("Header row is empty");
            }

            var headersInFile = new HashSet<string>();
            var headersMap = new Dictionary<int, string>();

            foreach (var indexValuePair in headerRow)
            {
                var headerIndex = indexValuePair.Key;
                var headerName = indexValuePair.Value;
                if (headerSubstitutionMap != null && headerSubstitutionMap.TryGetValue(headerIndex, out var substitutionName))
                {
                    headerName = substitutionName;
                }

                if (requiredHeaders.Contains(headerName))
                {
                    if (headersInFile.Contains(headerName))
                    {
                        throw new Exception($"Header '{headerName}' is duplicated");
                    }
                    headersInFile.Add(headerName);

                    headersMap[headerIndex] = headerName;
                }
                else
                {
                    if (checkForAdditionalHeadersInFile)
                    {
                        throw new Exception($"Header '{headerName}' is not supported");
                    }
                }
            }

            if (checkForMissingHeadersInFile && (headersMap.Count != requiredHeaders.Count))
            {
                // We don't have each required header in the file
                requiredHeaders.ExceptWith(headersMap.Values);
                throw new Exception("The following headers are missing in the file: " + string.Join("; ", requiredHeaders));
            }

            return headersMap;
        }

        public static Dictionary<int, string> GetRow(ISheet sheet, int rowIndex, Regex cleanRegex, bool treatWhiteSpaceCellsAsNull, HashSet<int> specificCellsOnly = null)
        {
            var rowData = new Dictionary<int, string>();

            IRow row = sheet.GetRow(rowIndex);
            if (row != null)
            {
                foreach (var cell in row.Cells)
                {
                    if (specificCellsOnly == null || specificCellsOnly.Contains(cell.ColumnIndex))
                    {
                        rowData[cell.ColumnIndex] = GetStringCellValue(cell, cleanRegex, treatWhiteSpaceCellsAsNull);
                    }
                }
            }

            return rowData;
        }

        public static string GetStringCellValue(ICell cell, Regex cleanRegex, bool treatWhiteSpaceCellsAsNull)
        {
            var cellType = cell.CellType;
            var dataFormatString = cell.CellStyle.GetDataFormatString();

            var isCellTypeText =
                cellType == CellType.Blank ||
                (cellType == CellType.String &&  dataFormatString == "@") ||
                (cellType == CellType.Numeric && dataFormatString == "@") ||
                (cellType == CellType.Boolean && dataFormatString == "@") ||
                (cellType == CellType.Formula && dataFormatString == "@");

            if (!isCellTypeText)
            {
                throw new Exception("Ensure all Excel cells to be read have a CellType.String or CellType.Blank");
            }

            // a special case - type "Numeric" && format "@" happens when copy-pasting from Google Docs or from Excel with external data source. 
            string result;

            var type = cell.CellType == CellType.Formula ? cell.CachedFormulaResultType : cell.CellType;

            switch (type)
            {
                case CellType.Numeric:
                    result = cell.NumericCellValue.ToString(CultureInfo.InvariantCulture);
                    break;

                case CellType.Boolean:
                    result = cell.BooleanCellValue.ToString(CultureInfo.InvariantCulture);
                    break;

                default:
                    result = cell.StringCellValue;
                    break;
            }

            if (cleanRegex != null && result != null)
            {
                result = cleanRegex.Replace(result, "");
            }

            if (treatWhiteSpaceCellsAsNull && string.IsNullOrWhiteSpace(result))
            {
                result = null;
            }

            return result;
        }

        public static bool IsEmptyRow<TK, TV>(Dictionary<TK, TV> row)
        {
            return row == null || !row.Any() || row.Values.All(v => v == null);
        }
    }
}
