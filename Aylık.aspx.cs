using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using QmaticDeneme.qmaticData;
using System.Web.UI.WebControls;
using System.Diagnostics;
using System.Threading;
using System.Globalization;

namespace QmaticDeneme
{
    public partial class Aylık : System.Web.UI.Page
    {
        private Dictionary<int, string> branchesDict = new Dictionary<int, string>();
        private Dictionary<string, int> tellerStats = new Dictionary<string, int>();
        private Dictionary<string, int> branchStats = new Dictionary<string, int>();

        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                //welcome text 
               Label1.Text = "Qmatic Aylık Şube İstatsitiklerini görebilmek için, Şube ismini, ayını, ve yılını seçiniz";
                //----------------------------get months --------------------------------
                PickMonth.DataSource = CreateMonthDataSource();
                PickMonth.DataTextField = "Ay";
                PickMonth.DataValueField = "AyDegeri";
                PickMonth.DataBind();
                PickMonth.SelectedIndex = 0;

                //-----------------------------yıl select-------------------------------
                PickYear.DataSource = CreateYearDataSource();
                PickYear.DataTextField = "Yıl";
                PickYear.DataValueField = "YılDeger";
                PickYear.DataBind();
                PickYear.SelectedIndex = 0;

                //------------------------------sube ler--------------------------------
                DropDownList1.DataSource = CreateDataSource();
                DropDownList1.DataTextField = "Sube";
                DropDownList1.DataValueField = "Subnet";
                DropDownList1.DataBind();
                DropDownList1.SelectedIndex = 0;

                //------------------------------grafik kısmı-------------------------------
                LineChart1.ChartTitle = "Aylık Müşteri Tablosu";
                LineChart1.ChartWidth = "800";

