# Kigali Market Price Tracker

Kigali Market Price Tracker is an ASP.NET Web Forms application for recording and reviewing the prices of everyday products across Kigali markets. It provides a shared place for users to submit observations such as the price of rice, beans, or tomatoes at a selected market and date. The resulting reports can support market comparisons and provide a foundation for tracking price patterns over time.

The application is organized around two roles. Users can create an account, sign in, maintain their profile, and manage their own price reports. Administrators have management access to market and product records, can review and remove reports across the system, and can manage user accounts and roles. Page access and role-specific navigation are handled through the application’s session and shared `Site.Master` layout. Report ownership must be enforced in the data operations as well as in the interface so a user cannot modify another user’s records.

The database design contains four related SQL Server tables in `MarketTrackerDB`: `Users`, `Markets`, `Products`, and `PriceReports`. Foreign keys connect users to roles and price reports to the reporting user, market, and product. A report stores a price and report date; this structure keeps market and product details in their own records while allowing reports to reference them consistently. The design also calls for unique user email addresses.

The main workflow begins at the Dashboard, which presents an overview for the signed-in user or administrator. The dashboard design includes summary counts, average prices grouped by product, report totals grouped by market or product, and a recent reports table. It uses SQL aggregation and grouping for these summaries, with Chart.js rendering the chart data in the browser. The Price Reports page provides the detailed report workflow, while administration pages manage markets, products, and users. Profile functionality supports updating personal details and changing a password.

The application uses C# with ASP.NET Web Forms on .NET Framework, a master page for shared navigation, SQL Server for relational storage, and ADO.NET for database access. It is developed with Visual Studio Community 2026; the database is managed with SQL Server Management Studio (SSMS) 22.11.3. Login state and role information are held in ASP.NET Session as described in the project workflow. Database commands should use parameters for user-supplied values. Password handling should follow the implementation’s verified hashing approach; the assignment’s sample SHA-256 code is a basic demonstration and should not be represented as a production-grade password storage design without further security review.

The system’s intended performance comes from a focused workflow and database-side aggregation: users can move from the dashboard to detailed reports, and summary values are calculated from report records rather than manually maintained totals. No measured response times, load tests, data refresh guarantees, or deployment configuration were provided, so this README makes no benchmark or availability claims.

## Core areas

- **Dashboard** — role-aware summary cards, charts, and recent reports.
- **Price Reports** — create, view, edit, and delete price observations subject to report ownership and administrator permissions.
- **Markets and Products** — administrator-managed reference data used by reports.
- **Accounts and Profiles** — signup, login, role management, and personal profile maintenance.
- **Shared navigation** — `Site.Master` provides the common page layout and role-aware links.

## From Market Visit to Insight

A price report starts with a simple observation: a product, its market, its price, and the date it was recorded. The tracker connects that observation to the relevant product, market, and user records, creating a consistent record that can be reviewed alongside other submissions.

As reports accumulate, the Dashboard turns them into a broader picture of market activity. Users can review their own reporting activity, while administrators can see system-wide summaries, including average prices by product and report counts across markets. This gives the project a clear path from individual data entry to useful market comparisons and future price-trend analysis.

The role-based workflow supports shared data collection while keeping responsibilities clear: users manage their own reports, and administrators maintain the products and markets used throughout the system.
