using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using SS.Core.Utilities.Contract;
using System;
using System.Collections.Generic;
using System.IO;

namespace SS.Core.Utilities
{
    public class FileUtility<T> : IFileUtility
    {
        public IEnumerable<string> ReadXlsxFile(string fileName)
        {
            var fileInfo = new List<string>();

            try
            {
                IWorkbook workbook;
                ISheet sheet;
                FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

                if (fileName.IndexOf(".xlsx") > 0) { workbook = new XSSFWorkbook(fs); }
                else { workbook = new HSSFWorkbook(fs); }

                sheet = workbook.GetSheetAt(0);

                if (sheet != null)
                {
                    int rowCount = sheet.LastRowNum; // This may not be valid row count.
                                                     // If first row is table head, i starts from 1
                    for (int i = 1; i <= rowCount; i++)
                    {
                        IRow curRow = sheet.GetRow(i);
                        // Works for consecutive data. Use continue otherwise 
                        if (curRow == null)
                        {
                            // Valid row count
                            rowCount = i - 1;
                            break;
                        }
                        // Get data from the 4th column (4th cell of each row)
                        var cellValue = curRow.GetCell(3).StringCellValue.Trim();

                        fileInfo.Add(cellValue);
                    }
                }

                return fileInfo;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return null;
            }
        }
    }
}
