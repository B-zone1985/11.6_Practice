using MyTelegramBot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using MyTelegramBot.Services;

namespace MyTelegramBot.Controllers
{
    public class TextMessageController
    {
        private readonly ITelegramBotClient _telegramClient;
        private readonly IStorage _memoryStorage; 


        public TextMessageController(ITelegramBotClient telegramBotClient, IStorage memoryStorage)
        {
            _telegramClient = telegramBotClient;
            _memoryStorage = memoryStorage; 

        }
        public async Task Handle(Message message, CancellationToken ct)
        {
            switch (message.Text)
            {
                case "/start":

                    // Объект, представляющий кноки
                    var buttons = new List<InlineKeyboardButton[]>();
                    buttons.Add(new[]
                    {
                        InlineKeyboardButton.WithCallbackData($" Кол-во символов" , $"1"),
                        InlineKeyboardButton.WithCallbackData($" Сумма чисел" , $"2")
                    });
                    await _telegramClient.SendTextMessageAsync(message.Chat.Id, $"<b>  Наш бот подсчитывает кол-во символов и чисел.</b> {Environment.NewLine}" +
                        $"{Environment.NewLine}{Environment.NewLine}", cancellationToken: ct, parseMode: ParseMode.Html, replyMarkup: new InlineKeyboardMarkup(buttons));

                    break;
                default:
                    string typeCode = _memoryStorage.GetSession(message.Chat.Id).TypeCode; 

                    if (typeCode == "1")
                    {
                        int count = message.Text.Length;
                        if (count == 1)
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "В вашем сообщении " + count.ToString() + " символ");
                        }
                        else if (count == 2)
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "В вашем сообщении " + count.ToString() + " символа");
                        }
                        else
                        {
                            await _telegramClient.SendTextMessageAsync(message.Chat.Id, "В вашем сообщении " + count.ToString() + " символов");
                        }

                        

                    }
                    if (typeCode == "2")
                    {
                        string[] numbers = message.Text.Split(' ');
                        double total = 0;
                        foreach (string word in numbers)
                        {
                            //Делаем проверку на числа
                            if (double.TryParse(word, out double result) == true)
                            {
                                total = total + Convert.ToDouble(result);
                            }
                        }
                        await _telegramClient.SendTextMessageAsync(message.Chat.Id, "Сумма чисел: " + total.ToString());

                    }
                    break;
            }
        }
    }
}