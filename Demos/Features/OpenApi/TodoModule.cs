namespace Demos.Features.OpenApi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using Carter;
    using Carter.ModelBinding;
    using Carter.Request;
    using Carter.Response;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Routing;
    using OpenApiMetaData;

    public class TodoModule : CarterModule
    {
        private Dictionary<int, TodoItem> tasks = new Dictionary<int, TodoItem>();
        
        public TodoModule() : base("/todo")
        {
            Get<GetTodoItems>("/", GetAll);
            Post<CreateTodoItem>("/", CreateTodoItem);
            Get<GetTodoItem>("/{id:int}", GetTodoItem);
            Put<UpdateTodoItem>("/{id:int}", UpdateTodoItem);
            Delete<DeleteTodoItem>("/{id:int}", DeleteTodoItem);
        }

        private Task GetAll(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            return res.Negotiate(tasks.Values);
        }
        
        private Task GetTodoItem(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var id = routeData.As<int>("id");
            if (tasks.TryGetValue(id, out var task))
            {
                return res.Negotiate(task);
            }

            res.StatusCode = (int) HttpStatusCode.NotFound;
            return Task.CompletedTask;
        }
        
        private Task CreateTodoItem(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var result = req.BindAndValidate<TodoItem>();

            if (!result.ValidationResult.IsValid)
            {
                res.StatusCode = 422;
                return res.Negotiate(result.ValidationResult.GetFormattedErrors());
            }

            var nextId = tasks.Keys.Any() 
                ? tasks.Keys.Max() +1 
                : 1;

            result.Data.Id = nextId;
            tasks.Add(nextId, result.Data);


            res.StatusCode = 201;
            return res.Negotiate(result.Data);
        }
        
        private Task UpdateTodoItem(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var id = routeData.As<int>("id");
            if (!tasks.ContainsKey(id))
            {
                res.StatusCode = (int) HttpStatusCode.NotFound;
                return Task.CompletedTask;
            }
            
            var result = req.BindAndValidate<TodoItem>();

            if (!result.ValidationResult.IsValid)
            {
                res.StatusCode = 422;
                return res.Negotiate(result.ValidationResult.GetFormattedErrors());
            }

            tasks[id] = result.Data;
            res.StatusCode = (int) HttpStatusCode.NoContent;
            return Task.CompletedTask;
        }
        
        private Task DeleteTodoItem(HttpRequest req, HttpResponse res, RouteData routeData)
        {
            var id = routeData.As<int>("id");
            tasks.Remove(id);
            res.StatusCode = (int) HttpStatusCode.NoContent;
            return Task.CompletedTask;
        }
    }
}