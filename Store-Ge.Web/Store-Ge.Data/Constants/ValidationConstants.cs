namespace Store_Ge.Data.Constants
{
    public static class ValidationConstants
    {
        public const string PASSWORD_VALIDATION_PATTERN = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,20}$";
        public const int EMAIL_MAX_LENGTH = 50;
        public const int STORE_NAME_MAX_LENGTH = 20;
        public const int PRODUCT_NAME_MAX_LENGTH = 50;
        public const int SUPPLIER_NAME_MAX_LENGTH = 20;
    }
}
