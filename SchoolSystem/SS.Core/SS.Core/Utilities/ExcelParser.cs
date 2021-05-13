using NPOI.SS.UserModel;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SS.Core.Utilities
{
    public class ExcelParser
    {
        public const string SourceRowHeaderName = "DefaultRowHeader-51B631A2-A896-47B1-A316-95AA2BAD35BC";
        private readonly Settings _settings;

        public ExcelParser(Settings settings)
        {
            _settings = settings;
        }

        public List<Dictionary<string, string>> ParseSheet(ISheet sheet, IEnumerable<string> headersCollection, int headerRowNum, int firstDataRowNum, IReadOnlyDictionary<int, string> headerSubstitutionMap = null)
        {
            var requiredHeaders = new HashSet<string>(headersCollection);
            var headersMap = ExcelHelpersNpoi.BuildHeadersMap(sheet, headerRowNum, requiredHeaders, _settings.CleanHeaderRegex, _settings.CheckForAdditionalHeadersInFile, _settings.CheckForMissingHeadersInFile, headerSubstitutionMap);

            var result = new List<Dictionary<string, string>>();
            for (int i = firstDataRowNum; i <= sheet.LastRowNum; i++)
            {
                var rowValues = ParseDataRow(sheet, i, headersMap);

                if (_settings.StopAtFirstEmptyRow && ExcelHelpersNpoi.IsEmptyRow(rowValues))
                {
                    break;
                }

                if (!_settings.SkipEmptyRows || !ExcelHelpersNpoi.IsEmptyRow(rowValues))
                {
                    if (_settings.SetNullForMissingValues && rowValues.Count != requiredHeaders.Count)
                    {
                        foreach (var header in requiredHeaders)
                        {
                            if (!rowValues.ContainsKey(header))
                            {
                                rowValues[header] = null;
                            }
                        }
                    }

                    rowValues[SourceRowHeaderName] = i.ToString();
                    result.Add(rowValues);
                }
            }

            return result;
        }

        private Dictionary<string, string> ParseDataRow(ISheet sheet, int rowIndex, Dictionary<int, string> headersMap)
        {
            var dataRow = ExcelHelpersNpoi.GetRow(sheet, rowIndex, null, _settings.TreatWhiteSpaceCellsAsNull, new HashSet<int>(headersMap.Keys));

            var rowValues = new Dictionary<string, string>();

            foreach (var indexValuePair in dataRow)
            {
                if (headersMap.TryGetValue(indexValuePair.Key, out var columnName))
                {
                    rowValues[columnName] = indexValuePair.Value;
                }
            }

            return rowValues;
        }
    }

    public class Settings
    {
        public bool CheckForAdditionalHeadersInFile { get; set; }

        public bool CheckForMissingHeadersInFile { get; set; }

        public bool TreatWhiteSpaceCellsAsNull { get; set; }

        public bool SetNullForMissingValues { get; set; }

        public bool StopAtFirstEmptyRow { get; set; }

        public bool SkipEmptyRows { get; set; }

        public Regex CleanHeaderRegex { get; set; }
    }
}
