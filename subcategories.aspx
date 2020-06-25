<%@ Page Title="Subkategorier" MasterPageFile="~/Site.Master" Language="C#"  AutoEventWireup="true" CodeBehind="subcategories.aspx.cs" Inherits="Forum_Team2.subcategories" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
<!DOCTYPE html>
    <script src="https://hammerjs.github.io/dist/hammer.js"></script>
    <script src="https://github.com/hammerjs/jquery.hammer.js/blob/master/jquery.hammer.js"></script>
    <script> // https://hammerjs.github.io/getting-started/ , HammerJS
       window.addEventListener('load', function () {
            var swipe = new Hammer(document, { inputClass: Hammer.TouchMouseInput });

            swipe.on("swipeleft swiperight", function (ev) {
                
                if (ev.type == 'swipeleft') {
                    // open panel
                    $('#panel').animate({
                        right: '0'
                    });
                } else {
                    // close/hide panel
                    $('#panel').animate({
                        right: '-100%'
                    });
                }
            })
            }, false);
    </script>
     <div class="jumbotron">
        <h2>Subkategorier</h2>
        <p> For å se en beskrivelse av kategorien du er inne i, swipe til venstre.
        </p>
     </div>

    <div class="panel panel-primary">
        <div id="panel">
            <asp:PlaceHolder ID="phCategoryDesc" runat="server"></asp:PlaceHolder>
        </div>

        <div class="panel-heading"><p align="center">SubKategorier</p></div>
        <div class="row">
            <div class="col-md-12">
                <asp:PlaceHolder ID="phSubCategories" runat="server"></asp:PlaceHolder>

            </div>
        </div>
    </div>
    
</asp:Content>