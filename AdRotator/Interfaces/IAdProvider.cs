
namespace AdRotator.Model
{
    public interface IAdProvider
    {
        AdType AdProviderType
        {
            get;
            set;
        }

        string AppId
        {
            get;
            set;
        }

        string SecondaryId
        {
            get;
            set;
        }

        int Probability
        {
            get;
            set;
        }

        bool ProbabilitySpecified
        {
            get;
        }

        bool IsTest
        {
            get;
            set;
        }

        bool IsTestSpecified
        {
            get;
        }

        int AdOrder
        {
            get;
            set;
        }

        bool AdOrderSpecified
        {
            get;
        }
    }
}