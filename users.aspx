<%@ Page Title="Brukere" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="users.aspx.cs" Inherits="Forum_Team2.users" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<br /><br />

    <div class="row">
  <div class="col-md-3">
      <div style="height:310px;" class="panel panel-primary">
       <div class="panel-heading"><p align="center">Profil</p></div>
          <asp:PlaceHolder ID="phProfile" runat="server"></asp:PlaceHolder>
          </div>

  </div>
  <div class="col-md-9">
      <div style="height:310px;" class="panel panel-primary">
       <div class="panel-heading"><p align="center">Beskrivelse</p></div>
          <asp:PlaceHolder ID="phDesc" runat="server"></asp:PlaceHolder>
          
          </div>
      </div>

        <div class="col-md-6">
      <div class="panel panel-primary">
       <div class="panel-heading"><p align="center">Statusoppdateringer</p></div>
          <asp:PlaceHolder ID="phStatusUpdates" runat="server"></asp:PlaceHolder>
          </div>
      </div>

        <div class="col-md-6">
      <div class="panel panel-primary">
       <div class="panel-heading"><p align="center">Nylige poster</p></div>
          <asp:PlaceHolder ID="phUserActivity" runat="server"></asp:PlaceHolder>
          </div>
      </div>



        </div>

</asp:Content>