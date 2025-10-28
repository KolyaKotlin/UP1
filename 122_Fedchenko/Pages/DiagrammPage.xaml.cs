using System.Linq;
using System.Windows.Controls;
using System.Windows.Forms.DataVisualization.Charting;
using Word = Microsoft.Office.Interop.Word;
using Excel = Microsoft.Office.Interop.Excel;
using System;

namespace _122_Fedchenko.Pages
{
    public partial class DiagrammPage : Page
    {
        private Entities _context = new Entities();

        public DiagrammPage()
        {
            InitializeComponent();

            ChartPayments.ChartAreas.Add(new ChartArea("Main"));
            var currentSeries = new Series("Платежи") { IsValueShownAsLabel = true };
            ChartPayments.Series.Add(currentSeries);

            CmbUser.ItemsSource = _context.User.ToList();
            CmbDiagram.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
            CmbDiagram.SelectedIndex = 0;
        }

        private void UpdateChart(object sender, SelectionChangedEventArgs e)
        {
            if (CmbUser.SelectedItem is User currentUser && CmbDiagram.SelectedItem is SeriesChartType currentType)
            {
                var currentSeries = ChartPayments.Series.FirstOrDefault();
                if (currentSeries == null) return;

                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categoriesList = _context.Category.ToList();
                foreach (var category in categoriesList)
                {
                    var sum = _context.Payment
                        .Where(p => p.UserID == currentUser.ID && p.CategoryID == category.ID)
                        .Sum(p => p.Price * p.Num);

                    currentSeries.Points.AddXY(category.Name, sum);
                }
            }
        }

        private void ExportToExcel_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var allUsers = _context.User.ToList().OrderBy(u => u.FIO).ToList();
            Excel.Application excel = new Excel.Application();
            excel.SheetsInNewWorkbook = allUsers.Count;
            Excel.Workbook workbook = excel.Workbooks.Add(Type.Missing);

            for (int i = 0; i < allUsers.Count; i++)
            {
                int startRowIndex = 1;
                Excel.Worksheet worksheet = (Excel.Worksheet)excel.Worksheets[i + 1];

                string sheetName = allUsers[i].FIO;
                if (sheetName.Length > 31)
                    sheetName = sheetName.Substring(0, 31);

                int suffix = 1;
                string originalName = sheetName;
                while (WorksheetExists(workbook, sheetName))
                {
                    sheetName = originalName.Substring(0, Math.Min(28, originalName.Length)) + "_" + suffix;
                    suffix++;
                }
                worksheet.Name = sheetName;

                worksheet.Cells[startRowIndex, 1] = "Дата платежа";
                worksheet.Cells[startRowIndex, 2] = "Название";
                worksheet.Cells[startRowIndex, 3] = "Стоимость";
                worksheet.Cells[startRowIndex, 4] = "Количество";
                worksheet.Cells[startRowIndex, 5] = "Сумма";

                Excel.Range headerRange = worksheet.Range[worksheet.Cells[startRowIndex, 1], worksheet.Cells[startRowIndex, 5]];
                headerRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                headerRange.Font.Bold = true;
                startRowIndex++;

                var userCategories = allUsers[i].Payment
                    .OrderBy(p => p.Date)
                    .GroupBy(p => p.Category)
                    .OrderBy(g => g.Key.Name);

                foreach (var groupCategory in userCategories)
                {
                    Excel.Range categoryRange = worksheet.Range[worksheet.Cells[startRowIndex, 1], worksheet.Cells[startRowIndex, 5]];
                    categoryRange.Merge();
                    categoryRange.Value = groupCategory.Key.Name;
                    categoryRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    categoryRange.Font.Italic = true;
                    startRowIndex++;

                    foreach (var payment in groupCategory)
                    {
                        worksheet.Cells[startRowIndex, 1] = payment.Date;
                        (worksheet.Cells[startRowIndex, 1] as Excel.Range).NumberFormat = "dd.MM.yyyy";
                        worksheet.Cells[startRowIndex, 2] = payment.Name;
                        worksheet.Cells[startRowIndex, 3] = payment.Price;
                        (worksheet.Cells[startRowIndex, 3] as Excel.Range).NumberFormat = "0.00";
                        worksheet.Cells[startRowIndex, 4] = payment.Num;
                        worksheet.Cells[startRowIndex, 5].Formula = $"=C{startRowIndex}*D{startRowIndex}";
                        (worksheet.Cells[startRowIndex, 5] as Excel.Range).NumberFormat = "0.00";
                        startRowIndex++;
                    }

                    Excel.Range sumRange = worksheet.Range[worksheet.Cells[startRowIndex, 1], worksheet.Cells[startRowIndex, 4]];
                    sumRange.Merge();
                    sumRange.Value = "ИТОГО:";
                    sumRange.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                    worksheet.Cells[startRowIndex, 5].Formula = $"=SUM(E{startRowIndex - groupCategory.Count()}:" +
                                                                $"E{startRowIndex - 1})";
                    sumRange.Font.Bold = worksheet.Cells[startRowIndex, 5].Font.Bold = true;
                    startRowIndex++;

                    Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1, 1], worksheet.Cells[startRowIndex - 1, 5]];
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                    rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;

