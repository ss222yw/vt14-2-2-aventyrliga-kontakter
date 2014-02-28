<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Aventyrliga_Kontakter.Default" ViewStateMode="Disabled" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Äventyrliga kontakter</title>
     <link href ="~/Content/Color.css" rel="stylesheet" />
</head>

<body>
    <form id="form1" runat="server">
        
    <div id="container">

        <h1>Äventyrliga kontakter</h1>

        <asp:Panel ID="MessagePanel" runat="server" Visible="false">
            <asp:Label ID="Label1" runat="server"></asp:Label>
            <a href="Default.aspx" class="Close">x</a>
        </asp:Panel>
        </div>
        
    
    
        <div id="content">
            <%-- Validering --%>
            <asp:ValidationSummary ID="AventyligaValidationSummary1" runat="server"
                 HeaderText="Fel inträffade. Korrigera det som är fel och försök igen."
                 ValidationGroup="InsertAventyligaValidationSummary1"/>

            <%-- Visar alla kunder --%>
            <asp:ListView ID="ContactListView" runat="server"
                 ItemType="Aventyrliga_Kontakter.Model.Contact"
                 SelectMethod="ContactListView_GetDataFromDataBase"
                  InsertMethod="ContactListView_InsertItem"
                 UpdateMethod="ContactListView_UpdateItem"
                 DeleteMethod="ContactListView_DeleteItem"
                 DataKeyNames="ContactId"
                 InsertItemPosition="FirstItem">
                <LayoutTemplate>
                    
                    <table class="Layout">
                        <thead>
                        <tr>
                            <th>
                                Förnamn
                            </th>
                            <th>
                                Efternamn
                            </th>
                            <th>
                                E-post
                            </th>
                            <th>

                            </th>
                        </tr>
                            </thead>
                         <%-- Platshållare för nya rader --%>
                        <asp:PlaceHolder ID="itemPlaceholder" runat="server" />
                    </table>
                         
                    <%-- Pages visar 20 kontkater i en sida.--%>      
                    <asp:DataPager ID="DataPager1" runat="server" PageSize="20">
                        <Fields>
                             <asp:NextPreviousPagerField 
                                 ShowNextPageButton="false" 
                                 ShowPreviousPageButton="false" 
                                 ShowFirstPageButton="true" 
                                 FirstPageText="Första" 
                                 ShowLastPageButton="true" 
                                 LastPageText="Sista"/>
                            <asp:NumericPagerField 
                                NextPageText="Nästa" 
                                PreviousPageText="Förra"/> 

                        </Fields>
                    </asp:DataPager>
       
                </LayoutTemplate>
                <ItemTemplate>
                    <%-- Mall för nya rader. --%>
                    <tr>
                        <td>
                            '<%# Item.FirstName %>' 
                        </td>
                        <td>
                          '<%# Item.LastName %>'
                        </td>
                        <td>
                            '<%# Item.EmailAddress %>' 
                        </td>
                        <td class="KommandNamn">

                            <asp:LinkButton ID="DelLinkButton" runat="server" CausesValidation="false" CommandName="Delete" Text="Ta bort" 
                                OnClientClick='<%# String.Format("return confirm(\" Vill du verkligen ta bort ({0} {1} {2}) ?\")" , Item.FirstName , Item.LastName, Item.EmailAddress) %>'></asp:LinkButton>

                            <asp:LinkButton ID="EdLinkButton" runat="server" 
                                CommandName="Edit" Text="Redigera"
                                 CausesValidation="false"></asp:LinkButton>

                        </td>
                    </tr>
                </ItemTemplate>
           
                <EmptyDataTemplate>
                    <table class="Saknas">
                        <tr>
                            <td>
                                KundUppgifter Saknas!
                            </td>
                        </tr>
                    </table>
                </EmptyDataTemplate>
                <InsertItemTemplate>
                    <%-- Mall för rad i tabellen för att lägga till nya  kunduppgifter. --%>
                    <tr>
                        <td>
                            <asp:TextBox ID="FirstName" runat="server" 
                                Text='<%# BindItem.FirstName %>' MaxLength="50" ></asp:TextBox>

                            <asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                 runat="server" ErrorMessage="Förnamnet måste anges!" 
                                ControlToValidate="FirstName" ValidationGroup="InsertAventyligaValidationSummary1" 
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="LastName" runat="server" 
                                Text='<%# BindItem.LastName %>' MaxLength="50" ></asp:TextBox>

                        </td><asp:RequiredFieldValidator ID="RequiredFieldValidator2" 
                            runat="server" ErrorMessage="Efternamnet måste anges!" 
                            ControlToValidate="LastName" ValidationGroup="InsertAventyligaValidationSummary1"
                             Display="Dynamic"></asp:RequiredFieldValidator>
                        <td>
                            <asp:TextBox ID="EmailAddress" runat="server" 
                                Text='<%# BindItem.EmailAddress %>'
                                 MaxLength="50"></asp:TextBox>

                        </td><asp:RequiredFieldValidator ID="RequiredFieldValidator3" 
                            runat="server" ErrorMessage="Epost måste anges!" 
                            ControlToValidate="EmailAddress" ValidationGroup="InsertAventyligaValidationSummary1"
                             Display="Dynamic" ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$@"></asp:RequiredFieldValidator>
                        <td>
                            <asp:LinkButton ID="InsertLinkButton" runat="server" 
                                CommandName="Insert" Text="Lägg till"></asp:LinkButton>

                            <asp:LinkButton ID="CancelLinkButton" runat="server" CommandName="Cancel" 
                                Text="Rensa" CausesValidation="false"></asp:LinkButton>
                        </td>
                    </tr>
                </InsertItemTemplate>
                <EditItemTemplate>
                    <%-- Mall för rad i tabellen för att redigera kunduppgifter. --%>
                    <tr>
                        <td>
                            <asp:TextBox ID="FirstName1" runat="server" 
                                Text='<%# BindItem.FirstName %>' 
                                MaxLength="50"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="FirstName" 
                                runat="server" ErrorMessage="Förnamnet måste anges!" 
                                ControlToValidate="FirstName1" ValidationGroup="InsertAventyligaValidationSummary1" 
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="LastName" runat="server" 
                                Text='<%# BindItem.LastName %>' 
                                MaxLength="50"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="LastName1" 
                                runat="server" ErrorMessage="Efternamnet måste anges!" 
                                ControlToValidate="LastName" ValidationGroup="InsertAventyligaValidationSummary1" 
                                Display="Dynamic"></asp:RequiredFieldValidator>
                        </td>
                        <td>
                            <asp:TextBox ID="EmailAddress" runat="server" 
                                Text='<%# BindItem.EmailAddress %>' 
                                MaxLength="50"></asp:TextBox>

                            <asp:RequiredFieldValidator ID="EmailAddress1"
                                 runat="server" ErrorMessage="E-post måste anges!"
                                 ControlToValidate="EmailAddress" ValidationGroup="InsertAventyligaValidationSummary1" 
                                Display="None"></asp:RequiredFieldValidator>

                            <asp:RegularExpressionValidator ID="EmailAddress2"
                                 runat="server" ErrorMessage="E-post verkar inte vara korrekt" 
                                ControlToValidate="EmailAddress" Display="Dynamic"
                                 ValidationExpression="^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,6}$"
                                 ValidationGroup="EditAventyligaValidationSummary2"></asp:RegularExpressionValidator>
                        </td>
                        <td>
                               <asp:LinkButton ID="UpdateLinkButton" runat="server" 
                                CommandName="Update" Text="Spara"></asp:LinkButton>

                            <asp:LinkButton ID="CancelLinkButton2" runat="server" CommandName="Cancel" 
                                Text="Avbryt" CausesValidation="false"></asp:LinkButton>
                        </td>
                        <td>

                        </td>
                    </tr>
                </EditItemTemplate>
            </asp:ListView>          
        </div>
    </form>
</body>
</html>
