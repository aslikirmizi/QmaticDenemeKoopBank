using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.Configuration;
using QmaticDeneme.qmaticData;
using System.Web.Services;
using System.Globalization;

namespace QmaticDeneme
{
    
    public partial class landing : System.Web.UI.Page
    {
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)              
                firstMsg.Text = "Merhaba, Şube istatistiklerini görüntülemek için, görüntülemek istediğiniztarihi seçip, ara tuşuna tıklayınız.";
            else
                firstMsg.Text = "Arama sonuçlarınızı aşağıda görebilirsiniz. Akordiyonların üzerine tıklayarak şube hakkında detaylı bilgi elde edebilirsiniz.";
        }

        protected void btnAra_Click(object sender, EventArgs e)
        {

            //System.Threading.Thread.Sleep(100000);


            if (Request["dateDeneme"] != null) {
                string dateInp = Request["dateDeneme"];

                total.Text = "";
                if (checkIfInFuture(dateInp)) firstMsg.Text = DateTime.Now.ToShortDateString() + "'dan önce olan bir tarih giriniz.";
                else if (checkIfWeekend(dateInp)) firstMsg.Text = "Bankamız Cumartesi ve Pazar günleri açık olmadığından dolayı bügünler için verimiz bulunmamaktadır";
                else
                {
                    Qmatic_DailyMainReport[] webData = null;
                    bool continueExec = true; 
                    try
                    {
                        webData = getServiceData(Convert.ToDateTime(dateInp));
                    }
                    catch (Exception)
                    {
                        firstMsg.Text = "Servisimizde bir hata olmuştur, Lütfen daha sonra tekrar deyeniz";
                        continueExec = false; 
                    }
                    if (continueExec) { 
                        if (webData.Length == 0) firstMsg.Text = "Aradığınız gün için sonuç bulunamamıştır";
                        else
                        {
                            Accordion acrDynamic = new Accordion();
                            acrDynamic.ID = "MyAccordion";
                            acrDynamic.SelectedIndex = -1;//No default selection  
                            acrDynamic.RequireOpenedPane = false;//no open pane  
                            acrDynamic.HeaderCssClass = "accordionHeader";//Header class  
                            acrDynamic.HeaderSelectedCssClass = "accordionHeaderSelected";//Selected herder class  
                            acrDynamic.ContentCssClass = "accordionContent";//Content class  

                            Label lbTitle;
                            Label lbContent;
                            AccordionPane pane;
                            string Content = "";
                            int bankCustomers = 0;
                            foreach (var element in webData)
                            {

                                string BranchName = BeautifyString(element.ReferenceName);
                                string Next_Branch = "";
                                bankCustomers += element.TotalTicket;

                                Content = "";
                                //personal/mneygram/corporate banking stats
                                var subeDetailedInfo = getDetailedBranchInfo(Convert.ToDateTime(dateInp), element.TicketSubnet, "type");
                                foreach (var subEl in subeDetailedInfo) Content += BeautifyString(subEl.ReferenceName) + ": " + subEl.TotalTicket + "</br>";
                                Content += "<br/>";
                                //teller info
                                var tellerDetailedInfo = getDetailedBranchInfo(Convert.ToDateTime(dateInp), element.TicketSubnet, "teller");
                                foreach (var teller in tellerDetailedInfo) Content += BeautifyString(teller.ReferenceName) + ": " + teller.TotalTicket + "</br>";

                                if (BranchName != Next_Branch)
                                {

                                    pane = new AccordionPane();
                                    lbTitle = new Label();
                                    lbContent = new Label();
                                    pane.ID = "Pane_" + element.TicketSubnet;
                                    lbTitle.Text = BranchName + " : " + element.TotalTicket + " Müşteri İşlemi";
                                    pane.HeaderContainer.Controls.Add(lbTitle);
                                    lbContent.Text = Content;
                                    pane.ContentContainer.Controls.Add(lbContent);
                                    acrDynamic.Panes.Add(pane);
                                    Content = "";
                                }
                            }

                            this.MyContent.Controls.Add(acrDynamic);
                            if (bankCustomers > 0)
                            {
                                total.Text = "Total: " + bankCustomers.ToString() + " Müşteri";
                            }
                            else total.Text = "";
                        }
                    }
                }
            }
        }

        /* -------------------------------------------------
         * helper methods for verification and data retrieval
         ---------------------------------------------------*/

        /**
         * returns true if the passed date string is bigger than current date
         * returns true if the string is of invalid format and can't be formatted into a date
         */
        public bool checkIfInFuture(string date )
        {
            var checkDate = DateTime.Now; 
            try
            {
                 checkDate = Convert.ToDateTime(date);
            }
            catch (FormatException)
            {
                return true;
            }
            return DateTime.Now <= checkDate ; 
        }

        /**
         * checks if the date is a saturday or a sunday 
         * returns true if date is an invalid form
         * returns true if weekend 
         */
        public bool checkIfWeekend(string date)
        {
            try
            {
                DayOfWeek day = Convert.ToDateTime(date).DayOfWeek;
                if ((day == DayOfWeek.Saturday) || (day == DayOfWeek.Sunday))
                {
                    return true;
                }
            }
            catch (FormatException)
            {
                return true;
            }
            return false;
        }

        /**
         * @param DateTime for which stats are being searched for
         * @param subnet id of the branch (integer)
         * @param type this can either be teller or type --> teller returns teller stats 
         * --> type returns stats for transaction types such as personal banking/moneygram/corporate
         */
        public Qmatic_DailySubReport[] getDetailedBranchInfo(DateTime searchDate, int subnet, string type)
        {
            QmaticWS webData = new QmaticWS();
            var detailedInfo = webData.GetDailySubReport(subnet, searchDate, searchDate, type);
            return detailedInfo;
        }

        /**
         * a function to return stats for each of the branches available
         * @param searchDate of type DateTime :  the date for which stats are being searched for
         * @return an ordered list of type Qmatic_DailyMainReport
         */
        public Qmatic_DailyMainReport[] getServiceData(DateTime searchDate)
        {
            QmaticWS webData = new QmaticWS();
            //var extra = webData.GetDailySubReport(DateTime.Today, 34, "type"); //tets if works
            var response = webData.GetMainRangeReport(searchDate, searchDate, "BRANCH");
            return response;
        }

        /*formats the string
         * removes dots
         * capitalizes only the first letter of every word
         */
        public string BeautifyString(string heading)
        {
            System.Globalization.TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            string formatted = heading.ToLower().Replace('.', ' ');
            formatted = textInfo.ToTitleCase(formatted); ;
            return formatted;
        }

    }
}