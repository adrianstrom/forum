<%@ Page Title="Emner" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="topics.aspx.cs" Inherits="Forum_Team2.topics" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="jumbotron">
        <h2>Registrer emne</h2>
        <p><asp:TextBox ID="txtTopicName" placeholder="Emnenavn" runat="server" Width="279px"></asp:TextBox>
        </p>
        <asp:TextBox ID="txtTopicText" TextMode="MultiLine" placeholder="Emnetekst" runat="server" Height="157px" Width="282px"></asp:TextBox>
        <br /><asp:Button ID="btnRegisterTopic" runat="server" OnClick="btnRegisterTopic_Click" Text="Registrer emne" CssClass="btn btn-primary" />
     </div>

    <div class="panel panel-primary">
     <div class="panel-heading"><p>Emner</p></div>
        <div class="row">
            <div class="col-md-12">
                <asp:PlaceHolder ID="phTopics" runat="server"></asp:PlaceHolder>

            </div>
        </div>
    </div>

</asp:Content>
