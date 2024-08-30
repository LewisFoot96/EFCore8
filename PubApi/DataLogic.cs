using PublishData;
using PublisherDomain;

namespace PubApi
{
    public class DataLogic
    {
        PubContext _pubContext;

        public DataLogic(PubContext pubContext)
        {
            _pubContext = pubContext;
        }

        public DataLogic()
        {
            _pubContext = new PubContext();
        }

        public int ImportAuthors(Dictionary<string, string> authorList)
        {

            foreach (var author in authorList)
            {
                _pubContext.Authors.Add(new Author
                {
                    FirstName = author.Key,
                    LastName = author.Value
                });
            }

            return _pubContext.SaveChanges();
        }
    }
}
