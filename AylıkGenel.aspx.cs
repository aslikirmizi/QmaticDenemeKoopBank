using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using QmaticDeneme.qmaticData; 

namespace QmaticDeneme
{
    public partial class AylıkGenel : System.Web.UI.Page
    {
        QmaticWS qmaticWS = new QmaticWS();
        Dictionary<string, List<decimal>> BranchesDailyDict = new Dictionary<string, List<decimal>>();
        HashSet<string> dateTimes = new HashSet<string>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                

                PickMonth.DataSource = CreateMonthDataSource();
                PickMonth.DataTextField = "Ay";
                PickMonth.DataValueField = "AyDegeri";
                PickMonth.DataBind();
                PickMonth.SelectedIndex = 0;

                PickYear.DataSource = CreateYearDataSource();
                PickYear.DataTextField = "Yıl";
                PickYear.DataValueField = "YılDeger";
                PickYear.DataBind();
                PickYear.SelectedIndex = 0;

                Table1.Visible = false;
                Table2.Visible = false;
                LineChart1.Visible = false;

            }
            else
            {
                Table1.Visible = false;
                Table2.Visible = false;
                errorLabel.Text = "";
                LineChart1.Visible = false;
            }
        }

        private Dictionary<string, int> retrieveMonthData(DateTime start, DateTime end )
        {
            var data = qmaticWS.GetMainRangeReport(start, end, "BRANCH");

            Dictionary<string, int> BranchMonthly = new Dictionary<string, int>();

            foreach (var d in data)
            {
                //Console.WriteLine(d.ReferenceName + "----" + d.ReferenceDate + "----" + d.TotalTicket );
                if (BranchMonthly.ContainsKey(d.ReferenceName)) BranchMonthly[d.ReferenceName] += d.TotalTicket;
                else BranchMonthly.Add(d.ReferenceName, d.TotalTicket);

                if (BranchesDailyDict.ContainsKey(d.ReferenceName)) BranchesDailyDict[d.ReferenceName].Add(d.TotalTicket);
                else
                {
                    List<Decimal> list = new List<decimal>();
                    //list.Insert(0, item);
                    list.Add(d.TotalTicket);
                    BranchesDailyDict.Add(d.ReferenceName, list);
                }
                
            }
            //foreach (var branch in BranchesDailyDict)
            //{
            //  Label1.Text += branch.Value.Count + "-";
            //}
            //errorLabel.Text = BranchesDailyDict.Count.ToString();
            var maxCount = 0;
            foreach (var branch in BranchesDailyDict)
            {
                if (branch.Value.Count() > maxCount) maxCount = branch.Value.Count();
            }

            foreach (var branch in BranchesDailyDict)
            {
                if (branch.Value.Count() < maxCount)
                {
                    while (branch.Value.Count() < maxCount)
                    {
                        branch.Value.Insert(0, 0);
                    }
                }
            }
            return BranchMonthly;
        }

        private Dictionary<string, int> retrieveMonthDataForType(DateTime start, DateTime end)
        {
            var data = qmaticWS.GetMainRangeReport(start, end, "TYPE");

            Dictionary<string, int> BranchMonthly = new Dictionary<string, int>();

            foreach (var d in data)
            {
                dateTimes.Add(d.ReferenceDate);
                //Console.WriteLine(d.ReferenceName + "----" + d.ReferenceDate + "----" + d.TotalTicket );
                if (BranchMonthly.ContainsKey(d.ReferenceName)) BranchMonthly[d.ReferenceName] += d.TotalTicket;
                else BranchMonthly.Add(d.ReferenceName, d.TotalTicket);
            }

            return BranchMonthly;
        }

        ICollection CreateMonthDataSource()
        {
            DataTable dt = new DataTable();

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("Ay", typeof(String)));
            dt.Columns.Add(new DataColumn("AyDegeri", typeof(String)));

            // Populate the table with sample values.
            dt.Rows.Add(CreateRow("Ocak", "1", dt));
            dt.Rows.Add(CreateRow("Subat", "2", dt));
            dt.Rows.Add(CreateRow("Mart", "3", dt));
            dt.Rows.Add(CreateRow("Nisan", "4", dt));
            dt.Rows.Add(CreateRow("Mayıs", "5", dt));
            dt.Rows.Add(CreateRow("Haziran", "6", dt));
            dt.Rows.Add(CreateRow("Temmuz", "7", dt));
            dt.Rows.Add(CreateRow("Ağustos", "8", dt));
            dt.Rows.Add(CreateRow("Eylül", "9", dt));
            dt.Rows.Add(CreateRow("Ekim", "10", dt));
            dt.Rows.Add(CreateRow("Kasım", "11", dt));
            dt.Rows.Add(CreateRow("Aralık", "12", dt));

            DataView dv = new DataView(dt);
            return dv;
        }

        DataRow CreateRow(String Text, String Value, DataTable dt)
        {

            // Create a DataRow using the DataTable defined in the 
            // CreateDataSource method.
            DataRow dr = dt.NewRow();
            dr[0] = Text;
            dr[1] = Value;
            return dr;
        }

        public static IEnumerable<DateTime> AllDatesInMonth(int year, int month)
        {
            int days = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= days; day++)
            {
                DateTime newDay = new DateTime(year, month, day);
                DayOfWeek Checkday = newDay.DayOfWeek;
                if (!((Checkday == DayOfWeek.Saturday) || (Checkday == DayOfWeek.Sunday)))
                {
                    yield return newDay;
                }
                
            }
        }

        ICollection CreateYearDataSource()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("Yıl", typeof(String)));
            dt.Columns.Add(new DataColumn("YılDeger", typeof(String)));
            QmaticWS subeData = new QmaticWS();
            int startYear = 2020;
            int EndYear = DateTime.Today.Year;
            while (startYear <= EndYear)
            {
                dt.Rows.Add(CreateRow(startYear.ToString(), (startYear).ToString(), dt));
                startYear++;
            }

            DataView dv = new DataView(dt);
            return dv;
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            var date = AllDatesInMonth( int.Parse(PickYear.SelectedValue), int.Parse(PickMonth.SelectedValue));
            Dictionary<string, int> monthData = retrieveMonthData(date.First(), date.Last());
            CreateTable(monthData);

            Dictionary<string, int> monthDataByType = retrieveMonthDataForType(date.First(), date.Last());
            CreateTableOfType(monthDataByType);

            createGraph();

        }

        private void createGraph()
        {
            var dates = AllDatesInMonth(int.Parse(PickYear.SelectedValue), int.Parse(PickMonth.SelectedValue));
            var xAxis = "" ;
            foreach(var element in dateTimes)
            {
                //xAxis += element.Day;
            }
            for (var i = 0; i<dates.Count(); ++i)
            {
                if(i == dates.Count() - 1)
                {
                    xAxis += dates.ElementAt(i).Day;
                }
                else
                {
                    xAxis += dates.ElementAt(i).Day + ","; 
                }
            }
            LineChart1.ChartWidth = "800";//(prices.Length * 75).ToString();
            LineChart1.ChartHeight = "400";
            LineChart1.DisplayValues = false;
            foreach(var item in BranchesDailyDict)
            {
                var output = Regex.Match(item.Key, @"^([\w\-]+)");  //remove the other words except 1st one
                LineChart1.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = output.ToString() , Data = item.Value.ToArray() });
            }
            LineChart1.CategoriesAxis = xAxis;
            LineChart1.Visible = true;

        }

        private void CreateTableOfType(Dictionary<string, int> monthDataByType)
        {
            if (monthDataByType.Count() > 1) { 
                TableRow headerRow = new TableRow();
                var emptyCell = new TableCell();
                emptyCell.Text = "Şube İsmi";
                var totCell = new TableCell();
                totCell.Text = "Müşteri";
                headerRow.Cells.Add(emptyCell);
                headerRow.Cells.Add(totCell);
                Table2.Rows.Add(headerRow);
                int sum = 0;
                foreach (var key in monthDataByType)
                {
                    TableRow dataRow = new TableRow();
                    TableCell cell = new TableCell();
                    cell.Text = key.Key;
                    dataRow.Cells.Add(cell);
                    TableCell data = new TableCell();
                    sum += monthDataByType[key.Key];
                    data.Text = monthDataByType[key.Key].ToString();
                    dataRow.Cells.Add(data);
                    Table2.Rows.Add(dataRow);
                }
                Table2.Visible = true;
            }
            else
            {
                errorLabel.Text = "Aradığnız Ay için veri bulunamamştır.";
            }
        }

        private void CreateTable(Dictionary<string, int> monthData)
        {
            

            TableRow headerRow = new TableRow();
            var emptyCell = new TableCell();
            emptyCell.Text = "Şube İsmi";
            var totCell = new TableCell();
            totCell.Text = "Müşteri";
            headerRow.Cells.Add(emptyCell);
            headerRow.Cells.Add(totCell);
            Table1.Rows.Add(headerRow);
            int sum = 0;
            foreach (var key in monthData)
            {
                TableRow dataRow = new TableRow();
                TableCell cell = new TableCell();
                cell.Text = key.Key;
                dataRow.Cells.Add(cell);
                TableCell data = new TableCell();
                sum += monthData[key.Key];
                data.Text = monthData[key.Key].ToString();
                dataRow.Cells.Add(data);
                Table1.Rows.Add(dataRow);
            }
            if(sum>10) Table1.Visible = true;

        }


    }
}