                    worksheet.Columns.AutoFit();
                }
            }

            excel.Visible = true;
        }

        private bool WorksheetExists(Excel.Workbook wb, string name)
        {
            foreach (Excel.Worksheet ws in wb.Worksheets)
            {
                if (ws.Name == name) return true;
            }
            return false;
        }


        private void ExportToWord_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Word.Application word = new Word.Application();
            Word.Document doc = word.Documents.Add();

            var allUsers = _context.User.ToList();
            var allCategories = _context.Category.ToList();

            foreach (var user in allUsers)
            {
                Word.Paragraph userParagraph = doc.Paragraphs.Add();
                Word.Range userRange = userParagraph.Range;
                userRange.Text = user.FIO;
                userParagraph.set_Style(Word.WdBuiltinStyle.wdStyleHeading1);
                userRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                userRange.InsertParagraphAfter();

                Word.Paragraph tableParagraph = doc.Paragraphs.Add();
                Word.Range tableRange = tableParagraph.Range;
                Word.Table paymentsTable = doc.Tables.Add(tableRange, allCategories.Count + 1, 2);
                paymentsTable.Borders.InsideLineStyle = paymentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
                paymentsTable.Range.Cells.VerticalAlignment = Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

                paymentsTable.Cell(1, 1).Range.Text = "Категория";
                paymentsTable.Cell(1, 2).Range.Text = "Сумма расходов";
                paymentsTable.Rows[1].Range.Font.Name = "Times New Roman";
                paymentsTable.Rows[1].Range.Font.Size = 14;
                paymentsTable.Rows[1].Range.Bold = 1;
                paymentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                for (int i = 0; i < allCategories.Count; i++)
                {
                    var category = allCategories[i];
                    paymentsTable.Cell(i + 2, 1).Range.Text = category.Name;
                    paymentsTable.Cell(i + 2, 1).Range.Font.Name = "Times New Roman";
                    paymentsTable.Cell(i + 2, 1).Range.Font.Size = 12;

                    var sum = user.Payment.Where(p => p.CategoryID == category.ID).Sum(p => p.Price * p.Num);
                    paymentsTable.Cell(i + 2, 2).Range.Text = sum.ToString("N2") + " руб.";
                    paymentsTable.Cell(i + 2, 2).Range.Font.Name = "Times New Roman";
                    paymentsTable.Cell(i + 2, 2).Range.Font.Size = 12;
                }

                Payment maxPayment = user.Payment.OrderByDescending(p => p.Price * p.Num).FirstOrDefault();
                if (maxPayment != null)
                {
                    Word.Paragraph maxPaymentParagraph = doc.Paragraphs.Add();
                    Word.Range maxPaymentRange = maxPaymentParagraph.Range;
                    maxPaymentRange.Text = $"Самый дорогой платеж - {maxPayment.Name} за {(maxPayment.Price * maxPayment.Num).ToString("N2")} руб. от {maxPayment.Date:dd.MM.yyyy}";
                    maxPaymentParagraph.set_Style(Word.WdBuiltinStyle.wdStyleSubtitle);
                    maxPaymentRange.Font.Color = Word.WdColor.wdColorDarkRed;
                    maxPaymentRange.InsertParagraphAfter();
                }

                Payment minPayment = user.Payment.OrderBy(p => p.Price * p.Num).FirstOrDefault();
                if (minPayment != null)
                {
                    Word.Paragraph minPaymentParagraph = doc.Paragraphs.Add();
                    Word.Range minPaymentRange = minPaymentParagraph.Range;
                    minPaymentRange.Text = $"Самый дешевый платеж - {minPayment.Name} за {(minPayment.Price * minPayment.Num).ToString("N2")} руб. от {minPayment.Date:dd.MM.yyyy}";
                    minPaymentParagraph.set_Style(Word.WdBuiltinStyle.wdStyleSubtitle);
                    minPaymentRange.Font.Color = Word.WdColor.wdColorDarkGreen;
                    minPaymentRange.InsertParagraphAfter();
                }

                if (user != allUsers.LastOrDefault())
                {
                    doc.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);
                }
            }

            foreach (Word.Section section in doc.Sections)
            {
                Word.HeaderFooter footer = section.Footers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary];
                footer.PageNumbers.Add(Word.WdPageNumberAlignment.wdAlignPageNumberCenter);

                Word.Range headerRange = section.Headers[Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Text = DateTime.Now.ToString("dd/MM/yyyy");
                headerRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                headerRange.Font.ColorIndex = Word.WdColorIndex.wdBlack;
                headerRange.Font.Size = 10;
            }

            word.Visible = true;
        }
    }
}
