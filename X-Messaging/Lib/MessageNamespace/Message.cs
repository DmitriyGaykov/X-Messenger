global using System.Text.Json.Serialization;

namespace Lib.MessageNamespace;

public class Message
{
    #region Fields

    protected readonly Guid id = Guid.NewGuid();

    #endregion

    #region Props
    public Guid Id => id;
    public TypeOfMessage MessageType { get; set; }
    public int IDUserFrom { get; set; }
    public int IDUserTo { get; set; }
    public bool NeedResnd { get; set; }

    #endregion

    #region Ctors

    public Message(TypeOfMessage type)
    {
        MessageType = type;
        NeedResnd = true;
    }

    public Message(Message msg)
    {
        if (msg is not null)
        {
            id = msg.Id;
            MessageType = msg.MessageType;
            IDUserFrom = msg.IDUserFrom;
            IDUserTo = msg.IDUserTo;
            NeedResnd = msg.NeedResnd;
        }
    }

    public Message()
    {
        NeedResnd = false;
    }

    #endregion

    #region Methods

    public override bool Equals(object? obj) => obj is Message m && m.Id.Equals(id);

    #endregion

    #region TypeOfMessage

    public enum TypeOfMessage
    {
        ViewMessage, // Текстовое сообщение | Голосовое сообщение
        AnswerMessage, // Ответ от сервера на успешный или неуспешный прием сообщения
        OnlineMessage, // Сообщение от клиента посылается каждые 20 секунд на сервер, чтобы указать, что он в сети
        CallMessage // Сообщение, работающей со звонками
    }

    #endregion
}
