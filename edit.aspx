<%@ Page Title="Rediger bruker" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="edit.aspx.cs" Inherits="Forum_Team2.edit" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />
   <div align="center" class="container">    
    <div  class="row">
         <div class="col-md-6 col-md-offset-3">
            <div style="height:500px; margin:auto;" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Rediger brukerprofilen din</p></div><br />
  
                   <asp:Label ID="lblImage" runat="server" Text="Bilde: "></asp:Label>
                   <asp:TextBox CssClass="form-control" ID="txtImage" runat="server"></asp:TextBox><br />

                   <asp:Label ID="lblPassword" runat="server" Text="Passord: "></asp:Label>
                   <asp:TextBox CssClass="form-control" ID="txtPassword" type="password" runat="server"></asp:TextBox><br />

                   <asp:Label ID="lblGender" runat="server" Text="Kjønn: "></asp:Label>
                   <asp:TextBox CssClass="form-control" ID="txtGender" runat="server"></asp:TextBox><br />

                   <asp:Label ID="lblEducation" runat="server" Text="Utdanning: "></asp:Label>
                   <asp:TextBox CssClass="form-control" ID="txtEducation" runat="server"></asp:TextBox><br />

                   <asp:Label ID="lblDescription" runat="server" Text="Beskrivelse: "></asp:Label>
                   <asp:TextBox CssClass="form-control" ID="txtDescription" runat="server"></asp:TextBox><br />
                   
                   <asp:Button ID="btnUpdateUser" runat="server" OnClick="btnUpdateUser_Click" CssClass="btn btn-primary" Text="Oppdater bruker" />
                   <asp:Button ID="btnGetInfo" runat="server" OnClick="btnGetInfo_Click" CssClass="btn btn-primary" Text="Hent brukerdata" />
                   <asp:Button ID="btnDeleteData" runat="server" OnClick="btnDeleteData_Click" CssClass="btn btn-primary" Text="Slett brukerdata" />

                </div>
             </div>
         </div>

       </div>
</asp:Content>