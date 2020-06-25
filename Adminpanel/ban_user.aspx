<%@ Page Title="Ban Bruker" Language="C#"  MasterPageFile="~/Adminpanel/Admin.Master" AutoEventWireup="true" CodeBehind="ban_user.aspx.cs" Inherits="Forum_Team2.Adminpanel.ban_user" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<br />
<div class="container">  
    <div class="row">
     <div align="center" class="panel panel-primary">
        <div class="panel-heading"><p align="center">Utesteng bruker</p></div>
            <div class="row">
                <div style="height:280px;" class="col-md-12">
                    <br />

                    <asp:Label ID="lblUsername" runat="server" Text="Brukernavn: "></asp:Label>
                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form form-control"></asp:TextBox><br />

                    <asp:Button ID="btnBan" runat="server" Text="Utesteng" OnClick="btnBan_Click" CssClass="btn btn-primary" />
                    <br />
                </div>
        </div>
    </div>
   </div>
</div>
</asp:Content>
