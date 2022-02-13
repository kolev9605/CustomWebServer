using CustomWebServer.Server.Attributes;
using CustomWebServer.Server.Controllers;
using CustomWebServer.Server.HTTP;
using CustomWebServer.Server.Responses;
using System.Reflection;

namespace CustomWebServer.Server.Routing;

public static class RoutingTableExtensions
{
    public static IRoutingTable MapGet<TController>(
        this IRoutingTable routingTable,
        string path,
        Func<TController, Response> controllerFunc)
        where TController : Controller
    {
        return routingTable.MapGet(path, request => controllerFunc(CreateController<TController>(request)));
    }

    public static IRoutingTable MapPost<TController>(
    this IRoutingTable routingTable,
    string path,
    Func<TController, Response> controllerFunc)
    where TController : Controller
    {
        return routingTable.MapPost(path, request => controllerFunc(CreateController<TController>(request)));
    }

    public static IRoutingTable MapControllers(this IRoutingTable routingTable)
    {
        var controllerActions = GetControllerActions();

        foreach (var controllerAction in controllerActions)
        {
            var controllerName = controllerAction
                .DeclaringType
                .Name
                .Replace(nameof(Controller), string.Empty);

            var actionName = controllerAction.Name;

            var path = $"/{controllerName}/{actionName}";

            var responseFunction = GetResponseFunction(controllerAction);

            var httpMethod = Method.Get;
            var actionMethodAttribute = controllerAction.GetCustomAttribute<HttpMethodAttribute>();
            if (actionMethodAttribute != null)
            {
                httpMethod = actionMethodAttribute.Method;
            }

            routingTable.Map(httpMethod, path, responseFunction);

            MapDefaultRoutes(
                routingTable,
                httpMethod,
                controllerName,
                actionName,
                responseFunction);
        }

        return routingTable;
    }

    private static Func<Request, Response> GetResponseFunction(MethodInfo controllerAction)
    {
        return request =>
        {
            var controllerInstance = CreateController(controllerAction.DeclaringType, request);
            var parameterValues = GetParameterValues(controllerAction, request);

            return (Response)controllerAction.Invoke(controllerInstance, parameterValues);
        };
    }

    private static object[] GetParameterValues(MethodInfo controllerAction, Request request)
    {
        var actionParameters = controllerAction
            .GetParameters()
            .Select(p => new
            {
                Name = p.Name,
                Type = p.ParameterType
            })
            .ToArray();

        var parameterValues = new object[actionParameters.Length];
        for (int i = 0; i < actionParameters.Length; i++)
        {
            var parameter = actionParameters[i];
            var parameterName = parameter.Name;
            var parameterType = parameter.Type;

            if (parameterType.IsPrimitive || parameterType == typeof(string))
            {
                var parameterValue = request.GetValue(parameterName);

                parameterValues[i] = Convert.ChangeType(parameterValue, parameterType);
            }
            else
            {
                var parameterValue = Activator.CreateInstance(parameterType);

                var parameterProperties = parameterType.GetProperties();

                foreach (var property in parameterProperties)
                {
                    var propertyValue = request.GetValue(property.Name);

                    property.SetValue(
                        parameterValue,
                        Convert.ChangeType(propertyValue, property.PropertyType));
                }

                parameterValues[i] = parameterValue;
            }
        }

        return parameterValues;
    }

    private static IEnumerable<MethodInfo> GetControllerActions()
    {
        return Assembly
            .GetEntryAssembly()
            .GetExportedTypes()
            .Where(t => !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(Controller)))
            .Where(t => t.Name.EndsWith(nameof(Controller)))
            .SelectMany(t => t
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.ReturnType.IsAssignableTo(typeof(Response))))
            .ToList();
    }

    private static TController CreateController<TController>(Request request) 
        where TController : Controller
    {
        var controller = (TController)CreateController(typeof(TController), request);
        return controller;
    }

    private static Controller CreateController(Type controllerType, Request request)
    {
        var controller = (Controller)Request.ServiceCollection.CreateInstance(controllerType);

        controllerType
            .GetProperty("Request", BindingFlags.Instance | BindingFlags.NonPublic)
            .SetValue(controller, request);

        return controller;
    }

    private static string GetValue(this Request request, string parameterName)
        => request.Query.GetValueOrDefault(parameterName) ??
            request.Form.GetValueOrDefault(parameterName);

    private static void MapDefaultRoutes(
            IRoutingTable routingTable,
            Method httpMethod,
            string controllerName,
            string actionName,
            Func<Request, Response> responseFunction)
    {
        const string defaultActionName = "Index";
        const string defaultControllerName = "Home";

        if (actionName == defaultActionName)
        {
            routingTable.Map(httpMethod, $"/{controllerName}", responseFunction);

            if (controllerName == defaultControllerName)
            {
                routingTable.Map(httpMethod, "/", responseFunction);
            }
        }
    }


}
