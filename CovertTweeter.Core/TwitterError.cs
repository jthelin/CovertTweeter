namespace CovertTweeter.Core
{
    public class TwitterError
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }

    public class TwitterStatus
    {
        public string  CreatedAt { get; set; }
        public long Id { get; set; }
        public string Text { get; set; }

        public string InReplyToScreenName { get; set; }
        public long? InReplyToUserId { get; set; }
        public TwitterStatus RetweededStatus { get; set; }

        public TwitterUser User { get; set; }
    }

    public class TwitterUser
    {
        public string Name { get; set; }
        public string ScreenName { get; set; }
    }   
}