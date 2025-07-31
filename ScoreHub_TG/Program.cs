using System.Net;
using System.Net.Http.Headers;
using ScoreHub_TG;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;
using User = ScoreHub_TG.User;

var botClient = new TelegramBotClient("8248315151:AAHhR39w2gAAmrWSJk8hvMjLQeUy3sQjmNc"); 

using var cts = new CancellationTokenSource();
using var context = new TgBotDbContext();
var userRepository = new UserRepository(context);

var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = Array.Empty<UpdateType>()
};

botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    errorHandler: HandleErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);

var me = await botClient.GetMe();
Console.WriteLine($"Бот запущен: @{me.Username}");
Console.ReadLine(); 

cts.Cancel();

async Task HandleUpdateAsync(ITelegramBotClient bot, Update update, CancellationToken token)
{
    var messageText = update.Message.Text;
    var chatId = update.Message.Chat.Id;
    if (messageText.ToLower().Contains("/start"))
    {
       StartCommandHandler(messageText,bot,token,chatId);
    }
}

Task HandleErrorAsync(ITelegramBotClient bot, Exception exception, CancellationToken token)
{
    Console.WriteLine($"Ошибка: {exception.Message}");
    return Task.CompletedTask;
}

async Task StartCommandHandler(string messageText, ITelegramBotClient bot, 
    CancellationToken token, long chatId)
{
    if (messageText.Split(' ').Length != 2)
    {
        await bot.SendMessage(
            chatId: chatId,
            text: $"Упс! Отсутствует токен. Пожалуйста, перейдите по ссылке в вашем профиле для успешной авторизации.",
            cancellationToken: token);
        return;
    }
    
    var userId = messageText.Split(' ')[1];
    
    if (await API.VerifyData(userId))
    {
        //await SaveUser(chatId, userId);
        var user = await API.GetUser(userId);
        string welcomeMessageText = "";
        if (user.Role == Role.Student)
            welcomeMessageText =
                $"Приветствую тебя, {user.Name}!\nРоль: {user.Role}\nEmail: {user.Email}\n\nТебе доступны следующие функции:\n- Посмотреть баллы\n Ну и пока все, чо хотел то";
        else if (user.Role == Role.Teacher)
            welcomeMessageText =
                $"Доброго времени суток, {user.Name}! К сожалению, весь функционал преподавателей поместился на scorehub.ru((";
        else
            welcomeMessageText = $"Здравствуй, {user.Name}. Ты можешь сделать следующее:\n- Посмотреть, в каких предметах ты задействован";
        
        await bot.SendMessage(
            chatId: chatId,
            text: welcomeMessageText,
            cancellationToken: token);
    }
    else
    {
        await bot.SendMessage(
            chatId: chatId,
            text: $"Ошибка авторизации. Неверные данные",
            cancellationToken: token);
    }
}

async Task SaveUser(long chatId, string token)
{
    var user = new User()
    {
        UserId = chatId,
        Token = token
    };
    await userRepository.AddUser(user);
}