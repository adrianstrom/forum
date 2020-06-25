<%@ Page Title="Admin" Language="C#" MasterPageFile="~/Adminpanel/Admin.Master" AutoEventWireup="true" CodeBehind="Admin.aspx.cs" Inherits="Forum_Team2.Adminpanel.Admin" %>

<%@ Register assembly="System.Web.DataVisualization, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35" namespace="System.Web.UI.DataVisualization.Charting" tagprefix="asp" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="container">
        <br />
        <div class="alert alert-success" role="alert">Du er nå logget inn som en administrator!</div>

        <div class="panel panel-danger">
            <div class="panel-heading">
                <h3 class="panel-title">Admin</h3></div>
            <div class="panel-body">
            <p>Registrerte brukere:</p>
                <p>
                    <asp:Chart ID="chrtRegisteredUsers" runat="server" DataSourceID="sqlRegisteredUsers" Width="847px">
                        <Series>
                            <asp:Series ChartType="Spline" Name="Series1" XValueMember="date" YValueMembers="dateregistered" YValuesPerPoint="2">
                            </asp:Series>
                        </Series>
                        <ChartAreas>
                            <asp:ChartArea Name="ChartArea1">
                            </asp:ChartArea>
                        </ChartAreas>
                    </asp:Chart>
                    <asp:SqlDataSource ID="sqlRegisteredUsers" runat="server" ConnectionString="<%$ ConnectionStrings:ForumDatabaseConnectionString %>" 
                        SelectCommand="SELECT Users.date, COUNT(Users.date) as dateregistered FROM Users GROUP BY Users.date">
                    </asp:SqlDataSource>
                </p>

           </div>
       </div>

        <div style="height:250px;" align="center" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Notifiser brukere</p></div><br />
                    <p>Her kan du sende email til alle registrerte brukere på forumet!</p><br />
                    <asp:TextBox ID="txtSubject" runat="server">Emne</asp:TextBox>
                    <asp:TextBox ID="txtBody" runat="server">Tekst</asp:TextBox>
                    <asp:Button ID="btnAlertUsers" runat="server" Text="Alert users" CssClass="btn-danger" Height="28px" OnClick="btnAlertUsers_Click" Width="82px" />
                    <br />
            </div>

    </div>

</asp:Content>

