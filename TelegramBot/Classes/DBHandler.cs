using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using Microsoft.Data.Sqlite;

namespace TelegramBot.Classes
{
    class DBHandler : IDbHelper
    {
        readonly string dbPath;
        SqliteConnection connection;
        SqliteDataReader dataReader;
        string RecipeName = null;
        string Description = null;

        public DBHandler()
        {
            this.dbPath = ConfigurationManager.AppSettings["SQLiteConnectionString"];
            // Инициализируем подключение к базе данных.
            connection = new SqliteConnection(dbPath);
        }

        public List<List<string>> Search(string keyword)
        {
            /* База данных использует связь один к одному.
            Строка говорит: выбрать все записи из таблицы CookBook присоедив таблицу Ingredients используя поля recipe_id,
            где в колонках ingr1..ingr10 есть искомое слово.
            */
            string SearchQuery = $"SELECT * FROM Cookbook JOIN Ingredients ON Cookbook.recipe_id = Ingredients.ingredient_id " +
                $"WHERE ingr1 LIKE '%{keyword}%' " +
                $"OR ingr2 LIKE '%{keyword}%' " +
                $"OR ingr3 LIKE '%{keyword}%' " +
                $"OR ingr4 LIKE '%{keyword}%' " +
                $"OR ingr5 LIKE '%{keyword}%' " +
                $"OR ingr6 LIKE '%{keyword}%' " +
                $"OR ingr7 LIKE '%{keyword}%' " +
                $"OR ingr8 LIKE '%{keyword}%' " +
                $"OR ingr9 LIKE '%{keyword}%' " +
                $"OR ingr10 LIKE '%{keyword}%' ";

            // Создаем временную переменную для формирования выводной строки.
            string tmp = null;
            //StringBuilder result = new StringBuilder();
            // Создаем объект StringBuilder для хранения извлеченных ингредиентов.
            StringBuilder ingr_tmp = new StringBuilder();
            /* 
            Создаем список списков в котором будем хранить все найденные рецепты.
            Структура такая:
            res -> Recipe1
                -> Recipe2
                -> Recipe3
                -> ...
            */
            List<List<string>> res = new List<List<string>>();
            // Переменная для подсчета найденных рецептов. Чтобы не было ошибок что список пустой.
            int count = 0;
            // Создаем переменную типа SqliteCommand которая отвечает за выполнение выборки.
            SqliteCommand command = new SqliteCommand(SearchQuery, connection);

            try
            {
                // Открываем подключение к базе данных
                connection.Open();

                // Это коллекция прочитанных строк в базе данных. ExecuteReader выполняет выборку и возвращает dataReader.
                dataReader = command.ExecuteReader();

                // Метод Read() dataReader'а передвигается к следующей строке и возвращает false если строк нет.
                while (dataReader.Read())
                {
                    // Увеличиваем счетчик рецептов
                    count++;
                    /*
                    Цикл проходит от 4 до 14 потому что, в таблице Ingedients после выборки и объединения таблиц
                    с 0 по 4 столбец занимают столбцы RecipeId, RecipeName, Description, Ingredients, recipe_id.
                    */
                    for (int i = 5; i <= 14; i++)
                    {
                        // Проверяем если i-тый столбец не пустой
                        if (!dataReader.IsDBNull(i))
                        {
                            // то получаем строку i-того столбца из dataReader и добавляем в переменую ingr_tmp
                            ingr_tmp.Append(dataReader.GetString(i) + ", ");
                        }
                        else
                        {
                            // иначе, если стоблец пустой, выходим из цикла.
                            // В данной базе данных все столбцы заполнены последовательно.
                            // Поэтому пустых столбцов в середине строки не будет.
                            break;
                        }
                    }
                    // Получаем первый и второй столбцы из datareader.
                    RecipeName = dataReader.GetString(1);
                    Description = dataReader.GetString(2);
                    // Формируем строку вывода.
                    tmp = $"\nНазвание: {RecipeName}\nИнгредиенты: {ingr_tmp}\nОписание: {Description}";
                    // result.Append(tmp);
                    // result.Append("\n");
                    // Если количество рецептов больше нуля то записываем в список списков.
                    if (count > 0)
                    {
                        res.Add(new List<string> { tmp });
                    }
                    // Очищаем временную переменную для следующего поиска.
                    tmp = null;
                }
            }
            finally
            {
                connection.Close();
                dataReader.Close();
                command.Dispose();
            }
            return res;
        }
    }
}
