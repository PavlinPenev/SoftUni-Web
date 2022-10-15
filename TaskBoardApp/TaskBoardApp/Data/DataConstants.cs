namespace TaskBoardApp.Data
{
    public static class DataConstants
    {
        public static class User
        {
            public const int FIRST_NAME_MAX_LENGTH = 15;
            public const int LAST_NAME_MAX_LENGTH = 15;
        }

        public static class Task
        {
            public const int TITLE_MAX_LENGTH = 70;
            public const int TITLE_MIN_LENGTH = 5;
            public const int DESCRIPTION_MAX_LENGTH = 1000;
            public const int DESCRIPTION_MIN_LENGTH = 10;
        }

        public static class Board
        {
            public const int NAME_MAX_LENGTH = 30;
            public const int NAME_MIN_LENGTH = 3;
        }
    }
}
