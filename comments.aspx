<%@ Page Title="Kommentarer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="comments.aspx.cs" Inherits="Forum_Team2.comments" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class ="content">


   <div class="jumbotron">
        <h2>Kommentarer</h2>
       <asp:TextBox ID="txtCommentText" Width="300px" TextMode="MultiLine" placeholder="Kommentartekst" runat="server"></asp:TextBox>
       <br /><asp:Button ID="btnPostComment" runat="server" Text="Post" CssClass="btn btn-primary" OnClick="btnPostComment_Click" />
       <asp:Button ID="btnUpvote" runat="server" Text="Stem" CssClass="btn btn-danger" OnClick="btnVote_Click" />
     </div>

    <div class="panel panel-primary">
        <div class="row">
            <div class="col-md-12 content-align-center">
                <asp:PlaceHolder ID="phTopic" runat="server"></asp:PlaceHolder>
            </div>
        </div>
     
        <div class="row">
            <p>
                <asp:PlaceHolder ID="phComments" runat="server"></asp:PlaceHolder>
            </p>
            </div>
        </div>
</div>


</asp:Content>