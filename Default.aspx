<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="QmaticDeneme.landing" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title></title>
        <webopt:bundlereference runat="server" path="~/Content/css" />
        <style type="text/css">
            .auto-style1 {
                font-family: Arial;
            }
        </style>
    </head>
    <body>
        <div class="wrapper">
          <!-- <img id="bg-image" src="/images/back-3.png"/> -->

            <form id="form1" runat="server">
                <asp:ScriptManager ID="ScriptManager1" runat="server">
                    <Scripts>
                        <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                        <%--Framework Scripts--%>
                        <asp:ScriptReference Name="MsAjaxBundle" />
                        <asp:ScriptReference Name="jquery" />
                        <asp:ScriptReference Name="bootstrap" />
                        <asp:ScriptReference Name="WebForms.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebForms.js" />
                        <asp:ScriptReference Name="WebUIValidation.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebUIValidation.js" />
                        <asp:ScriptReference Name="MenuStandards.js" Assembly="System.Web" Path="~/Scripts/WebForms/MenuStandards.js" />
                        <asp:ScriptReference Name="GridView.js" Assembly="System.Web" Path="~/Scripts/WebForms/GridView.js" />
                        <asp:ScriptReference Name="DetailsView.js" Assembly="System.Web" Path="~/Scripts/WebForms/DetailsView.js" />
                        <asp:ScriptReference Name="TreeView.js" Assembly="System.Web" Path="~/Scripts/WebForms/TreeView.js" />
                        <asp:ScriptReference Name="WebParts.js" Assembly="System.Web" Path="~/Scripts/WebForms/WebParts.js" />
                        <asp:ScriptReference Name="Focus.js" Assembly="System.Web" Path="~/Scripts/WebForms/Focus.js" />
                        <asp:ScriptReference Name="WebFormsBundle" />
                        <%--Site Scripts--%>
                    </Scripts>
                </asp:ScriptManager>
                <!-- menu -->
                <div class="navbar navbar-inverse navbar-fixed-top">
                    <div class="container">
                        <div class="navbar-header">
                            <button type="button" class="navbar-toggle" data-toggle="collapse" data-target=".navbar-collapse">
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                                <span class="icon-bar"></span>
                            </button>
                            <a class="navbar-brand" runat="server" href="~/">Qmatic Bilgi Sistemi</a>
                        </div>
                        <div class="navbar-collapse collapse">
                            <ul class="nav navbar-nav">
                                <li><a runat="server" href="~/Default">Günlük İstatistikler</a></li>
                                <li><a runat="server" href="~/Aylık">Aylık Şube İstatistikleri</a></li>
                                <li><a runat="server" href="~/AylıkGenel">Aylık Genel İstatistikleri</a></li>
                            </ul>
                        </div>
                    </div>
                </div>
                <div>
                    <asp:UpdateProgress runat="server">
                        <ProgressTemplate>
                            <div id="dvProgressBar1" >
                                <img id="loading-icon-1" src="/images/loading.gif" /> Veriler Yükleniyor, lütfen bekleyiniz...
                            </div>
                        </ProgressTemplate>
                   </asp:UpdateProgress>
                  <div id="spacing"></div>
                  <div id="subeStatistik">
                    <img id="logo" src="../images/kooplogo.png" />
                      <asp:UpdatePanel runat="server">
                        <ContentTemplate>
                            <h1 id ="Qmatic"> QMatic Günlük Müşteri İstatistikleri </h1>
                            <p><asp:Literal ID="firstMsg" runat="server"></asp:Literal> </p>
                            <p> <asp:Label ID="lblEmail" runat="server" Text="Tarih giriniz"></asp:Label> </p>
                            <p> <input ID="dateDeneme" type="date" runat="server" /></p>
                            <asp:Button OnClick="btnAra_Click" ID="btnAra" runat="server" Text="Ara" Width="58px" Height="24px" />
                    <br />
                    <br />
                    <br />
                <asp:Panel ID="MyContent" runat="server">
                    <!-- Dynamic content comes here -->
                </asp:Panel>  
                <ajaxToolkit:Accordion ID="Accordion1" runat="server"></ajaxToolkit:Accordion>           
                      <span class="auto-style1">
                      <br />
                          <div id="total-customers">
                            <asp:Label ID="total" runat="server"></asp:Label>
                          </div>
                      </span>     
                    </ContentTemplate>
                </asp:UpdatePanel>
            </form>
        </div>
    </body>
</html>

