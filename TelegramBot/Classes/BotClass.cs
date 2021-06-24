/*
 const string keyboard =
             "{" +
                    "\"inline_keyboard\": [" +
                            "[{\"text\": \"Show more...\", \"callback_data\": \"more\"}, {\"text\": \"Test...\", \"callback_data\": \"test\"}]" +
                    "]" +
                "}";
 */

using System;
using System.Configuration;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;

using TelegramBot.POCOs;

namespace TelegramBot.Classes
{
    class BotClass
    {
        readonly string TOKEN;
        HttpClient httpClient;
        Root rt;
        IDbHelper IDbHelper;
        int update_id;
        int lastMessageChatId;
        string lastMessage;
        string Username;
        string FirstName;
        string temp;
        int count = 0;
        List<List<string>> result = new List<List<string>>();
        const string keyboard = // Сериализованный json объект с Inline клавиатурой.
             "{" +
                    "\"inline_keyboard\": [" +
                            "[{\"text\": \"Show more...\", \"callback_data\": \"more\"}]" +
                    "]" +
                "}";

        public BotClass()
        {
            httpClient = new HttpClient();
            rt = new Root();
            IDbHelper = new DBHandler();
        }

        public BotClass(IDbHelper _IDbHelper) 
        {
            this.TOKEN = ConfigurationManager.AppSettings["TelegramApiToken"];
            this.IDbHelper = _IDbHelper;
            httpClient = new HttpClient();
            rt = new Root();
        }

        public async void StartBot()
        {
            while (true)
            {
                /*
                Отправляем Telegram API токен с методом getUpdates.
                Получаем ответ от сервера в виде json файла.
                Timeout = 5, означает что подключение будет открыто 5 секунд.
                Чтобы сильно не нагружать сервер, лучше увеличить timeout или использовать WebHooks.
                */
                string responce = await httpClient.GetStringAsync($"https://api.telegram.org/bot{TOKEN}/getUpdates?offset={update_id}&timeout=5");
                // Десериализуем наш json строку в класс Root.
                rt = JsonSerializer.Deserialize<Root>(responce);
                // Если результат ответа от Api в разделе result json файла НЕ пустой.
                if (rt.result.Count != 0)
                {
                    // Если последнее сообщение ответа от API не пустое.
                    if (rt.result[rt.result.Count - 1].message != null)
                    {
                        // Получаем идентификатор последнего обновления.
                        update_id = rt.result[rt.result.Count - 1].update_id;
                        // Получаем идентификатор последнего сообщения.
                        lastMessageChatId = rt.result[rt.result.Count - 1].message.chat.id;
                        // Получаем текст последнего сообщения.
                        lastMessage = rt.result[rt.result.Count - 1].message.text;
                        // Получаем псевдоним пользователя, который отправил последнее сообщение.
                        Username = rt.result[rt.result.Count - 1].message.from.username;
                        // Получаем имя пользователя, который отправил последнее сообщение.
                        FirstName = rt.result[rt.result.Count - 1].message.from.first_name;
                    }
                    /*
                    Увеличиваем update_id на единицу.
                    Эта переменная нужна для того, чтобы отследить последнее обновление на сервере.
                    В строке 63 мы передаем параметр offset с данной переменной.
                    Каждое обновление на сервере имеет свой идентификатор. А каждое новое обновление имеет идентификатор + 1.
                    Например последнее обновление пришло с идентификатором 1000. Следующее обновление будет уже 1001.
                    Поэтому, чтобы клиент не отсылал нам обновления постоянно один за другим, мы передаем на сервер update_id + 1.
                    Получается, когда мы передаем например 1002 (а последнее было 1001), то сервер просто будет ждать, 
                    когда это 1002-обновление произойдет и ждать оно будет 5 секунд.
                    А произойдет оно тогда, когда пользователь отправит сообщение боту и update_id на сервере инкрементируется автоматически.
                    Соответственно когда на сервере update_id станет 1002, то API пришлет нам ответ.
                    */
                    update_id++;
                    /*
                    Callback Query нужен для Inline Keyboard.
                    Когда пользователь нажимает на кнопку под сообщением, приходит обновление в котором содержится раздел
                    callback_query.
                    Если callback_query последнего обновления не пустой.
                    */
                    if (rt.result[rt.result.Count - 1].callback_query != null)
                    {
                        /*
                        Count это счетчик нажатия на кнопку.
                        Result это список списков для нескольких сообщений.
                        Если счетчик не равен количеству элементов списка.
                        */
                        if(count != result.Count)
                        {
                            // Получаем индентификатор последнего сообщения callback запроса.
                            var message_id = rt.result[rt.result.Count - 1].callback_query.message.message_id;
                            // Обнуляем переменную, чтобы бот повторно не отсылал результат поиска.
                            lastMessage = null;
                            /*
                            Отправляем запрос в API с методом editMessageText.
                            chat_id - это идентификатор чата. То есть, в какой чат будем редактировать сообщение.
                            message_id - идентификатор последнего сообщения callback запроса. Это сообщение мы будем менять.
                            text - извлекаем новое сообщение из списка списков, полученного из базы данных.
                            reply_markup - отправляем клавиатуру в виде сериализованного json объекта.
                            */
                            await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/editMessageText?chat_id={lastMessageChatId}&message_id={message_id}&text={result[count][0]}&reply_markup={keyboard}");
                            count++;
                        }
                        else
                        {
                            count = 0;
                            string no_ingr = "Больше нет рецептов с таким ингредиентом.";
                            // Отправляем сообщение.
                            await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={lastMessageChatId}&text={no_ingr}");
                        }
                    }
                    // Вызываем метод обработки команд передавая в него:
                    // Идентификатор чата последнего обновления.
                    // Имя пользователя последнего обновления.
                    // Само сообщение из последнего обновления.
                    ProcessCommands(ChatId: lastMessageChatId, Firstname: FirstName, message: lastMessage);
                }
            }
        }

