Store-Ge Documentation (Product/Technical)
==========================================
<pre>
  The StoreGe App is an application which helps the user manage their stores and make sales easier.
  He has the options to add a store to his account, to add orders, to add products and suppliers.
  He can even registers another users to his stores assigning them as the stores' cashiers.
  
  The idea of this project was inspired by my first job ever. I was working at a supermarket.
  And since I have some clarity over the sector I decided that to be my final project for my SoftUni web course.
  Hope you enjoy the code and using the app!
  Now let's see the app documentation. 
</pre>

Product Documentation
==========================================
### Application Flow
    When the user starts the app he's redirected to the home page.
    The user scrolls and he'll see cards which describe the app main functionality.
  <p>
    <img height="300em" src="https://i.ibb.co/4d2kLzc/home-page-screen.jpg"></img>
    <img height="300em" src="https://i.ibb.co/stsBYr7/home-page-screen-scrolled.jpg"></img>
  </p>
    <pre>
    The three buttons on the right side of the navbar are:
    1. The home button leads the user to the home page if not logged.
       If he's logged depending on his role(Admin or Cashier) he gets redirected respectively to the dashboard or sales page.
    2. The login button leads to the login page.
    3. The register button leads to the register page.(duh!)
    </pre>
    <p>
      <img src="https://i.ibb.co/7NBSMQJ/home-page-action-buttons.jpg"></img>
    </p>
    <pre>
      The login form asks for the user's email address and password. 
      If by any chance the user forgot his password he can click the forgot password link and 
    resets his password with a reset password mail.
    </pre>
    <p>
      <img height="300em" src="https://i.ibb.co/BtQKysX/login-page-form.jpg"></img>
      <img height="300em" src="https://i.ibb.co/yVrRthL/forgotten-pass-page-form.jpg"></img>
    </p>
    <pre>
    The register form asks for:
      - Username
      - Email Address
      - Password
      - Confirm Password
      It also asks to accept our terms of agreement. The application uses cookies so that's the place where the user is informed about this.
    The application has MIT License so it's non commercial and the terms of agreement are a bit shallow(auto-generated).
    </pre>
    <img height="350em" src="https://i.ibb.co/z5CDLNC/register-page-form.jpg"></img>
    <pre>
      When the user logs as admin he's redirected to the store dashboard page.
      At first glance he sees:
        1. All orders button leading to a page containing a table with all the orders for all stores.
        2. Home button which leads to this page when the user is on the other pages.
        3. Hamburger menu containing options for the user's account.
        4. A block containing the user's Username and Email Address.
        5. Account Settings link where the user can change his email address and username.
        6. Logout button that deletes all the cookies and ends the user's session.
        7. Add store button which adds a store to the user's profile. The user picks the store's name and type(Supermarket etc.).
        8. Card buttons for each store in the user's profile, containing the store's name and type.
    </pre>
    <img src="https://i.ibb.co/WxGCTmh/store-dashboard-page.jpg"></img>
    <pre>
      The store's data page contains:
      1. Go back button leading to the previous page the user was on.
      2. A card with:
         - The store's name and type
         - A card for the store's orders and a button leading to the store's orders page
         - A card for the store's suppliers page and a button leading to the store's suppliers page
         - A card with button leading to the store's sales page where cashiers and admin sell store's products
      3. Export Excel Report button which exports an excel worksheet with all the database logged events for the current store.
      4. Sort arrow controlling the sort order of the table items.
      5. Search by name field.
      6. Table with all the products available in the store.
      7. Paginator for the table.
    </pre>
    <img src="https://i.ibb.co/X5D5DtL/store-data-page.jpg"></img>
    <pre>
      The information the store orders page holds is a single table with all the orders for the store.
      The data in the table is:
        - Order number
        - Supplier name
        - Date the order is added
      Also the table's functionalities are:
        - Search by all fields
        - Sort by all fields(individually)
        - Filter the results by the date added(date range option)
        - Paginator for the table
      The Go back button leads to the previous page the user was on.
    </pre>
    <img src="https://i.ibb.co/0hBZr5b/store-orders-page.jpg"></img>
    <pre>
    Also the store's orders page has Add Order Button which adds an order.
    The add order bottom sheet modal has a three-tabbed stepper:
      1. Choose Supplier. If the user haven't added a supplier yet 
         he will be prompted to go to the suppliers page and add one.
      2. Fill the order number and add products to the order.
         You can choose from a dropdown with already existing in the database products
         or add a new one.
      3. Press Add Order to finish the process or press Previous Step to go to the previous step and edit.
    </pre>
    <p>
      <img height="300em" src="https://i.ibb.co/vPmsxG6/add-order-sheet-step-one.jpg"></img>
      <img height="300em" src="https://i.ibb.co/hgHx26k/add-order-sheet-step-two.jpg"></img>
      <img height="300em" src="https://i.ibb.co/mFnjcjf/add-order-sheet-step-three.jpg"></img>
    </p>
    <pre>
    The store's suppliers page is equivalent to the orders one. It has the same functionality. 
    The difference is the add supplier bottom sheet modal which asks for the suppliers name.
    </pre>
    <img src="https://i.ibb.co/CMgH6H5/store-suppliers-page.jpg"></img>
    <pre>
    The sales page is the place where the user is managing his sales. The Go Back button is included here as well.
    The functionality on this page is:
    1. Add(register) cashier to your store button is the place
       where the admin account may register a new user attached as a cashier to the current store.
    2. A block with all the added products for the sale that's coming.
    3. Dropdown menu with products in the database 
       and a quantity field for the amount of the product that the customer wants to buy.
    4. The Add Product button adds the currently selected product to the products list.
    5. The Sell button finishes the sale and subtracts the amount sold from the product quantity in the database.
    6. The bill that the customer should pay.
    </pre>
    <img src="https://i.ibb.co/yy59PGc/store-sales-page.jpg"></img>
    <pre>
    The Register Cashier bottom sheet modal is no different from the regular Register form on the register page.
    The functionality here though is different.
    - The admin registers the cashier
    - The cashier logs with his email and password
    - When the cashier's authenticated instead of the store's dashboard page 
      he instantly gets redirected to the sales page
    </pre>
    <pre>
      Finally, 
    the All Orders page shows all the orders for the user's stores, the name of the store it got added to
    and the date it got added.
    </pre>
    <img src="https://i.ibb.co/vzK1Tjq/all-orders-page.jpg"></img>
    
