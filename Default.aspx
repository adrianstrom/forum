<%@ Page Title="Forside" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Forum_Team2._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">


<br />
<div id="container">
    <div class="jumbotron">
        <h1>Leksehjelp forum!</h1>
        <p> Sliter du med noe fagrelatert? Frykt ikke! På forumet har du muligheten til å poste emner hvor du kan spørre
           om fagrelaterte spørsmål, og få hjelp med oppgaver du sliter med!
        </p>
  
        <asp:Button ID="btnRegister" runat="server" CssClass="btn btn-primary" Height="52px"
            OnClick="btnRegister_Click" Text="Registrer deg her!" Width="221px" />
        
    </div>

    <div class="panel panel-primary">
        <div class="panel-heading"><p align="center">Kategorier</p></div>
            <div class="row">
                <div class="col-md-12">
                    <asp:PlaceHolder ID="phCategories" runat="server"></asp:PlaceHolder>
                </div>
        </div>
    </div>
 
    <div class="row">

        <div class="col-md-6">
            <div class="panel panel-info">
                <div class="panel-heading"><p align="center">Populære emner</p></div>
                    <asp:PlaceHolder ID="phPopularTopics" runat="server"></asp:PlaceHolder>
           </div>

        </div>

        <div class="col-md-6">
            <div class="panel panel-info">
                <div class="panel-heading"><p align="center">Nye emner</p></div>
                    <asp:PlaceHolder ID="phNewTopics" runat="server"></asp:PlaceHolder>
          
            </div>
         </div>
    </div>
</div>

   
</asp:Content>
