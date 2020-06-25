<%@ Page Title="Medlemsliste" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="memberlist.aspx.cs" Inherits="Forum_Team2.memberlist" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

<script type="text/javascript">
<!-- https://css-tricks.com/snippets/javascript/redirect-mobile-devices/ --->
if (screen.width <= 699) {
document.location = "/memberlist_mobile.aspx";
}
//-->
</script>

<br />
<div align="center" class="container">    
    <div  class="row">
         <div class="col-md-8 col-md-offset-2">
            <div style=" margin:auto;" class="panel panel-success">
                <div class="panel-heading"><p align="center">Medlemsliste</p></div><br />

                    <asp:PlaceHolder ID="phMemberlist" runat="server"></asp:PlaceHolder>
                    <br /><br /><br />
                    
                </div>

           </div>
      </div>
</div>

</asp:Content>