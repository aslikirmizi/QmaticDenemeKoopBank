<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Aylık.aspx.cs" Inherits="QmaticDeneme.Aylık" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
            <title></title>
            <webopt:bundlereference runat="server" path="~/Content/css" />
            <script src="https://ajax.googleapis.com/ajax/libs/jquery/3.5.1/jquery.min.js"></script>
                <!-- <asp:ScriptReference Name="Chart" Path="~/Scripts/Chart.js"></asp:ScriptReference>
     -->
    </head>
    <body > <!-- onload="javascript:HideProgressBar()" -->
        <div class="wrapper">
            <!-- <img id="bg-image" src="/images/back-3.png"/> -->
        <form id="form1" runat="server">
            <div id="load"></div>
            <asp:ScriptManager ID="ScriptManager1" runat="server">
                        <Scripts>
                            <%--To learn more about bundling scripts in ScriptManager see https://go.microsoft.com/fwlink/?LinkID=301884 --%>
                            <%--Framework Scripts--%>
                            <asp:ScriptReference Name="MsAjaxBundle" />
                            <asp:ScriptReference Name="jquery" />
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
                                    <li><a runat="server" href="~/">Günlük İstatistikler</a></li>
                                    <li><a runat="server" href="~/Aylık">Aylık Şube İstatistikleri</a></li>
                                    <li><a runat="server" href="~/AylıkGenel">Aylık Genel İstatistikleri</a></li>

                                    <!-- <li><a runat="server" href="~/Contact">Contact</a></li> -->
                                </ul>
                            </div>
                        </div>
                    </div>

                    <div id="ContentAylık">
                        <asp:UpdateProgress runat="server" id="PageUpdateProgress">
                            <ProgressTemplate>
                                <div id="dvProgressBar" >
                                    <img id="loading-icon" src="/images/loading.gif" /> Veriler Yükleniyor, lütfen bekleyiniz...
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                        <div >
                            <br />
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label
                                ID="Label1" runat="server" Text="Label"></asp:Label>
                            <br />
                            <br />
                        </div>
                        <asp:UpdatePanel runat="server" id="UpdatePanel1">
                    <ContentTemplate>
                        <asp:Panel runat="server" id ="Panel1">
                    &nbsp;<asp:DropDownList ID="DropDownList1" runat="server"  Width="194px" Height="22px">
                    </asp:DropDownList>

                    &nbsp;<asp:DropDownList ID="PickMonth" runat="server">
                    </asp:DropDownList>
                    <asp:DropDownList ID="PickYear" runat="server">
                    </asp:DropDownList>
            
                    <asp:Button ID="Button1" runat="server" Height="25px"  OnClick="Button1_Click"  Text="Ara" Width="135px" /> <!-- OnClientClick="javascript:ShowProgressBar()" -->
                    <br />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                    <p> &nbsp;&nbsp;&nbsp;&nbsp;
                        </p>
                    <div style="margin-left: 40px">
                        <asp:Table ID="Table1" runat="server" Height="20px" Width="600">
                            
                        </asp:Table>
                    </div>

                    <br />
                        <asp:Table ID="tellerTable" runat="server" Height="32px" Width="376px">
                            <asp:TableRow>
                                <asp:TableCell>Veznedar</asp:TableCell>
                                <asp:TableCell>Bakdığı Müşteri Sayısı</asp:TableCell>
                            </asp:TableRow>
                        </asp:Table>
                    <br />
                        <div style="margin-left: 40px">
                            <asp:Label ID="errorLabel" runat="server"></asp:Label>
                            <ajaxToolkit:LineChart ID="LineChart1" runat="server"  Width="600px"> </ajaxToolkit:LineChart>
                        </div>
                    </div>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                        
                    </div>
            </form>
        </div>
    </body>
</html>
