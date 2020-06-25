<%@ Page Title="Registrer deg" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="register.aspx.cs" Inherits="Forum_Team2.register" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
     <br />
<div align="center" class="container">    
    <div  class="row">
         <div class="col-md-6">
            <div style="height:700px; margin:auto;" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Registrer deg</p></div><br />

                    <asp:Label ID="lblUserName" runat="server" Text="Brukernavn:"></asp:Label>
                    <asp:TextBox ID="txtUserName" align="center" placeholder="Brukernavn" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblPassword" runat="server" Text="Passord:"></asp:Label>
                    <asp:TextBox type="password" ID="txtPassword" placeholder="Passord" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblPasswordCheck" runat="server" Text="Gjenta Passord:"></asp:Label>
                    <asp:TextBox type="password" ID="txtPasswordCheck" placeholder="Gjenta Passord" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblEmail" runat="server" Text="Email:"></asp:Label>
                    <asp:TextBox ID="txtEmail" placeholder="Email" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblGender" runat="server" Text="Kjønn:"></asp:Label><br />
                    <asp:DropDownList ID="ddlGender" Width="300px" class="btn btn-default dropdown-toggle" data-toggle="dropdown" aria-haspopup="true" aria-expanded="false" runat="server">
                        <asp:ListItem>Mann</asp:ListItem>
                        <asp:ListItem>Kvinne</asp:ListItem>
                    </asp:DropDownList><br /><br />

                    <asp:Label ID="lblEducation" runat="server" Text="Utdanning:"></asp:Label>
                    <asp:TextBox ID="txtEducation" placeholder="Utdanning" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:CheckBox ID="chkTermsOfService" runat="server" Text="&nbsp;Jeg godtar <a target='_blank' href='/brukervilkår.pdf'>brukervilkårene.</a>" /><br /><br />

                    <asp:Button ID="btnRegister" runat="server" Text="Registrer deg" CssClass="btn btn-primary" OnClick="btnRegister_Click" /><br />

                </div>
             </div>

        <div class="col-md-6">
            <div style="height:300px; margin:auto;" class="panel panel-primary">
                <div class="panel-heading"><p align="center">Aktiver deg</p></div><br />

                    <asp:Label ID="lblUserNameCert" runat="server" Text="Brukernavn:"></asp:Label>
                    <asp:TextBox ID="txtUserNameCert" align="center" placeholder="Brukernavn" runat="server" CssClass="form-control"></asp:TextBox><br />

                    <asp:Label ID="lblCertificationCode" runat="server" Text="Kode:"></asp:Label>
                    <asp:TextBox ID="txtCertificationCode" align="center" placeholder="Kode" runat="server" CssClass="form-control"></asp:TextBox><br />    
                    
                    <asp:Button ID="btnActivate" runat="server" Text="Aktiver" CssClass="btn btn-primary" OnClick="btnActivate_Click" /><br />

                </div>
            </div>

        </div>
    </div>

  

</asp:Content>
