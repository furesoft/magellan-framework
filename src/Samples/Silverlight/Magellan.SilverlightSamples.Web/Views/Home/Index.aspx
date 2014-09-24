<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="indexTitle" ContentPlaceHolderID="TitleContent" runat="server">Magellan Silverlight Samples</asp:Content>

<asp:Content ID="indexContent" ContentPlaceHolderID="MainContent" runat="server">
    <h1>Welcome to Magellan Silverlight Samples</h1>
    <p>
        This page contains a number of Silverlight demonstrations of Magellan. The following samples are 
        available:
    </p>
    <ul>
      <li><%= Html.ActionLink("Quickstart", "Quickstart", "Home") %></li>
    </ul>
</asp:Content>