        public async void ProcessCommands(int ChatId, string Firstname, string message)
        {
            if (message != null)
            {
                // Если сообщение содержит команду, то отправить соответствующее сообщение.
                switch (message)
                {
                    case "/start":
                        temp = $"Привет {FirstName}! Я совсем не бот а целый помощник в поиске рецептов. Я могу помочь тебе приготовить что-нибудь вкусненькое, если ты расскажешь " +
                            $"какие игридиенты у тебя есть. Пока, моя память насчитывает не слишком много вкуснейших рецептов, но я обязательно буду узнавать новые чтобы " +
                            $"ты не подох с голоду или со скуки. Если ты не знаешь как мной пользоваться, напиши мне /help";
                        await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={ChatId}&text={temp}");
                        break;
                    case "/help":
                        temp = $"/help - эта справка.\n" +
                            $"/search - поиск рецептов. По имени рецепта и по ингредиентам.\n Чтобы произвести поиск по рецепту просто напиши мне \"/search ингредиент\".\n" +
                                                     $"Так же знай что твой запрос поиска чувствителен к регистру. То есть слова \"Мясо\" и \"мясо\" являются разными.\n" +
                            $"/about - обо мне.";
                        await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={ChatId}&text={temp}");
                        break;
                    case "/about":
                        temp = "Связаться с хозяином ты можешь если напишешь ему @xorbyten";
                        await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={ChatId}&text={temp}");
                        break;
                    case "/keyboard":
                        temp = "This is a InlineKeyboard";
                        break;
                }
                if (message.Contains("/search"))
                {
                    // Пользователь будет отправлять сообщение поиска в формате "/search Трава". Поэтому отрезаем /search.
                    string tmp = message.Substring(7);
                    // Если искомого слова нет, то отправить сообщение пользователю.
                    if(tmp == "") 
                    {
                        await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={ChatId}&text=Введите имя игредиента с большой буквы.");
                    }
                    else 
                    {
                    // Убираем пробелы с перед и после слова.
                    tmp = tmp.Trim();
                    // Передаем искомое слово в обработчик базы данных. И записываем возвращаемый список в список result.
                    result = IDbHelper.Search(tmp);
                    // Если список пустой то отправляем пользователю сообщение.
                    if(result.Count == 0)
                    {
                        await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={ChatId}&text=Рецептов с таким ингредиентом не найдено.");
                    }
                    else // Иначе, если список не пустой, то отправляем найденный первый рецепт пользователю.
                    {
                        await httpClient.GetAsync($"https://api.telegram.org/bot{TOKEN}/sendMessage?chat_id={ChatId}&text={result[0][0]}&reply_markup={keyboard}");
                    }
                    }
                }
            }
        }
    }
}
