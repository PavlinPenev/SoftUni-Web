namespace Store_Ge.Web.Constants
{
    public static class Constants
    {
        public static class Accounts
        {
            public static class Routes
            {
                public const string ACCOUNTS_CONTROLLER = "api/accounts";
                public const string ACCOUNTS_LOGIN_ENDPOINT = "login";
                public const string ACCOUNTS_REGISTER_ENDPOINT = "register";
                public const string REFRESH_ACCESS_TOKEN_ENDPOINT = "refresh-token";
                public const string CONFIRM_EMAIL_ENDPOINT = "confirm-email";
                public const string RESEND_EMAIL_ENDPOINT = "resend-confirmation-email";
                public const string FORGOT_PASSWORD_ENDPOINT = "forgot-password";
                public const string PASSWORD_RESET_ENDPOINT = "password-reset";
                public const string ADD_CASHIER_ENDPOINT = "add-cashier";
                public const string GET_USER_ENDPOINT = "get-user";
                public const string UPDATE_USER_INFO_ENDPOINT = "update-user";
            }

            public static class Shared
            {
                public const string CONFIRM_PASSWORD_STRING_LITERAL = "Confirm Password";
                public const string CONFIRM_PASSWORD_SHOULD_MATCH_THE_PASSWORD = "Confirm Password input should match the Password.";
                public const string INVALID_CREDENTIALS = "Invalid credentials";
            }
        }

        public static class Stores
        {
            public static class Routes
            {
                public const string STORES_CONTROLLER = "api/stores";
                public const string GET_USER_STORES_ENDPOINT = "get-stores";
                public const string ADD_STORE_ENDPOINT = "add-store";
                public const string GET_STORE_ENDPOINT = "get-store";
                public const string EXPORT_EXCEL_REPORT_ENDPOINT = "export-report";
            }
        }

        public static class Products
        {
            public static class Routes
            {
                public const string PRODUCTS_CONTROLLER = "api/products";
                public const string GET_STORE_PRODUCTS_ENDPOINT = "get-store-products";
                public const string GET_STORE_ADD_PRODUCTS_ENDPOINT = "get-store-add-products";
                public const string SELL_PRODUCTS_ENDPOINT = "sell-products";
            }
        }

        public static class Orders
        {
            public static class Routes
            {
                public const string ORDERS_CONTROLLER = "api/orders";
                public const string GET_USER_ORDERS_ENDPOINT = "get-user-orders";
                public const string GET_STORE_ORDERS_ENDPOINT = "get-store-orders";
                public const string ADD_ORDER_ENDPOINT = "add-order";
            }
        }

        public static class Suppliers
        {
            public static class Routes
            {
                public const string SUPPLIERS_CONTROLLER = "api/suppliers";
                public const string GET_USER_SUPPLIERS_ENDPOINT = "get-user-suppliers";
                public const string GET_USER_SUPPLIERS_PAGED_ENDPOINT = "get-user-suppliers-paged";
                public const string ADD_SUPPLIER_ENDPOINT = "add-supplier";
            }
        }
    }
}
