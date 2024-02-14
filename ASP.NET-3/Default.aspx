<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="ASP.NET_3._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <h1 class="text-center">Prenotazione Cinema</h1>
        <div class="Container">
            <div class="row">
                <div class="col-6">
                    <asp:TextBox ID="nome" runat="server"></asp:TextBox>
                  
                    <br />
                    <asp:TextBox ID="cognome" runat="server"></asp:TextBox>

                    <br />
                    <asp:DropDownList ID="sala" runat="server">
                        <asp:ListItem Text="SALA NORD" Value="SALA NORD"></asp:ListItem>
                        <asp:ListItem Text="SALA EST" Value="SALA EST"></asp:ListItem>
                        <asp:ListItem Text="SALA SUD" Value="SALA SUD"></asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Label Text="Ridotto" runat="server" />
                    <asp:CheckBox ID="ridotto" runat="server" />
                    <br />
                    <asp:Button ID="Prenota" runat="server" Text="Prenota" OnClick="Prenota_Click" />

                    <asp:Button ID="Delete" Text="Delete all" runat="server" OnClick="Delete_Click" />

                    <p id="risultato" runat="server"></p>
                </div>
                <div class="col-6" id="countSala"  runat="server"></div>
            </div>
        </div>
        <div id="listaPrenotazioni" runat="server">
        </div>
    </main>

</asp:Content>
