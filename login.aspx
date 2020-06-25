<%@ Page Title="Logg inn" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="Forum_Team2.login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <br />
<div align="center" class="container">    
    <div  class="row">
         <div class="col-md-6 col-md-offset-3">
            <div style="height:300px; margin:auto;" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Logg inn</p></div><br />

                    <asp:Label ID="lblUsername" runat="server" Text="Brukernavn:"></asp:Label>
                    <asp:TextBox ID="txtUsername" placeholder="Brukernavn" runat="server" CssClass="form form-control"></asp:TextBox>
                    <br />

                    <asp:Label ID="lblPassword" runat="server" Text="Passord:"></asp:Label>
                    <asp:TextBox type="password" ID="txtPassword" placeholder="Passord" runat="server" CssClass="form form-control"></asp:TextBox><br />
                    
                    <asp:Button ID="btnLogIn" runat="server" Text="Logg inn" OnClick="btnLogIn_Click" CssClass="btn btn-primary" />
                    <br />
                    
                </div>
           </div>
      </div>
</div>

</asp:Content>