                //hide them for now
                LineChart1.Visible = false;
                Table1.Visible = false;
                tellerTable.Visible = false;
            }
            else{
                CreateDataSource();
                LineChart1.Visible = false;
                Table1.Visible = false;
                tellerTable.Visible = false;
            }
        }

        ICollection CreateDataSource()
        {

            // Create a table to store data for the DropDownList control.
            DataTable dt = new DataTable();

            // Define the columns of the table.
            dt.Columns.Add(new DataColumn("Sube", typeof(String)));
            dt.Columns.Add(new DataColumn("Subnet", typeof(String)));

            QmaticWS subeData = new QmaticWS();
            DateTime testDate = new DateTime(2020, 7, 6);
            var branchData = subeData.GetActiveBranches();
            
            // Populate the table with sample values.
            foreach (var branch in branchData)
            {
                //Label1.Text += branch.TicketSubnet + branch.BranchName; 
                dt.Rows.Add(CreateRow(branch.BranchName, (branch.BranchSubnet).ToString() , dt));
                branchesDict.Add(branch.BranchSubnet, branch.BranchName); 
            }
            // Create a DataView from the DataTable to act as the data source
            // for the DropDownList control.
            DataView dv = new DataView(dt);
            return dv;

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

        /*
         * gets an iterable collection of all dates in that month
         * v1.1 gets all days except satudays and sundays
         */
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
                else { 
                    //yield return newDay ;
                }
            }
        }

        /*
         * On button click, retrieve data for the month from the web service
         * manipulate the data and show it on  a table and on ajax control toolkit line graph
         * this process takes a 5-10 seconds depending on the network speed and load on the web service
         * optimzations that can be applied:
         * 1. create a web service method for retrieving the montly data at one go
         *  a. current method retrieves the data at 30 requests -- makes 30 daily sub report requests
         * 2. may use threads for faster execution -- current complexity O(n^2)
         *  a. one thread may do from 1st to 15 and the other 15th to end of the month
         */
        protected void Button1_Click(object sender, EventArgs e)
        {

            QmaticWS qmatic = new QmaticWS();
            var selectedBranch = DropDownList1.SelectedValue; //sube subnet
            var month = Int32.Parse(PickMonth.SelectedValue);
            var year = Int32.Parse(PickYear.SelectedValue);
            var daysInMonth = AllDatesInMonth(year, month);
            
            List<decimal> decimalData = new List<decimal>();
            Dictionary<string, int> dayTotalByKey = new Dictionary<string, int>();
            string xAxiS = "";

            var montlyData = qmatic.GetMainBranchRangeReport(int.Parse(selectedBranch), daysInMonth.First(), daysInMonth.Last(), "Branch");
            var pulledData = qmatic.GetDailySubReport(int.Parse(selectedBranch), daysInMonth.First(), daysInMonth.Last(), "teller");
            var pulledBranchData = qmatic.GetMainBranchRangeReport(int.Parse(selectedBranch), daysInMonth.First(), daysInMonth.Last(), "type");
            foreach (var day in montlyData)
            {
                dayTotalByKey.Add(DateTime.ParseExact(day.ReferenceDate, "yyyy-MM-dd", CultureInfo.InvariantCulture).ToShortDateString(), day.TotalTicket);
            }
            var i = 0;
            var limit = montlyData.Length;
            foreach(var newDay in daysInMonth)
            {
                Label1.Text += newDay;
                //  if (!(newDay.Day.ToString() == "1")) xAxiS += "," + newDay.Day.ToString();
                if (dayTotalByKey.ContainsKey(newDay.ToShortDateString()))
                {
                    decimalData.Add(dayTotalByKey[newDay.ToShortDateString()]);
                    xAxiS += newDay.Day.ToString();
                    if (i < limit-1)
                    {
                        xAxiS += ",";
                        i++;
                    }
                }
            }
            CreateGraph(xAxiS, decimalData.ToArray());
            CreateTellerTable(pulledData);
            CreateBranchTable(pulledBranchData);
        }

        /**
         * drawsa graph with the available total customer data per day for the month
         * @param x axis data , which is the days of the month may be 28/29/30/31 days depending on the month and the year
         * @param grah y data decimal array containing all the y values 
         */
        public void CreateGraph(string xAxiS, decimal[] graphYData)
        {
            LineChart1.ChartWidth = "800";//(prices.Length * 75).ToString();
            var subeName = branchesDict[int.Parse(DropDownList1.SelectedValue)];
            LineChart1.Series.Add(new AjaxControlToolkit.LineChartSeries { Name = "Müşteri", Data = graphYData });

            if (subeName != null) LineChart1.ChartTitle = subeName + " Aylık Müşteri Grafiği";
            LineChart1.AreaDataLabel = " Müşteri ";
            LineChart1.CategoriesAxis = xAxiS;
            //LineChart1.ChartAreas["ChartArea1"].Area3DStyle.Enable3D = true;

            if (graphYData.Length > 4) LineChart1.Visible = true;
        }

        /**
         * create teller table 
         * from the dictionary values
         * @param dont show the data if not more than one teller
         */
        public void CreateTellerTable(Qmatic_DailySubReport[] pulledData)
        {
            foreach (var teller in pulledData)
            {
                if (tellerStats.ContainsKey(teller.ReferenceName)) tellerStats[teller.ReferenceName] += teller.TotalTicket;
                else tellerStats.Add(teller.ReferenceName, teller.TotalTicket);
            }

            foreach (var key in tellerStats)
            {
                TableRow tellerRow = new TableRow();
                TableCell tellerName = new TableCell();
                tellerName.Text = key.Key;
                TableCell tellerTotal = new TableCell();
                tellerTotal.Text = (key.Value).ToString();
                tellerRow.Cells.Add(tellerName);
                tellerRow.Cells.Add(tellerTotal);
                tellerTable.Rows.Add(tellerRow);
            }
            if (tellerStats.Count() <= 1) tellerTable.Visible = false;
            else tellerTable.Visible = true;
        }

        public void CreateBranchTable(Qmatic_DailyMainReport[] pulledData)
        {
            //keep a dict of dates that have an associated customer 
            HashSet<string> dates = new HashSet<string>();
            foreach (var teller in pulledData)
            {
                dates.Add(teller.ReferenceDate);
                if (branchStats.ContainsKey(teller.ReferenceName)) branchStats[teller.ReferenceName] += teller.TotalTicket;
                else branchStats.Add(teller.ReferenceName, teller.TotalTicket);
            }

            var keys = branchStats.Keys;
            TableRow headerRow = new TableRow();
            TableRow dataRow = new TableRow();
            int sum = 0; 
            foreach(var key in keys)
            {
                
                TableCell cell = new TableCell();
                cell.Text = key; 
                headerRow.Cells.Add(cell);
                TableCell data = new TableCell();
                sum += branchStats[key]; 
                data.Text = branchStats[key].ToString() ;
                dataRow.Cells.Add(data);

            }
            //manuel rows for comparison
            TableCell tot = new TableCell();
            tot.Text = "Toplam"; headerRow.Cells.Add(tot);
            TableCell totDat = new TableCell();
            totDat.Text = sum.ToString();dataRow.Cells.Add(totDat);
            //working days
            TableCell workTot = new TableCell();
            workTot.Text = "İş günleri"; headerRow.Cells.Add(workTot);
            TableCell workDat = new TableCell();
            workDat.Text = dates.Count().ToString(); dataRow.Cells.Add(workDat);
            //günlük averaj
            TableCell workAv = new TableCell();
            workAv.Text = "Ortalama"; headerRow.Cells.Add(workAv);
            TableCell worAvDat = new TableCell();
            if(dates.Count() > 0 )
                worAvDat.Text = (sum/dates.Count()).ToString(); dataRow.Cells.Add(worAvDat);

            Table1.Rows.Add(headerRow);
            Table1.Rows.Add(dataRow);
            if (sum > 10)
            {
                Table1.Visible = true;
                errorLabel.Text = "";
            }
            else
            {
                errorLabel.Text = "Aradığınız tarih aralığı için bu şubede veri bulunamamıştır.";
                Table1.Visible = false;
            }

        }
        
    }
}