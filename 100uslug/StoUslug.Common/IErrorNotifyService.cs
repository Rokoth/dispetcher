using System.Threading.Tasks;

namespace StoUslug.Common
{
    /// <summary>
    /// Wrapper интеграции с сервисом сбора уведомлений об ошибках
    /// </summary>
    public interface IErrorNotifyService
    {        
        /// <summary>
        /// Отправить уведомление
        /// </summary>
        /// <param name="message">текст уведомления</param>
        /// <param name="level">Уровень сообщения</param>
        /// <param name="title">Заголовок (не обяз)</param>
        /// <returns></returns>
        Task Send(string message, MessageLevelEnum level = MessageLevelEnum.Error, string title = null);
    }
}