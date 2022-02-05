using CustomWebServer.Server.HTTP;

namespace CustomWebServer.Server.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public abstract class HttpMethodAttribute : Attribute
    {
        protected HttpMethodAttribute(Method method)
        {
            Method = method;
        }

        public Method Method { get; private set; }
    }
}
