<%@ Page Title="Søk" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="search.aspx.cs" Inherits="Forum_Team2.faq" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<br />
<div align="center" class="container">    
    <div  class="row">
         <div class="col-md-6 col-md-offset-3">
            <div style=" margin:auto;" class="panel panel-success">
                <div class="panel-heading"><p align="center">Søk etter emnenavn</p></div><br />
                    
                    <h2>Søkeresultater</h2>
                    <asp:TextBox Placeholder="Søk etter emner..." ID="txtSearch" runat="server"  CssClass="form form-control " Height="25px" Width="350px" Rows="0"></asp:TextBox>
                    <br />
                    <asp:Button ID="btnSearch" runat="server" href="/SearchResult.aspx" Text="Søk" Width="100px" OnClick="btnSearch_Click" CssClass="btn btn-primary" />
                    <br /><br /><br />
                    <asp:PlaceHolder ID="phTopics" runat="server"></asp:PlaceHolder>
                    <br /><br /><br />
                    
                </div>

           </div>
      </div>
</div>

</asp:Content>