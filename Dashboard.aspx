<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Dashboard.aspx.cs" Inherits="MarketPriceTracker.Dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .stat-card {
            border: 0;
            border-radius: 14px;
            color: #fff;
        }

        .stat-value {
            font-size: 2rem;
            font-weight: 700;
        }
    </style>

    <h2>Dashboard</h2>
    <p class="text-muted">
        <asp:Label ID="lblWelcome" runat="server" /></p>

    <div class="row g-3 mb-4">
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card" style="background: #4070f4;">
                <div class="card-body">
                    <div>
                        <asp:Label ID="lblT1" runat="server" /></div>
                    <div class="stat-value">
                        <asp:Label ID="lblV1" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card" style="background: #0ea5e9;">
                <div class="card-body">
                    <div>
                        <asp:Label ID="lblT2" runat="server" /></div>
                    <div class="stat-value">
                        <asp:Label ID="lblV2" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card" style="background: #6366f1;">
                <div class="card-body">
                    <div>
                        <asp:Label ID="lblT3" runat="server" /></div>
                    <div class="stat-value">
                        <asp:Label ID="lblV3" runat="server" /></div>
                </div>
            </div>
        </div>
        <div class="col-sm-6 col-lg-3">
            <div class="card stat-card" style="background: #14b8a6;">
                <div class="card-body">
                    <div>
                        <asp:Label ID="lblT4" runat="server" /></div>
                    <div class="stat-value">
                        <asp:Label ID="lblV4" runat="server" /></div>
                </div>
            </div>
        </div>
    </div>

    <div class="row g-4 mb-4">
        <div class="col-lg-7">
            <div class="card">
                <div class="card-body">
                    <h5>Average price by product (RWF)</h5>
                    <canvas id="chartAvg" height="140"></canvas>
                </div>
            </div>
        </div>
        <div class="col-lg-5">
            <div class="card">
                <div class="card-body">
                    <h5>
                        <asp:Label ID="lblChart2Title" runat="server" /></h5>
                    <canvas id="chartSecond" height="200"></canvas>
                </div>
            </div>
        </div>
    </div>

    <h5>Recent reports</h5>
    <asp:GridView ID="gvRecent" runat="server" AutoGenerateColumns="False"
        CssClass="table table-striped" EmptyDataText="No reports yet.">
        <Columns>
            <asp:BoundField DataField="Reporter" HeaderText="Reported by" />
            <asp:BoundField DataField="Market" HeaderText="Market" />
            <asp:BoundField DataField="Product" HeaderText="Product" />
            <asp:BoundField DataField="Price" HeaderText="Price (RWF)" DataFormatString="{0:N0}" />
            <asp:BoundField DataField="ReportDate" HeaderText="Date" DataFormatString="{0:yyyy-MM-dd}" />
        </Columns>
    </asp:GridView>
    <asp:HiddenField ID="hfAvgLabels" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfAvgValues" runat="server" ClientIDMode="Static" />

    <asp:HiddenField ID="hfSecondLabels" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hfSecondValues" runat="server" ClientIDMode="Static" />




<script src="https://cdn.jsdelivr.net/npm/chart.js@4.4.1/dist/chart.umd.min.js"></script>

<script>
window.addEventListener('load', function () {

    const ChartJS = Reflect.get(window, 'Chart');

    if (typeof ChartJS !== 'function') {
        console.error("Chart.js library was not loaded.");
        return;
    }

    /** @param {string} id */
    function getHiddenValue(id = '') {

        const element = document.getElementById(id);

        if (element === null) {
            console.error("Element not found: " + id);
            return '[]';
        }

        return element.getAttribute('value') || '[]';
    }

    const avgLabels = JSON.parse(getHiddenValue('hfAvgLabels'));
    const avgValues = JSON.parse(getHiddenValue('hfAvgValues'));

    const secondLabels = JSON.parse(getHiddenValue('hfSecondLabels'));
    const secondValues = JSON.parse(getHiddenValue('hfSecondValues'));

    // CHART 1: Average price by product
    new ChartJS(document.getElementById('chartAvg'), {
        type: 'bar',
        data: {
            labels: avgLabels,
            datasets: [{
                label: 'Average price (RWF)',
                data: avgValues,
                backgroundColor: '#4070f4',
                borderRadius: 6
            }]
        },
        options: {
            plugins: {
                legend: { display: false }
            },
            scales: {
                y: { beginAtZero: true }
            }
        }
    });

    // CHART 2: Reports per market / product
    new ChartJS(document.getElementById('chartSecond'), {
        type: 'doughnut',
        data: {
            labels: secondLabels,
            datasets: [{
                data: secondValues,
                backgroundColor: [
                    '#4070f4',
                    '#0ea5e9',
                    '#6366f1',
                    '#14b8a6',
                    '#f59e0b',
                    '#ef4444',
                    '#8b5cf6',
                    '#22c55e'
                ]
            }]
        }
    });

});
</script>
</asp:Content>