Technical Documentation
=====================================
### Brief information
    The application uses SqlServer DbContext + Identity for the user control.
    The application uses POCO classes for the Configuration root json's.
    The application uses JWT Bearer scheme and custom refresh tokens for the authentication/authorization.
    The application uses Repository pattern for getting data from the database.
    The application uses Swagger in Development Environment for the API documentation.
    The application uses Email confirmation registration.

### Application setup on local machine
    Step 1. Clone the repo and open the Store-Ge.Web.sln project file in the Store-Ge.Web folder
    Step 2. Initialize user secrets for the project
      The Keys needed for the project to work properly are:
      -"SendGridSettings:SendGridApiKey"
      -"JwtSettings:Secret"
      -"DbUser"
      -"DbPassword"
      -"DbConfiguration:ConnectionString"
      -"StoreGeAppSettings:DataProtectionKey"
    Step 3. Register in https://sendgrid.com/ with free account and get the API key that's been given to you(don't give it to anyone else). Add the key to the "SendGridSettings:SendGridApiKey" secret
    Step 4. The other secrets are up to you to give them random suitable value(strong highly unguessable values are recommended)
    Step 5. If you run your SQL Server on Docker start it.
    Step 6. Open the Store-Ge.UI project with VS Code and run the "ng serve -o" command in the terminal. This will serve the front end project and will be opened on port 4200.
    Step 7. Start the Web project under the IIS Express profile
    Step 8. Should be ready to use!
    
### Application Flow
<p>
    <pre>                    - Registration Flow                                     - Functionality</pre>
    <p>
      <img height="400em" src="https://i.ibb.co/LCGWh18/store-ge-registration-flow.jpg"/></img>
      <img height="600em" src="https://i.ibb.co/WpMJJ1t/store-ge-component-diagram.jpg"/></img>
    </p>
</p>

Tech Stack:
==========================================

### API
<p></p>
<ul>
  <li>ASP.Net Core 6.0</li>
  <li>EntityFramework Core 6.0.1</li>
  <li>Z.EntityFramework.Extensions.EFCore 6.16.1</li>
  <li>AutoMapper 12.0</li>
  <li>LinqKit 1.2.2</li>
  <li>SendGrid 9.28.1</li>
  <li>EPPlus 6.1.1</li>
  <li>Swashbuckle.AspNetCore.Swagger 6.4</li>
  <li>Microsoft.AspNetCore.Identity 6.0.1</li>
  <li>Microsoft.AspNetCore.Authentication.JwtBearer 6.0.9</li>
</ul>

### Front-End
<p></p>
<ul>
  <li>Angular 14.2.6</li>
  <li>Material 13.3.9</li>
  <li>RxJs 7.5.7</li>
  <li>TypeScript 4.6.4</li>
  <li>JwtHelper</li>
</ul>

### Database
<p></p>
<ul>
  <li>MSSQL Server</li>
</ul>

### Tests
<p></p>
<ul>
  <li>NUnit 3.13.3</li>
  <li>NUnit3TestAdapter 4.3.1</li>
  <li>Moq 4.18.2</li>
  <li>Microsoft.EntityFrameworkCore.InMemory 6.0.11</li>
  <li>Microsoft.NET.Test.Sdk 17.4</li>
  <li>coverlet.collector 3.2</li>
</ul>

### Git tools
<p></p>
<ul>
  <li>GitHub</li>
  <li>GitHub Desktop/Tortoise Git</li>
</ul>
